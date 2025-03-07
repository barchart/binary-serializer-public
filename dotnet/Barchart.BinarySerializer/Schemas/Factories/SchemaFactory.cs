﻿#region Using Statements

using System.Linq.Expressions;
using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Schemas.Collections;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Types.Exceptions;
using Barchart.BinarySerializer.Types.Factories;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Schemas.Factories;

/// <inheritdoc />
public class SchemaFactory : ISchemaFactory
{
    #region Fields

    private readonly IBinaryTypeSerializerFactory _binaryTypeSerializerFactory;
    
    #endregion
    
    #region Constructor(s)

    /// <summary>
    ///     Initializes a new instance of the <see cref="SchemaFactory"/> class.
    /// </summary>
    /// <param name="binarySerializerFactory">
    ///     The factory for creating binary type serializers.
    /// </param>
    public SchemaFactory(IBinaryTypeSerializerFactory binarySerializerFactory)
    {
        _binaryTypeSerializerFactory = binarySerializerFactory;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SchemaFactory"/> class with a default binary type serializer factory.
    /// </summary>
    public SchemaFactory() : this(new BinaryTypeSerializerFactory())
    {
        
    }
    
    #endregion
    
    #region Methods

    /// <inheritdoc />
    public ISchema<TEntity> Make<TEntity>(byte entityId = 0) where TEntity : class, new()
    {
        return Make<TEntity>(entityId, false);
    }

    private ISchema<TEntity> Make<TEntity>(byte entityId, bool nested) where TEntity : class, new()
    {
        if (!nested && entityId == 0)
        {
            throw new InvalidEntityIdException();
        }
        
        Type entityType = typeof(TEntity);

        IEnumerable<MemberInfo> members = Reflection.GetMembersForType(entityType);

        ISchemaItem<TEntity>[] schemaItems = members.Select(MakeSchemaItem<TEntity>).ToArray();

        Array.Sort(schemaItems, CompareSchemaItems);
        
        return new Schema<TEntity>(entityId, schemaItems);
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Type memberType = Reflection.GetMemberType(memberInfo);
        
        Type[] typeParameters;

        MethodInfo[] methods = typeof(SchemaFactory).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        
        MethodInfo unboundMethod;
        
        if (IsTypeSupported(memberType))
        {
            typeParameters = [typeof(TEntity), memberType];

            unboundMethod = methods.Single(GetMakeSchemaItemPredicate(typeParameters));
        }
        else if (IsCollectionType(memberType))
        {
            Type? itemType;

            if (memberType.IsArray)
            {
                itemType = memberType.GetElementType();
            }
            else
            {
                itemType = memberType.GetGenericArguments()[0];
            }

            typeParameters = [typeof(TEntity), itemType!];

            if (_binaryTypeSerializerFactory.Supports(itemType!))
            {
                unboundMethod = methods.Single(GetMakeSchemaItemCollectionPrimitivePredicate(typeParameters));
            }
            else
            {
                unboundMethod = methods.Single(GetMakeSchemaItemCollectionObjectPredicate(typeParameters));
            }
        }
        else if (IsNestedClass(memberType))
        {
            typeParameters = [typeof(TEntity), memberType];

            unboundMethod = methods.Single(GetMakeSchemaItemNestedPredicate(typeParameters));
        }
        else
        {
            throw new UnsupportedTypeException(memberType);
        }
        
        MethodInfo boundMethod = unboundMethod.MakeGenericMethod(typeParameters);

        object[] methodParameters = [memberInfo];
        
        object? schemaItem = boundMethod.Invoke(this, methodParameters);
        
        if (schemaItem == null)
        {
            throw new NullReferenceException("When called via reflection, the SchemaFactory.MakeSchemaItem<TEntity, TMember> returned a null reference. This should not be possible.");
        }

        return (ISchemaItem<TEntity>)schemaItem;
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity, TMember>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Func<TEntity, TMember> getter = MakeMemberGetter<TEntity, TMember>(memberInfo);
        Action<TEntity, TMember> setter = MakeMemberSetter<TEntity, TMember>(memberInfo);

        IBinaryTypeSerializer<TMember> serializer = _binaryTypeSerializerFactory.Make<TMember>();
        
        SerializeAttribute attribute = GetSerializeAttribute(memberInfo);

        string name = memberInfo.Name;
        bool key = attribute.Key;
        
        return new SchemaItem<TEntity, TMember>(name, key, getter, setter, serializer);
    }

    private ISchemaItem<TEntity> MakeSchemaItemNested<TEntity, TMember>(MemberInfo memberInfo) where TEntity: class, new() where TMember: class, new()
    {
        string name = memberInfo.Name;

        Func<TEntity, TMember> getter = MakeMemberGetter<TEntity, TMember>(memberInfo);
        Action<TEntity, TMember?> setter = MakeMemberSetter<TEntity, TMember>(memberInfo);

        ISchema<TMember> schema = Make<TMember>(0, true);

        return new SchemaItemNested<TEntity, TMember>(name, getter, setter, schema);
    }

    private ISchemaItem<TEntity> MakeSchemaItemCollectionPrimitive<TEntity, TItem>(MemberInfo memberInfo) where TEntity : class, new()
    {
        string name = memberInfo.Name;

        Func<TEntity, IList<TItem?>?> getter = MakeCollectionMemberGetter<TEntity, TItem>(memberInfo);
        Action<TEntity, IList<TItem?>?> setter = MakeCollectionMemberSetter<TEntity, TItem>(memberInfo);

        IBinaryTypeSerializer<TItem> itemSerializer = _binaryTypeSerializerFactory.Make<TItem>();
        
        return new SchemaItemListPrimitive<TEntity, TItem>(name, getter!, setter!, itemSerializer);
    }

    private ISchemaItem<TEntity> MakeSchemaItemCollectionObject<TEntity, TItem>(MemberInfo memberInfo) where TEntity : class, new() where TItem : class, new()
    {
        string name = memberInfo.Name;

        Func<TEntity, IList<TItem?>?> getter = MakeCollectionMemberGetter<TEntity, TItem>(memberInfo);
        Action<TEntity, IList<TItem?>?> setter = MakeCollectionMemberSetter<TEntity, TItem>(memberInfo);

        ISchema<TItem> itemSchema = Make<TItem>(0, true);

        return new SchemaItemList<TEntity, TItem>(name, getter, setter, itemSchema);
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItem) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemNestedPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItemNested) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemCollectionPrimitivePredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItemCollectionPrimitive) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemCollectionObjectPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItemCollectionObject) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }
    
    private static SerializeAttribute GetSerializeAttribute(MemberInfo memberInfo)
    {
        SerializeAttribute? serializeAttribute = memberInfo.GetCustomAttribute<SerializeAttribute>();

        if (serializeAttribute == null)
        {
            throw new NullReferenceException("An attempt to get a SerializeAttribute returned a null reference. This should not be possible.");
        }

        return serializeAttribute;
    }

    private static Func<TEntity, TMember> MakeMemberGetter<TEntity, TMember>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions =
        [
            Expression.Parameter(typeof(TEntity))
        ];
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        
        return Expression.Lambda<Func<TEntity, TMember>>(propertyAccessExpression, typeParameterExpressions).Compile();
    }

    private static Func<TEntity, IList<TItem?>?> MakeCollectionMemberGetter<TEntity, TItem>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions =
        [
            Expression.Parameter(typeof(TEntity))
        ];

        MemberExpression memberAccess = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);

        Type memberType = memberInfo is PropertyInfo propertyInfo ? propertyInfo.PropertyType : (memberInfo as FieldInfo)!.FieldType;

        Expression conversion;
        
        if (IsArrayType(memberType))
        {
            MethodInfo toListMethod = typeof(Enumerable).GetMethod("ToList")!.MakeGenericMethod(memberType.GetElementType()!);

            conversion = Expression.Call(toListMethod, Expression.Convert(memberAccess, memberType.GetElementType()!.MakeArrayType()));
            conversion = Expression.Convert(conversion, typeof(IList<TItem>));
        }
        else
        {
            conversion = Expression.Convert(memberAccess, typeof(IList<TItem>));
        }

        Expression nullCheckExpression = Expression.Condition(Expression.Equal(memberAccess, Expression.Constant(null, memberAccess.Type)), Expression.Constant(null, typeof(IList<TItem>)), conversion);

        return Expression.Lambda<Func<TEntity, IList<TItem?>?>>(nullCheckExpression, typeParameterExpressions[0]).Compile();
    }
    
    private static Action<TEntity, TMember?> MakeMemberSetter<TEntity, TMember>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions =
        [
            Expression.Parameter(typeof(TEntity)),
            Expression.Parameter(typeof(TMember))
        ];
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        BinaryExpression propertyAssignmentExpression = Expression.Assign(propertyAccessExpression, typeParameterExpressions[1]);

        return Expression.Lambda<Action<TEntity, TMember?>>(propertyAssignmentExpression, typeParameterExpressions).Compile();
    }
    
    private static Action<TEntity, IList<TItem?>?> MakeCollectionMemberSetter<TEntity, TItem>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions =
        [
            Expression.Parameter(typeof(TEntity)),
            Expression.Parameter(typeof(IList<TItem>))
        ];

        MemberExpression memberAccess = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);

        Type memberType = memberInfo is PropertyInfo propertyInfo ? propertyInfo.PropertyType : (memberInfo as FieldInfo)!.FieldType;

        Expression nullCheckExpression = Expression.Equal(typeParameterExpressions[1], Expression.Constant(null, typeof(IList<TItem>)));

        Expression assignmentExpression;
        
        if (IsArrayType(memberType))
        {
            MethodInfo toArrayMethod = typeof(Enumerable).GetMethod("ToArray")!.MakeGenericMethod(memberType.GetElementType()!);
            MethodCallExpression listToArrayConversion = Expression.Call(toArrayMethod, typeParameterExpressions[1]);

            assignmentExpression = Expression.Condition(nullCheckExpression, Expression.Constant(null, memberType), Expression.Convert(listToArrayConversion, memberType));
        }
        else
        {
            Type itemType = memberType.GetGenericArguments()[0];
            MethodInfo toListMethod = typeof(Enumerable).GetMethod("ToList")!.MakeGenericMethod(itemType);
            MethodCallExpression listToListConversion = Expression.Call(toListMethod, typeParameterExpressions[1]);

            assignmentExpression = Expression.Condition(nullCheckExpression, Expression.Constant(null, memberType), Expression.Convert(listToListConversion, memberType));
        }

        Expression assignToMember = Expression.Assign(memberAccess, assignmentExpression);
   
        return Expression.Lambda<Action<TEntity, IList<TItem?>?>>(assignToMember, typeParameterExpressions[0], typeParameterExpressions[1]).Compile();
    }

    private static int CompareSchemaItems<TEntity>(ISchemaItem<TEntity> a, ISchemaItem<TEntity> b) where TEntity: class, new()
    {
        int comparison = a.Key.CompareTo(b.Key);

        if (comparison == 0)
        {
            comparison = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
        }

        return comparison;
    }

    private static bool IsCollectionType(Type type)
    {
        return IsListType(type) || IsArrayType(type);
    }

    private static bool IsListType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    private static bool IsArrayType(Type type)
    {
        return type.IsArray;
    }

    private static bool IsNestedClass(Type type)
    {
        return type.IsClass;
    }

    private bool IsTypeSupported(Type type)
    {
        return _binaryTypeSerializerFactory.Supports(type);
    }

    #endregion
}