namespace Barchart.BinarySerializer.Types.Factories;

public class BinaryTypeSerializerFactory : IBinaryTypeSerializerFactory
{
    private static readonly IDictionary<Type, IBinaryTypeSerializer> _serializers;
    
    static BinaryTypeSerializerFactory()
    {
        _serializers = new Dictionary<Type, IBinaryTypeSerializer>();
        
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
        AddStructSerializer(new BinarySerializerSbyte());
        AddStructSerializer(new BinarySerializerShort());
        AddStructSerializer(new BinarySerializerUInt());
        AddStructSerializer(new BinarySerializerULong());
        AddStructSerializer(new BinarySerializerUShort());
        
        AddSerializer(new BinarySerializerString());
    }
    
    public BinaryTypeSerializerFactory()
    {
        
    }
    
    public virtual IBinaryTypeSerializer<T> Make<T>()
    {
        IBinaryTypeSerializer serializer = _serializers[typeof(T)];

        return (IBinaryTypeSerializer<T>)serializer;
    }

    private static void AddSerializer<T>(IBinaryTypeSerializer<T> serializer)
    {
        _serializers[typeof(T)] = serializer;
    }
    
    private static void AddStructSerializer<T>(IBinaryTypeSerializer<T> serializer) where T: struct
    {
        AddSerializer(serializer);
        AddSerializer(new BinarySerializerNullable<T>(serializer));
    }
}