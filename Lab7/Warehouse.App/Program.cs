using System;
using System.Linq;
using Warehouse.Core;
using ConsoleTables;

namespace Warehouse.App;

class Program
{
    static void Main()
    {
        // Используем CSV-хранилище, файл создастся в папке запуска
        var storage = new CsvProductStorage("warehouse.csv");
        var service = new WarehouseService(storage);

        while (true)
        {
            Console.WriteLine("\n=== Складской учёт ===");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Приход товара");
            Console.WriteLine("3. Расход товара");
            Console.WriteLine("4. Отчёт по остаткам");
            Console.WriteLine("5. Топ-товары");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1": AddProduct(service); break;
                case "2": ReceiveProduct(service); break;
                case "3": ShipProduct(service); break;
                case "4": ShowInventory(service); break;
                case "5": ShowTopProducts(service); break;
                case "0": return;
                default: Console.WriteLine("Неверный ввод."); break;
            }
        }
    }

    static void AddProduct(WarehouseService service)
    {
        try
        {
            Console.Write("ID: ");
            string id = Console.ReadLine()!;
            Console.Write("Название: ");
            string name = Console.ReadLine()!;
            Console.Write("Цена: ");
            decimal price = decimal.Parse(Console.ReadLine()!);
            Console.Write("Начальное количество: ");
            int qty = int.Parse(Console.ReadLine()!);

            service.AddProduct(id, name, price, qty);
            Console.WriteLine("Товар добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ReceiveProduct(WarehouseService service)
    {
        try
        {
            Console.Write("ID товара: ");
            string id = Console.ReadLine()!;
            Console.Write("Количество: ");
            int qty = int.Parse(Console.ReadLine()!);

            service.Receive(id, qty);
            Console.WriteLine("Приход оформлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ShipProduct(WarehouseService service)
    {
        try
        {
            Console.Write("ID товара: ");
            string id = Console.ReadLine()!;
            Console.Write("Количество: ");
            int qty = int.Parse(Console.ReadLine()!);

            service.Ship(id, qty);
            Console.WriteLine("Расход оформлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ShowInventory(WarehouseService service)
    {
        var products = service.GetInventoryReport();
        if (!products.Any())
        {
            Console.WriteLine("Склад пуст.");
            return;
        }

        var table = new ConsoleTable("ID", "Название", "Цена", "Количество");
        foreach (var p in products)
        {
            table.AddRow(p.Id, p.Name, p.Price, p.Quantity);
        }
        table.Write();
    }

    static void ShowTopProducts(WarehouseService service)
    {
        Console.Write("Сколько топ-товаров вывести? ");
        if (int.TryParse(Console.ReadLine(), out int count))
        {
            var products = service.GetTopProducts(count);
            if (!products.Any())
            {
                Console.WriteLine("Нет данных.");
                return;
            }

            var table = new ConsoleTable("ID", "Название", "Количество");
            foreach (var p in products)
            {
                table.AddRow(p.Id, p.Name, p.Quantity);
            }
            table.Write();
        }
    }
}