using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides additional methods for encoding and decoding values of type <typeparamref name="TContainer"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeObjectSerializer<TContainer> : IBinaryTypeSerializer<TContainer>
    {
        public void Encode(DataBuffer dataBuffer, TContainer oldValue, TContainer newValue);
        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer, TContainer existing);
    }
}