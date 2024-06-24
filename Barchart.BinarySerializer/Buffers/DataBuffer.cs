namespace Barchart.BinarySerializer.Buffers
{
    /// <summary>
    ///     A data buffer which uses a fixed-length byte array to store data.
    /// </summary>
    public class DataBuffer : IDataBuffer
    {
        #region Fields
        
        public static readonly int NumberOfBitsIsMissing = 1;
        public static readonly int NumberOfHeaderBitsNonString = 2;
        public static readonly int NumberOfHeaderBitsString = 8;
        
        private readonly byte[] _byteArray;
        
        private int _positionByte;
        private int _positionBit;

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
        
        /// <inheritdoc />
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
        
        /// <inheritdoc />
        public byte ReadByte()
        {
            byte value = 0;
            
            for (int j = 7; j >= 0; j--)
            {
                bool bit = ReadBit();

                if (bit)
                {
                    value = (byte)(value | TRUE << j);
                }
                
                value |= (byte)(bit ? (1 << j) : 0); 
            }

            return value;
        }
        
        /// <inheritdoc />
        public byte[] ReadBytes(int size)
        {
            byte[] bytes = new byte[size];
            
            for (int i = 0; i < size; i++)
            {
                bytes[i] = ReadByte();
            }

            return bytes;
        }
        
        /// <inheritdoc />
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
        
        /// <inheritdoc />
        public void WriteByte(byte value)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit(((value >> j) & 1) == 1);
            }
        }
        
        /// <inheritdoc />
        public void WriteBytes(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteByte(value[i]);
            }
        }

        /// <inheritdoc />
        public byte[] ToBytes()
        {
            int byteCount = _positionByte + (_positionBit == 0 ? 0 : 1);
            
            return _byteArray.Take(byteCount).ToArray();
        }

        private void AdvanceBit()
        {
            if (_positionBit == 7)
            {
                _positionBit = 0;
                _positionByte++;
            }
            else
            {
                _positionBit++;
            }
        }

        private bool IsBufferFull()
        {
            return _positionByte >= _byteArray.Length;
        }

        #endregion
    }
}