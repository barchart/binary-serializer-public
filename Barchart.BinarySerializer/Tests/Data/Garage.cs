using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class Garage
    {
        [BinarySerialize(include: true, key: false)]
        public Car? CarObject { get; set; }

        [BinarySerialize(include: true, key: false)]
        public Person? PersonObject { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double? doubleNumberGarage;

        [BinarySerialize(include: true, key: false)]
        public decimal? DecimalNumberGarage { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringNameGarage { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDateGarage { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherGarage = (Garage)obj;

            return (CarObject?.Equals(otherGarage.CarObject) ?? otherGarage.CarObject == null) &&
                   (PersonObject?.Equals(otherGarage.PersonObject) ?? otherGarage.PersonObject == null) &&
                   doubleNumberGarage == otherGarage.doubleNumberGarage &&
                   DecimalNumberGarage == otherGarage.DecimalNumberGarage &&
                   StringNameGarage == otherGarage.StringNameGarage &&
                   DateTimeDateGarage == otherGarage.DateTimeDateGarage;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return doubleNumberGarage.GetHashCode() + DecimalNumberGarage.GetHashCode() + (StringNameGarage != null ? StringNameGarage.GetHashCode() : 0) +
                   (DateTimeDateGarage != null ? DateTimeDateGarage.GetHashCode() : 0) + (CarObject != null ? CarObject.GetHashCode() : 0) + (PersonObject != null ? PersonObject.GetHashCode() : 0);
            }
        }
    }
}