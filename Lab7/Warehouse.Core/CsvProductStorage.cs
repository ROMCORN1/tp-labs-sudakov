using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Warehouse.Core;

public class CsvProductStorage : IProductStorage
{
    private readonly string _filePath;

    public CsvProductStorage(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Product> Load()
    {
        if (!File.Exists(_filePath))
            return new List<Product>();

        using var reader = new StreamReader(_filePath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var csv = new CsvReader(reader, config);
        return csv.GetRecords<Product>().ToList();
    }

    public void Save(IEnumerable<Product> products)
    {
        using var writer = new StreamWriter(_filePath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var csv = new CsvWriter(writer, config);
        csv.WriteRecords(products);
    }
}