using System.Collections.Generic;

namespace VanType.Tests
{
    public class Product : ProductBase
    {
        public decimal Price { get; set; }

        public bool IsVisible { get; set; }

        public ProductStatus Status { get; set; }

        public List<Tag> Tags { get; set; }
    }
}