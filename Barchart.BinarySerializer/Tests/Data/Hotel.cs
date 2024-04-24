using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class Hotel
    {
        [BinarySerialize(key: false)]
        public List<int>? RoomNumbers { get; set; }

        [BinarySerialize(key: false)]
        public string? Data { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Hotel otherHotel = (Hotel)obj;

            if ((RoomNumbers?.Count ?? 0) != (otherHotel.RoomNumbers?.Count ?? 0))
            {
                return false;
            }

            if (RoomNumbers != null)
            {
                for (int i = 0; i < RoomNumbers.Count; i++)
                {
                    if (RoomNumbers[i] != otherHotel?.RoomNumbers?[i])
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
                hashCode += RoomNumbers != null ? RoomNumbers.GetHashCode() : 0;
                hashCode += Data != null ? Data.GetHashCode() : 0;
                return hashCode;
            }
        }
    }
}