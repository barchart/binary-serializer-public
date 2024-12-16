#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Utilities;

internal static class Serialization
{
    #region Methods
    
    internal static bool ReadMissingFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    internal static void WriteMissingFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }
    
    internal static bool ReadNullFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    internal static void WriteNullFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }
    
    internal static void NormalizeListSizes<TItem>(IList<TItem>? currentItems, IList<TItem>? previousItems)
    {
        int maxCount = Math.Max(currentItems?.Count ?? 0, previousItems?.Count ?? 0);

        if (currentItems != null)
        {
            while (currentItems.Count < maxCount)
            {
                currentItems.Add(default!);
            }
        }

        if (previousItems != null)
        {
            while (previousItems.Count < maxCount)
            {
                previousItems.Add(default!);
            }
        }
    }
    
    #endregion
}