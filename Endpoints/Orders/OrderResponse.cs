namespace IWantApp.Endpoints.Orders;

public record OrderResponse(Guid Id, string ClientName, decimal Total, IEnumerable<OrderProduct> Products, string DeliveryAddress);

public record OrderProduct(Guid Id, string Name, decimal Price);