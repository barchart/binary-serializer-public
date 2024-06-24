﻿using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUshort : BinarySerializerNumeric<ushort>
    {
        #region Properties

        public override int Size => sizeof(ushort);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, ushort value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }

        #endregion
    }
}