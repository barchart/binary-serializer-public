using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public interface IBinaryTypeSerializer<T>
    {
        public void Encode(DataBuffer dataBuffer, T? value);
        public HeaderWithValue<T> Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(T? value);
    }
}