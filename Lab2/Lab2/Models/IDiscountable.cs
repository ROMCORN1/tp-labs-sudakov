using System;

namespace Lab2.Models
{
    public interface IDiscountable
    {
        double DiscountPercent { get; }
        double GetFinalPrice();
    }
}