using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MRMWebAPI.Controllers;

namespace MRMWebAPI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("MRM Brand .Net Exercise - Mikey Gray", result.ViewBag.Title);
        }
    }
}
