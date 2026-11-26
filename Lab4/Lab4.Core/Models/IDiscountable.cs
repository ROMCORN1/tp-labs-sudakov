namespace Lab4.Core.Models
{
    public interface IDiscountable
    {
        double DiscountPercent { get; }
        double GetFinalPrice();
    }
}