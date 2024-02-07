namespace JerqAggregatorNew.Types
{
    public interface IBinaryTypeSerializer<T> : ISerializer
    {
        public void Encode(List<byte> buffer, T? value, ref int offset, ref int offsetInLastByte);
        public new HeaderWithValue Decode(List<byte> buffer, ref int offset, ref int offsetInLastByte);
        public int GetLengthInBytes(T? value);
    }

    public interface ISerializer
    {
        public void Encode(List<byte> buffer, object? value, ref int offset, ref int offsetInLastByte);
        public HeaderWithValue Decode(List<byte> buffer, ref int offset, ref int offsetInLastByte);
        public int GetLengthInBytes(object? value);
    }
}
