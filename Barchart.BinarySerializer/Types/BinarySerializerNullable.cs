#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for value types (that are nullable).
    /// </summary>
    /// <typeparam name="T">
    ///     The value type to serialize.
    /// </typeparam>
    public class BinarySerializerNullable<T> : IBinaryTypeSerializer<T?> where T : struct
	{
        #region Fields

        private readonly IBinaryTypeSerializer<T> _typeSerializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerNullable(IBinaryTypeSerializer<T> typeSerializer)
        {
            _typeSerializer = typeSerializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, T? value)
        {
            dataBuffer.WriteBit(!value.HasValue);
            
            if (value.HasValue)
            {
                _typeSerializer.Encode(dataBuffer, value.Value);
            }
        }

        /// <inheritdoc />
        public T? Decode(IDataBufferReader dataBuffer)
        {
            if (dataBuffer.ReadBit())
            {
                return null;
            }

            return _typeSerializer.Decode(dataBuffer);
        }

        /// <inheritdoc />
        public int GetLengthInBits(T? value)
        {
            int length = 1;
            
            if (value.HasValue)
            {
                length += _typeSerializer.GetLengthInBits(value.Value);
            }

            return length;
        }

        #endregion
    }
}