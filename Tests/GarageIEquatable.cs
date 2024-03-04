using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class GarageIEquatable : IEquatable<GarageIEquatable>
    {
        [BinarySerialize(include: true, key: false)]
        public CarIEquatable? CarObject { get; set; }

        [BinarySerialize(include: true, key: false)]
        public PersonIEquatable? PersonObject { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumberGarage = 0;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumberGarage { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringNameGarage { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDateGarage { get; set; }

        public bool Equals(GarageIEquatable? other)
        {
            if (other == null) return false;

            return (CarObject?.Equals(other.CarObject) ?? other.CarObject == null) &&
                   (PersonObject?.Equals(other.PersonObject) ?? other.PersonObject == null) &&
                   doubleNumberGarage == other.doubleNumberGarage &&
                   DecimalNumberGarage == other.DecimalNumberGarage &&
                   StringNameGarage == other.StringNameGarage &&
                   DateTimeDateGarage == other.DateTimeDateGarage;
        }
    }
}

