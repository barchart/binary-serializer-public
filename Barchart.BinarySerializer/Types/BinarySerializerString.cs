#region Using Statements

using System.Text;

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for strings.
    /// </summary>
    public class BinarySerializerString : IBinaryTypeSerializer<string>
    {
        #region Constants
        
        private const int MAXIMUM_STRING_LENGTH_IN_BYTES = UInt16.MaxValue;
        
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
        public void Encode(IDataBufferWriter dataBuffer, string value)
        {
            byte[] bytes = _encoding.GetBytes(value);

            if (bytes.Length > MAXIMUM_STRING_LENGTH_IN_BYTES)
            {
                throw new ArgumentException($"Unable to serialize string. Serialized string would require {bytes.Length} bytes; however, the maximum size of a serialized string is {MAXIMUM_STRING_LENGTH_IN_BYTES}", nameof(value));
            }
            
            _binarySerializerUShort.Encode(dataBuffer, Convert.ToUInt16(bytes.Length));
            
            dataBuffer.WriteBytes(bytes);
        }

        /// <inheritdoc />
        public string Decode(IDataBufferReader dataBuffer)
        {
            return _encoding.GetString(dataBuffer.ReadBytes(_binarySerializerUShort.Decode(dataBuffer)));
        }

        /// <inheritdoc />
        public int GetLengthInBits(string value)
        {
            return sizeof(ushort) + _encoding.GetByteCount(value);
        }

        #endregion
    }
}