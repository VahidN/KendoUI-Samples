using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.DynamicLinq;
using KendoUI06Mvc.Models;
using Newtonsoft.Json;

namespace KendoUI06Mvc.Controllers
{
    // »« «—À »—Ì° ŒÊ«’ «÷«›Ì Ê ”›«—‘Ì —« »Â ﬂ·«” Å«ÌÂ «÷«›Â „Ìùﬂ‰Ì„
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
            var queryString = this.Request.Url.ParseQueryString().GetKey(0);
            //«ÿ·«⁄«  —”ÌœÂ »« ›—„  ÃÌù”Ê‰ œ— œ«Œ· ﬂÊ∆—Ì «” —Ì‰ê ﬁ—«— ê—› Âù«” 
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

            // ê—Ìœ ¬Ì œÌ ÃœÌœ —« »Â «Ì‰ ’Ê—  œ—Ì«›  „Ìùﬂ‰œ
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