#region Using Statements

using System.Linq.Expressions;
using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Schemas.Factories;

public class SchemaFactory : ISchemaFactory
{
    #region Fields

    private readonly IBinaryTypeSerializerFactory _binaryTypeSerializerFactory;

    #endregion
    
    #region Constructor(s)

    public SchemaFactory(IBinaryTypeSerializerFactory binarySerializerFactory)
    {
        _binaryTypeSerializerFactory = binarySerializerFactory;
    }

    public SchemaFactory() : this(new BinaryTypeSerializerFactory())
    {
        
    }
    
    #endregion
    
    #region Methods

    /// <inheritdoc />
    public ISchema<TEntity> Make<TEntity>() where TEntity: class, new()
    {
        Type entityType = typeof(TEntity);

        IEnumerable<MemberInfo> members = GetMembersForType(entityType);

        ISchemaItem<TEntity>[] schemaItems = members.Select(MakeSchemaItem<TEntity>).ToArray();

        Array.Sort(schemaItems, CompareSchemaItems);
        
        return new Schema<TEntity>(schemaItems);
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Type memberType = GetMemberType(memberInfo);
        Type[] typeParameters;

        MethodInfo[] methods = typeof(SchemaFactory).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        
        MethodInfo unboundMethod;
        
        if (_binaryTypeSerializerFactory.Supports(memberType))
        {
            typeParameters = new [] { typeof(TEntity), memberType };

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

            typeParameters = new[] { typeof(TEntity), itemType! };

            if (_binaryTypeSerializerFactory.Supports(itemType!))
            {
                unboundMethod = methods.Single(GetMakeSchemaItemCollectionPrimitivePredicate(typeParameters));
            }
            else
            {
                unboundMethod = methods.Single(GetMakeSchemaItemCollectionObjectPredicate(typeParameters));
            }
        }
        else
        {
            typeParameters = new [] { typeof(TEntity), memberType };

            unboundMethod = methods.Single(GetMakeSchemaItemNestedPredicate(typeParameters));
        }
        
        MethodInfo boundMethod = unboundMethod.MakeGenericMethod(typeParameters);

        object[] methodParameters = { memberInfo };
        
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
        Action<TEntity, TMember> setter = MakeMemberSetter<TEntity, TMember>(memberInfo);

        ISchema<TMember> schema = Make<TMember>();

        return new SchemaItemNested<TEntity, TMember>(name, getter, setter, schema);
    }

    private ISchemaItem<TEntity> MakeSchemaItemColletionPrimitive<TEntity, TItem>(MemberInfo memberInfo) where TEntity : class, new()
    {
        string name = memberInfo.Name;

        Func<TEntity, IList<TItem>> getter = MakeCollectionMemberGetter<TEntity, TItem>(memberInfo);
        Action<TEntity, IList<TItem>> setter = MakeCollectionMemberSetter<TEntity, TItem>(memberInfo);

        IBinaryTypeSerializer<TItem> itemSerializer = _binaryTypeSerializerFactory.Make<TItem>();
        
        return new SchemaItemCollectionPrimitive<TEntity, TItem>(name, getter, setter, itemSerializer);
    }

    private ISchemaItem<TEntity> MakeSchemaItemCollectionObject<TEntity, TItem>(MemberInfo memberInfo) where TEntity : class, new() where TItem : class, new()
    {
        string name = memberInfo.Name;

        Func<TEntity, IList<TItem>> getter = MakeCollectionMemberGetter<TEntity, TItem>(memberInfo);
        Action<TEntity, IList<TItem>> setter = MakeCollectionMemberSetter<TEntity, TItem>(memberInfo);

        ISchema<TItem> itemSchema = Make<TItem>();

        return new SchemaItemCollectionObject<TEntity, TItem>(name, getter, setter, itemSchema);
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
        return methodInfo => methodInfo.Name == nameof (MakeSchemaItemColletionPrimitive) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemCollectionObjectPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItemCollectionObject) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
    }
    
    private static Type GetMemberType(MemberInfo memberInfo)
    {
        if (memberInfo is PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }

        if (memberInfo is FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType;
        }
        
        throw new ArgumentException("When using reflection to build a Schema<T>, the serializable members must be fields or properties");
    }

    private static IEnumerable<MemberInfo> GetMembersForType(Type entityType)
    {
        IEnumerable<MemberInfo> fields = entityType
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(FieldHasSerializeAttribute)
            .Where(FieldCanBeWritten);

        IEnumerable<MemberInfo> properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(PropertyHasSerializeAttribute)
            .Where(PropertyCanBeWritten);

        return fields.Concat(properties);
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
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        
        return Expression.Lambda<Func<TEntity, TMember>>(propertyAccessExpression, typeParameterExpressions).Compile();
    }

    private static Func<TEntity, IList<TItem>> MakeCollectionMemberGetter<TEntity, TItem>(MemberInfo memberInfo)
    {
        ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity));
        MemberExpression memberAccess = Expression.MakeMemberAccess(entityParameter, memberInfo);

        if (memberInfo is PropertyInfo propertyInfo && propertyInfo.MemberType == MemberTypes.Property && propertyInfo.PropertyType.IsArray)
        {
            UnaryExpression arrayToIListConversion = Expression.Convert(memberAccess, typeof(IList<TItem>));
            return Expression.Lambda<Func<TEntity, IList<TItem>>>(arrayToIListConversion, entityParameter).Compile();
        }
        
        return Expression.Lambda<Func<TEntity, IList<TItem>>>(memberAccess, entityParameter).Compile();
    }
    
    private static Action<TEntity, TMember> MakeMemberSetter<TEntity, TMember>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity)),
            Expression.Parameter(typeof(TMember))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        BinaryExpression propertyAssignmentExpression = Expression.Assign(propertyAccessExpression, typeParameterExpressions[1]);

        return Expression.Lambda<Action<TEntity, TMember>>(propertyAssignmentExpression, typeParameterExpressions).Compile();
    }
    
    private static Action<TEntity, IList<TItem>> MakeCollectionMemberSetter<TEntity, TItem>(MemberInfo memberInfo)
    {
        ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity));
        ParameterExpression listParameter = Expression.Parameter(typeof(IList<TItem>));
        MemberExpression memberAccess = Expression.MakeMemberAccess(entityParameter, memberInfo);

        if (memberInfo is PropertyInfo propertyInfo && propertyInfo.MemberType == MemberTypes.Property && propertyInfo.PropertyType.IsArray)
        {
            UnaryExpression listToArrayConversion = Expression.Convert(listParameter, propertyInfo.PropertyType);
            BinaryExpression arrayAssignment = Expression.Assign(memberAccess, listToArrayConversion);
            return Expression.Lambda<Action<TEntity, IList<TItem>>>(arrayAssignment, entityParameter, listParameter).Compile();
        }

        BinaryExpression assignment = Expression.Assign(memberAccess, listParameter);
        return Expression.Lambda<Action<TEntity, IList<TItem>>>(assignment, entityParameter, listParameter).Compile();
    }

    private static int CompareSchemaItems<TEntity>(ISchemaItem<TEntity> a, ISchemaItem<TEntity> b) where TEntity: class, new()
    {
        int comparison = a.Key.CompareTo(b.Key);

        if (comparison == 0)
        {
            comparison = string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        return comparison;
    }

    private static bool PropertyCanBeWritten(PropertyInfo memberInfo)
    {
        return memberInfo.GetSetMethod() != null;
    }

    private static bool PropertyHasSerializeAttribute(PropertyInfo memberInfo)
    {
        return MemberHasSerializeAttribute(memberInfo);
    }

    private static bool FieldCanBeWritten(FieldInfo fieldInfo)
    {
        return !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
    }
    
    private static bool FieldHasSerializeAttribute(FieldInfo fieldInfo)
    {
        return MemberHasSerializeAttribute(fieldInfo);
    }
    
    private static bool MemberHasSerializeAttribute(MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<SerializeAttribute>() != null;
    }

    private static bool IsCollectionType(Type type)
    {
        return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) || type.IsArray;
    }

    #endregion
}