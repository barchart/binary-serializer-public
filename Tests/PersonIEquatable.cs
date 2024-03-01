using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class PersonIEquatable : IEquatable<PersonIEquatable>
    {
        [BinarySerialize(include: true, key: false)]
        public int? IntNumber { get; set; }

        [BinarySerialize(include: true, key: true)]
        public bool? BoolNumber { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumber;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate { get; set; }

        [BinarySerialize(include: true, key: false)]
        public int? IntNumber2 { get; set; }

        [BinarySerialize(include: true, key: true)]
        public bool? BoolNumber2 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumber2;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber2 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName2 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate2 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public int? IntNumber3 { get; set; }

        [BinarySerialize(include: true, key: true)]
        public bool? BoolNumber3 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumber3;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber3 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName3 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate3 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public int? IntNumber4 { get; set; }

        [BinarySerialize(include: true, key: true)]
        public bool? BoolNumber4 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumber4;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber4 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName4 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate4 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public int? IntNumber5 { get; set; }

        [BinarySerialize(include: true, key: true)]
        public bool? BoolNumber5 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public double doubleNumber5;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber5 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName5 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate5 { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateOnly? DateOnly { get; set; }

        public bool Equals(PersonIEquatable? other)
        {
            if (other == null) return false;

            return IntNumber == other.IntNumber &&
                   DecimalNumber == other.DecimalNumber &&
                   doubleNumber == other.doubleNumber &&
                   BoolNumber == other.BoolNumber &&
                   DateTimeDate == other.DateTimeDate &&
                   StringName == other.StringName &&
                   IntNumber2 == other.IntNumber2 &&
                   DecimalNumber2 == other.DecimalNumber2 &&
                   doubleNumber2 == other.doubleNumber2 &&
                   BoolNumber2 == other.BoolNumber2 &&
                   DateTimeDate2 == other.DateTimeDate2 &&
                   StringName2 == other.StringName2 &&
                   IntNumber3 == other.IntNumber3 &&
                   DecimalNumber3 == other.DecimalNumber3 &&
                   doubleNumber3 == other.doubleNumber3 &&
                   BoolNumber3 == other.BoolNumber3 &&
                   DateTimeDate3 == other.DateTimeDate3 &&
                   StringName3 == other.StringName3 &&
                   IntNumber4 == other.IntNumber4 &&
                   DecimalNumber4 == other.DecimalNumber4 &&
                   doubleNumber4 == other.doubleNumber4 &&
                   BoolNumber4 == other.BoolNumber4 &&
                   DateTimeDate4 == other.DateTimeDate4 &&
                   StringName4 == other.StringName4 &&
                   IntNumber5 == other.IntNumber5 &&
                   DecimalNumber5 == other.DecimalNumber5 &&
                   doubleNumber5 == other.doubleNumber5 &&
                   BoolNumber5 == other.BoolNumber5 &&
                   DateTimeDate5 == other.DateTimeDate5 &&
                   StringName5 == other.StringName5 &&
                   DateOnly == other.DateOnly;
        }
    }
}

