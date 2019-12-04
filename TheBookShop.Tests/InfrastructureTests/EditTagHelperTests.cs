using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using TheBookShop.Infrastructure;
using TheBookShop.Models.DataModels;
using Xunit;

namespace TheBookShop.Tests.InfrastructureTests
{
    public class EditTagHelperTests
    {
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;
        private readonly ModelExplorer _modelExplorer;
        private readonly string _modelName = nameof(DeliveryMethod);
        private readonly string _propertyName = nameof(DeliveryMethod.DeliveryMethodId);

        public EditTagHelperTests()
        {
            var content = new Mock<TagHelperContent>();
            _context = new TagHelperContext(new TagHelperAttributeList(),
               new Dictionary<object, object>(), "");
            _output = new TagHelperOutput("<edit/>", new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));
            var metadataProvider = new EmptyModelMetadataProvider();
            _modelExplorer = metadataProvider
                .GetModelExplorerForType(typeof(DeliveryMethod), null)
                .GetExplorerForProperty(nameof(DeliveryMethod.DeliveryMethodId));
        }

        [Fact]
        public void Process_Edit_To_Input_Works_Correctly()
        {
            var editTagHelper = new EditTagHelper {AspFor = new ModelExpression("", _modelExplorer)};
            
            editTagHelper.Process(_context, _output);
           
            Assert.Equal("input", _output.TagName);
        }

        [Fact]
        public void Add_Id_Attribute_Works_Correctly()
        {
            var editTagHelper = new EditTagHelper { AspFor = new ModelExpression("", _modelExplorer) };
            editTagHelper.Process(_context, _output);
            
            Assert.Equal($"{_modelName}_{_propertyName}", _output.Attributes["id"].Value);
        }

        [Fact]
        public void Add_Name_Attribute_Works_Correctly()
        {
            var editTagHelper = new EditTagHelper { AspFor = new ModelExpression("", _modelExplorer) };
            editTagHelper.Process(_context, _output);
            
            Assert.Equal($"{_modelName}.{_propertyName}", _output.Attributes["name"].Value);
        }
    }
}