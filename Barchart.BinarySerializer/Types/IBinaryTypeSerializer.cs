using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(DataBuffer dataBuffer, T? value);
        public new HeaderWithValue Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(T? value);
    }
}