using System;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests;

public class Lab1AlgorithmsTests
{

    [Fact]
    public void Factorial_Zero_ReturnsOne()
    {
        long result = Lab1Algorithms.Factorial(0);
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    [InlineData(20, 2432902008176640000)]
    public void Factorial_ValidInput_ReturnsFactorial(int n, long expected)
    {
        long actual = Lab1Algorithms.Factorial(n);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Factorial_NegativeInput_Throws(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Lab1Algorithms.Factorial(n));
    }

    [Theory]
    [InlineData(21)]
    [InlineData(50)]
    public void Factorial_AboveMax_Throws(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Lab1Algorithms.Factorial(n));
    }

    [Fact]
    public void Fibonacci_MaxZero_ReturnsSingleZero()
    {
        var seq = Lab1Algorithms.Fibonacci(0);
        Assert.Equal(new long[] { 0 }, seq);
    }

    [Fact]
    public void Fibonacci_MaxOne_ReturnsZeroAndOne()
    {
        var seq = Lab1Algorithms.Fibonacci(1);
        Assert.Equal(new long[] { 0, 1 }, seq);
    }

    [Theory]
    [InlineData(2, new long[] { 0, 1, 1, 2 })]
    [InlineData(5, new long[] { 0, 1, 1, 2, 3, 5 })]
    [InlineData(10, new long[] { 0, 1, 1, 2, 3, 5, 8 })]
    public void Fibonacci_ValidMax_ReturnsSequence(long max, long[] expected)
    {
        var seq = Lab1Algorithms.Fibonacci(max);
        Assert.Equal(expected, seq);
    }

    [Fact]
    public void Fibonacci_NegativeMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Lab1Algorithms.Fibonacci(-1));
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(20)]
    public void CalculateA_ValidX_ReturnsFiniteResult(double x)
    {
        double result = Lab1Algorithms.CalculateA(x);
        Assert.False(double.IsNaN(result));
        Assert.False(double.IsInfinity(result));
    }

    [Fact]
    public void CalculateA_XEquals5_MatchesManualComputation()
    {
        double expected = Math.Exp(Math.Tan(25) + Math.Atan(5)) * Math.Cosh(10);
        double actual = Lab1Algorithms.CalculateA(5);
        Assert.Equal(expected, actual, 10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4.99)]
    [InlineData(-1)]
    public void CalculateA_XBelowFive_Throws(double x)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Lab1Algorithms.CalculateA(x));
    }

    [Fact]
    public void TaylorExp_Zero_ReturnsOne()
    {
        double result = Lab1Algorithms.TaylorExp(0);
        Assert.Equal(1.0, result, 12);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(2.0)]
    [InlineData(0.5)]
    [InlineData(-1.0)]
    public void TaylorExp_MatchesMathExp(double x)
    {
        double actual = Lab1Algorithms.TaylorExp(x);
        double expected = Math.Exp(x);
        Assert.Equal(expected, actual, 5);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1e-6)]
    public void TaylorExp_InvalidEps_Throws(double eps)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Lab1Algorithms.TaylorExp(1.0, eps));
    }
}