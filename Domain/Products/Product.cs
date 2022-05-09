using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; set; }
    public ICollection<Order> Orders { get; private set; }

    public Product(string name, Guid categoryId, string description, bool hasStock, decimal price, string createdBy)
    {
        Name = name;
        CategoryId = categoryId;
        Description = description;
        HasStock = hasStock;
        Price = price;

        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedBy = createdBy;
        EditedOn = DateTime.Now;

        Validate();
    }
    public void EditCategory(string name, Guid categoryId, string description, bool hasStock, bool active, decimal price, string editedBy)
    {
        Name = name;
        CategoryId = categoryId;
        Description = description;
        HasStock = hasStock;
        Active = active;
        Price = price;

        EditedBy = editedBy;
        EditedOn = DateTime.Now;

        Validate();
    }
    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CategoryId.ToString(), "CategoryId", "Category not found")
            .IsGreaterOrEqualsThan(Description, 5, "Description")
            .IsGreaterOrEqualsThan(Price, 0, "Price")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy");
        AddNotifications(contract);
    }
}
