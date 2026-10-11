using System;

namespace Lab2.Models
{
    public class Food : Product
    {
        public DateTime ExpirationDate { get; private set; }

        public Food(string name, double price, DateTime expirationDate, double discount = 0)
            : base(name, price, discount)
        {
            ExpirationDate = expirationDate;
        }

        public override string Category { get { return "Продукты"; } }

        public int DaysUntilExpiration()
        {
            return (ExpirationDate.Date - DateTime.Today).Days;
        }

        public override double GetFinalPrice()
        {
            int daysLeft = DaysUntilExpiration();
            if (daysLeft < 0) return 0;
            double price = base.GetFinalPrice();
            if (daysLeft <= 3) price *= 0.5;
            return price;
        }

        public override string ToString()
        {
            int days = DaysUntilExpiration();
            string status;
            if (days < 0) status = "ПРОСРОЧЕН";
            else if (days <= 3) status = string.Format("истекает через {0} дн.", days);
            else status = string.Format("годен до {0:dd.MM.yyyy}", ExpirationDate);

            return base.ToString() + " | " + status;
        }
    }
}