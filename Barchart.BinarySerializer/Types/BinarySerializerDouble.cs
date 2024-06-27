#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) double values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDouble : IBinaryTypeSerializer<double>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(double);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, double value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public double Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToDouble(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(double value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(double a, double b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}