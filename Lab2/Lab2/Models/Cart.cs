using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2.Models
{
    public class Cart
    {
        private readonly List<Product> _items = new List<Product>();

        public IReadOnlyList<Product> Items { get { return _items; } }

        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _items.Add(product);
        }

        public bool Remove(Product product)
        {
            return _items.Remove(product);
        }

        public double TotalWithoutDiscount()
        {
            double sum = 0;
            foreach (var p in _items) sum += p.Price;
            return sum;
        }

        public double TotalWithDiscount()
        {
            double sum = 0;
            foreach (var p in _items) sum += p.GetFinalPrice();
            return sum;
        }

        public double TotalSaved()
        {
            return TotalWithoutDiscount() - TotalWithDiscount();
        }

        public string GetReceipt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Чек ===");
            foreach (Product p in _items)
                sb.AppendLine(p.ToString());
            sb.AppendLine(string.Format("Сумма без скидок: {0:F2} руб.", TotalWithoutDiscount()));
            sb.AppendLine(string.Format("Скидка:           {0:F2} руб.", TotalSaved()));
            sb.AppendLine(string.Format("К оплате:         {0:F2} руб.", TotalWithDiscount()));
            return sb.ToString();
        }
    }
}