#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt : IBinaryTypeSerializer<int>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(int);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, int value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public int Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToInt32(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(int value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(int a, int b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}