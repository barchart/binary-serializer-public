using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class Car
    {
        [BinarySerialize(include: true, key: false)]
        public double? doubleNumber;

        [BinarySerialize(include: true, key: false)]
        public decimal? DecimalNumber { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate { get; set; }

        [BinarySerialize(include: true, key: false)]
        public sbyte? SByte { get; set; }

        [BinarySerialize(include: true, key: false)]
        public byte? Byte { get; set; }

        [BinarySerialize(include: true, key: false)]
        public Person? PersonObjectInCar { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherCar = (Car)obj;

            return doubleNumber == otherCar.doubleNumber &&
                   DecimalNumber == otherCar.DecimalNumber &&
                   StringName == otherCar.StringName &&
                   DateTimeDate == otherCar.DateTimeDate &&
                   SByte == otherCar.SByte &&
                   Byte == otherCar.Byte &&
                   (PersonObjectInCar?.Equals(otherCar.PersonObjectInCar) ?? otherCar.PersonObjectInCar == null);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return doubleNumber.GetHashCode() + DecimalNumber.GetHashCode() + (StringName != null ? StringName.GetHashCode() : 0) + DateTimeDate.GetHashCode() +
                    SByte.GetHashCode() + Byte.GetHashCode() + (PersonObjectInCar != null ? PersonObjectInCar.GetHashCode() : 0);
            }
        }
    }
}