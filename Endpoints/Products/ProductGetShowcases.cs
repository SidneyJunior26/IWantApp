namespace IWantApp.Endpoints.Categories;

public class ProductGetShowcases
{
    public static string Template => "/products/showcases";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static IResult Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "name")
    {
        if (rows > 10)
            return Results.Problem(title: "Row with max 10", statusCode: 400);

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.HasStock && p.Active && p.Category.Active);

        if (orderBy.ToUpper() == "NAME")
            queryBase = queryBase.OrderBy(p => p.Name);
        else if (orderBy.ToUpper() == "PRICE")
            queryBase = queryBase.OrderBy(p => p.Price);
        else
            return Results.Problem(title: "Order only by price or name", statusCode: 400);

        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows);       

        var products = queryFilter.ToList();

        if (products == null)
            return Results.NotFound();

        var response = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.Active));

        return Results.Ok(response);
    }
}
