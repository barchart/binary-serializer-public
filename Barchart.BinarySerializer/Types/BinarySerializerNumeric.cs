
#region Using Statements

using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Types
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
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = false });

            EncodeValue(dataBuffer, value);
        }

        /// <inheritdoc />
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TMember>(header, default);
            }

            TMember decodedValue;
            
            if (typeof(TMember) == typeof(bool))
            {
                decodedValue = (TMember)(object)dataBuffer.ReadBit();
            }
            else
            {
                byte[] valueBytes = dataBuffer.ReadBytes(Size);
                decodedValue = DecodeBytes(valueBytes);
            }


            return new HeaderWithValue<TMember>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TMember value)
        {
            return Size * 8 + DataBuffer.NumberOfHeaderBitsNonString;
        }

        protected abstract void EncodeValue(DataBuffer dataBuffer, TMember value);
        protected abstract TMember DecodeBytes(byte[] bytes);

        #endregion
    }
}