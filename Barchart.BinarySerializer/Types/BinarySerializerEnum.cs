using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides binary serialization functionality for enum types.
    /// This class implements the IBinaryTypeSerializer interface for enum types.
    /// </summary>
    /// <typeparam name="TMember">The enum type to be serialized.</typeparam>
    public class BinarySerializerEnum<TMember> : IBinaryTypeSerializer<TMember> where TMember : Enum
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

        public void Encode(DataBuffer dataBuffer, TMember? value)
        {
            int? integerValue = value != null? Convert.ToInt32(value) : null;

            if (integerValue != null)
            {
                _serializer.Encode(dataBuffer, (int)integerValue);
            }
        }

        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<int> headerWithValue = _serializer.Decode(dataBuffer);
            int value = headerWithValue.Value;

            return new HeaderWithValue<TMember>(headerWithValue.Header, (TMember?)Enum.Parse(typeof(TMember), value.ToString(), true));
        }

        public int GetLengthInBits(TMember? value)
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