using System;

namespace Lab2.Models
{
    public abstract class Product : IDiscountable
    {
        private string _name = string.Empty;
        private double _price;
        private double _discountPercent;

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название не может быть пустым.", nameof(value));
                _name = value;
            }
        }

        public double Price
        {
            get { return _price; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Цена не может быть отрицательной.");
                _price = value;
            }
        }

        public double DiscountPercent
        {
            get { return _discountPercent; }
            private set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(value), "Скидка должна быть в диапазоне 0..100.");
                _discountPercent = value;
            }
        }

        protected Product(string name, double price, double discountPercent = 0)
        {
            Name = name;
            Price = price;
            DiscountPercent = discountPercent;
        }

        public void SetDiscount(double percent)
        {
            DiscountPercent = percent;
        }

        public virtual double GetFinalPrice()
        {
            return Price * (1 - DiscountPercent / 100.0);
        }

        public abstract string Category { get; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}, цена: {2:F2} руб., скидка: {3:F0}%, итог: {4:F2} руб.",
                Category, Name, Price, DiscountPercent, GetFinalPrice());
        }
    }
}