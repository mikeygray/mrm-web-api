using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MRMWebAPI.Models;
using MRMWebAPI.Controllers;
using static MRMWebAPI.Tests.Helpers.MockDbListHelper;


namespace MRMWebAPI.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTest
    {
        [TestMethod]
        public void GetReturnsUniqueCategories()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetQueryableMockDbSet(MockProductListFull).Object);
            var controller = new CategoriesController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<List<string>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEquivalent(MockCategoriesList, contentResult.Content);
        }
    }
}
