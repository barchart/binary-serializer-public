#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) decimal values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDecimal : IBinaryTypeSerializer<decimal>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(decimal);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion
        
        #region Fields

        private readonly BinarySerializerInt _binarySerializerInt;
        
        #endregion

        #region Constructors

        public BinarySerializerDecimal()
        {
            _binarySerializerInt = new BinarySerializerInt();
        }
        
        #endregion
        
        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, decimal value)
        {
            int[] components = Decimal.GetBits(value);
            
            _binarySerializerInt.Encode(dataBuffer, components[0]);
            _binarySerializerInt.Encode(dataBuffer, components[1]);
            _binarySerializerInt.Encode(dataBuffer, components[2]);
            _binarySerializerInt.Encode(dataBuffer, components[3]);
        }

        /// <inheritdoc />
        public decimal Decode(IDataBufferReader dataBuffer)
        {
            int[] components = {
                _binarySerializerInt.Decode(dataBuffer),
                _binarySerializerInt.Decode(dataBuffer),
                _binarySerializerInt.Decode(dataBuffer),
                _binarySerializerInt.Decode(dataBuffer)
            };

            return new decimal(components);
        }

        /// <inheritdoc />
        public int GetLengthInBits(decimal value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}