using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class Hotel
    {
        [BinarySerialize(include: true, key: false)]
        public List<string>? roomNumbers;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Hotel otherHotel = (Hotel)obj;

            if (roomNumbers?.Count != otherHotel.roomNumbers?.Count)
            {
                return false;
            }

            for (int i = 0; i < roomNumbers?.Count; i++)
            {
                if (roomNumbers[i] != otherHotel?.roomNumbers?[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return roomNumbers != null? roomNumbers.GetHashCode() : 0;
            }
        }
    }
}