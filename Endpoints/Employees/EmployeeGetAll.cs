namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(
        int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        var hasError = new Dictionary<string, string[]>();

        if (page == null)
            page = 1;

        if (rows == null)
            rows = 10;
        else
            hasError = Validate(rows);

        if (hasError.Count > 0)
            return Results.BadRequest(hasError);

        var result = await query.Execute(page.Value, rows.Value);

        return Results.Ok(result);
    }

    private static Dictionary<string, string[]> Validate(int? rows)
    {
        var dictionary = new Dictionary<string, string[]>();

        if (rows > 10 || rows < 1)
        {            
            string[] error = { "You must enter a value between 1 and 10 for the rows" };

            dictionary.Add("Error", error);

            return dictionary;
        }

        return dictionary;
    }
}
