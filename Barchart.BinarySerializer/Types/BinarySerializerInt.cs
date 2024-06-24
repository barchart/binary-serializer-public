﻿using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt : BinarySerializerNumeric<int>
    {
        #region Properties

        public override int Size => sizeof(int);

        #endregion

        #region Methods

        protected override int DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }

        protected override void EncodeValue(IDataBuffer dataBuffer, int value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        #endregion
    }
}