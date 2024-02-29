using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(BufferHelper bufferHelper, T? value);

        public new HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBits(T? value);
    }

    internal interface ISerializer
    {
        public void Encode(BufferHelper bufferHelper,object? value);

        public HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBits(object? value);
    }
}