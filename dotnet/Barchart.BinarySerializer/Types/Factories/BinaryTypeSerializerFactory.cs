#region Using Statements

using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

/// <inheritdoc />
public class BinaryTypeSerializerFactory : IBinaryTypeSerializerFactory
{
    #region Fields

    private static readonly IDictionary<Type, IBinaryTypeSerializer> Serializers = new Dictionary<Type, IBinaryTypeSerializer>();

    #endregion

    #region Constructor(s)
    
    static BinaryTypeSerializerFactory()
    {
        InitializeSerializers();
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
        if (Serializers.TryGetValue(typeof(T), out IBinaryTypeSerializer? serializer))
        {
            return (IBinaryTypeSerializer<T>)serializer;
        }

        return (IBinaryTypeSerializer<T>)CreateSerializer(typeof(T));
    }

    private static void InitializeSerializers()
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
        AddEnumSerializer((BinarySerializerInt)Serializers[typeof(int)]);
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

    private IBinaryTypeSerializer CreateSerializer(Type type)
    {
        if (type.IsEnum)
        {
            return CreateEnumSerializer(type);
        }

        if (IsNullableEnum(type))
        {
            return CreateNullableEnumSerializer(type);
        }

        throw new UnsupportedTypeException(type);
    }

    private IBinaryTypeSerializer CreateEnumSerializer(Type enumType)
    {
        Type serializerType = typeof(BinarySerializerEnum<>).MakeGenericType(enumType);
        return (IBinaryTypeSerializer)Activator.CreateInstance(serializerType, Serializers[typeof(int)])!;
    }

    private IBinaryTypeSerializer CreateNullableEnumSerializer(Type nullableEnumType)
    {
        Type underlyingType = Nullable.GetUnderlyingType(nullableEnumType)!;
        IBinaryTypeSerializer underlyingSerializer = CreateSerializer(underlyingType);

        Type serializerType = typeof(BinarySerializerNullable<>).MakeGenericType(underlyingType);
        return (IBinaryTypeSerializer)Activator.CreateInstance(serializerType, underlyingSerializer)!;
    }

    private static bool IsNullableEnum(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum;
    }

    #endregion
}