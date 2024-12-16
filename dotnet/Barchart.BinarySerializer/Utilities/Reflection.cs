#region Using Statements

using System.Linq.Expressions;
using System.Reflection;

using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Utilities;

internal static class Reflection
{
    #region Methods
    
    internal static IEnumerable<MemberInfo> GetMembersForType(Type entityType, bool keysOnly = false)
    {
        IEnumerable<MemberInfo> fields = entityType.GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(FieldCanBeWritten);

        IEnumerable<MemberInfo> properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(PropertyCanBeWritten);

        IEnumerable<MemberInfo> members = fields.Concat(properties)
            .Where(MemberIsSerializable);

        if (keysOnly)
        {
            members = members.Where(MemberIsKey);
        }
        
        return members;
    }
    
    internal static Type GetMemberType(MemberInfo memberInfo)
    {
        if (memberInfo is PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }

        if (memberInfo is FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType;
        }
        
        throw new ArgumentException("Unable to extract member type, only properties and fields are supported.");
    }

    internal static Expression GetMemberAccessExpression(MemberInfo memberInfo, Expression typeParameterExpression)
    {
        if (memberInfo.MemberType == MemberTypes.Property)
        {
            return Expression.Property(typeParameterExpression, (PropertyInfo)memberInfo);
        }

        if (memberInfo.MemberType == MemberTypes.Field)
        {
            return Expression.Field(typeParameterExpression, (FieldInfo)memberInfo);
        }
        
        throw new ArgumentException("Unable to extract member access expression, only properties and fields are supported.");
    }

    internal static ConstructorInfo GetTupleConstructor(Type[] types)
    {
        string name = $"System.Tuple`{types.Length}";
        
        Type? type = Type.GetType(name);

        if (type == null)
        {
            throw new InvalidOperationException($"The [ {name} ] type does not exist.");
        }
        
        ConstructorInfo? constructor = type.MakeGenericType(types).GetConstructor(types);

        if (constructor == null)
        {
            throw new NullReferenceException($"The constructor for [ {name} ] could not be found. This should not be possible.");
        }
        
        return constructor;
    }
    
    private static bool MemberIsKey(MemberInfo memberInfo)
    {
        SerializeAttribute? attribute = memberInfo.GetCustomAttribute<SerializeAttribute>();

        return attribute is { Key: true };
    }
    
    private static bool MemberIsSerializable(MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<SerializeAttribute>() != null;
    }
    
    private static bool FieldCanBeWritten(FieldInfo fieldInfo)
    {
        return fieldInfo is { IsInitOnly: false, IsLiteral: false };
    }
    
    private static bool PropertyCanBeWritten(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetSetMethod() != null;
    }
    
    #endregion
}