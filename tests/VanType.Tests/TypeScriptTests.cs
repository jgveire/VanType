using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VanType.Tests
{
    using System;
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
                .GenerateInterfaces();

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
                .GenerateInterfaces();

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
                .GenerateInterfaces();

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
                .GenerateInterfaces();

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

        [TestMethod]
        public void When_generate_classes_is_called_then_tag_class_should_be_generated_correctly()
        {
            // Arrange
            var expected = "export class Tag\r\n{\r\n\tid: number = 0;\r\n\tname: string | null = '';\r\n}\r\n\r\n";
            var systemUnderTest = new TypeScript()
                .AddType<Tag>();

            // Act
            string result = systemUnderTest.GenerateClasses();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_classes_is_called_then_simple_product_class_should_be_generated_correctly()
        {
            // Arrange
            var expected = "export class SimpleProduct\r\n{\r\n\tid: number = 0;\r\n\tname: string | null = '';\r\n\tstatus: ProductStatus = ProductStatus.InStock;\r\n}\r\n\r\n";
            var systemUnderTest = new TypeScript()
                .IncludeEnums(false)
                .AddType<SimpleProduct>();

            // Act
            string result = systemUnderTest.GenerateClasses();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_classes_is_called_then_type_converter_should_take_null_value_into_account()
        {
            // Arrange
            var expected = "export class Person\r\n{\r\n\tbirthDate: Date | null = null;\r\n\tfullName: string | null = '';\r\n}\r\n\r\n";
            var systemUnderTest = new TypeScript()
                .AddTypeConverter<DateTime>("Date", "null", true)
                .AddType<Person>();

            // Act
            string result = systemUnderTest.GenerateClasses();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void When_generate_classes_is_called_then_sort_should_be_correct()
        {
            // Arrange
            var systemUnderTest = new TypeScript()
                .PreserveInheritance(true)
                .AddType<TeamAddModel>()
                .AddType<TeamDeleteModel>()
                .AddType<TeamDetailModel>()
                .AddType<TeamIdModel>()
                .AddType<TeamModelBase>()
                .AddType<TeamReadModel>()
                .AddType<TeamsAddModel>()
                .AddType<TeamUpdateModel>();

            // Act
            string result = systemUnderTest.GenerateClasses();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
