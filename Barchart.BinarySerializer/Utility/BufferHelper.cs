using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;

namespace Barchart.BinarySerializer.Utility
{
	public static class BufferHelper
	{
        public static void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(1);
        }

        public static Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        public static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));

            if (!header.IsMissing)
            {
                dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
            }
        }

        public static int ReadLength(DataBuffer dataBuffer)
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        public static void WriteLength(DataBuffer dataBuffer, int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }
        }
    }
}

