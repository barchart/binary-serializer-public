﻿using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a base class for binary serializers handling numeric types.
    /// </summary>
    /// <typeparam name="TContainer">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNumeric<TContainer> : IBinaryTypeSerializer<TContainer> where TContainer : struct
    {
        public const int NumberOfHeaderBitsNumeric = 2;

        public abstract int Size { get; }

        public void Encode(DataBuffer dataBuffer, TContainer value)
        {
            Header header = new() { IsMissing = false, IsNull = false };
            WriteHeader(dataBuffer, header);

            byte[] valueBytes = ConvertToByteArray(value);
            WriteValueBytes(dataBuffer, valueBytes);
        }

        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            byte[] valueBytes = ReadValueBytes(dataBuffer);

            return new HeaderWithValue<TContainer>(header, DecodeBytes(valueBytes));
        }

        public int GetLengthInBits(TContainer value)
        {
            return Size * 8 + NumberOfHeaderBitsNumeric;
        }

        protected abstract byte[] ConvertToByteArray(TContainer value);
        protected abstract TContainer DecodeBytes(byte[] bytes);

        private static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
        }

        private static Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private static void WriteValueBytes(DataBuffer dataBuffer, byte[] valueBytes)
        {
            for (int i = valueBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }

        private byte[] ReadValueBytes(DataBuffer dataBuffer)
        {
            byte[] valueBytes = new byte[Size];
            for (int i = Size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }
            return valueBytes;
        }
    }
}
