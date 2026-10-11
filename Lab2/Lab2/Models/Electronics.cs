using System;

namespace Lab2.Models
{
    public class Electronics : Product
    {
        private int _warrantyMonths;

        public string Brand { get; private set; }

        public int WarrantyMonths
        {
            get { return _warrantyMonths; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Гарантия не может быть отрицательной.");
                _warrantyMonths = value;
            }
        }

        public Electronics(string name, double price, string brand, int warrantyMonths, double discount = 0)
            : base(name, price, discount)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Бренд не может быть пустым.", nameof(brand));
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string Category { get { return "Электроника"; } }

        public override double GetFinalPrice()
        {
            double result = base.GetFinalPrice();
            if (Price > 50000)
                result *= 0.95;
            return result;
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(" | Бренд: {0}, гарантия: {1} мес.", Brand, WarrantyMonths);
        }
    }
}