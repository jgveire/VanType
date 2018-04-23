using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VanType.Tests
{
    [TestClass]
    public class TypeScriptTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Act
            string result = new TypeScript()
                .Add<Product>()
                .Add<Tag>()
                .IncludeEnums()
                .Generate();

            // Assert
            Assert.IsNotNull(result);
        }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsVisible { get; set; }

        public ProductStatus Status { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public enum ProductStatus
    {
        InStock = 0,

        OutOfStock = 1
    }
}
