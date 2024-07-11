namespace Barchart.BinarySerializer.Buffers;

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
        if (IsBufferFull())
        {
            throw new InvalidOperationException("Buffer is full.");
        }

        byte result;

        if (_positionBit == 0)
        {
            result = _byteArray[_positionByte++];
        }
        else
        {
            int firstPartLength = 8 - _positionBit;
            byte firstPart = (byte)(_byteArray[_positionByte] & ((1 << firstPartLength) - 1));
            firstPart = (byte)(firstPart << _positionBit);

            byte secondPart = 0;
            if (_positionByte + 1 < _byteArray.Length)
            {
                secondPart = (byte)(_byteArray[_positionByte + 1] >> firstPartLength);
            }

            result = (byte)(firstPart | secondPart);
            _positionByte++;
        }

        return result;
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
    public byte[] ReadBytes2(int size)
    {
        if (size == 0) return new byte[0];

        byte[] bytes = new byte[size];
        int byteReadPosition = _positionByte;

        if (_positionBit != 0)
        {
            for (int i = 0; i < size; i++)
            {
                byte currentBytePart = (byte)(_byteArray[byteReadPosition] << _positionBit);
                byteReadPosition++;

                byte nextBytePart = 0;
                if (byteReadPosition < _byteArray.Length)
                {
                    nextBytePart = (byte)(_byteArray[byteReadPosition] >> (8 - _positionBit));
                }

                bytes[i] = (byte)(currentBytePart | nextBytePart);
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                if (byteReadPosition < _byteArray.Length)
                {
                    bytes[i] = _byteArray[byteReadPosition];
                    byteReadPosition++;
                }
                else
                {
                    bytes[i] = 0;
                }
            }
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