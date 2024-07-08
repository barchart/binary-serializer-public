namespace Barchart.BinarySerializer.Buffers
{
    /// <summary>
    ///     A data buffer reader which uses a fixed-length byte array
    ///     for internal storage.
    /// </summary>
    public class DataBufferReader : IDataBufferReader
    {
        #region Fields

        private readonly byte[] _byteArray;

        private int _positionByte;
        private int _positionBit;

        private const byte TRUE = 1;

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Creates a new <see cref="DataBufferReader"/> instance.
        /// </summary>
        /// <param name="byteArray">
        ///     The internal storage mechanism for the class.
        /// </param>
        public DataBufferReader(byte[] byteArray)
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
                    value = (byte)(value | (TRUE << j));
                }
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
        public void Reset()
        {
            _positionByte = 0;
            _positionBit = 0;
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