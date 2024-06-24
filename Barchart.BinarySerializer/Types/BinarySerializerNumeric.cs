#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Represents a base class for binary serializers handling numeric types.
    /// </summary>
    /// <typeparam name="T">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T> where T : struct
    {
        #region Properties

        public abstract int Size { get; }

        #endregion
        
        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, T value)
        {
            AttributeHeader.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = false });

            EncodeValue(dataBuffer, value);
        }

        /// <inheritdoc />
        public AttributeValue<T> Decode(IDataBuffer dataBuffer)
        {
            AttributeHeader header = AttributeHeader.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new AttributeValue<T>(header, default);
            }

            T decodedValue;
            
            if (typeof(T) == typeof(bool))
            {
                decodedValue = (T)(object)dataBuffer.ReadBit();
            }
            else
            {
                byte[] valueBytes = dataBuffer.ReadBytes(Size);
                decodedValue = DecodeBytes(valueBytes);
            }


            return new AttributeValue<T>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(T value)
        {
            return Size * 8 + DataBuffer.NumberOfHeaderBitsNonString;
        }

        protected abstract void EncodeValue(IDataBuffer dataBuffer, T value);
        protected abstract T DecodeBytes(byte[] bytes);

        #endregion
    }
}