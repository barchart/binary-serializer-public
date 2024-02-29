using Xunit;
using Barchart.BinarySerializer.Schemas;
using System.Diagnostics;
using Xunit.Abstractions;
using System.Reflection;

namespace Barchart.BinarySerializer.Tests
{
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
                    StringName = null,

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
                    StringName5 = "Luka",

                    DateOnly = new DateOnly(2022, 2, 14)
                };

                Schema<Person> personSchema = SchemaFactory.GetSchema<Person>();

                stopwatch.Start();

                byte[] serializedData = personSchema.Serialize(person);

                Person deserializedPerson = personSchema.Deserialize(serializedData);

                stopwatch.Stop();
                output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(person, deserializedPerson);
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
                    Byte = (byte)1,
                    sByte = (sbyte)2
                };

                Car carNew = new Car()
                {
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka2",
                    Byte = (byte)2,
                    sByte = (sbyte)-2
                };

                Schema<Car> carSchema = SchemaFactory.GetSchema<Car>();

                stopwatch.Start();

                for (int i = 0; i < 1000; i++)
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
            PropertyInfo? decimalNumberProperty = carType.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberProperty = carType.GetField("doubleNumber");

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                decimalNumberProperty?.SetValue(carOld, 22.5m);
                double? a = (double?)doubleNumberProperty?.GetValue(carOld);
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
                StringName = "Luka"
            };

            var carType = carOld.GetType();
            FieldInfo? fieldInfo = carType.GetField("doubleNumber");
            PropertyInfo? propertyInfo = carType.GetProperty("DecimalNumber");

            if (fieldInfo == null || propertyInfo == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getMethod = SchemaFactory.GenerateGetter<Car>(fieldInfo);
            Action<Car, object?> setMethod = SchemaFactory.GenerateSetter<Car>(propertyInfo);

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                setMethod(carOld, 22.5m);
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestClassProperties()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Person person = new Person()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

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
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Car car = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person
            };

            Garage garage = new Garage()
            {
                CarObject = car,
                PersonObject = person,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 12.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d
            };

            Schema<Garage> garageSchema = SchemaFactory.GetSchema<Garage>();

            stopwatch.Start();

            byte[] serializedData = garageSchema.Serialize(garage);

            Garage deserializedGarage = garageSchema.Deserialize(serializedData);

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            Assert.Equal(garage?.PersonObject?.IntNumber, deserializedGarage?.PersonObject?.IntNumber);
            Assert.Equal(garage?.PersonObject?.BoolNumber, deserializedGarage?.PersonObject?.BoolNumber);
            Assert.Equal(garage?.PersonObject?.DecimalNumber, deserializedGarage?.PersonObject?.DecimalNumber);
            Assert.Equal(garage?.PersonObject?.doubleNumber, deserializedGarage?.PersonObject?.doubleNumber);
            Assert.Equal(garage?.PersonObject?.StringName, deserializedGarage?.PersonObject?.StringName);
            Assert.Equal(garage?.PersonObject?.DateTimeDate, deserializedGarage?.PersonObject?.DateTimeDate);

            Assert.Equal(garage?.PersonObject?.IntNumber2, deserializedGarage?.PersonObject?.IntNumber2);
            Assert.Equal(garage?.PersonObject?.BoolNumber2, deserializedGarage?.PersonObject?.BoolNumber2);
            Assert.Equal(garage?.PersonObject?.DecimalNumber2, deserializedGarage?.PersonObject?.DecimalNumber2);
            Assert.Equal(garage?.PersonObject?.doubleNumber2, deserializedGarage?.PersonObject?.doubleNumber2);
            Assert.Equal(garage?.PersonObject?.StringName2, deserializedGarage?.PersonObject?.StringName2);
            Assert.Equal(garage?.PersonObject?.DateTimeDate2, deserializedGarage?.PersonObject?.DateTimeDate2);

            Assert.Equal(garage?.PersonObject?.IntNumber3, deserializedGarage?.PersonObject?.IntNumber3);
            Assert.Equal(garage?.PersonObject?.BoolNumber3, deserializedGarage?.PersonObject?.BoolNumber3);
            Assert.Equal(garage?.PersonObject?.DecimalNumber3, deserializedGarage?.PersonObject?.DecimalNumber3);
            Assert.Equal(garage?.PersonObject?.doubleNumber3, deserializedGarage?.PersonObject?.doubleNumber3);
            Assert.Equal(garage?.PersonObject?.StringName3, deserializedGarage?.PersonObject?.StringName3);
            Assert.Equal(garage?.PersonObject?.DateTimeDate3, deserializedGarage?.PersonObject?.DateTimeDate3);

            Assert.Equal(person?.IntNumber4, deserializedGarage?.PersonObject?.IntNumber4);
            Assert.Equal(person?.BoolNumber4, deserializedGarage?.PersonObject?.BoolNumber4);
            Assert.Equal(person?.DecimalNumber4, deserializedGarage?.PersonObject?.DecimalNumber4);
            Assert.Equal(person?.doubleNumber4, deserializedGarage?.PersonObject?.doubleNumber4);
            Assert.Equal(person?.StringName4, deserializedGarage?.PersonObject?.StringName4);
            Assert.Equal(person?.DateTimeDate4, deserializedGarage?.PersonObject?.DateTimeDate4);

            Assert.Equal(garage?.PersonObject?.IntNumber5, deserializedGarage?.PersonObject?.IntNumber5);
            Assert.Equal(garage?.PersonObject?.BoolNumber5, deserializedGarage?.PersonObject?.BoolNumber5);
            Assert.Equal(garage?.PersonObject?.DecimalNumber5, deserializedGarage?.PersonObject?.DecimalNumber5);
            Assert.Equal(garage?.PersonObject?.doubleNumber5, deserializedGarage?.PersonObject?.doubleNumber5);
            Assert.Equal(garage?.PersonObject?.StringName5, deserializedGarage?.PersonObject?.StringName5);
            Assert.Equal(garage?.PersonObject?.DateTimeDate5, deserializedGarage?.PersonObject?.DateTimeDate5);

            Assert.Equal(garage?.PersonObject?.DateOnly, deserializedGarage?.PersonObject?.DateOnly);

            Assert.Equal(garage?.CarObject?.DateTimeDate, deserializedGarage?.CarObject?.DateTimeDate);
            Assert.Equal(garage?.CarObject?.DecimalNumber, deserializedGarage?.CarObject?.DecimalNumber);
            Assert.Equal(garage?.CarObject?.doubleNumber, deserializedGarage?.CarObject?.doubleNumber);
            Assert.Equal(garage?.CarObject?.StringName, deserializedGarage?.CarObject?.StringName);

            Assert.Equal(garage?.DateTimeDateGarage, deserializedGarage?.DateTimeDateGarage);
            Assert.Equal(garage?.DecimalNumberGarage, deserializedGarage?.DecimalNumberGarage);
            Assert.Equal(garage?.doubleNumberGarage, deserializedGarage?.doubleNumberGarage);
            Assert.Equal(garage?.StringNameGarage, deserializedGarage?.StringNameGarage);
        }

        [Fact]
        public void TestClassPropertiesDifference()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Person person = new Person()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

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
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Person person2 = new Person()
            {
                IntNumber = 4,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

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
                StringName4 = "Luka22",

                IntNumber5 = 1,
                DecimalNumber5 = 1.5m,
                doubleNumber5 = 10.5,
                BoolNumber5 = false,
                DateTimeDate5 = roundedDateTime,
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 15)
            };

            Person person3 = new Person()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

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
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Person person4 = new Person()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

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
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Car car = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person3
            };

            Car car2 = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person4
            };

            Garage garage = new Garage()
            {
                CarObject = car,
                PersonObject = person,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 12.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d
            };

            Garage garage2 = new Garage()
            {
                CarObject = car2,
                PersonObject = person2,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 122.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d,
            };

            Schema<Garage> garageSchema = SchemaFactory.GetSchema<Garage>();

            stopwatch.Start();

            byte[] serializedDataDifference = garageSchema.Serialize(garage, garage2);
            Garage deserializedGarageDifference = garageSchema.Deserialize(serializedDataDifference, garage);

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }
    }
}