using IWantApp.Services.Users;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(
        EmployeeRequest employeeRequest,  HttpContext http, UserCreatorService userCreator)
    {
        string user = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaimns = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy", user)
        };

        (IdentityResult result, string userId) result = 
            await userCreator.Create(employeeRequest.Email, employeeRequest.Password, userClaimns);

        if (!result.result.Succeeded)
            return Results.ValidationProblem(result.result.Errors.ConvertToProblemDetails());

        return Results.Created($"/employees/{result.userId}", result.userId);
    }
}
