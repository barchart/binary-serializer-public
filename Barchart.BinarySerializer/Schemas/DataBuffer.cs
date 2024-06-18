#region Using Statements

using Barchart.BinarySerializer.Logging;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     A wrapper around a byte array with convenience reading and writing to the buffer.
    /// </summary>
    public class DataBuffer
    {
        #region Fields
        
        public static readonly int NumberOfBitsIsMissing = 1;
        public static readonly int NumberOfHeaderBitsNonString = 2;
        public static readonly int NumberOfHeaderBitsString = 8;
        private readonly byte[] _buffer;
        private int _offset;
        private int _offsetInLastByte;

        #endregion

        #region Constructor(s)
        /// <summary>
        ///     Instantiates the class using an external buffer.
        /// </summary>
        public DataBuffer(byte[] buffer)
        {
            _buffer = buffer;
            _offset = 0;
            _offsetInLastByte = 0;
        }
        #endregion

        #region Methods

        /// <summary>
        ///     Resets the current byte index to zero.
        /// </summary>
        public void ResetByte()
        {
            _buffer[_offset] = 0;
        }

        /// <summary>
        ///     Generates a copy of the buffer, as a byte array, from from the
        ///     start of the array to the current position.
        /// </summary>
        /// <returns>A byte array containing the data up to the current offset.</returns>
        public byte[] ToBytes()
        {
            if (_offset == 0)
            {
                return new byte[0];
            }

            return _buffer.Take(_offset + 1).ToArray();
        }

        /// <summary>
        ///     Writes a single bit to the buffer.
        /// </summary>
        /// <param name="bit">Boolean value representing the bit to write (true for 1, false for 0).</param>
        /// <exception cref="Exception">Thrown if the buffer is full.</exception>
        public void WriteBit(bool bit)
        {
            if (IsBufferFull())
            {
                throw new Exception($"Object is larger than {_buffer.Length} bytes.");
            }

            byte valueToWrite = (byte)(bit ? 1 : 0);
            int bitPosition = 7 - _offsetInLastByte;

            _buffer[_offset] |= (byte)(valueToWrite << bitPosition);

            _offsetInLastByte++;

            if (_offsetInLastByte == 8)
            {
                _offsetInLastByte = 0;
                _offset++;

                if (_offset < _buffer.Length)
                {
                    _buffer[_offset] = 0;
                }
            }
        }

        /// <summary>
        ///     Reads a single bit from the buffer.
        /// </summary>
        public byte ReadBit()
        {
            try
            {
                if (IsBufferFull())
                {
                    throw new InvalidOperationException("Attempt to read beyond the end of the buffer.");
                }

                byte bit = (byte)((_buffer[_offset] >> (7 - _offsetInLastByte)) & 1);
                _offsetInLastByte = (_offsetInLastByte + 1) % 8;

                if (_offsetInLastByte == 0)
                {
                    _offset++;
                }

                return bit;
            }
            catch (InvalidOperationException ex)
            {
                LoggerWrapper.LogError($"Buffer read error at offset {_offset}, bit {_offsetInLastByte}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError($"Unexpected error while reading bit at offset {_offset}, bit {_offsetInLastByte}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        ///     Writes a byte to the buffer.
        /// </summary>
        public void WriteByte(byte valueByte)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit(((valueByte >> j) & 1) == 1);
            }
        }

        /// <summary>
        ///     Reads a byte from the buffer.
        /// </summary>
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

        /// <summary>
        ///     Encodes the missing flag into the provided DataBuffer.
        /// </summary>
        public void EncodeMissingFlag()
        {
            WriteBit(true);
        }

        /// <summary>
        ///     Reads the length of a value from the provided DataBuffer.
        /// </summary>
        /// <returns>The length of the value.</returns>
        public int ReadLength()
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = 0; i < lengthBytes.Length; i++)
            {
                lengthBytes[i] = ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        /// <summary>
        ///     Writes the length of a value to the provided DataBuffer.
        /// </summary>
        /// <param name="length">The length of the value.</param>
        public void WriteLength(int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = 0; i < lengthBytes.Length; i++)
            {
                WriteByte(lengthBytes[i]);
            }
        }

        /// <summary>
        ///     Writes an array of bytes to the provided DataBuffer.
        /// </summary>
        /// <param name="valueBytes">The array of bytes to write.</param>
        public void WriteValueBytes(byte[] valueBytes)
        {
            for (int i = 0; i < valueBytes.Length; i++)
            {
                WriteByte(valueBytes[i]);
            }
        }

        /// <summary>
        ///     Reads an array of bytes from the provided DataBuffer.
        /// </summary>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>The read array of bytes.</returns>
        public byte[] ReadValueBytes(int size)
        {
            byte[] valueBytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                valueBytes[i] = ReadByte();
            }

            return valueBytes;
        }

        private bool IsBufferFull()
        {
            return _offset >= _buffer.Length;
        }

        #endregion
    }
}