using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MRMWebAPI.Models;
using MRMWebAPI.Tests.TestDbAsync;
using Moq;


namespace MRMWebAPI.Tests.Helpers
{
    static class MockDbListHelper
    {
        public static List<Product> MockProductListFull
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

        public static List<Product> MockProductListDairy
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

        public static List<string> MockCategoriesList
        {
            get
            {
                return new List<string> { "Dairy", "Snack" };
            }
        }

        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
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

        public static Mock<DbSet<T>> GetAsyncMockDbSet<T>(List<T> sourceList) where T : class
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

    }
}
