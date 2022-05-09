﻿namespace IWantApp.Endpoints.Clients;

public class ClientGetAll
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext context)
    {
        var user = context.User;
        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(c => c.Type == "Name").Value,
            Cpf = user.Claims.First(c => c.Type == "Cpf").Value
        };
        
        return Results.Ok(result);
    }
}
