using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface ISerializer
    {
        public void Encode(BufferHelper bufferHelper,object? value);

        public HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBits(object? value);
    }
}