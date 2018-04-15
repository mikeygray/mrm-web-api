using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MRMWebAPI.Models;
using MRMWebAPI.Controllers;
using static MRMWebAPI.Tests.Helpers.MockDbListHelper;

namespace MRMWebAPI.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void GetReturnsAllProducts()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetQueryableMockDbSet(MockProductListFull).Object);
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<List<Product>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(MockProductListFull, contentResult.Content);
        }

        [TestMethod]
        public async Task GetReturnsCorrectProductsForCategory()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetAsyncMockDbSet(MockProductListFull).Object);
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.Get("Dairy");
            var contentResult = actionResult as OkNegotiatedContentResult<List<Product>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(MockProductListDairy, contentResult.Content);
        }

        [TestMethod]
        public async Task PostSetsHeaderAndReturnsProduct()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetAsyncMockDbSet(MockProductListFull).Object);
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.Post(new Product { Id = 99, Name = "Crackers" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(99, createdResult.RouteValues["id"]);
            Assert.AreEqual("Crackers", createdResult.Content.Name);
        }

        [TestMethod, Ignore]
        public async Task PutReturnsContentResult()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetAsyncMockDbSet(MockProductListFull).Object);
            var controller = new ProductsController(mockRepository.Object);
            
            /*
             * TODO: I can not get this to work for love nor money
             * 
            // Act
            //var mockController = new Mock<ProductsController>(mockRepository.Object);
            //mockController.Setup(c => c.Put()).Returns(p)...?
 
            var actionResult = await controller.Put(3, new Product { Id = 3, Name = "Super Smelly Cheese" });
            var contentResult = actionResult as NegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(3, contentResult.Content.Id);
            Assert.AreEqual("Super Smelly Cheese", contentResult.Content.Name);
            */   
        }

        [TestMethod]
        public async Task DeleteReturnsOkAndRemovesProduct()
        {
            // Arrange
            var mockDbSet = GetAsyncMockDbSet(MockProductListFull);
          
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(mockDbSet.Object);

            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.Delete(7);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Product>));
            Assert.AreEqual(3, mockRepository.Object.Products.Count());
        }
    }
}
