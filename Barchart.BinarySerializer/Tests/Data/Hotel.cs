using Barchart.BinarySerializer.Schemas;
using Google.Protobuf;

namespace Barchart.BinarySerializer.Tests
{
    class Hotel
    {
        [BinarySerialize(include: true, key: false)]
        public List<int>? roomNumbers { get; set; }

        [BinarySerialize(include: true, key: false)]
        public ByteString? Data { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Hotel otherHotel = (Hotel)obj;

            if ((roomNumbers?.Count ?? 0) != (otherHotel.roomNumbers?.Count ?? 0))
            {
                return false;
            }

            if (roomNumbers != null)
            {
                for (int i = 0; i < roomNumbers.Count; i++)
                {
                    if (roomNumbers[i] != otherHotel?.roomNumbers?[i])
                    {
                        return false;
                    }
                }
            }

            return Data == otherHotel.Data;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                hashCode += roomNumbers != null ? roomNumbers.GetHashCode() : 0;
                hashCode += Data != null ? Data.GetHashCode() : 0;
                return hashCode;
            }
        }
    }
}