using System.Collections.Generic;
using Newtonsoft.Json;

namespace KendoUI12.Models
{
    public class Category
    {
        public int CategoryId { set; get; }
        public string CategoryName { set; get; }

        [JsonIgnore]
        public IList<Product> Products { set; get; }
    }
}