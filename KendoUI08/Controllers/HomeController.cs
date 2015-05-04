using System.Web.Mvc;
using Mvc4TestViewModel.Models;

namespace Mvc4TestViewModel.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OrderDetailViewModel model)
        {
            this.ModelState.AddModelError("test", "خطای آزمایشی سمت سرور");
            return View();
        }


        //[HttpPost]
        public ActionResult DoesUserExist(string username)
        {
            return Json(username == "Vahid", JsonRequestBehavior.AllowGet);
        }
    }
}