namespace IWantApp.Endpoints.Categories;

public class CategoryDelete
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var category = context.Categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
            return Results.NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Results.Ok();

    }
}
