﻿#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerULong : IBinaryTypeSerializer<ulong>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(ulong);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, ulong value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public ulong Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToUInt64(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(ulong value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}