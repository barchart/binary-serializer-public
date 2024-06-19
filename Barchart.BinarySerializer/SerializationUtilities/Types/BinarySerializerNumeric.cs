
#region Using Statements

using Barchart.BinarySerializer.SerializationUtilities.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.SerializationUtilities.Types
{
    /// <summary>
    ///     Represents a base class for binary serializers handling numeric types.
    /// </summary>
    /// <typeparam name="TMember">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNumeric<TMember> : IBinaryTypeSerializer<TMember> where TMember : struct
    {
        #region Properties

        public abstract int Size { get; }

        #endregion
        
        #region Methods

        /// <inheritdoc />
        public void Encode(DataBuffer dataBuffer, TMember value)
        {
            Header header = new() { IsMissing = false, IsNull = false };
            
            header.WriteToBuffer(dataBuffer);

            byte[] valueBytes = ConvertToByteArray(value);
            dataBuffer.WriteValueBytes(valueBytes);
        }

        /// <inheritdoc />
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TMember>(header, default);
            }

            byte[] valueBytes = dataBuffer.ReadValueBytes(Size);

            return new HeaderWithValue<TMember>(header, DecodeBytes(valueBytes));
        }

        /// <inheritdoc />
        public int GetLengthInBits(TMember value)
        {
            return Size * 8 + DataBuffer.NumberOfHeaderBitsNonString;
        }

        protected abstract byte[] ConvertToByteArray(TMember value);
        protected abstract TMember DecodeBytes(byte[] bytes);

        #endregion
    }
}