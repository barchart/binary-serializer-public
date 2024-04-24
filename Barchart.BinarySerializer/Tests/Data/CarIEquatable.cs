using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class CarIEquatable : IEquatable<CarIEquatable>
    {
        [BinarySerialize(key: false)]
        public double doubleNumber;

        [BinarySerialize(key: false)]
        public decimal DecimalNumber { get; set; }

        [BinarySerialize(key: false)]
        public string? StringName { get; set; }

        [BinarySerialize(key: false)]
        public DateTime? DateTimeDate { get; set; }

        [BinarySerialize(key: false)]
        public sbyte? SByte { get; set; }

        [BinarySerialize(key: false)]
        public byte? Byte { get; set; }

        [BinarySerialize(key: false)]
        public PersonIEquatable? PersonObjectInCar { get; set; }

        public bool Equals(CarIEquatable? other)
        {
            if (other == null) return false;

            return doubleNumber == other.doubleNumber &&
                DecimalNumber == other.DecimalNumber &&
                StringName == other.StringName &&
                DateTimeDate == other.DateTimeDate &&
                SByte == other.SByte &&
                Byte == other.Byte &&
                (PersonObjectInCar?.Equals(other.PersonObjectInCar) ?? other.PersonObjectInCar == null);
        }
    }
}