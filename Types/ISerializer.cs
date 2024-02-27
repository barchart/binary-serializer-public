using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    public interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(BufferHelper bufferHelper, T? value);

        public new HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBytes(T? value);
    }

    public interface ISerializer
    {
        public void Encode(BufferHelper bufferHelper,object? value);

        public HeaderWithValue Decode(BufferHelper bufferHelper);

        public int GetLengthInBytes(object? value);
    }
}