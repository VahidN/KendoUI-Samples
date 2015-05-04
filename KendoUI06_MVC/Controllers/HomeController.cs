using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.DynamicLinq;
using KendoUI06Mvc.Models;
using Newtonsoft.Json;

namespace KendoUI06Mvc.Controllers
{
    // با ارث بری، خواص اضافی و سفارشی را به کلاس پایه اضافه می‌کنیم
    public class CustomDataSourceRequest : DataSourceRequest
    {
        public string Param1 { set; get; }
        public string Param2 { set; get; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(); // shows the page.
        }

        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            var item = ProductDataSource.LatestProducts.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return new HttpNotFoundResult();

            ProductDataSource.LatestProducts.Remove(item);

            return Json(item);
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            System.Threading.Thread.Sleep(3000); //شبیه سازی یک عملیات طولانی برای نمایش منتظر بمانید

            var queryString = this.Request.Url.ParseQueryString().GetKey(0);
            //اطلاعات رسیده با فرمت جی‌سون در داخل کوئری استرینگ قرار گرفته‌است
            //{"param1":"val1","param2":"val2","take":10,"skip":0,"page":1,"pageSize":10,"sort":[{"field":"Id","dir":"desc"}]}
            var request = JsonConvert.DeserializeObject<CustomDataSourceRequest>(queryString);

            var list = ProductDataSource.LatestProducts;
            return Json(list.AsQueryable()
                       .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter),
                       JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var id = 1;
            var lastItem = ProductDataSource.LatestProducts.LastOrDefault();
            if (lastItem != null)
            {
                id = lastItem.Id + 1;
            }
            product.Id = id;
            ProductDataSource.LatestProducts.Add(product);

            // گرید آی دی جدید را به این صورت دریافت می‌کند
            return Json(new DataSourceResult { Data = new[] { product } });
        }

        [HttpPut] // Add it to fix this error: The requested resource does not support http method 'PUT'
        public ActionResult UpdateProduct(int id, Product product)
        {
            var item = ProductDataSource.LatestProducts
                                        .Select(
                                            (prod, index) =>
                                                new
                                                {
                                                    Item = prod,
                                                    Index = index
                                                })
                                        .FirstOrDefault(x => x.Item.Id == id);
            if (item == null)
                return new HttpNotFoundResult();


            if (!ModelState.IsValid || id != product.Id)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductDataSource.LatestProducts[item.Index] = product;

            //Return HttpStatusCode.OK
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}