using System;
using System.IO;
using System.Linq;
using Warehouse.Core;
using Xunit;

namespace Warehouse.Tests;

public class WarehouseServiceTests
{
    private readonly InMemoryProductStorage _storage;
    private readonly WarehouseService _service;

    public WarehouseServiceTests()
    {
        _storage = new InMemoryProductStorage();
        _service = new WarehouseService(_storage);
    }

    [Fact]
    public void AddProduct_ValidData_AddsProduct()
    {
        _service.AddProduct("1", "Телефон", 10000, 5);
        var products = _service.GetInventoryReport();
        Assert.Single(products);
        Assert.Equal("Телефон", products[0].Name);
    }

    [Fact]
    public void AddProduct_DuplicateId_Throws()
    {
        _service.AddProduct("1", "Телефон", 10000, 5);
        Assert.Throws<InvalidOperationException>(() => _service.AddProduct("1", "Ноутбук", 20000, 2));
    }

    [Fact]
    public void Receive_ValidQuantity_IncreasesStock()
    {
        _service.AddProduct("1", "Телефон", 10000, 5);
        _service.Receive("1", 3);
        Assert.Equal(8, _service.GetInventoryReport().First().Quantity);
    }

    [Fact]
    public void Receive_NonExistentProduct_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => _service.Receive("999", 1));
    }

    [Fact]
    public void Ship_ValidQuantity_DecreasesStock()
    {
        _service.AddProduct("1", "Телефон", 10000, 5);
        _service.Ship("1", 2);
        Assert.Equal(3, _service.GetInventoryReport().First().Quantity);
    }

    [Fact]
    public void Ship_InsufficientStock_Throws()
    {
        _service.AddProduct("1", "Телефон", 10000, 2);
        Assert.Throws<InvalidOperationException>(() => _service.Ship("1", 3));
    }

    [Fact]
    public void GetInventoryReport_SortsByName()
    {
        _service.AddProduct("1", "Яблоко", 10, 100);
        _service.AddProduct("2", "Банан", 20, 50);
        var report = _service.GetInventoryReport();
        Assert.Equal("Банан", report[0].Name);
        Assert.Equal("Яблоко", report[1].Name);
    }

    [Fact]
    public void GetTopProducts_ReturnsTopByQuantity()
    {
        _service.AddProduct("1", "Товар1", 10, 10);
        _service.AddProduct("2", "Товар2", 10, 50);
        _service.AddProduct("3", "Товар3", 10, 30);
        
        var top2 = _service.GetTopProducts(2);
        
        Assert.Equal(2, top2.Count);
        Assert.Equal("Товар2", top2[0].Name);
        Assert.Equal("Товар3", top2[1].Name);
    }

    [Fact]
    public void CsvStorage_SaveAndLoad_PersistsData()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            var csvService = new WarehouseService(new CsvProductStorage(tempFile));
            csvService.AddProduct("1", "Телефон", 10000, 5);

            // Создаем новый сервис, чтобы имитировать перезапуск приложения
            var newCsvService = new WarehouseService(new CsvProductStorage(tempFile));
            
            Assert.Single(newCsvService.GetInventoryReport());
            Assert.Equal("Телефон", newCsvService.GetInventoryReport().First().Name);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void AddProduct_NegativePrice_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.AddProduct("1", "Телефон", -100, 5));
    }
}