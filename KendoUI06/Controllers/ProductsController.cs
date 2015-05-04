using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kendo.DynamicLinq;
using KendoUI06.Models;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace KendoUI06.Controllers
{
    public class ProductsController : ApiController
    {
        public HttpResponseMessage Delete(int id)
        {
            var item = ProductDataSource.LatestProducts.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            ProductDataSource.LatestProducts.Remove(item);

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            var request = JsonConvert.DeserializeObject<DataSourceRequest>(
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            var list = ProductDataSource.LatestProducts;
            return list.AsQueryable()
                       .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        public HttpResponseMessage Post(Product product)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var id = 1;
            var lastItem = ProductDataSource.LatestProducts.LastOrDefault();
            if (lastItem != null)
            {
                id = lastItem.Id + 1;
            }
            product.Id = id;
            ProductDataSource.LatestProducts.Add(product);

            var response = Request.CreateResponse(HttpStatusCode.Created, product);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.Id }));
            // گرید آی دی جدید را به این صورت دریافت می‌کند
            response.Content = new ObjectContent<DataSourceResult>(
                new DataSourceResult { Data = new[] { product } }, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPut] // Add it to fix this error: The requested resource does not support http method 'PUT'
        public HttpResponseMessage Update(int id, Product product)
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
                return Request.CreateResponse(HttpStatusCode.NotFound);


            if (!ModelState.IsValid || id != product.Id)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            ProductDataSource.LatestProducts[item.Index] = product;
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}