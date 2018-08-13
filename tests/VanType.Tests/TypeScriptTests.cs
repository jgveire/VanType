using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VanType.Tests
{
    using VanType.Models;

    [TestClass]
    public class TypeScriptTests
    {
        [TestMethod]
        public void When_generate_is_called_then_result_should_not_be_null()
        {
            // Arrange
            string expected = Resource.TypeScript;

            // Act
            string result = TypeScript
                .Config()
                .IncludeEnums(true)
                .PrefixClasses(true)
                .PrefixInterfaces(false)
                .OrderPropertiesByName(true)
                .AddClass<ProductModel>()
                .AddClass<Tag>()
                .TransformClassName(name =>
                {
                    if (name.EndsWith("Model"))
                    {
                        return name.Substring(0, name.Length - 5);
                    }

                    return name;
                })
                .Generate();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
