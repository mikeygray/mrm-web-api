using System.Data.Entity;

namespace MRMWebAPI.Models
{
    /// <summary>
    /// </summary>
    public class ProductServiceContext : DbContext
    {
        /// <summary>
        /// </summary>
        public ProductServiceContext() : base("name=ProductServiceContext")
        {}

        /// <summary>
        /// </summary>
        public virtual DbSet<Product> Products { get; set; }
    
    }
}
