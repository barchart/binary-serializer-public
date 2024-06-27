#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) enumeration values to (and from) a binary data source.
    /// </summary>
    /// <typeparam name="T">
    ///     The enumeration type.
    /// </typeparam>
    public class BinarySerializerEnum<T> : IBinaryTypeSerializer<T> where T : Enum
    {
        #region Fields

        private readonly BinarySerializerInt _binarySerializerInt;

        #endregion

        #region Constructor(s)

        public BinarySerializerEnum(BinarySerializerInt binarySerializerInt)
        {
            _binarySerializerInt = binarySerializerInt;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, T value)
        {
            _binarySerializerInt.Encode(dataBuffer, Convert.ToInt32(value));
        }

        /// <inheritdoc />
        public T Decode(IDataBufferReader dataBuffer)
        {
            return (T)Enum.Parse(typeof(T), _binarySerializerInt.Decode(dataBuffer).ToString(), true);
        }

        public bool GetEquals(T a, T b)
        {
            return _binarySerializerInt.GetEquals(Convert.ToInt32(a), Convert.ToInt32(b));
        }
        
        /// <inheritdoc />
        public int GetLengthInBits(T value)
        {
            return _binarySerializerInt.GetLengthInBits(Convert.ToInt32(value));
        }

        #endregion
    }
}