﻿#region Using Statements

using Barchart.BinarySerializer.Logging;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     A utility for writing (and reading) binary data to (and from) a byte array.
    /// </summary>
    public class DataBuffer
    {
        #region Fields
        
        public static readonly int NumberOfBitsIsMissing = 1;
        public static readonly int NumberOfHeaderBitsNonString = 2;
        public static readonly int NumberOfHeaderBitsString = 8;
        
        private readonly byte[] _byteArray;
        
        private int _offset;
        private int _offsetInLastByte;

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Instantiates the class using an external buffer.
        /// </summary>
        /// <param name="byteArray">
        ///     The internal storage mechanism for the class. Write operations will mutate this array.
        /// </param>
        public DataBuffer(byte[] byteArray)
        {
            _byteArray = byteArray;
            
            _offset = 0;
            _offsetInLastByte = 0;
        }
        
        #endregion

        #region Methods

        /// <summary>
        ///     Writes a single bit to the internal storage.
        /// </summary>
        /// <param name="value">
        ///     Value of the bit to write.
        /// </param>
        /// <exception cref="Exception">
        ///     Thrown when internal storage is already full.
        /// </exception>
        public virtual void WriteBit(bool value)
        {
            if (IsBufferFull())
            {
                throw new Exception($"Object is larger than {_byteArray.Length} bytes.");
            }

            if (IsBeginningOfNewByte())
            {
                ResetByte();
            }

            byte valueToWrite = (byte)(value ? 1 : 0);
            int bitPosition = 7 - _offsetInLastByte;

            _byteArray[_offset] |= (byte)(valueToWrite << bitPosition);

            _offsetInLastByte++;

            if (_offsetInLastByte == 8)
            {
                _offsetInLastByte = 0;
                _offset++;
            }
        }
        
        /// <summary>
        ///     Writes a byte to the internal storage.
        /// </summary>
        /// <param name="value">
        ///     Value of the byte to write.
        /// </param>
        /// <exception cref="Exception">
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
        /// <exception cref="Exception">
        ///     Thrown when remaining internal storage is than the number of bytes to write.
        /// </exception>
        public virtual void WriteBytes(byte[] value)
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
        /// <exception cref="Exception">
        ///     Thrown when the internal storage has been read completely.
        /// </exception>
        public bool ReadBit()
        {
            try
            {
                if (IsBufferFull())
                {
                    throw new InvalidOperationException("Attempt to read beyond the end of the buffer.");
                }

                byte bit = (byte)((_byteArray[_offset] >> (7 - _offsetInLastByte)) & 1);
                _offsetInLastByte = (_offsetInLastByte + 1) % 8;

                if (IsBeginningOfNewByte())
                {
                    _offset++;
                }

                return bit == 1;
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
        ///     Reads a byte from the internal storage.
        /// </summary>
        /// <returns>
        ///     The next byte from the internal storage.
        /// </returns>
        /// <exception cref="Exception">
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
        /// <exception cref="Exception">
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
        
        public void ResetByte()
        {
            _byteArray[_offset] = 0;
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
            int byteCount = _offset + (_offsetInLastByte == 0 ? 0 : 1);
            
            return _byteArray.Take(byteCount).ToArray();
        }

        private bool IsBufferFull()
        {
            return _offset >= _byteArray.Length;
        }

        private bool IsBeginningOfNewByte()
        {
            return _offsetInLastByte == 0;
        }

        #endregion
    }
}