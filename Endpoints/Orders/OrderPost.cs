namespace IWantApp.Endpoints.Orders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(HttpContext http, ApplicationDbContext context, OrderRequest orderRequest)
    {
        var userId = http.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userName = http.User.Claims
            .First(c => c.Type == "Name").Value;

        List<Product> products = null;
        if (orderRequest.ProductsIds != null)
            products = context.Products.Where(p => orderRequest.ProductsIds.Contains(p.Id)).ToList();

        var order = new Order(userId, userName, products, orderRequest.DeliveryAddress);

        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}
