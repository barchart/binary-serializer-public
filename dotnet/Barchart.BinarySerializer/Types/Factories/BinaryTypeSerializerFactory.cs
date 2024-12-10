#region Using Statements

using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

/// <inheritdoc />
public class BinaryTypeSerializerFactory : IBinaryTypeSerializerFactory
{
    #region Fields
    
    private readonly IDictionary<Type, IBinaryTypeSerializer> _serializers = new Dictionary<Type, IBinaryTypeSerializer>();
    
    #endregion

    #region Constructor(s)
    
    public BinaryTypeSerializerFactory()
    {
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
    }

    #endregion
    
    #region Methods

    /// <inheritdoc />
    public virtual bool Supports(Type type)
    {
        return _serializers.ContainsKey(type) || IsSupportedEnum(type) || IsSupportedNullableEnum(type);
    }
    
    /// <inheritdoc />
    public virtual bool Supports<T>()
    {
        return Supports(typeof(T));
    }

    /// <inheritdoc />
    public virtual IBinaryTypeSerializer<T> Make<T>()
    {
        if (_serializers.TryGetValue(typeof(T), out IBinaryTypeSerializer? serializer))
        {
            return (IBinaryTypeSerializer<T>)serializer;
        }

        return (IBinaryTypeSerializer<T>)CreateSerializer(typeof(T));
    }
    
    private void AddSerializer<T>(IBinaryTypeSerializer<T> serializer)
    {
        _serializers[typeof(T)] = serializer;
    }
    
    private void AddStructSerializer<T>(IBinaryTypeSerializer<T> serializer) where T: struct
    {
        AddSerializer(serializer);
        AddSerializer(new BinarySerializerNullable<T>(serializer));
    }

    private IBinaryTypeSerializer CreateSerializer(Type type)
    {
        if (IsSupportedEnum(type))
        {
            return CreateEnumSerializer(type);
        }

        if (IsSupportedNullableEnum(type))
        {
            return CreateNullableEnumSerializer(type);
        }

        throw new UnsupportedTypeException(type);
    }
    
    private IBinaryTypeSerializer CreateEnumSerializer(Type enumType)
    {
        Type underlyingType = Enum.GetUnderlyingType(enumType);
        
        if (underlyingType == typeof(byte))
        {
            Type serializerType = typeof(BinarySerializerEnumByte<>).MakeGenericType(enumType);
            
            return (IBinaryTypeSerializer)Activator.CreateInstance(serializerType, _serializers[typeof(byte)])!;
        }
        
        if (underlyingType == typeof(int))
        {
            Type serializerType = typeof(BinarySerializerEnum<>).MakeGenericType(enumType);
            
            return (IBinaryTypeSerializer)Activator.CreateInstance(serializerType, _serializers[typeof(int)])!;
        }

        throw new UnsupportedTypeException(enumType);
    }

    private IBinaryTypeSerializer CreateNullableEnumSerializer(Type nullableEnumType)
    {
        Type underlyingType = Nullable.GetUnderlyingType(nullableEnumType)!;
        IBinaryTypeSerializer underlyingSerializer = CreateSerializer(underlyingType);

        Type serializerType = typeof(BinarySerializerNullable<>).MakeGenericType(underlyingType);
        return (IBinaryTypeSerializer)Activator.CreateInstance(serializerType, underlyingSerializer)!;
    }

    private static bool IsSupportedEnum(Type type)
    {
        if (!type.IsEnum)
        {
            return false;
        }
        
        Type underlyingType = Enum.GetUnderlyingType(type);
        
        return underlyingType == typeof(byte) || underlyingType == typeof(int);
    }
    
    private static bool IsSupportedNullableEnum(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSupportedEnum(type.GetGenericArguments()[0]);
    }

    #endregion
}