﻿using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerList<T> : IBinaryTypeSerializer<List<T>?>
    {
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<T> _serializer;

        public BinarySerializerList(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, List<T>? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            WriteHeader(dataBuffer, header);

            if (value != null)
            {
                int length = value.Count;
                WriteLength(dataBuffer, length);

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        public HeaderWithValue<List<T>?> Decode(DataBuffer dataBuffer)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<List<T>?>(header, default);
            }

            int length = ReadLength(dataBuffer);
            List<T> list = ReadList(dataBuffer, length);

            return new HeaderWithValue<List<T>?>(header, list);
        }
        
        public int GetLengthInBits(List<T>? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNumeric;
            }

            int length = NumberOfHeaderBitsNumeric;

            foreach (var item in value)
            {
                length += NumberOfHeaderBitsNumeric;

                if (item != null)
                {
                    length += _serializer.GetLengthInBits(item);
                }
            }

            return length;
        }

        private Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));

            if (!header.IsMissing)
            {
                dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
            }
        }

        private int ReadLength(DataBuffer dataBuffer)
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        private void WriteLength(DataBuffer dataBuffer, int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }
        }

        private List<T> ReadList(DataBuffer dataBuffer, int length)
        {
            List<T> list = new();

            for (int i = 0; i < length; i++)
            {
                T? value = _serializer.Decode(dataBuffer).Value;
                if (value != null) list.Add(value);
            }

            return list;
        }
    }
}