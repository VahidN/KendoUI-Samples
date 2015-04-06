using System.Linq;
using System.Text;
using System.Web.Mvc;
using KendoUI12.Models;
using Newtonsoft.Json;

namespace KendoUI12.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(); // shows the page.
        }

        [HttpGet]
        public ActionResult GetCategories()
        {
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(CategoriesDataSource.Items),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        [HttpGet]
        public ActionResult GetProducts(int categoryId)
        {

            var products = CategoriesDataSource.Items
                            .Where(category => category.CategoryId == categoryId)
                            .SelectMany(category => category.Products)
                            .ToList();

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(products),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }
    }
}