#region Using Statements

using System.Text;

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) string values to (and from) a binary data source.
/// </summary>
public class BinarySerializerString : IBinaryTypeSerializer<string>
{
    #region Constants

    private const int MAXIMUM_STRING_LENGTH_IN_BYTES = ushort.MaxValue;

    private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;

    #endregion

    #region Fields

    private readonly BinarySerializerUShort _binarySerializerUShort;

    private readonly Encoding _encoding;

    #endregion

    #region Constructor(s)

    public BinarySerializerString() : this(DEFAULT_ENCODING)
    {

    }

    public BinarySerializerString(Encoding encoding)
    {
        _binarySerializerUShort = new BinarySerializerUShort();

        _encoding = encoding;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, string value)
    {
        Serialization.WriteNullFlag(writer, value == null);
        
        if (value == null)
        {
            return;
        }

        byte[] bytes = _encoding.GetBytes(value);

        if (bytes.Length > MAXIMUM_STRING_LENGTH_IN_BYTES)
        {
            throw new ArgumentException( $"Unable to serialize string. Serialized string would require {bytes.Length} bytes; however, the maximum size of a serialized string is {MAXIMUM_STRING_LENGTH_IN_BYTES}", nameof(value));
        }

        _binarySerializerUShort.Encode(writer, Convert.ToUInt16(bytes.Length));

        writer.WriteBytes(bytes);
    }

    /// <inheritdoc />
    public string Decode(IDataBufferReader reader)
    {
        if (Serialization.ReadNullFlag(reader))
        {
            return null!;
        }

        return _encoding.GetString(reader.ReadBytes(_binarySerializerUShort.Decode(reader)));
    }

    /// <inheritdoc />
    public bool GetEquals(string a, string b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return a.Equals(b);
    }

    #endregion
}