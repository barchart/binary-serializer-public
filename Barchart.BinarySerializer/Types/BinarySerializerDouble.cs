﻿using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        #region Properties
        
        public override int Size => sizeof(double);

        #endregion

        #region Methods

        protected override void EncodeValue(DataBuffer dataBuffer, double value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }

        #endregion
    }
}