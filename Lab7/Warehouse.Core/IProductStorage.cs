namespace Warehouse.Core;

public interface IProductStorage
{
    IEnumerable<Product> Load();
    void Save(IEnumerable<Product> products);
}