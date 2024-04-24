using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class GarageIEquatable : IEquatable<GarageIEquatable>
    {
        [BinarySerialize(key: false)]
        public CarIEquatable? CarObject { get; set; }

        [BinarySerialize(key: false)]
        public PersonIEquatable? PersonObject { get; set; }

        [BinarySerialize(key: false)]
        public double doubleNumberGarage = 0;

        [BinarySerialize(key: false)]
        public decimal DecimalNumberGarage { get; set; }

        [BinarySerialize(key: false)]
        public string? StringNameGarage { get; set; }

        [BinarySerialize(key: false)]
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