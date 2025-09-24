public class ProductDto
{
    public int Id { get; set; }
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string? ImagePath { get; set; }
    public decimal Price { get; set; }
    public int MinQuantity { get; set; }
    public decimal DiscountRate { get; set; }  
}
