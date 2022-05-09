namespace IWantApp.Endpoints.Products;

public class ProductMostBought
{
    public static string Template => "/products/mostbought";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;
    private static int Page;
    private static int Rows;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        int? page, int? rows, QueryProductsMostBought query)
    {
        var hasError = Validate(page, rows);

        if (hasError.Any())
            return Results.BadRequest(hasError);

        var result = await query.Execute(Page, Rows);

        return Results.Ok(result);
    }
    private static Dictionary<string, string[]> Validate(int? page, int? rows)
    {
        var dictionary = new Dictionary<string, string[]>();

        if (page == null)
            Page = 1;

        if (rows == null)
            Rows = 10;
        else if (rows > 10 || rows < 1)
        {
            string[] error = { "You must enter a value between 1 and 10 for the rows" };

            dictionary.Add("Error", error);

            return dictionary;
        }

        return dictionary;
    }
}
