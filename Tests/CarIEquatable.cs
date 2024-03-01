using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class CarIEquatable : IEquatable<CarIEquatable>
    {
        [BinarySerialize(include: true, key: false)]
        public double doubleNumber;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate { get; set; }

        [BinarySerialize(include: true, key: false)]
        public sbyte? sByte { get; set; }

        [BinarySerialize(include: true, key: false)]
        public byte? Byte { get; set; }

        [BinarySerialize(include: true, key: false)]
        public PersonIEquatable? PersonObjectInCar { get; set; }

        public bool Equals(CarIEquatable? other)
        {
            if (other == null) return false;

            return doubleNumber == other.doubleNumber &&
                DecimalNumber == other.DecimalNumber &&
                StringName == other.StringName &&
                DateTimeDate == other.DateTimeDate &&
                sByte == other.sByte &&
                Byte == other.Byte &&
                (PersonObjectInCar?.Equals(other.PersonObjectInCar) ?? other.PersonObjectInCar == null);
        }
    }
}

