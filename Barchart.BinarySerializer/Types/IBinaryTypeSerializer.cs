using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides methods for encoding and decoding values of type <typeparamref name="TContainer"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeSerializer<TContainer>
    {
        public void Encode(DataBuffer dataBuffer, TContainer value);
        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(TContainer value);
    }
}