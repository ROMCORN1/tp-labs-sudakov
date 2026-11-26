using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4.Core;

public static class Lab1Algorithms
{
    public static long Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 0");
        if (n > 20)
            throw new ArgumentOutOfRangeException(nameof(n),
                "n > 20 вызывает переполнение типа long");

        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    public static IReadOnlyList<long> Fibonacci(long max)
    {
        if (max < 0)
            throw new ArgumentOutOfRangeException(nameof(max), "max должно быть >= 0");

        var result = new List<long> { 0, 1 };
        while (result[^1] < max)
        {
            long next = result[^1] + result[^2];
            if (next < result[^1])
                break;
            result.Add(next);
        }
        return result.Where(x => x <= max).ToList();
    }

    public static double CalculateA(double x)
    {
        if (double.IsNaN(x) || double.IsInfinity(x))
            throw new ArgumentOutOfRangeException(nameof(x), "x должно быть конечным");

        if (Math.Abs(Math.Cos(x * x)) < 1e-12)
            throw new ArgumentOutOfRangeException(nameof(x),
                "tg(x?) не определён: cos(x?) ? 0");

        if (x < 5)
            throw new ArgumentOutOfRangeException(nameof(x),
                "x должно быть >= 5 (подкоренное выражение)");

        double tgPart = Math.Tan(x * x);
        double atanPart = Math.Atan(x);
        double coshPart = Math.Cosh(15 - x);
        double rootPart = Math.Pow(x - 5, 1.0 / 5.0);

        return Math.Exp(tgPart + atanPart) * coshPart - rootPart;
    }

    public static double TaylorExp(double x, double eps = 1e-6)
    {
        if (eps <= 0)
            throw new ArgumentOutOfRangeException(nameof(eps), "eps должно быть > 0");

        double sum = 1.0, term = 1.0;
        int n = 0;
        while (true)
        {
            n++;
            term *= x / n;
            if (Math.Abs(term) <= eps)
                break;
            sum += term;
        }
        return sum;
    }
}