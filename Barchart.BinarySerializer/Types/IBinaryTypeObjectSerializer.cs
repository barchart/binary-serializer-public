using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides additional methods for encoding and decoding values of type <typeparamref name="T"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeObjectSerializer<T> : IBinaryTypeSerializer<T>
    {
        public void Encode(DataBuffer dataBuffer, T? oldValue, T? newValue);
        public HeaderWithValue<T> Decode(DataBuffer dataBuffer, T existing);
    }
}