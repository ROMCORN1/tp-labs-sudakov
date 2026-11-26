using System;
using Lab4.Core.Models;
using Xunit;

namespace Lab4.Tests;

public class DomainModelTests
{

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Product_EmptyName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => new Electronics(name, 100, "Brand", 12));
    }

    [Fact]
    public void Product_NullName_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Electronics(null!, 100, "Brand", 12));
    }

    [Fact]
    public void Product_NegativePrice_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Electronics("X", -1, "Brand", 12));
    }

    [Fact]
    public void Product_ZeroPrice_IsAllowed()
    {
        var p = new Electronics("Free", 0, "Brand", 12);
        Assert.Equal(0, p.Price);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.01)]
    [InlineData(101)]
    public void Product_InvalidDiscountPercent_Throws(double discount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Electronics("X", 100, "Brand", 12, discount));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Product_ValidDiscountPercent_IsAllowed(double discount)
    {
        var p = new Electronics("X", 100, "Brand", 12, discount);
        Assert.Equal(discount, p.DiscountPercent);
    }

    [Fact]
    public void Product_SetDiscount_UpdatesFinalPrice()
    {
        var p = new Electronics("X", 1000, "Brand", 12);
        Assert.Equal(1000, p.GetFinalPrice(), 10);

        p.SetDiscount(50);

        Assert.Equal(50, p.DiscountPercent);
        Assert.Equal(500, p.GetFinalPrice(), 10);
    }

    [Fact]
    public void Electronics_PriceAboveThreshold_AppliesExtraFivePercent()
    {
        var e = new Electronics("Laptop", 100000, "Brand", 12);
        Assert.Equal(95000, e.GetFinalPrice(), 10);
    }

    [Fact]
    public void Electronics_PriceEqualsThreshold_NoExtraDiscount()
    {
        var e = new Electronics("Laptop", 50000, "Brand", 12);
        Assert.Equal(50000, e.GetFinalPrice(), 10);
    }

    [Fact]
    public void Electronics_PriceBelowThreshold_NoExtraDiscount()
    {
        var e = new Electronics("Laptop", 49999, "Brand", 12);
        Assert.Equal(49999, e.GetFinalPrice(), 10);
    }

    [Fact]
    public void Electronics_CombinesBaseDiscountAndExtraDiscount()
    {
        var e = new Electronics("Laptop", 100000, "Brand", 12, discount: 10);
        Assert.Equal(85500, e.GetFinalPrice(), 10);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Electronics_NegativeWarranty_Throws(int months)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Electronics("X", 100, "Brand", months));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Electronics_EmptyBrand_Throws(string brand)
    {
        Assert.Throws<ArgumentException>(() => new Electronics("X", 100, brand, 12));
    }

    [Fact]
    public void Electronics_NullBrand_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Electronics("X", 100, null!, 12));
    }

    [Fact]
    public void Electronics_Category_IsElectronics()
    {
        var e = new Electronics("X", 100, "Brand", 12);
        Assert.Equal("Электроника", e.Category);
    }

    [Fact]
    public void Food_Expired_ReturnsZeroPrice()
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(-1));
        Assert.Equal(0, f.GetFinalPrice());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Food_ExpiresInThreeDaysOrLess_HalfDiscount(int days)
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(days));
        Assert.Equal(50, f.GetFinalPrice(), 10);
    }

    [Fact]
    public void Food_ExpiresInFourDays_NoExtraDiscount()
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(4));
        Assert.Equal(100, f.GetFinalPrice(), 10);
    }

    [Fact]
    public void Food_ExpiringSoon_CombinesWithBaseDiscount()
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(2), discount: 20);
        Assert.Equal(40, f.GetFinalPrice(), 10);
    }

    [Fact]
    public void Food_DaysUntilExpiration_IsCorrect()
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(5));
        Assert.Equal(5, f.DaysUntilExpiration());
    }

    [Fact]
    public void Food_Category_IsFood()
    {
        var f = new Food("Milk", 100, DateTime.Today.AddDays(10));
        Assert.Equal("Продукты", f.Category);
    }

    [Theory]
    [InlineData("XL")]
    [InlineData("xl")]
    [InlineData("Xl")]
    [InlineData("XXL")]
    [InlineData("xxl")]
    [InlineData("XxL")]
    public void Clothes_SizeXLorXXL_TenPercentDiscount(string size)
    {
        var c = new Clothes("Shirt", 1000, size);
        Assert.Equal(900, c.GetFinalPrice(), 10);
    }

    [Theory]
    [InlineData("S")]
    [InlineData("M")]
    [InlineData("L")]
    [InlineData("XXXL")]
    public void Clothes_RegularSize_NoExtraDiscount(string size)
    {
        var c = new Clothes("Shirt", 1000, size);
        Assert.Equal(1000, c.GetFinalPrice(), 10);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Clothes_EmptySize_Throws(string size)
    {
        Assert.Throws<ArgumentException>(() => new Clothes("Shirt", 100, size));
    }

    [Fact]
    public void Clothes_NullSize_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Clothes("Shirt", 100, null!));
    }

    [Fact]
    public void Clothes_Category_IsClothes()
    {
        var c = new Clothes("Shirt", 100, "M");
        Assert.Equal("Одежда", c.Category);
    }

    [Fact]
    public void Cart_New_IsEmpty()
    {
        var cart = new Cart();
        Assert.Empty(cart.Items);
        Assert.Equal(0, cart.TotalWithoutDiscount());
        Assert.Equal(0, cart.TotalWithDiscount());
        Assert.Equal(0, cart.TotalSaved());
    }

    [Fact]
    public void Cart_Add_IncreasesItemsCount()
    {
        var cart = new Cart();
        var p = new Electronics("X", 100, "Brand", 12);

        cart.Add(p);

        Assert.Single(cart.Items);
        Assert.Same(p, cart.Items[0]);
    }

    [Fact]
    public void Cart_AddNull_Throws()
    {
        var cart = new Cart();
        Assert.Throws<ArgumentNullException>(() => cart.Add(null!));
    }

    [Fact]
    public void Cart_RemoveExisting_ReturnsTrue()
    {
        var cart = new Cart();
        var p = new Electronics("X", 100, "Brand", 12);
        cart.Add(p);

        bool removed = cart.Remove(p);

        Assert.True(removed);
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Cart_RemoveNonExistent_ReturnsFalse()
    {
        var cart = new Cart();
        cart.Add(new Electronics("X", 100, "Brand", 12));

        bool removed = cart.Remove(new Electronics("Y", 200, "Brand", 12));

        Assert.False(removed);
        Assert.Single(cart.Items);
    }

    [Fact]
    public void Cart_Totals_AreCorrect()
    {
        var cart = new Cart();
        cart.Add(new Electronics("A", 1000, "Brand", 12, discount: 10));
        cart.Add(new Electronics("B", 2000, "Brand", 12));

        Assert.Equal(3000, cart.TotalWithoutDiscount(), 10);
        Assert.Equal(2900, cart.TotalWithDiscount(), 10);
        Assert.Equal(100, cart.TotalSaved(), 10);
    }

    [Fact]
    public void Cart_GetReceipt_ContainsHeaderAndTotals()
    {
        var cart = new Cart();
        cart.Add(new Electronics("Laptop", 1000, "Brand", 12, discount: 10));

        string receipt = cart.GetReceipt();

        Assert.Contains("=== Чек ===", receipt);
        Assert.Contains("Сумма без скидок", receipt);
        Assert.Contains("Скидка", receipt);
        Assert.Contains("К оплате", receipt);
        Assert.Contains("Laptop", receipt);
    }

    [Fact]
    public void Cart_Empty_ReceiptStillHasTotals()
    {
        string receipt = new Cart().GetReceipt();
        Assert.Contains("=== Чек ===", receipt);
        Assert.Contains("0,00", receipt);
    }
}