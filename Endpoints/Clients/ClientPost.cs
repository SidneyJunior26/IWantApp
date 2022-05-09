using IWantApp.Services.Users;

namespace IWantApp.Endpoints.Clients;

public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ClientRequest clientRequest, UserCreatorService userCreator)
    {
        var userClaimns = new List<Claim>
        {
            new Claim("Cpf", clientRequest.Cpf),
            new Claim("Name", clientRequest.Name)
        };

        (IdentityResult result, string userId) result = await userCreator.Create(
            clientRequest.Email, clientRequest.Password, userClaimns);

        if (!result.result.Succeeded)
            return Results.BadRequest(result.result.Errors.First());

        return Results.Created($"/clients/{result.userId}", result.userId);
    }
}
