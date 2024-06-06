using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// A data type that helps to use a buffer efficiently with the possibility to read or write bytes and bits.
    /// </summary>
    public class DataBuffer
    {
        private readonly byte[] _buffer;
        private int _offset;
        private int _offsetInLastByte;

        /// <summary>
        /// Initializes a new instance of the DataBuffer class with the specified buffer.
        /// </summary>
        /// <param name="buffer">The byte array to be used as the buffer.</param>
        public DataBuffer(byte[] buffer)
        {
            _buffer = buffer;
            _offset = 0;
            _offsetInLastByte = 0;
        }

        /// <summary>
        /// Resets the current byte in the buffer to zero.
        /// </summary>
        public void ResetByte()
        {
            _buffer[_offset] = 0;
        }

        /// <summary>
        /// Converts the buffer to a byte array up to the current offset.
        /// </summary>
        /// <returns>A byte array containing the data up to the current offset.</returns>
        public byte[] ToBytes()
        {
            return _buffer.Take(_offset + 1).ToArray();
        }

        /// <summary>
        /// Writes a single bit to the buffer.
        /// </summary>
        /// <param name="bit">The bit to write (0 or 1).</param>
        /// <exception cref="Exception">Thrown if the buffer is full.</exception>
        public void WriteBit(byte bit)
        {
            _buffer[_offset] |= (byte)(bit << (7 - _offsetInLastByte));
            _offsetInLastByte = (_offsetInLastByte + 1) % 8;

            if (_offsetInLastByte == 0)
            {
                _offset++;

                if (_offset >= _buffer.Length)
                {
                    throw new Exception($"Object is larger than {_buffer.Length} bytes.");
                }

                _buffer[_offset] = 0;
            }
        }

        /// <summary>
        /// Reads a single bit from the buffer.
        /// </summary>
        /// <returns>The bit read (0 or 1).</returns>
        public byte ReadBit()
        {
            try
            {
                byte bit = (byte)((_buffer[_offset] >> (7 - _offsetInLastByte)) & 1);
                _offsetInLastByte = (_offsetInLastByte + 1) % 8;

                if (_offsetInLastByte == 0)
                {
                    _offset++;
                }
                return bit;
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Writes a byte to the buffer.
        /// </summary>
        /// <param name="valueByte">The byte to write.</param>
        public void WriteByte(byte valueByte)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit((byte)((valueByte >> j) & 1));
            }
        }

        /// <summary>
        /// Reads a byte from the buffer.
        /// </summary>
        /// <returns>The byte read from the buffer.</returns>
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