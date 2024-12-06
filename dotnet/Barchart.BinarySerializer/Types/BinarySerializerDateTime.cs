#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.Common.Extensions;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) DateTime values to (and from) a binary data source.
/// </summary>
public class BinarySerializerDateTime : IBinaryTypeSerializer<DateTime>
{
    #region Fields

    private readonly BinarySerializerLong _binarySerializerLong = new();

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, DateTime value)
    {
        _binarySerializerLong.Encode(writer, value.GetMillisecondsSinceUnixEpoch());
    }

    /// <inheritdoc />
    public DateTime Decode(IDataBufferReader reader)
    {
        long millisecondsSinceEpoch = _binarySerializerLong.Decode(reader);

        return DateTime.UnixEpoch.AddMilliseconds(millisecondsSinceEpoch);
    }

    /// <inheritdoc />
    public bool GetEquals(DateTime a, DateTime b)
    {
        return a.Equals(b);
    }

    #endregion
}