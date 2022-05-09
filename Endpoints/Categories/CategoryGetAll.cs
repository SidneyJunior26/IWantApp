namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(ApplicationDbContext context)
    {
        List<Category> category = context.Categories.ToList();

        if (category == null)
            return Results.NotFound();

        var response = category.Select(c => new CategoryResponse(c.Id, c.Name, c.Active));

        return Results.Ok(response);
    }
}
