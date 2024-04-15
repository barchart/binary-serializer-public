using Barchart.BinarySerializer.Schemas;
using Google.Protobuf;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerByteString : IBinaryTypeSerializer<ByteString>
    {
        public const int NumberOfHeaderBitsNumeric = 2;

        public void Encode(DataBuffer dataBuffer, ByteString? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            WriteHeader(dataBuffer, header);

            if (value != null)
            {
                byte[] valueBytes = value.ToByteArray();
                WriteLength(dataBuffer, valueBytes.Length);
                WriteValue(dataBuffer, valueBytes);
            }
        }

        public HeaderWithValue<ByteString> Decode(DataBuffer dataBuffer)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<ByteString>(header, default);
            }

            int length = ReadLength(dataBuffer);
            ByteString result = ReadValue(dataBuffer, length);

            return new HeaderWithValue<ByteString>(header, result);
        }

        public int GetLengthInBits(ByteString? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNumeric;
            }

            byte[] valueBytes = value.ToByteArray();
            byte[] lengthBytes = BitConverter.GetBytes(valueBytes.Length);

            return lengthBytes.Length * 8 + valueBytes.Length * 8 + NumberOfHeaderBitsNumeric;
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

        private static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));

            if (!header.IsMissing)
            {
                dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
            }
        }

        private static int ReadLength(DataBuffer dataBuffer)
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        private static void WriteLength(DataBuffer dataBuffer, int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }
        }

        private static ByteString ReadValue(DataBuffer dataBuffer, int length)
        {
            byte[] valueBytes = new byte[length];

            for (int i = length - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            return ByteString.CopyFrom(valueBytes);
        }

        private static void WriteValue(DataBuffer dataBuffer, byte[] valueBytes)
        {
            for (int i = valueBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }
    }
}