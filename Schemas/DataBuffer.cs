﻿namespace Barchart.BinarySerializer.Schemas
{
    public class DataBuffer
    {
        private byte[] _buffer;
        private int _offset;
        private int _offsetInLastByte;

        public DataBuffer(byte[] buffer)
        {
            _buffer = buffer;
            _offset = 0;
            _offsetInLastByte = 0;
        }

        public void ResetByte()
        {
            _buffer[_offset] = 0;
        }

        public byte[] ToBytes()
        {
            return _buffer.Take(_offset + 1).ToArray();
        }

        public void WriteBit(byte bit)
        {
            _buffer[_offset] |= (byte)(bit << (7 - _offsetInLastByte));
            _offsetInLastByte = (_offsetInLastByte + 1) % 8;

            if (_offsetInLastByte == 0)
            {
                _offset++;

                if (_offset >= _buffer.Length)
                {
                    throw new Exception($"Object is larger then {_buffer.Length} bytes.");
                }

                _buffer[_offset] = 0;
            }
        }

        public byte ReadBit()
        {
            byte bit = (byte)((_buffer[_offset] >> (7 - _offsetInLastByte)) & 1);
            _offsetInLastByte = (_offsetInLastByte + 1) % 8;

            if (_offsetInLastByte == 0)
            {
                _offset++;
            }

            return bit;
        }

        public void WriteByte(byte valueByte)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit((byte)((valueByte >> j) & 1));
            }
        }

        public byte ReadByte()
        {
            byte byteToAdd = 0;

            for (int j = 7; j >= 0; j--)
            {
                byte bit = ReadBit();
                byteToAdd |= (byte)(bit << j);
            }

            return byteToAdd;
        }
    }
}

