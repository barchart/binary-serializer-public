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
        if (size == 0) return Array.Empty<byte>();
        
        if (size > _byteArray.Length)
        {
            throw new InvalidOperationException("Unable to read bytes. Request exceeds available buffer size.");
        }

        byte[] bytes = new byte[size];

        if (_positionBit != 0)
        {
            for (int i = 0; i < size; i++)
            {
                if (_positionByte + 1 < _byteArray.Length)
                {
                    byte currentBytePart = (byte)(_byteArray[_positionByte] << _positionBit);                
                    byte nextBytePart = (byte)(_byteArray[_positionByte + 1] >> (8 - _positionBit));
                    bytes[i] = (byte)(currentBytePart | nextBytePart);
                }
                else 
                {
                    bytes[i] = (byte)(_byteArray[_positionByte] << _positionBit);;
                }
                
                _positionByte++;
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                bytes[i] = _byteArray[_positionByte];
                _positionByte++;
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

    /// <inheritdoc />
    public int BytesWritten()
    {
        return _byteArray.Length;
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