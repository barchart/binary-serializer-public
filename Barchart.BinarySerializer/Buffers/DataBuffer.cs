#region Using Statements

#endregion

namespace Barchart.BinarySerializer.Buffers
{
    /// <summary>
    ///     A utility for writing (and reading) binary data to (and from) a byte array.
    /// </summary>
    public class DataBuffer : IDataBuffer
    {
        #region Fields
        
        public static readonly int NumberOfBitsIsMissing = 1;
        public static readonly int NumberOfHeaderBitsNonString = 2;
        public static readonly int NumberOfHeaderBitsString = 8;
        
        private readonly byte[] _byteArray;
        
        private int _positionByte;
        private byte _positionBit;

        private const byte TRUE = 1;

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Creates a new <see cref="DataBuffer"/> instance.
        /// </summary>
        /// <param name="byteArray">
        ///     The internal storage mechanism for the class. Write operations will mutate this array.
        /// </param>
        public DataBuffer(byte[] byteArray)
        {
            _byteArray = byteArray;
            
            _positionByte = 0;
            _positionBit = 0;
        }
        
        #endregion

        #region Methods

        /// <summary>
        ///     Writes a single bit to the internal storage.
        /// </summary>
        /// <param name="value">
        ///     Value of the bit to write.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when internal storage is full.
        /// </exception>
        public void WriteBit(bool value)
        {
            if (IsBufferFull())
            {
                throw new InvalidOperationException("Unable to write bit. The buffer is currently positioned at the end of the internal byte array.");
            }

            if (value)
            {
                byte byteCurrent;

                if (_positionBit == 0)
                {
                    byteCurrent = 0;
                }
                else
                {
                    byteCurrent = _byteArray[_positionByte];
                }

                byte byteMask = (byte)(TRUE << (7 - _positionBit));

                _byteArray[_positionByte] = (byte)(byteCurrent | byteMask);
            }

            AdvanceBit();
        }
        
        /// <summary>
        ///     Writes a byte to the internal storage.
        /// </summary>
        /// <param name="value">
        ///     Value of the byte to write.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when remaining internal storage is less than one byte.
        /// </exception>
        public void WriteByte(byte value)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit(((value >> j) & 1) == 1);
            }
        }
        
        /// <summary>
        ///     Writes an array of bytes to the internal storage.
        /// </summary>
        /// <param name="value">
        ///     Value of the bytes to write.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when remaining internal storage is than the number of bytes to write.
        /// </exception>
        public void WriteBytes(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteByte(value[i]);
            }
        }
        
        /// <summary>
        ///     Reads a single bit from the internal storage.
        /// </summary>
        /// <returns>
        ///     The next bit from the internal storage.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the internal storage has been read completely.
        /// </exception>
        public bool ReadBit()
        {
            if (IsBufferFull())
            {
                throw new InvalidOperationException("Unable to read bit. The buffer is currently positioned at the end of the internal byte array.");
            }

            int bit = (_byteArray[_positionByte] >> (7 - _positionBit)) & 1;

            AdvanceBit();

            return bit == 1;
        }
        
        /// <summary>
        ///     Reads a byte from the internal storage.
        /// </summary>
        /// <returns>
        ///     The next byte from the internal storage.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the internal storage has less than one byte remaining.
        /// </exception>
        public byte ReadByte()
        {
            byte byteToAdd = 0;

            for (int j = 7; j >= 0; j--)
            {
                bool bit = ReadBit();
                
                byteToAdd |= (byte)(bit ? (1 << j) : 0); 
            }

            return byteToAdd;
        }
        
        /// <summary>
        ///     Reads multiple bytes from the internal storage.
        /// </summary>
        /// <param name="size">
        ///     The number of bytes to read from the internal storage.
        /// </param>
        /// <returns>
        ///     A byte array, with length of <paramref name="size"/>, with data from the internal storage.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the internal storage has less remaining space than the <paramref name="size"/> requested.
        /// </exception>
        public byte[] ReadBytes(int size)
        {
            byte[] valueBytes = new byte[size];
            
            for (int i = 0; i < size; i++)
            {
                valueBytes[i] = ReadByte();
            }

            return valueBytes;
        }

        /// <summary>
        ///     Generates a copy of the internal storage, as a byte array, containing
        ///     the data that has been written to the buffer.
        /// </summary>
        /// <returns>
        ///     A byte array containing the data up to the current offset.
        /// </returns>
        public byte[] ToBytes()
        {
            int byteCount = _positionByte + (_positionBit == 0 ? 0 : 1);
            
            return _byteArray.Take(byteCount).ToArray();
        }

        private void AdvanceBit()
        {
            unchecked
            {
                _positionBit++;
            }

            if (_positionBit == 0)
            {
                _positionByte++;
            }
        }

        private bool IsBufferFull()
        {
            return _positionByte >= _byteArray.Length;
        }

        #endregion
    }
}