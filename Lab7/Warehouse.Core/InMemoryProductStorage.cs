namespace Warehouse.Core;

public class InMemoryProductStorage : IProductStorage
{
    private List<Product> _products = new();

    public IEnumerable<Product> Load() => _products.ToList();

    public void Save(IEnumerable<Product> products) => _products = products.ToList();
}