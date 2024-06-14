using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;

namespace Barchart.BinarySerializer.Utility
{
    /// <summary>
    /// Utility class for various binary serialization operations.
    /// </summary>
    public static class UtilityKit
    {
        public static readonly int NumberOfBitsIsMissing = 1;
        public static readonly int NumberOfHeaderBitsNonString = 2;
        public static readonly int NumberOfHeaderBitsString = 8;

        /// <summary>
        /// Encodes the missing flag into the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to write to.</param>
        public static void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(true);
        }

        /// <summary>
        /// Reads a header from the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to read from.</param>
        /// <returns>The read Header.</returns>
        public static Header ReadHeader(DataBuffer dataBuffer)
        {
            bool headerIsMissing = dataBuffer.ReadBit() == 1;
            bool headerIsNull = !headerIsMissing && dataBuffer.ReadBit() == 1;
            
            return new() { IsMissing = headerIsMissing, IsNull = headerIsNull };
        }

        /// <summary>
        /// Reads the length of a value from the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to read from.</param>
        /// <returns>The length of the value.</returns>
        public static int ReadLength(DataBuffer dataBuffer)
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = 0; i < lengthBytes.Length; i++)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        /// <summary>
        /// Writes the length of a value to the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to write to.</param>
        /// <param name="length">The length of the value.</param>
        public static void WriteLength(DataBuffer dataBuffer, int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = 0; i < lengthBytes.Length; i++)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }
        }

        /// <summary>
        /// Writes an array of bytes to the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to write to.</param>
        /// <param name="valueBytes">The array of bytes to write.</param>
        public static void WriteValueBytes(DataBuffer dataBuffer, byte[] valueBytes)
        {
            for (int i = 0; i < valueBytes.Length; i++)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }

        /// <summary>
        /// Reads an array of bytes from the provided DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to read from.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>The read array of bytes.</returns>
        public static byte[] ReadValueBytes(DataBuffer dataBuffer, int size)
        {
            byte[] valueBytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            return valueBytes;
        }

        /// <summary>
        /// Checks if the header indicates that the value is missing or null.
        /// </summary>
        /// <param name="header">The Header to check.</param>
        /// <returns>True if the value is missing or null, otherwise false.</returns>
        public static bool IsHeaderMissingOrNull(Header header)
        {
            return header.IsMissing || header.IsNull;
        }
    }
}