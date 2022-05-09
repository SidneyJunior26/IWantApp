namespace IWantApp.Endpoints.Orders;

public class OrderGet
{
    public static string Template => "/orders/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize]
    public static async Task<IResult> Action(
        Guid id, ApplicationDbContext context, HttpContext app, UserManager<IdentityUser> userManager)
    {
        var clientClaim = app.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var employeeClaim = app.User.Claims
            .FirstOrDefault(c => c.Type == "EmployeeCode");

        var order = await context.Orders
            .Include(o => o.Products)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return Results.NotFound();

        if (order.ClientId != clientClaim.Value && employeeClaim == null)
            return Results.Forbid();

        var client = await userManager.FindByIdAsync(order.ClientId);

        var productsResponse = order.Products.Select(p => new OrderProduct(p.Id, p.Name, p.Price));
        var orderResponse = 
            new OrderResponse(order.Id, client.UserName, order.Total, productsResponse, order.DeliveryAddress);

        return Results.Ok(orderResponse);
    }
}
