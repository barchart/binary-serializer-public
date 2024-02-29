using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface ISerializer
    {
        public void Encode(DataBuffer bufferHelper,object? value);
        public HeaderWithValue Decode(DataBuffer bufferHelper);
        public int GetLengthInBits(object? value);
    }
}