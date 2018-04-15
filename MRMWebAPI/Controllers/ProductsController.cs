using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MRMWebAPI.Models;

namespace MRMWebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private ProductServiceContext _repository;

        public ProductsController()
        {
            _repository = new ProductServiceContext();
        }

        public ProductsController(ProductServiceContext repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns a full list of all current products. (e.g. GET api/Products)
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IList<Product>))]
        public IHttpActionResult Get()
        {
            var productList = _repository.Products.ToList();
            return Ok(productList);
        }

        /// <summary>
        /// Returns a list of all products belonging to a specific category. (e.g. GET api/Products?category=Cocktails)
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<Product>))]
        public async Task<IHttpActionResult> Get(string category)
        {
            var products = await (from p in _repository.Products
                                  where p.Category.Equals(category)
                                  orderby p.Name
                                  select p).ToListAsync();

            if (products.Count() > 0)
                return Ok(products);

            return NotFound();
        }
        
        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _repository.Products.Count(e => e.Id == id) > 0;
        }
    }
}