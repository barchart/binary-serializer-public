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
                var x = carOld.DecimalNumber;
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
                double? a = (double?)doubleNumberProperty?.GetValue(carOld);
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
                var x = getMethod(carOld);
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
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void DelegatesPerformanceTest()
        {
            int iterations = 100000000;

            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();

            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

            Car car1 = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            Car car2 = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            var carType1 = car1.GetType();
            PropertyInfo? decimalNumberInfo = carType1.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo = carType1.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo = carType1.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo = carType1.GetProperty("StringName");

            if (doubleNumberInfo == null || decimalNumberInfo == null || dateTimeDateInfo == null || stringNameInfo == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getDecimalNumberMethod = SchemaFactory.GenerateGetter<Car>(decimalNumberInfo);
            Func<Car, object?> getDoubleNumberMethod = SchemaFactory.GenerateGetter<Car>(doubleNumberInfo);
            Func<Car, object?> getDateTimeNumberMethod = SchemaFactory.GenerateGetter<Car>(dateTimeDateInfo);
            Func<Car, object?> getStringNumberMethod = SchemaFactory.GenerateGetter<Car>(stringNameInfo);
         
            var carType2 = car2.GetType();
            PropertyInfo? decimalNumberInfo2 = carType2.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberInfo2 = carType2.GetField("doubleNumber");
            PropertyInfo? dateTimeDateInfo2 = carType2.GetProperty("DateTimeDate");
            PropertyInfo? stringNameInfo2 = carType2.GetProperty("StringName");

            if (doubleNumberInfo2 == null || decimalNumberInfo2 == null || dateTimeDateInfo2 == null || stringNameInfo2 == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getDecimalNumberMethod2 = SchemaFactory.GenerateGetter<Car>(decimalNumberInfo2);
            Func<Car, object?> getDoubleNumberMethod2 = SchemaFactory.GenerateGetter<Car>(doubleNumberInfo2);
            Func<Car, object?> getDateTimeNumberMethod2 = SchemaFactory.GenerateGetter<Car>(dateTimeDateInfo2);
            Func<Car, object?> getStringNumberMethod2 = SchemaFactory.GenerateGetter<Car>(stringNameInfo2);

            stopwatch1.Start();

            for (long i = 0; i < iterations; i++)
            {
                _ = getDecimalNumberMethod(car1) == getDecimalNumberMethod2(car2) &&
                getDoubleNumberMethod(car1) == getDoubleNumberMethod2(car2) &&
                getDateTimeNumberMethod(car1) == getDateTimeNumberMethod2(car2) &&
                getStringNumberMethod(car1) == getStringNumberMethod2(car2);
            }

            stopwatch1.Stop();

            stopwatch2.Start();
            for (long i = 0; i < iterations; i++)
            {
                car1.Equals(car2);
            }
            stopwatch2.Stop();

            output.WriteLine($"Using Delegates: {stopwatch1.ElapsedTicks} ticks");
            output.WriteLine($"Using Equals: {stopwatch2.ElapsedTicks} ticks");

        }
    }
}

