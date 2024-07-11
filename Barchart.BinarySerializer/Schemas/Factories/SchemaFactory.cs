﻿#region Using Statements

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
    
    #endregion
    
    #region Methods

    /// <inheritdoc />
    public ISchema<TEntity> Make<TEntity>() where TEntity: class, new()
    {
        Type entityType = typeof(TEntity);

        IEnumerable<FieldInfo> fields = entityType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(FieldHasSerializeAttribute)
            .Where(FieldCanBeWritten);
        
        IEnumerable<PropertyInfo> properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(PropertyHasSerializeAttribute)
            .Where(PropertyCanBeWritten);

        ISchemaItem<TEntity>[] schemaItems = fields.Select(MakeSchemaItem<TEntity>)
            .Concat(properties.Select(MakeSchemaItem<TEntity>))
            .ToArray();

        Array.Sort(schemaItems, CompareSchemaItems);
        
        return new Schema<TEntity>(schemaItems);
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Type[] typeParameters;

        if (memberInfo is PropertyInfo propertyInfo)
        {
            typeParameters = new Type[] { typeof(TEntity), propertyInfo.PropertyType };
        }
        else if (memberInfo is FieldInfo fieldInfo)
        {
            typeParameters = new Type[] { typeof(TEntity), fieldInfo.FieldType };
        }
        else 
        {
            throw new Exception("Unsupported member type encountered. Only PropertyInfo and FieldInfo are supported.");
        }

        MethodInfo[] methods = typeof(SchemaFactory).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

        MethodInfo unboundMethod = methods.Single(GetMakeSchemaItemPredicate(typeParameters));
        MethodInfo boundMethod = unboundMethod.MakeGenericMethod(typeParameters);

        object[] methodParameters = { memberInfo };
        
        object? schemaItem = boundMethod.Invoke(this, methodParameters);
        
        if (schemaItem == null)
        {
            throw new NullReferenceException("When called via reflection, the SchemaFactory.MakeSchemaItem<TEntity, TProperty> returned a null reference. This should not be possible.");
        }
        
        return (ISchemaItem<TEntity>)schemaItem;
    }
    
    private ISchemaItem<TEntity> MakeSchemaItem<TEntity, TProperty>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Func<TEntity, TProperty> getter = MakeMemberGetter<TEntity, TProperty>(memberInfo);
        Action<TEntity, TProperty> setter = MakeMemberSetter<TEntity, TProperty>(memberInfo);

        IBinaryTypeSerializer<TProperty> serializer = _binaryTypeSerializerFactory.Make<TProperty>();
        
        SerializeAttribute attribute = GetSerializeAttribute(memberInfo);

        string name = memberInfo.Name;
        bool key = attribute.Key;
        
        return new SchemaItem<TEntity, TProperty>(name, key, getter, setter, serializer);
    }
    
    private static Func<TEntity, TProperty> MakeMemberGetter<TEntity, TProperty>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        
        return Expression.Lambda<Func<TEntity, TProperty>>(propertyAccessExpression, typeParameterExpressions).Compile();
    }
    
    private static Action<TEntity, TProperty> MakeMemberSetter<TEntity, TProperty>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity)),
            Expression.Parameter(typeof(TProperty))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        BinaryExpression propertyAssignmentExpression = Expression.Assign(propertyAccessExpression, typeParameterExpressions[1]);

        return Expression.Lambda<Action<TEntity, TProperty>>(propertyAssignmentExpression, typeParameterExpressions).Compile();
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

    private static SerializeAttribute GetSerializeAttribute(MemberInfo memberInfo)
    {
        SerializeAttribute? serializeAttribute = memberInfo.GetCustomAttribute<SerializeAttribute>();

        if (serializeAttribute == null)
        {
            throw new NullReferenceException("An attempt to get a SerializeAttribute returned a null reference. This should not be possible.");
        }

        return serializeAttribute;
    }

    private static Func<MethodInfo, bool> GetMakeSchemaItemPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItem) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
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

    #endregion
}