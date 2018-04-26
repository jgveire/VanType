using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VanType.Tests
{
    [TestClass]
    public class TypeScriptTests
    {
        [TestMethod]
        public void When_generate_is_called_then_result_should_not_be_null()
        {
            // Act
            string result = TypeScript
                .Config()
                .IncludeEnums(true)
                .PrefixClasses(true)
                .PrefixInterfaces(false)
                .OrderPropertiesByName(true)
                .AddClass<Product>()
                .AddClass<Tag>()
                .Generate();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
