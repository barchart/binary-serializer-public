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

            stopwatch.Start();

            Type carType = typeof(Car);
            PropertyInfo? decimalNumberProperty = carType.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberProperty = carType.GetField("doubleNumber");

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

            stopwatch.Start();

            var getter = (Car carold) => carold.DecimalNumber;
            var setter = (Car carold, decimal value) => carold.DecimalNumber = value;

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
            stopwatch.Start();

            var carType = carOld.GetType();
            var getMethod = carType.GetProperty("DecimalNumber")!.GetGetMethod()!.CreateDelegate<Func<Car, decimal>>()!;
            var setMethod = carType.GetProperty("DecimalNumber")!.GetSetMethod()!.CreateDelegate<Action<Car, decimal>>()!;

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

            stopwatch.Start();

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
            int iterations = int.MaxValue / 2;

            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();

            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

            Car car = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            stopwatch1.Start();

            var carType = car.GetType();
            var getMethod = carType.GetProperty("DecimalNumber")!.GetGetMethod()!.CreateDelegate<Func<Car, decimal>>()!;
            var setMethod = carType.GetProperty("DecimalNumber")!.GetSetMethod()!.CreateDelegate<Action<Car, decimal>>()!;

            for (long i = 0; i < iterations; i++)
            {
                setMethod(car, 22.5m);
                var x = getMethod(car);
            }
            stopwatch1.Stop();

            output.WriteLine($"Using Delegates: {stopwatch1.ElapsedTicks} ticks");
            output.WriteLine($"Using Equals: {stopwatch2.ElapsedTicks} ticks");

        }
    }
}

