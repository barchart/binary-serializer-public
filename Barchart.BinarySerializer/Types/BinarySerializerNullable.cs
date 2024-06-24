#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for nullable value types.
    /// </summary>
    /// <typeparam name="T">The underlying value type of the nullable type.</typeparam>
    public class BinarySerializerNullable<T> : IBinaryTypeSerializer<T?> where T : struct
	{
        #region Fields

        private readonly IBinaryTypeSerializer<T> _serializer;

        #endregion

        #region  Constructor(s)

        public BinarySerializerNullable(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, T? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (T)value);
            }
            else
            {
                AttributeHeader.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = true });
            }
        }

        /// <inheritdoc />
        public AttributeValue<T?> Decode(IDataBuffer dataBuffer)
        {
            AttributeValue<T> attributeValue = _serializer.Decode(dataBuffer);

            if (attributeValue.AttributeHeader.IsValueMissingOrNull()) 
            {
                return new AttributeValue<T?>(attributeValue.AttributeHeader, null);
            }

            return new AttributeValue<T?>(attributeValue.AttributeHeader, attributeValue.Value);
        }

        /// <inheritdoc />
        public int GetLengthInBits(T? value)
        {
            return value == null ? DataBuffer.NumberOfHeaderBitsNonString : _serializer.GetLengthInBits((T)value);
        }

        #endregion
    }
}