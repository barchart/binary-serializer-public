#region Using Statements

using Barchart.BinarySerializer.SerializationUtilities.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.SerializationUtilities.Types
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
        public void Encode(DataBuffer dataBuffer, TMember? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (TMember)value);
            }
            else
            {
                Header header = new() { IsMissing = false, IsNull = true };
                
                header.WriteToBuffer(dataBuffer);
            }
        }

        /// <inheritdoc />
        public HeaderWithValue<TMember?> Decode(DataBuffer dataBuffer)
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