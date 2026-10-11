using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab2.Models;

namespace Lab2
{
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var cart = new Cart();

            var laptop = new Electronics("Ноутбук", 75000, "ASUS", 24, discount: 10);
            var phone = new Electronics("Смартфон", 30000, "Samsung", 12);
            var milk = new Food("Молоко", 90, DateTime.Today.AddDays(2));
            var bread = new Food("Хлеб", 45, DateTime.Today.AddDays(-1));
            var tshirt = new Clothes("Футболка", 2000, "M", discount: 15);
            var jacket = new Clothes("Куртка", 12000, "XXL");

            var products = new List<Product> { laptop, phone, milk, bread, tshirt, jacket };

            Console.WriteLine("--- Все товары ---");
            foreach (Product p in products)
            {
                cart.Add(p);
                Console.WriteLine(p);
            }

            Console.WriteLine();
            Console.WriteLine(cart.GetReceipt());

            double avg = 0;
            foreach (var p in products) avg += p.GetFinalPrice();
            avg /= products.Count;

            Product max = products[0];
            foreach (var p in products)
                if (p.GetFinalPrice() > max.GetFinalPrice()) max = p;

            Console.WriteLine(string.Format("Средняя цена со скидкой: {0:F2} руб.", avg));
            Console.WriteLine("Самый дорогой после скидки: " + max.Name);
        }
    }
}