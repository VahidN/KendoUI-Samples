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
                //دريافت ريشه‌ها
                return Json(
                    BlogCommentsDataSource.LatestComments
                        .Where(x => x.ParentId == null) // ريشه‌ها
                        .ToList(),
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                //دريافت فرزندهاي يك ريشه
                return Json(
                    BlogCommentsDataSource.LatestComments
                              .Where(x => x.ParentId == id)
                              .ToList(),
                              JsonRequestBehavior.AllowGet);
            }
        }
    }
}