#region Using Statements

using Barchart.BinarySerializer.Utility;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     A wrapper around a byte array with convenience reading and writing to the buffer.
    /// </summary>
    public class DataBuffer
    {
        #region Fields

        private readonly byte[] _buffer;
        
        private int _offset;
        private int _offsetInLastByte;

        #endregion

        #region Methods

        /// <summary>
        ///     Instantiates the class using an external buffer.
        /// </summary>
        public DataBuffer(byte[] buffer)
        {
            _buffer = buffer;
            
            _offset = 0;
            _offsetInLastByte = 0;
        }

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
            return _buffer.Take(_offset + 1).ToArray();
        }

        /// <summary>
        ///     Writes a single bit to the buffer.
        /// </summary>
        /// <param name="bit">Boolean value representing the bit to write (true for 1, false for 0).</param>
        /// <exception cref="Exception">Thrown if the buffer is full.</exception>
        public void WriteBit(bool bit)
        {
            byte valueToWrite = (byte)(bit ? 1 : 0);
            int bitPosition = 7 - _offsetInLastByte;

            _buffer[_offset] |= (byte)(valueToWrite << bitPosition);

            _offsetInLastByte++;

            if (_offsetInLastByte == 8)
            {
                _offsetInLastByte = 0;
                _offset++;

                if (_offset >= _buffer.Length)
                {
                    throw new Exception($"Object is larger than {_buffer.Length} bytes.");
                }

                _buffer[_offset] = 0;
            }
        }

        /// <summary>
        ///     Reads a single bit from the buffer.
        /// </summary>
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

        #endregion
    }
}