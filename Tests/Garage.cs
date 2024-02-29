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
        public double doubleNumberGarage;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumberGarage { get; set; }

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

            return CarObject.Equals(otherGarage.CarObject) &&
                   PersonObject.Equals(otherGarage.PersonObject) &&
                   doubleNumberGarage == otherGarage.doubleNumberGarage &&
                   DecimalNumberGarage == otherGarage.DecimalNumberGarage &&
                   StringNameGarage == otherGarage.StringNameGarage &&
                   DateTimeDateGarage == otherGarage.DateTimeDateGarage;
        }

    }
}

