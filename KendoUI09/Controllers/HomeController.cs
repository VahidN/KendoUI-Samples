using System.Collections.Generic;
using System.Web;
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
            this.ModelState.AddModelError("test", "Œÿ«Ì ¬“„«Ì‘Ì ”„  ”—Ê—");
            return View();
        }


        [HttpPost]
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, string codeId)
        {
            if (files != null)
            {
                // ...
                // Process the files and save them
                // ...
            }

            // Return an empty string to signify success
            return Content("");
        }

        [HttpPost]
        public ContentResult Remove(string[] fileNames)
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    // ...
                    // delete the files
                    // ...
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

    }
}