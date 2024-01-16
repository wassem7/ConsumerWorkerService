namespace KafkaDotNet.Dtos;

public class OrderRequest
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public Decimal Price { get; set; }
}