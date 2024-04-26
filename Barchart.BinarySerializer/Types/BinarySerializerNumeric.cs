using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a base class for binary serializers handling numeric types.
    /// </summary>
    /// <typeparam name="TMember">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNumeric<TMember> : IBinaryTypeSerializer<TMember> where TMember : struct
    {
        public abstract int Size { get; }

        public void Encode(DataBuffer dataBuffer, TMember value)
        {
            Header header = new() { IsMissing = false, IsNull = false };
            Utility.UtilityKit.WriteHeader(dataBuffer, header);

            byte[] valueBytes = ConvertToByteArray(value);
            Utility.UtilityKit.WriteValueBytes(dataBuffer, valueBytes);
        }

        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer)
        {
            Header header = Utility.UtilityKit.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<TMember>(header, default);
            }

            byte[] valueBytes = Utility.UtilityKit.ReadValueBytes(dataBuffer, Size);

            return new HeaderWithValue<TMember>(header, DecodeBytes(valueBytes));
        }

        public int GetLengthInBits(TMember value)
        {
            return Size * 8 + Utility.UtilityKit.NumberOfHeaderBitsNonString;
        }

        protected abstract byte[] ConvertToByteArray(TMember value);
        protected abstract TMember DecodeBytes(byte[] bytes);
    }
}