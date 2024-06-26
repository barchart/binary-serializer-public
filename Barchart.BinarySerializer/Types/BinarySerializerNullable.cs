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

        #region Constructor(s)

        public BinarySerializerNullable(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, T? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (T)value);
            }
            else
            {
                Header.WriteToBuffer(dataBuffer, false, true);
            }
        }

        /// <inheritdoc />
        public Attribute<T?> Decode(IDataBufferReader dataBuffer)
        {
            Attribute<T> attribute = _serializer.Decode(dataBuffer);

            return new Attribute<T?>(attribute.IsValueMissing, attribute.Value);
        }

        /// <inheritdoc />
        public int GetLengthInBits(T? value)
        {
            return value == null ? Header.NumberOfHeaderBitsNonString : _serializer.GetLengthInBits((T)value);
        }

        #endregion
    }
}