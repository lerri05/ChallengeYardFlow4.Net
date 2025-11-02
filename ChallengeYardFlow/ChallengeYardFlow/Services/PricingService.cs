using System;

namespace ChallengeYardFlow.Services
{
    public sealed class PricingService
    {
        public decimal CalculateTotal(DateTime startDate, DateTime endDate, decimal dailyPrice)
        {
            if (endDate < startDate) throw new ArgumentException("Data final não pode ser menor que a inicial");
            if (dailyPrice < 0) throw new ArgumentOutOfRangeException(nameof(dailyPrice));

            var days = (endDate.Date - startDate.Date).Days + 1;
            return days * dailyPrice;
        }
    }
}


