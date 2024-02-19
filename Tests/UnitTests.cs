using Xunit;
using JerqAggregatorNew.Schemas;
using System.Xml.Linq;
using System.Diagnostics;
using Xunit.Abstractions;
using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace JerqAggregatorNew.Tests
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
    }

    class Car
    {
        [BinarySerialize(include: true, key: false)]
        public double doubleNumber;

        [BinarySerialize(include: true, key: false)]
        public decimal DecimalNumber { get; set; }

        [BinarySerialize(include: true, key: false)]
        public string? StringName { get; set; }

        [BinarySerialize(include: true, key: false)]
        public DateTime? DateTimeDate { get; set; }
    }

    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();

                // Date is in UTC format and only milliseconds needs to be transmitted to the receiver side
                DateTime now = DateTime.UtcNow;
                long ticks = now.Ticks;
                long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
                DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

                Person person = new Person()
                {
                    IntNumber = 1,
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    BoolNumber = true,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka",

                    IntNumber2 = 1,
                    DecimalNumber2 = 1.5m,
                    doubleNumber2 = 2.5,
                    BoolNumber2 = true,
                    DateTimeDate2 = roundedDateTime,
                    StringName2 = "Luka",

                    IntNumber3 = 1,
                    DecimalNumber3 = 1.5m,
                    doubleNumber3 = 2.5,
                    BoolNumber3 = true,
                    DateTimeDate3 = roundedDateTime,
                    StringName3 = "Luka",

                    IntNumber4 = 1,
                    DecimalNumber4 = 1.5m,
                    doubleNumber4 = 2.5,
                    BoolNumber4 = true,
                    DateTimeDate4 = roundedDateTime,
                    StringName4 = "Luka",

                    IntNumber5 = 1,
                    DecimalNumber5 = 1.5m,
                    doubleNumber5 = 2.5,
                    BoolNumber5 = true,
                    DateTimeDate5 = roundedDateTime,
                    StringName5 = "Luka"
                };

                Schema<Person> personSchema = SchemaFactory.GetSchema<Person>();

                stopwatch.Start();

                byte[] serializedData = personSchema.Serialize(person);

                Person deserializedPerson = personSchema.Deserialize(serializedData);

                stopwatch.Stop();
                output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(person.IntNumber, deserializedPerson.IntNumber);
                Assert.Equal(person.BoolNumber, deserializedPerson.BoolNumber);
                Assert.Equal(person.DecimalNumber, deserializedPerson.DecimalNumber);
                Assert.Equal(person.doubleNumber, deserializedPerson.doubleNumber);
                Assert.Equal(person.StringName, deserializedPerson.StringName);
                Assert.Equal(person.DateTimeDate, deserializedPerson.DateTimeDate);

                Assert.Equal(person.IntNumber2, deserializedPerson.IntNumber2);
                Assert.Equal(person.BoolNumber2, deserializedPerson.BoolNumber2);
                Assert.Equal(person.DecimalNumber2, deserializedPerson.DecimalNumber2);
                Assert.Equal(person.doubleNumber2, deserializedPerson.doubleNumber2);
                Assert.Equal(person.StringName2, deserializedPerson.StringName2);
                Assert.Equal(person.DateTimeDate2, deserializedPerson.DateTimeDate2);

                Assert.Equal(person.IntNumber3, deserializedPerson.IntNumber3);
                Assert.Equal(person.BoolNumber3, deserializedPerson.BoolNumber3);
                Assert.Equal(person.DecimalNumber3, deserializedPerson.DecimalNumber3);
                Assert.Equal(person.doubleNumber3, deserializedPerson.doubleNumber3);
                Assert.Equal(person.StringName3, deserializedPerson.StringName3);
                Assert.Equal(person.DateTimeDate3, deserializedPerson.DateTimeDate3);

                Assert.Equal(person.IntNumber4, deserializedPerson.IntNumber4);
                Assert.Equal(person.BoolNumber4, deserializedPerson.BoolNumber4);
                Assert.Equal(person.DecimalNumber4, deserializedPerson.DecimalNumber4);
                Assert.Equal(person.doubleNumber4, deserializedPerson.doubleNumber4);
                Assert.Equal(person.StringName4, deserializedPerson.StringName4);
                Assert.Equal(person.DateTimeDate4, deserializedPerson.DateTimeDate4);

                Assert.Equal(person.IntNumber5, deserializedPerson.IntNumber5);
                Assert.Equal(person.BoolNumber5, deserializedPerson.BoolNumber5);
                Assert.Equal(person.DecimalNumber5, deserializedPerson.DecimalNumber5);
                Assert.Equal(person.doubleNumber5, deserializedPerson.doubleNumber5);
                Assert.Equal(person.StringName5, deserializedPerson.StringName5);
                Assert.Equal(person.DateTimeDate5, deserializedPerson.DateTimeDate5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void SingleAndDifferenceSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                DateTime now = DateTime.UtcNow;
                long ticks = now.Ticks;
                long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
                DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

                Car carOld = new Car()
                {
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka",
                };

                Car carNew = new Car()
                {
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka2",
                };

                Schema<Car> carSchema = SchemaFactory.GetSchema<Car>();

                stopwatch.Start();

                for (int i = 0; i < 10000000; i++)
                {
                    byte[] serializedData = carSchema.Serialize(carOld);
                    Car deserializedCar = carSchema.Deserialize(serializedData);

                    byte[] serializedDataDifference = carSchema.Serialize(carOld, carNew);
                    Car deserializedCarDifference = carSchema.Deserialize(serializedDataDifference, carOld);
                }
                stopwatch.Stop();
                output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void TestDirect()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                carOld.DecimalNumber = 22.5m;
                var x = carOld.DecimalNumber;
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestReflection()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            Type carType = typeof(Car);
            PropertyInfo decimalNumberProperty = carType.GetProperty("DecimalNumber");
            FieldInfo doubleNumberProperty = carType.GetField("doubleNumber");

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                decimalNumberProperty.SetValue(carOld, 22.5m);
                double a = (double) doubleNumberProperty.GetValue(carOld);
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestLambda()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            var getter = (Car carold) => carold.DecimalNumber;
            var setter = (Car carold, decimal value) => carold.DecimalNumber = value;

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                getter(carOld);
                setter(carOld, 22.5m);
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestDelegate()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            var carType = carOld.GetType();
            var getMethod = carType.GetProperty("DecimalNumber")!.GetGetMethod()!.CreateDelegate<Func<Car, decimal>>()!;
            var setMethod = carType.GetProperty("DecimalNumber")!.GetSetMethod()!.CreateDelegate<Action<Car, decimal>>()!;

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                setMethod(carOld, 22.5m);
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestDelegateGetterSetter()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };


            var carType = carOld.GetType();
            var getMethod = SchemaFactory.GenerateGetter<Car>(carType.GetField("doubleNumber"));
            var setMethod = SchemaFactory.GenerateSetter<Car>(carType.GetProperty("DecimalNumber"));


            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                setMethod(carOld, 22.5m);
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }
    }
}