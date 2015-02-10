using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using KendoChart.Models;

namespace KendoChart.Controllers
{
    public class ProductsController : ApiController
    {
        public IEnumerable<ProductViewModel> Get()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "ProductA"},
                new Product {Id = 2, Name = "ProductA"},
                new Product {Id = 3, Name = "ProductA"},
                new Product {Id = 4, Name = "ProductA"},
                new Product {Id = 5, Name = "ProductA"},
                new Product {Id = 6, Name = "ProductB"},
                new Product {Id = 7, Name = "ProductB"},
                new Product {Id = 8, Name = "ProductB"}
            };
            var query = products.GroupBy(p => p.Name).Select(p => new ProductViewModel
            {
                Value = p.Key,
                Count = p.Count()
            });
            return query;
        }
    }
}