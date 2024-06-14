#region Using Statements

using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Representing additional byte for a header for every property/field.
    ///     7_ 6_ 5_ 4_ 3_ 2_ 1_ 0_ (byte structure)
    ///     7 (Missing bit) 
    ///     6 (Null bit)
    ///     5-0 (bits for string length 0 max string length is 2^6 - 63)
    /// </summary>
    public readonly struct Header
    {
        #region Properties
        
        public bool IsMissing { get; init; }
        public bool IsNull { get; init; }
        
        #endregion
        
        #region Methods

        /// <summary>
        ///     Writes instance data to a buffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to write to.</param>
        public void WriteToBuffer(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(IsMissing);

            if (!IsMissing)
            {
                dataBuffer.WriteBit(IsNull);
            }
        }
        
        #endregion
    }
}