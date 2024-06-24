#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for nullable value types.
    /// </summary>
    /// <typeparam name="TMember">The underlying value type of the nullable type.</typeparam>
    public class BinarySerializerNullable<TMember> : IBinaryTypeSerializer<TMember?> where TMember : struct
	{
        #region Fields

        private readonly IBinaryTypeSerializer<TMember> _serializer;

        #endregion

        #region  Constructor(s)

        public BinarySerializerNullable(IBinaryTypeSerializer<TMember> serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TMember? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (TMember)value);
            }
            else
            {
                Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = true });
            }
        }

        /// <inheritdoc />
        public HeaderWithValue<TMember?> Decode(IDataBuffer dataBuffer)
        {
            HeaderWithValue<TMember> headerWithValue = _serializer.Decode(dataBuffer);

            if (headerWithValue.Header.IsValueMissingOrNull()) 
            {
                return new HeaderWithValue<TMember?>(headerWithValue.Header, null);
            }

            return new HeaderWithValue<TMember?>(headerWithValue.Header, headerWithValue.Value);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TMember? value)
        {
            return value == null ? DataBuffer.NumberOfHeaderBitsNonString : _serializer.GetLengthInBits((TMember)value);
        }

        #endregion
    }
}