#region Using Statements

using System.Reflection;
using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

/// <inheritdoc />
public class BinaryTypeSerializerFactory : IBinaryTypeSerializerFactory
{
    #region Fields

    private static readonly IDictionary<Type, IBinaryTypeSerializer> Serializers;

    #endregion

    #region Constructor(s)
    
    static BinaryTypeSerializerFactory()
    {
        Serializers = new Dictionary<Type, IBinaryTypeSerializer>();
        
        AddStructSerializer(new BinarySerializerBool());
        AddStructSerializer(new BinarySerializerByte());
        AddStructSerializer(new BinarySerializerChar());
        AddStructSerializer(new BinarySerializerDateOnly());
        AddStructSerializer(new BinarySerializerDateTime());
        AddStructSerializer(new BinarySerializerDecimal());
        AddStructSerializer(new BinarySerializerDouble());
        AddStructSerializer(new BinarySerializerFloat());
        AddStructSerializer(new BinarySerializerInt());
        AddStructSerializer(new BinarySerializerLong());
        AddStructSerializer(new BinarySerializerSByte());
        AddStructSerializer(new BinarySerializerShort());
        AddStructSerializer(new BinarySerializerUInt());
        AddStructSerializer(new BinarySerializerULong());
        AddStructSerializer(new BinarySerializerUShort());
        
        AddSerializer(new BinarySerializerString());
        AddEnumSerializer((BinarySerializerInt)Serializers[typeof(int)]);
    }

    #endregion
    
    #region Methods

    /// <inheritdoc />
    public virtual bool Supports(Type type)
    {
        return Serializers.ContainsKey(type) || type.IsEnum || IsNullableEnum(type);
    }
    
    /// <inheritdoc />
    public virtual bool Supports<T>()
    {
        return Supports(typeof(T));
    }
    
    /// <inheritdoc />
    public virtual IBinaryTypeSerializer<T> Make<T>()
    {
        Type type = typeof(T);

        IBinaryTypeSerializer? serializer = null;
        
        if (Serializers.ContainsKey(type))
        {
            serializer = Serializers[typeof(T)];
        } 
        else if (type.IsEnum)
        {
            Type genericType = typeof(BinarySerializerEnum<>);
            Type boundType = genericType.MakeGenericType(type);
            
            serializer = (IBinaryTypeSerializer)Activator.CreateInstance(boundType, Serializers[typeof(int)])!;
        }
        else if (IsNullableEnum(type))
        {
            Type underlyingType = Nullable.GetUnderlyingType(type)!;

            if (Supports(underlyingType))
            {
                Type genericType = typeof(BinarySerializerNullable<>);
                Type boundType = genericType.MakeGenericType(underlyingType);
                
                IBinaryTypeSerializer underlyingSerializer = Make(underlyingType);
                
                serializer = (IBinaryTypeSerializer)Activator.CreateInstance(boundType, underlyingSerializer)!;
            }
        }
        
        if (serializer == null)
        {
            throw new UnsupportedTypeException(type);
        }
        
        return (IBinaryTypeSerializer<T>)serializer;
    }

    private static void AddSerializer<T>(IBinaryTypeSerializer<T> serializer)
    {
        Serializers[typeof(T)] = serializer;
    }
    
    private static void AddStructSerializer<T>(IBinaryTypeSerializer<T> serializer) where T: struct
    {
        AddSerializer(serializer);
        AddSerializer(new BinarySerializerNullable<T>(serializer));
    }

    private static void AddEnumSerializer(BinarySerializerInt serializer)
    {
        AddSerializer(serializer);
        AddSerializer(new BinarySerializerNullable<int>(serializer));
    }

    private static bool IsNullableEnum(Type type)
    {
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
        {
            return false;
        }
        
        Type genericArgument = type.GetGenericArguments()[0];
        return genericArgument.IsEnum;
    }

    private IBinaryTypeSerializer Make(Type type)
    {
        MethodInfo method = typeof(BinaryTypeSerializerFactory).GetMethod(nameof(Make))!.MakeGenericMethod(type);
        return (IBinaryTypeSerializer)method.Invoke(this, null)!;
    }
    
    #endregion
}