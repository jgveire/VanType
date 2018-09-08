namespace VanType.Models
{
    using System;
    using System.Collections.Generic;

    public class ProductModel : ProductBase
    {
        public decimal Price { get; set; }

        public bool IsVisible { get; set; }

        public ProductStatus Status { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public DateTime LastUpdated { get; set; }

        public int InStock { get; set; }

        public string[] KeyWords { get; set; }
    }
}