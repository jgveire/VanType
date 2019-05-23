using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VanType.Tests
{
    using VanType.Models;

    [TestClass]
    public class TypeScriptTests
    {
        [TestMethod]
        public void When_generate_is_called_then_generic_should_be_handled_correctly()
        {
            // Arrange
            string expected = "export interface Lookup<T>\r\n{\r\n\tid: T | null;\r\n\tname: string | null;\r\n}\r\n\r\n";

            // Act
            string result = TypeScript
                .Config()
                .AddType(typeof(Lookup<>))
                .Generate();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_is_called_then_generic_should_not_be_processed()
        {
            // Arrange
            string expected = string.Empty;

            // Act
            string result = TypeScript
                .Config()
                .AddType(typeof(Lookup<int>))
                .Generate();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_is_called_then_enum_should_should_be_converted_to_int_values()
        {
            // Arrange
            string expected = "export enum ProductStatus\r\n{\r\n\tInStock = 0,\r\n\tOutOfStock = 1,\r\n}\r\n\r\n";

            // Act
            string result = TypeScript
                .Config()
                .UseEnumConversion(EnumConversionType.Numeric)
                .AddType<ProductStatus>()
                .Generate();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_is_called_then_enum_should_should_be_converted_to_string_values()
        {
            // Arrange
            string expected = "export enum ProductStatus\r\n{\r\n\tInStock = 'InStock',\r\n\tOutOfStock = 'OutOfStock',\r\n}\r\n\r\n";

            // Act
            string result = TypeScript
                .Config()
                .UseEnumConversion(EnumConversionType.String)
                .AddType<ProductStatus>()
                .Generate();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_get_interface_name_is_called_for_product_model_then_a_name_should_be_returned()
        {
            // Arrange
            var systemUnderTest = new TypeScript();
            systemUnderTest.PrefixClasses(true);

            // Act
            string result = systemUnderTest.GetInterfaceName(typeof(ProductModel));

            // Assert
            Assert.AreEqual("IProductModel", result);
        }

        [TestMethod]
        public void When_get_interface_name_is_called_for_generic_then_a_name_should_be_returned()
        {
            // Arrange
            var systemUnderTest = new TypeScript();

            // Act
            string result = systemUnderTest.GetInterfaceName(typeof(Lookup<int>));

            // Assert
            Assert.AreEqual("Lookup<number>", result);
        }

        [TestMethod]
        public void When_get_interface_name_is_called_for_complex_generic_then_a_name_should_be_returned()
        {
            // Arrange
            var systemUnderTest = new TypeScript();

            // Act
            string result = systemUnderTest.GetInterfaceName(typeof(Lookup<ProductModel>));

            // Assert
            Assert.AreEqual("Lookup<ProductModel>", result);
        }
    }
}
