using Kendo02MVC.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Kendo02MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProducts(string param1, string param2)
        {
            var products = new List<Product>();
            for (var i = 1; i <= 100; i++)
            {
                products.Add(new Product { Id = i, Name = "Product " + i });
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}