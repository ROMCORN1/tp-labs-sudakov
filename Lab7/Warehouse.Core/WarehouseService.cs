namespace Warehouse.Core;

public class WarehouseService
{
    private readonly IProductStorage _storage;
    private List<Product> _products;

    public WarehouseService(IProductStorage storage)
    {
        _storage = storage;
        _products = _storage.Load().ToList();
    }

    public void AddProduct(string id, string name, decimal price, int initialQuantity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID не может быть пустым", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название не может быть пустым", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной");
        if (initialQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(initialQuantity), "Количество не может быть отрицательным");

        if (_products.Any(p => p.Id == id))
            throw new InvalidOperationException($"Товар с ID '{id}' уже существует");

        _products.Add(new Product(id, name, price, initialQuantity));
        _storage.Save(_products);
    }

    public void Receive(string id, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество прихода должно быть > 0");

        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            throw new InvalidOperationException($"Товар с ID '{id}' не найден");

        product.Quantity += quantity;
        _storage.Save(_products);
    }

    public void Ship(string id, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество расхода должно быть > 0");

        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            throw new InvalidOperationException($"Товар с ID '{id}' не найден");

        if (product.Quantity < quantity)
            throw new InvalidOperationException($"Недостаточно товара '{product.Name}' на складе. Остаток: {product.Quantity}, запрошено: {quantity}");

        product.Quantity -= quantity;
        _storage.Save(_products);
    }

    public IReadOnlyList<Product> GetInventoryReport()
    {
        return _products.OrderBy(p => p.Name).ToList();
    }

    public IReadOnlyList<Product> GetTopProducts(int count)
    {
        if (count <= 0) return new List<Product>();
        return _products.OrderByDescending(p => p.Quantity).Take(count).ToList();
    }
}