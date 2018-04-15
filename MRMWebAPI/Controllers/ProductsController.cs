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
        /// Updates product data at a specific id. (e.g. PUT api/Products/5)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(int id, Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != product.Id)
                return BadRequest();

            if (!ProductExists(id))
                return NotFound();

            _repository.Entry(product).State = EntityState.Modified;

            // Product oldProduct = _repository.Products.Single(p => p.Id == id)...?
            
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new product. (e.g. POST api/Products)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _repository.Products.Add(product);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        /// <summary>
        /// Removes the product at a specific id. (e.g. DELETE api/Products/5)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Product product = _repository.Products.Single(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _repository.Products.Remove(product);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(product);
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