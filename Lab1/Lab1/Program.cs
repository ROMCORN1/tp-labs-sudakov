using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Program
    {
        static long Factorial()
        {
            int n = 1;
            Console.Write("Введите n (0..20): ");

            if (!int.TryParse(Console.ReadLine(),out n) || n < 0 || n > 20)
            {
                Console.WriteLine("Ошибка: нужно целое число от 0 до 20.");
            }
                long result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        static void Main(string[] args)
        {
            int x = 1;
            while (x!=0)
            {
                Console.WriteLine("Выберите метод 1 - фибоначи, 0 - выйти");
                if (int.TryParse(Console.ReadLine(), out x) && x <= 4 && x >= 1)
                {
                    switch (x)
                    {
                        case 1:
                            Console.WriteLine($"{Factorial()}");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: нужно целое число от 1 до 4.");
                    continue;
                }
            }
        }
    }
}
