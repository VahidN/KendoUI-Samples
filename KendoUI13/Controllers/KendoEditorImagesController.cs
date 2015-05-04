using System.Web.Mvc;

namespace KendoUI13.Controllers
{
    public class KendoEditorImagesController : KendoEditorFilesController
    {
        public KendoEditorImagesController()
        {
            // بازنویسی مسیر پوشه‌ی فایل‌ها
            FilesFolder = "~/images";
        }

        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public ActionResult GetThumbnail(string path)
        {
            //todo: create thumb/ resize image

            path = GetSafeFileAndDirPath(path);
            return File(path, "image/png");
        }
    }
}