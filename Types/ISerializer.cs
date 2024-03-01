using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    internal interface ISerializer
    {
        public void Encode(DataBuffer dataBuffer, object? value);
        public HeaderWithValue Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(object? value);
    }
}