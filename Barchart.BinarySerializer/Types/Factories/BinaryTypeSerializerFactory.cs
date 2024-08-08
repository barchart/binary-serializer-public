#region Using Statements

using System.Reflection;
using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

/// <inheritdoc />
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
        AddEnumSerializer((BinarySerializerInt)_serializers[typeof(int)]);
    }
    
    public BinaryTypeSerializerFactory()
    {
        
    }

    #endregion
    
    #region Methods

    /// <inheritdoc />
    public virtual bool Supports(Type type)
    {
        return _serializers.ContainsKey(type) || type.IsEnum || IsNullableEnum(type);
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

        IBinaryTypeSerializer serializer;
        
        if (_serializers.ContainsKey(type))
        {
            serializer = _serializers[typeof(T)];
        } 
        else if (type.IsEnum)
        {
            Type genericType = typeof(BinarySerializerEnum<>);
            Type boundType = genericType.MakeGenericType(new [] { type });
            
            serializer = (IBinaryTypeSerializer)Activator.CreateInstance(boundType, _serializers[typeof(int)])!;
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
            else
            {
                throw new UnsupportedTypeException(type);
            }
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

    private static void AddEnumSerializer(BinarySerializerInt serializer)
    {
        AddSerializer(serializer);
        AddSerializer(new BinarySerializerNullable<int>(serializer));
    }

    private static bool IsNullableEnum(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            Type genericArgument = type.GetGenericArguments()[0];
            if (genericArgument.IsEnum)
            {
                return true;
            }
        }

        return false;
    }

    private IBinaryTypeSerializer Make(Type type)
    {
        MethodInfo method = typeof(BinaryTypeSerializerFactory).GetMethod(nameof(Make))!.MakeGenericMethod(type);
        return (IBinaryTypeSerializer)method.Invoke(this, null)!;
    }
    
    #endregion
}