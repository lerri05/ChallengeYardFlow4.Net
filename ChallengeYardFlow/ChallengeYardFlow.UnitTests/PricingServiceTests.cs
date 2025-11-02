using ChallengeYardFlow.Services;
using FluentAssertions;
using Xunit;

namespace ChallengeYardFlow.UnitTests
{
    public class PricingServiceTests
    {
        [Fact]
        public void CalculateTotal_ShouldMultiplyDaysInclusive_ByDailyPrice()
        {
            var service = new PricingService();
            var start = new DateTime(2025, 1, 10);
            var end = new DateTime(2025, 1, 12); // 3 dias
            var price = 100m;

            var total = service.CalculateTotal(start, end, price);

            total.Should().Be(300m);
        }

        [Fact]
        public void CalculateTotal_SameDay_ShouldChargeOneDay()
        {
            var service = new PricingService();
            var day = new DateTime(2025, 5, 20);

            var total = service.CalculateTotal(day, day, 50m);

            total.Should().Be(50m);
        }

        [Fact]
        public void CalculateTotal_EndBeforeStart_ShouldThrow()
        {
            var service = new PricingService();
            var act = () => service.CalculateTotal(new DateTime(2025, 5, 10), new DateTime(2025, 5, 9), 10m);
            act.Should().Throw<ArgumentException>();
        }
    }
}


