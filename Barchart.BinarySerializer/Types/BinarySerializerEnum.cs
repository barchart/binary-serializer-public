#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for enum types.
    /// </summary>
    /// <typeparam name="T">The enum type to be serialized.</typeparam>
    public class BinarySerializerEnum<T> : IBinaryTypeSerializer<T> where T : Enum
    {
        #region Fields

        private readonly BinarySerializerInt _serializer;

        #endregion

        #region  Constructor(s)

        public BinarySerializerEnum(BinarySerializerInt serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, T? value)
        {
            int? integerValue = value != null? Convert.ToInt32(value) : null;

            if (integerValue != null)
            {
                _serializer.Encode(dataBuffer, (int)integerValue);
            }
        }

        /// <inheritdoc />
        public Attribute<T> Decode(IDataBufferReader dataBuffer)
        {
            Attribute<int> attribute = _serializer.Decode(dataBuffer);
            int value = attribute.Value;

            return new Attribute<T>(attribute.IsValueMissing, (T)Enum.Parse(typeof(T), value.ToString(), true));
        }

        /// <inheritdoc />
        public int GetLengthInBits(T? value)
        {
            int? integerValue = value != null ? Convert.ToInt32(value) : null;

            if (integerValue != null)
            {
                return _serializer.GetLengthInBits((int)integerValue);

            }
            return 0;
        }

        #endregion
    }
}