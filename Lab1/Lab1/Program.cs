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

        static void Main(string[] args)
        {
            int x = 1;
            while (x != 0)
            {
                Console.WriteLine("Выберите метод: 1 - факториал, 2 - фибоначчи, 0 - выйти");
                if (int.TryParse(Console.ReadLine(), out x) && x >= 0 && x <= 2)
                {
                    switch (x)
                    {
                        case 1:
                            Console.WriteLine($"Результат: {Factorial()}");
                            break;
                        case 2:
                            Console.WriteLine($"Результат: {Fibonacci()}");
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