#region Using Statements

using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

public class BinaryTypeSerializerFactory : IBinaryTypeSerializerFactory
{
    #region Fields

    private static readonly IDictionary<Type, IBinaryTypeSerializer> _serializers;

    #endregion

    #region Constructor(s)
    
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
        AddStructSerializer(new BinarySerializerSByte());
        AddStructSerializer(new BinarySerializerShort());
        AddStructSerializer(new BinarySerializerUInt());
        AddStructSerializer(new BinarySerializerULong());
        AddStructSerializer(new BinarySerializerUShort());
        
        AddSerializer(new BinarySerializerString());
    }
    
    public BinaryTypeSerializerFactory()
    {
        
    }

    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public virtual IBinaryTypeSerializer<T> Make<T>()
    {
        Type type = typeof(T);

        IBinaryTypeSerializer serializer;
        
        if (_serializers.ContainsKey(type))
        {
            serializer = _serializers[typeof(T)];
        } 
        else if (type.IsEnum)
        {
            Type genericType = typeof(BinarySerializerEnum<>);
            Type boundType = genericType.MakeGenericType(new Type[] { type });
            
            serializer = (IBinaryTypeSerializer)Activator.CreateInstance(boundType, _serializers[typeof(int)])!;
        }
        else
        {
            throw new UnsupportedTypeException(type);
        }
        
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

    #endregion
}