#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) float values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerFloat : IBinaryTypeSerializer<float>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(float);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter buffer, float value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public float Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToSingle(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(float value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(float a, float b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}