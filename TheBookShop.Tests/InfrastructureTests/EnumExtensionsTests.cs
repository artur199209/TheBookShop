using System.ComponentModel;
using TheBookShop.Infrastructure;
using Xunit;

namespace TheBookShop.Tests.InfrastructureTests
{
    public class EnumExtensionsTests
    {
        public enum EnumTest
        {
            [Description("First enum")]
            New = 1,
            [Description("Second enum")]
            Old = 2
        }

        [Fact]
        public void GetDescription_Returns_Enum_Description_Correctly()
        {
            var item1 = EnumTest.New.GetDescription();
            var item2 = EnumTest.Old.GetDescription();

            Assert.Equal("First enum", item1);
            Assert.Equal("Second enum", item2);
        }
    }
}