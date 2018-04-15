using System.Web.Mvc;

namespace MRMWebAPI.Controllers
{
    /// <summary>
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "MRM Brand .Net Exercise - Mikey Gray";

            return View();
        }
    }
}
