#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.Common.Extensions;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) DateOnly values to (and from) a binary data source.
/// </summary>
public class BinarySerializerDateOnly : IBinaryTypeSerializer<DateOnly>
{
    #region Fields

    private readonly BinarySerializerInt _binarySerializerInt = new();
    
    #endregion

    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, DateOnly value)
    {
        _binarySerializerInt.Encode(writer, value.GetDaysSinceEpoch());
    }

    /// <inheritdoc />
    public DateOnly Decode(IDataBufferReader reader)
    {
        int daysSinceEpoch = _binarySerializerInt.Decode(reader);
        
        return DateOnly.MinValue.AddDays(daysSinceEpoch);
    }
    
    /// <inheritdoc />
    public bool GetEquals(DateOnly a, DateOnly b)
    {
        return a.Equals(b);
    }

    #endregion
}