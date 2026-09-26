using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Program
    {
        static long Factorial()
        {
            int n = 0;
            Console.Write("Введите n (целое число): ");

            if (!int.TryParse(Console.ReadLine(), out n) || n < 1)
            {
                Console.WriteLine("Ошибка: нужно целое число от 1.");
            }

            long result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        static string Fibonacci()
        {
            int n;
            Console.Write("Введите n (целое число): ");

            if (!int.TryParse(Console.ReadLine(), out n) || n < 0)
            {
                return "Ошибка: нужно целое число от 0.";
            }

            List<long> fib = new List<long> { 0, 1 };
            while (fib[fib.Count - 1] < n)
            {
                fib.Add(fib[fib.Count - 1] + fib[fib.Count - 2]);
            }

            var result = fib.Where(x => x <= n);
            return string.Join(", ", result);
        }
        static double? CalculateA()
        {
            double x;
            Console.Write("Введите x: ");

            if (!double.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Ошибка: нужно число.");
                return null;
            }

            if (Math.Abs(Math.Cos(x * x)) < 1e-12)
            {
                Console.WriteLine("Ошибка: tg(x²) не определён (cos(x²) = 0).");
                return null;
            }

            if (x - 5 < 0)
            {
                Console.WriteLine("Ошибка: подкоренное выражение (x - 5) отрицательно.");
                return null;
            }

            double tgPart = Math.Tan(x * x);
            double atanPart = Math.Atan(x);
            double coshPart = Math.Cosh(15 - x);
            double rootPart = Math.Pow(x - 5, 1.0 / 5.0);

            double A = Math.Exp(tgPart + atanPart) * coshPart - rootPart;
            return A;
        }

        static void Main(string[] args)
        {
            int numbermetod = 1;
            while (numbermetod != 0)
            {
                Console.WriteLine("Выберите метод: 1 - факториал, 2 - фибоначчи, 3 - функция 0 - выйти");
                if (int.TryParse(Console.ReadLine(), out numbermetod) && numbermetod >= 0 && numbermetod <= 4)
                {
                    switch (numbermetod)
                    {
                        case 1:
                            Console.WriteLine($"Результат: {Factorial()}");
                            break;
                        case 2:
                            Console.WriteLine($"Результат: {Fibonacci()}");
                            break;
                        case 3:
                            Console.WriteLine($"Результат: {CalculateA()}");
                            break;
                        case 0:
                            Console.WriteLine("Выход...");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: нужно целое число от 0 до 2.");
                    continue;
                }
            }
        }
    }
}