using System.ComponentModel;
using System.Linq;
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
            Old = 2,
            [Description("In progress enum")]
            InProgress = 3
        }

        [Fact]
        public void GetDescription_Returns_Enum_Description_Correctly()
        {
            var item1 = EnumTest.New.GetDescription();
            var item2 = EnumTest.Old.GetDescription();
            var item3 = EnumTest.InProgress.GetDescription();

            Assert.Equal("First enum", item1);
            Assert.Equal("Second enum", item2);
            Assert.Equal("In progress enum", item3);
        }

        [Fact]
        public void GetEnumSelectList_Returns_All_Descriptions()
        {
            var result = EnumExtensions.GetEnumSelectList<EnumTest>().ToArray();

            Assert.NotNull(result);
            Assert.Equal("First enum", result[0].Text);
            Assert.Equal("Second enum", result[1].Text);
            Assert.Equal("In progress enum", result[2].Text);
        }
    }
}