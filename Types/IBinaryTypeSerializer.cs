using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(DataBuffer bufferHelper, T? value);
        public new HeaderWithValue Decode(DataBuffer bufferHelper);
        public int GetLengthInBits(T? value);
    }
}

