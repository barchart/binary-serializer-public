
using System.Runtime.CompilerServices;

namespace JerqAggregatorNew.Types
{
    public class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T?> where T : struct, IConvertible
    {
        private byte[] ConvertToByteArray(T value)
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(T));

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    byte[] byteArray = new byte[1];
                    byteArray[0] = (byte)((bool)(object)value ? 1 : 0);
                    return byteArray;
                case TypeCode.Byte:
                    return BitConverter.GetBytes((byte)(object)value);
                case TypeCode.SByte:
                    return BitConverter.GetBytes((sbyte)(object)value);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)(object)value);
                case TypeCode.Char:
                    return BitConverter.GetBytes((char)(object)value);
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)(object)value);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)(object)value);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)(object)value);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)(object)value);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)(object)value);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)(object)value);
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)(object)value);
                case TypeCode.Decimal:
                    return Decimal.GetBits((decimal)(object)value)
                        .SelectMany(BitConverter.GetBytes)
                        .ToArray();
                case TypeCode.DateTime:
                    DateTime dateTimeValue = (DateTime)(object)value;
                    TimeSpan unixTimeSpan = dateTimeValue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    long unixTime = (long)unixTimeSpan.TotalMilliseconds;
                    return BitConverter.GetBytes(unixTime);
                default:
                    throw new NotSupportedException($"Type {typeof(T)} is not supported.");
            }
        }

        public void Encode(List<byte> buffer, T? value, ref int offset, ref int offsetInLastByte) 
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            if (buffer.Count == 0)
            {
                buffer.Add(0);
            }

            // if byte has space for both flags
            if (offsetInLastByte < 7)
            {
                buffer[offset] |= (byte)((header.IsMissing ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte++;
                buffer[offset] |= (byte)((header.IsNull ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte++;

                if (offsetInLastByte == 7)
                {
                    offsetInLastByte = 0;
                    offset++;
                }
            }
            // if byte has space only for one flag
            else if (offsetInLastByte == 7)
            {
                buffer[offset] |= (byte)((header.IsMissing ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte = 0;
                offset++;
                buffer.Add(0);
                buffer[offset] |= (byte)((header.IsNull ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte++;
            }

            // if byte hasn't space for any flag move to the next byte and start writing header
            else
            {
                offsetInLastByte = 0;
                offset++;
                buffer.Add(0);
                buffer[offset] |= (byte)((header.IsMissing ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte++;
                buffer[offset] |= (byte)((header.IsNull ? 1 : 0) << (7 - offsetInLastByte));
                offsetInLastByte++;
            }

            if (value.HasValue)
            {
                byte[] valueBytes = ConvertToByteArray(value.Value);
                
                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        if (offsetInLastByte % 8 == 0)
                        {
                            buffer.Add(0);
                            offset++;
                            offsetInLastByte = 0;
                        }

                        buffer[offset] |= (byte)(((valueBytes[i] >> j) & 1) << ((7 - offsetInLastByte) % 8));
                        offsetInLastByte++;
                    }
                }
            }
            
        }
        public HeaderWithValue Decode(List<byte> buffer, ref int offset, ref int offsetInLastByte)
        {
            byte[] valueBytes = new byte[Unsafe.SizeOf<T>()];
            int size = Unsafe.SizeOf<T>();

            T value = default;

            Header header = new Header();
            bool isMissing = false;
            bool isNull = false;

            if (offsetInLastByte == 8)
            {
                offsetInLastByte = 0;
                offset++;
            }

            isMissing = ((buffer[offset] >> (7 - offsetInLastByte)) & 1) == 1;
            header.IsMissing = isMissing;
            offsetInLastByte++;

            if (offsetInLastByte == 8)
            {
                offsetInLastByte = 0;
                offset++;
            }

            isNull = ((buffer[offset] >> (7 - offsetInLastByte)) & 1) == 1;
            header.IsNull = isNull;

            offsetInLastByte++;

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                byte byteToAdd = 0;
                for (int j = 7; j >= 0; j--)
                {
                    if (offsetInLastByte % 8 == 0)
                    {
                        offset++;
                        offsetInLastByte = 0;
                    }

                    int bit = (buffer[offset] >> (7 - offsetInLastByte)) & 1;
                    byteToAdd |= (byte)(bit << j);
                    offsetInLastByte++;
                }
                valueBytes[i] = byteToAdd;
            }

            // datetime and decimal are non primitive types
            if (!typeof(T).IsPrimitive)
            {
                // decimal is only non primitive numerical type in C# and needs to be converted manually
                if (typeof(T) == typeof(decimal))
                {
                    int[] bits = new int[4];
                    Buffer.BlockCopy(valueBytes, 0, bits, 0, Unsafe.SizeOf<decimal>());
                    decimal decodedValue = new Decimal(bits);
                    value = (T)(object)decodedValue;
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
                    long milliSeconds = BitConverter.ToInt64(valueBytes, 0);
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime decodedDateTime = epoch.AddTicks(milliSeconds * ticksPerMillisecond);
                    value = (T)(object)decodedDateTime;
                }
            }
            // every other numerical type is primitive
            else
            {
                value = Unsafe.As<byte, T>(ref valueBytes[0]);
            }

            return new HeaderWithValue(header, value);
        }

        public int GetLengthInBytes(T? value)
        {
            // for datetime return size of long
            if(typeof(T) == typeof(DateTime))
            {
                if (value == null)
                {
                    return sizeof(byte);
                }

                return sizeof(long) + sizeof(byte);
            }

            return Unsafe.SizeOf<T>() + sizeof(byte);
        }

        #region ISerializer implementation
        void ISerializer.Encode(List<byte> buffer, object? value, ref int offset, ref int offsetInLastByte)
        {
            Encode(buffer, (T?)value, ref offset, ref offsetInLastByte);
        }

        HeaderWithValue ISerializer.Decode(List<byte> buffer, ref int offset, ref int offsetInLastByte)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<T?>)this).Decode(buffer, ref offset, ref offsetInLastByte);
        }

        int ISerializer.GetLengthInBytes(object? value)
        {
            return GetLengthInBytes((T?)value);
        }
        #endregion
    }
}
