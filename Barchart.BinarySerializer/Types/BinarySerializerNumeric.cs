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
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = false });

            EncodeValue(dataBuffer, value);
        }

        /// <inheritdoc />
        public Attribute<T> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<T>(header, default);
            }
            
            byte[] valueBytes = dataBuffer.ReadBytes(Size);
            T decodedValue = DecodeBytes(valueBytes);
                
            return new Attribute<T>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(T value)
        {
            return Size * 8 + Header.NumberOfHeaderBitsNonString;
        }

        protected abstract void EncodeValue(IDataBuffer dataBuffer, T value);
        protected abstract T DecodeBytes(byte[] bytes);

        #endregion
    }
}