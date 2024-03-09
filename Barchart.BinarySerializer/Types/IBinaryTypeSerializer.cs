using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides methods for encoding and decoding values of type <typeparamref name="T"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeSerializer<T>
    {
        public void Encode(DataBuffer dataBuffer, T? value);
        public HeaderWithValue<T> Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(T? value);
    }
}