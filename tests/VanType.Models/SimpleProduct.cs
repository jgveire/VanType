namespace VanType.Models
{
    public class SimpleProduct
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ProductStatus Status { get; set; }
    }
}
