namespace IWantApp.Endpoints.Categories;

public class ProductGetById
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(Guid id, ApplicationDbContext context)
    {
        var products = context.Products
            .Where(p => p.Id == id)
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToList();

        if (products == null)
            return Results.NotFound();

        var response = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock,p.Price, p.Active));

        return Results.Ok(response);
    }
}
