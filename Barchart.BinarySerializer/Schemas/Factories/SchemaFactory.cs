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
    
    #endregion
    
    #region Methods

    /// <inheritdoc />
    public ISchema<TEntity> Make<TEntity>() where TEntity: class, new()
    {
        Type entityType = typeof(TEntity);

        IEnumerable<MemberInfo> members = GetMembersForType(entityType);

        ISchemaItem<TEntity>[] schemaItems = members.Select(MakeSchemaItem<TEntity>)
            .ToArray();

        Array.Sort(schemaItems, CompareSchemaItems);
        
        return new Schema<TEntity>(schemaItems);
    }

    private ISchemaItem<TEntity> MakeSchemaItem<TEntity>(MemberInfo memberInfo) where TEntity: class, new()
    {
        Type memberType = GetMemberType(memberInfo);
        Type[] typeParameters = new Type[] { typeof(TEntity), GetMemberType(memberInfo) };

        MethodInfo[] methods = typeof(SchemaFactory).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        
        MethodInfo unboundMethod;
        MethodInfo boundMethod;
        
        if (IsPrimitiveOrBuiltInType(memberType))
        {
            unboundMethod = methods.Single(GetMakeSchemaItemPredicate(typeParameters));
            boundMethod = unboundMethod.MakeGenericMethod(typeParameters);
        }
        else
        {
            unboundMethod = methods.Single(GetMakeSchemaItemNestedPredicate(typeParameters));
            boundMethod = unboundMethod.MakeGenericMethod(typeParameters);
        }

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

    private static Func<TEntity, TMember> MakeMemberGetter<TEntity, TMember>(MemberInfo memberInfo)
    {
        ParameterExpression[] typeParameterExpressions = {
            Expression.Parameter(typeof(TEntity))
        };
        
        MemberExpression propertyAccessExpression = Expression.MakeMemberAccess(typeParameterExpressions[0], memberInfo);
        
        return Expression.Lambda<Func<TEntity, TMember>>(propertyAccessExpression, typeParameterExpressions).Compile();
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

    private static Func<MethodInfo, bool> GetMakeSchemaItemNestedPredicate(Type[] typeParameters)
    {
        return methodInfo => methodInfo.Name == nameof(MakeSchemaItemNested) && methodInfo.GetGenericArguments().Length == typeParameters.Length;
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

    private static Type GetMemberType(MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            PropertyInfo pi => pi.PropertyType,
            FieldInfo fi => fi.FieldType,
            _ => throw new ArgumentException("Unsupported member type")
        };
    }

    private static bool IsPrimitiveOrBuiltInType(Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsEnum;
    }

    private static IEnumerable<MemberInfo> GetMembersForType(Type entityType)
    {
        IEnumerable<MemberInfo> fields = entityType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(FieldHasSerializeAttribute)
            .Where(FieldCanBeWritten)
            .Cast<MemberInfo>();

        IEnumerable<MemberInfo> properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(PropertyHasSerializeAttribute)
            .Where(PropertyCanBeWritten)
            .Cast<MemberInfo>();

        return fields.Concat(properties);
    }

    #endregion
}