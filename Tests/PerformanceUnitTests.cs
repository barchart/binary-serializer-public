using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    public class PerformanceUnitTests
    {
        private readonly ITestOutputHelper output;

        public PerformanceUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DirectAccessTest()
        {
            int iterations = int.MaxValue / 2;

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

            stopwatch.Start();

            for (long i = 0; i < iterations; i++)
            {
                carOld.DecimalNumber = 22.5m;
                _ = carOld.DecimalNumber;
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestReflection()
        {
            int iterations = int.MaxValue / 2;

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

            Type carType = typeof(Car);
            PropertyInfo? decimalNumberProperty = carType.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberProperty = carType.GetField("doubleNumber");

            stopwatch.Start();

            for (long i = 0; i < iterations; i++)
            {
                decimalNumberProperty?.SetValue(carOld, 22.5m);
                _ = (double?)doubleNumberProperty?.GetValue(carOld);
            }
            stopwatch.Stop();

            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void LambdaTest()
        {
            int iterations = int.MaxValue / 2;

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

            var getter = (Car carold) => carold.DecimalNumber;
            var setter = (Car carold, decimal value) => carold.DecimalNumber = value;

            stopwatch.Start();

            for (long i = 0; i < iterations; i++)
            {
                getter(carOld);
                setter(carOld, 22.5m);
            }
            stopwatch.Stop();

            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void DelegateTest()
        {
            int iterations = int.MaxValue / 2;

            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

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

            stopwatch.Start();

            for (long i = 0; i < iterations; i++)
            {
                setMethod(carOld, 22.5m);
                _ = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void DelegateGetterSetterTest()
        {
            int iterations = int.MaxValue / 2;
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

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

            stopwatch.Start();

            for (long i = 0; i < iterations; i++)
            {
                setMethod(carOld, 22.5m);

                _ = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void DelegatesPerformanceTest()
        {
            int iterations = 10000000;

            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();

            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

            Person person1 = new Person()
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
            Car car1 = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
                PersonObjectInCar = person1
            };

            Car car2 = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
                PersonObjectInCar = person2
            };

            var carType1 = car1.GetType();
            PropertyInfo? decimalNumberInfo = carType1.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo = carType1.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo = carType1.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo = carType1.GetProperty("StringName");
            PropertyInfo? personObjectInCarPropertyInfo = carType1.GetProperty("PersonObjectInCar");

            if (doubleNumberInfo == null || decimalNumberInfo == null || dateTimeDateInfo == null || stringNameInfo == null || personObjectInCarPropertyInfo == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getDecimalNumberMethod = SchemaFactory.GenerateGetter<Car>(decimalNumberInfo);
            Func<Car, object?> getDoubleNumberMethod = SchemaFactory.GenerateGetter<Car>(doubleNumberInfo);
            Func<Car, object?> getDateTimeNumberMethod = SchemaFactory.GenerateGetter<Car>(dateTimeDateInfo);
            Func<Car, object?> getStringNumberMethod = SchemaFactory.GenerateGetter<Car>(stringNameInfo);
            Func<Car, object?> getPersonObjectInCarPropertyInfo = SchemaFactory.GenerateGetter<Car>(personObjectInCarPropertyInfo);

            var carType2 = car2.GetType();
            PropertyInfo? decimalNumberInfo2 = carType2.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo2 = carType2.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo2 = carType2.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo2 = carType2.GetProperty("StringName");
            PropertyInfo? personObjectInCarPropertyInfo2 = carType2.GetProperty("PersonObjectInCar");

            if (doubleNumberInfo2 == null || decimalNumberInfo2 == null || dateTimeDateInfo2 == null || stringNameInfo2 == null || personObjectInCarPropertyInfo2 == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getDecimalNumberMethod2 = SchemaFactory.GenerateGetter<Car>(decimalNumberInfo2);
            Func<Car, object?> getDoubleNumberMethod2 = SchemaFactory.GenerateGetter<Car>(doubleNumberInfo2);
            Func<Car, object?> getDateTimeNumberMethod2 = SchemaFactory.GenerateGetter<Car>(dateTimeDateInfo2);
            Func<Car, object?> getStringNumberMethod2 = SchemaFactory.GenerateGetter<Car>(stringNameInfo2);
            Func<Car, object?> getPersonObjectInCarPropertyInfo2 = SchemaFactory.GenerateGetter<Car>(personObjectInCarPropertyInfo2);

            stopwatch1.Start();

            for (long i = 0; i < iterations; i++)
            {
                _ = (decimal)getDecimalNumberMethod(car1)! == (decimal)getDecimalNumberMethod2(car2)! &&
                (double)getDoubleNumberMethod(car1)! == (double)getDoubleNumberMethod2(car2)! &&
                (DateTime)getDateTimeNumberMethod(car1)! == (DateTime)getDateTimeNumberMethod2(car2)! &&
                (string)getStringNumberMethod(car1)! == (string)getStringNumberMethod2(car2)! &&
                getPersonObjectInCarPropertyInfo(car1)!.Equals(getPersonObjectInCarPropertyInfo2(car2));
            }

            stopwatch1.Stop();

            stopwatch2.Start();
            for (long i = 0; i < iterations; i++)
            {
                _ = car1.Equals(car2);
            }
            stopwatch2.Stop();

            output.WriteLine($"Using Delegates: {stopwatch1.ElapsedTicks} ticks");
            output.WriteLine($"Using Equals: {stopwatch2.ElapsedTicks} ticks");

        }

        [Fact]
        public void DelegatesIEquatablePerformanceTest()
        {
            int iterations = 10000000;

            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();

            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

            PersonIEquatable person1 = new PersonIEquatable()
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

            PersonIEquatable person2 = new PersonIEquatable()
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
            CarIEquatable car1 = new CarIEquatable()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
                PersonObjectInCar = person1
            };

            CarIEquatable car2 = new CarIEquatable()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
                PersonObjectInCar = person2
            };

            var carType1 = car1.GetType();
            PropertyInfo? decimalNumberInfo = carType1.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo = carType1.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo = carType1.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo = carType1.GetProperty("StringName");
            PropertyInfo? personObjectInCarPropertyInfo = carType1.GetProperty("PersonObjectInCar");

            if (doubleNumberInfo == null || decimalNumberInfo == null || dateTimeDateInfo == null || stringNameInfo == null || personObjectInCarPropertyInfo == null)
            {
                Assert.Fail();
                return;
            }

            Func<CarIEquatable, object?> getDecimalNumberMethod = SchemaFactory.GenerateGetter<CarIEquatable>(decimalNumberInfo);
            Func<CarIEquatable, object?> getDoubleNumberMethod = SchemaFactory.GenerateGetter<CarIEquatable>(doubleNumberInfo);
            Func<CarIEquatable, object?> getDateTimeNumberMethod = SchemaFactory.GenerateGetter<CarIEquatable>(dateTimeDateInfo);
            Func<CarIEquatable, object?> getStringNumberMethod = SchemaFactory.GenerateGetter<CarIEquatable>(stringNameInfo);
            Func<CarIEquatable, object?> getPersonObjectInCarPropertyInfo = SchemaFactory.GenerateGetter<CarIEquatable>(personObjectInCarPropertyInfo);

            var carType2 = car2.GetType();
            PropertyInfo? decimalNumberInfo2 = carType2.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo2 = carType2.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo2 = carType2.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo2 = carType2.GetProperty("StringName");
            PropertyInfo? personObjectInCarPropertyInfo2 = carType2.GetProperty("PersonObjectInCar");

            if (doubleNumberInfo2 == null || decimalNumberInfo2 == null || dateTimeDateInfo2 == null || stringNameInfo2 == null || personObjectInCarPropertyInfo2 == null)
            {
                Assert.Fail();
                return;
            }

            Func<CarIEquatable, object?> getDecimalNumberMethod2 = SchemaFactory.GenerateGetter<CarIEquatable>(decimalNumberInfo2);
            Func<CarIEquatable, object?> getDoubleNumberMethod2 = SchemaFactory.GenerateGetter<CarIEquatable>(doubleNumberInfo2);
            Func<CarIEquatable, object?> getDateTimeNumberMethod2 = SchemaFactory.GenerateGetter<CarIEquatable>(dateTimeDateInfo2);
            Func<CarIEquatable, object?> getStringNumberMethod2 = SchemaFactory.GenerateGetter<CarIEquatable>(stringNameInfo2);
            Func<CarIEquatable, object?> getPersonObjectInCarPropertyInfo2 = SchemaFactory.GenerateGetter<CarIEquatable>(personObjectInCarPropertyInfo2);

            stopwatch1.Start();

            for (long i = 0; i < iterations; i++)
            {
                _ = (decimal)getDecimalNumberMethod(car1)! == (decimal)getDecimalNumberMethod2(car2)! &&
                (double)getDoubleNumberMethod(car1)! == (double)getDoubleNumberMethod2(car2)! &&
                (DateTime)getDateTimeNumberMethod(car1)! == (DateTime)getDateTimeNumberMethod2(car2)! &&
                (string)getStringNumberMethod(car1)! == (string)getStringNumberMethod2(car2)! &&
                ((PersonIEquatable)getPersonObjectInCarPropertyInfo(car1)!).Equals((PersonIEquatable)getPersonObjectInCarPropertyInfo2(car2)!);
            }

            stopwatch1.Stop();

            stopwatch2.Start();
            for (long i = 0; i < iterations; i++)
            {
                _ = car1.Equals(car2);
            }
            stopwatch2.Stop();

            output.WriteLine($"Using Delegates: {stopwatch1.ElapsedTicks} ticks");
            output.WriteLine($"Using Equals: {stopwatch2.ElapsedTicks} ticks");

        }
    }
}

