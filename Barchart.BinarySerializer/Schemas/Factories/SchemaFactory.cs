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
    private readonly IBinaryTypeSerializerFactory _binaryTypeSerializerFactory;
    
    public SchemaFactory(IBinaryTypeSerializerFactory binarySerializerFactory)
    {
        _binaryTypeSerializerFactory = binarySerializerFactory;
    }
    
    /// <inheritdoc />
    public ISchema<TEntity> Make<TEntity>() where TEntity: new()
    {
        Type entityType = typeof(TEntity);

        IEnumerable<FieldInfo> fields = entityType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(FieldHasSerializeAttribute)
            .Where(FieldCanBeWritten);
        
        IEnumerable<PropertyInfo> properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(PropertyHasSerializeAttribute)
            .Where(PropertyCanBeWritten);

        ISchemaItem<TEntity>[] schemaItems = properties.Select(MakeSchemaItem<TEntity>).ToArray();

        Array.Sort(schemaItems, CompareSchemaItems);
        
        return new Schema<TEntity>(schemaItems);
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity>(PropertyInfo propertyInfo) where TEntity: new()
    {
        Type[] typeParameters = { typeof(TEntity), propertyInfo.PropertyType };

        MethodInfo[] methods = typeof(SchemaFactory).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

        MethodInfo unboundMethod = methods.Single(GetMakeSchemaItemPredicate(typeParameters));
        MethodInfo boundMethod = unboundMethod.MakeGenericMethod(typeParameters);

        object[] methodParameters = { propertyInfo };
        
        object? schemaItem = boundMethod.Invoke(this, methodParameters);
        
        if (schemaItem == null)
        {
            throw new NullReferenceException("When called via reflection, the SchemaFactory.MakeSchemaItem<TEntity, TProperty> returned a null reference. This should not be possible.");
        }
        
        return (ISchemaItem<TEntity>)schemaItem;
    }
    
    private ISchemaItem<TEntity> MakeSchemaItem<TEntity, TProperty>(PropertyInfo propertyInfo) where TEntity: new()
    {
        Func<TEntity, TProperty> getter = MakePropertyGetter<TEntity, TProperty>(propertyInfo);
        Action<TEntity, TProperty> setter = MakePropertySetter<TEntity, TProperty>(propertyInfo);

        IBinaryTypeSerializer<TProperty> serializer = _binaryTypeSerializerFactory.Make<TProperty>();
        
        SerializeAttribute attribute = GetSerializeAttribute(propertyInfo);
        
        String name = propertyInfo.Name;
        bool key = attribute.Key;
        
        return new SchemaItem<TEntity, TProperty>(name, key, getter, setter, serializer);
    }
    
    private Func<TEntity, TProperty> MakePropertyGetter<TEntity, TProperty>(MemberInfo propertyInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], propertyInfo);
        
        return Expression.Lambda<Func<TEntity, TProperty>>(propertyAccessExpression, typeParameterExpressions).Compile();
    }
    
    private Action<TEntity, TProperty> MakePropertySetter<TEntity, TProperty>(PropertyInfo propertyInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity)),
            Expression.Parameter(typeof(TProperty))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], propertyInfo);
        BinaryExpression propertyAssignmentExpression = Expression.Assign(propertyAccessExpression, typeParameterExpressions[1]);

        return Expression.Lambda<Action<TEntity, TProperty>>(propertyAssignmentExpression, typeParameterExpressions).Compile();
    }
    
    private static bool PropertyCanBeWritten(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetSetMethod() != null;
    }

    private static bool PropertyHasSerializeAttribute(PropertyInfo propertyInfo)
    {
        return MemberHasSerializeAttribute(propertyInfo);
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

    private static int CompareSchemaItems<TEntity>(ISchemaItem<TEntity> a, ISchemaItem<TEntity> b) where TEntity: new()
    {
        int comparison = a.Key.CompareTo(b.Key);

        if (comparison == 0)
        {
            comparison = String.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        return comparison;
    }
}