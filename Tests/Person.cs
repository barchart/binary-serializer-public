using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class Person
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

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherPerson = (Person)obj;

            return IntNumber == otherPerson.IntNumber &&
                   BoolNumber == otherPerson.BoolNumber &&
                   doubleNumber == otherPerson.doubleNumber &&
                   DecimalNumber == otherPerson.DecimalNumber &&
                   StringName == otherPerson.StringName &&
                   DateTimeDate == otherPerson.DateTimeDate &&
                   IntNumber2 == otherPerson.IntNumber2 &&
                   BoolNumber2 == otherPerson.BoolNumber2 &&
                   doubleNumber2 == otherPerson.doubleNumber2 &&
                   DecimalNumber2 == otherPerson.DecimalNumber2 &&
                   StringName2 == otherPerson.StringName2 &&
                   DateTimeDate2 == otherPerson.DateTimeDate2 &&
                   IntNumber3 == otherPerson.IntNumber3 &&
                   BoolNumber3 == otherPerson.BoolNumber3 &&
                   doubleNumber3 == otherPerson.doubleNumber3 &&
                   DecimalNumber3 == otherPerson.DecimalNumber3 &&
                   StringName3 == otherPerson.StringName3 &&
                   DateTimeDate3 == otherPerson.DateTimeDate3 &&
                   IntNumber4 == otherPerson.IntNumber4 &&
                   BoolNumber4 == otherPerson.BoolNumber4 &&
                   doubleNumber4 == otherPerson.doubleNumber4 &&
                   DecimalNumber4 == otherPerson.DecimalNumber4 &&
                   StringName4 == otherPerson.StringName4 &&
                   DateTimeDate4 == otherPerson.DateTimeDate4 &&
                   IntNumber5 == otherPerson.IntNumber5 &&
                   BoolNumber5 == otherPerson.BoolNumber5 &&
                   doubleNumber5 == otherPerson.doubleNumber5 &&
                   DecimalNumber5 == otherPerson.DecimalNumber5 &&
                   StringName5 == otherPerson.StringName5 &&
                   DateTimeDate5 == otherPerson.DateTimeDate5 &&
                   DateOnly == otherPerson.DateOnly;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                hashCode += IntNumber.GetHashCode();
                hashCode += BoolNumber.GetHashCode();
                hashCode += doubleNumber.GetHashCode();
                hashCode += DecimalNumber.GetHashCode();
                hashCode += (StringName != null ? StringName.GetHashCode() : 0);
                hashCode += DateTimeDate.GetHashCode();
                hashCode += (IntNumber2 != null ? IntNumber2.GetHashCode() : 0);
                hashCode += BoolNumber2.GetHashCode();
                hashCode += doubleNumber2.GetHashCode();
                hashCode += DecimalNumber2.GetHashCode();
                hashCode += (StringName2 != null ? StringName2.GetHashCode() : 0);
                hashCode += DateTimeDate2.GetHashCode();
                hashCode += (IntNumber3 != null ? IntNumber3.GetHashCode() : 0);
                hashCode += BoolNumber3.GetHashCode();
                hashCode += doubleNumber3.GetHashCode();
                hashCode += DecimalNumber3.GetHashCode();
                hashCode += (StringName3 != null ? StringName3.GetHashCode() : 0);
                hashCode += DateTimeDate3.GetHashCode();
                hashCode += (IntNumber4 != null ? IntNumber4.GetHashCode() : 0);
                hashCode += BoolNumber4.GetHashCode();
                hashCode += doubleNumber4.GetHashCode();
                hashCode += DecimalNumber4.GetHashCode();
                hashCode += (StringName4 != null ? StringName4.GetHashCode() : 0);
                hashCode += DateTimeDate4.GetHashCode();
                hashCode += (IntNumber5 != null ? IntNumber5.GetHashCode() : 0);
                hashCode += BoolNumber5.GetHashCode();
                hashCode += doubleNumber5.GetHashCode();
                hashCode += DecimalNumber5.GetHashCode();
                hashCode += (StringName5 != null ? StringName5.GetHashCode() : 0);
                hashCode += DateTimeDate5.GetHashCode();
                hashCode += (DateOnly != null ? DateOnly.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}

