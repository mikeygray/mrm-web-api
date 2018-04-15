using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MRMWebAPI.Models;

namespace MRMWebAPI.Controllers
{
    public class CategoriesController : ApiController
    {
        private ProductServiceContext _repository;

        public CategoriesController()
        {
            _repository = new ProductServiceContext();
        }

        public CategoriesController(ProductServiceContext repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns a list of all current categories in use. (e.g. GET api/Categories)
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get()
        {
            var uniqueCategories = _repository.Products.Select(p => p.Category).Distinct();
            if (uniqueCategories.Count() > 0)
                return Ok(uniqueCategories.ToList());
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
    }
}