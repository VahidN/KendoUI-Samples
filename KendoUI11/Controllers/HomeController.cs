using System.Linq;
using System.Web.Mvc;
using KendoUI11.Models;

namespace KendoUI11.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(); // shows the page.
        }

        [HttpGet]
        public ActionResult GetBlogComments(int? id)
        {
            if (id == null)
            {
                //دریافت ریشه‌ها
                return Json(
                    BlogCommentsDataSource.LatestComments
                        .Where(x => x.ParentId == null) // ریشه‌ها
                        .ToList(),
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                //دریافت فرزندهای یک ریشه
                return Json(
                    BlogCommentsDataSource.LatestComments
                              .Where(x => x.ParentId == id)
                              .ToList(),
                              JsonRequestBehavior.AllowGet);
            }
        }
    }
}