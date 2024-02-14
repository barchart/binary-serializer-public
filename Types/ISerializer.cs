namespace JerqAggregatorNew.Types
{
    public interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(byte[] buffer, T? value, ref int offset, ref int offsetInLastByte);
        public new HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte);
        public int GetLengthInBytes(T? value);
    }

    public interface ISerializer
    {
        public void Encode(byte[] buffer, object? value, ref int offset, ref int offsetInLastByte);
        public HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte);
        public int GetLengthInBytes(object? value);
    }
}