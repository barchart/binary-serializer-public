using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    internal interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(BufferHelper bufferHelper, T? value);

        public new HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBytes(T? value);
    }

    internal interface ISerializer
    {
        public void Encode(BufferHelper bufferHelper,object? value);

        public HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBytes(object? value);
    }
}