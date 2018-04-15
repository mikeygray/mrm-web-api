using System.Web.Http;
using System.Web.Http.Results;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MRMWebAPI.Models;
using MRMWebAPI.Controllers;
using MRMWebAPI.Tests.TestDbAsync;

namespace MRMWebAPI.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        private List<Product> MockFullList
        {
            get
            {
                return new List<Product> {
                    new Product {
                        Id = 3,
                        Name = "Cheese",
                        Description = "Smelly",
                        Category = "Dairy"
                    },
                    new Product {
                        Id = 7,
                        Name = "Biscuit",
                        Description = "Crunchy",
                        Category = "Snack"
                    },
                    new Product {
                        Id = 17,
                        Name = "Crisps",
                        Description = "Also Crunchy",
                        Category = "Snack"
                    },
                    new Product {
                        Id = 32,
                        Name = "Milk",
                        Description = "Pasturised",
                        Category = "Dairy"
                    }
                };
            }
        }

        private List<Product> MockDairyList
        {
            get
            {
                return new List<Product> {
                    new Product {
                        Id = 3,
                        Name = "Cheese",
                        Description = "Smelly",
                        Category = "Dairy"
                    },
                    new Product {
                        Id = 32,
                        Name = "Milk",
                        Description = "Pasturised",
                        Category = "Dairy"
                    }
                };
            }
        }

        private static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet;
        }

        private static Mock<DbSet<T>> GetAsyncMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));
            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));
            
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            
            return dbSet;
        }


        [TestMethod]
        public void GetReturnsAllProducts()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetQueryableMockDbSet(MockFullList).Object);
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<List<Product>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(MockFullList, contentResult.Content);
        }

        [TestMethod]
        public async Task GetReturnsCorrectProductsForCategory()
        {
            // Arrange
            var mockRepository = new Mock<ProductServiceContext>();
            mockRepository.Setup(x => x.Products)
                .Returns(GetAsyncMockDbSet(MockFullList).Object);
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.Get("Dairy");
            var contentResult = actionResult as OkNegotiatedContentResult<List<Product>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(MockDairyList, contentResult.Content);
        }
        
    }
}
