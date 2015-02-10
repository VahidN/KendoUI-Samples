using System;

namespace KendoUI06Mvc.Models
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public decimal Price { set; get; }
        public bool IsAvailable { set; get; }
        public DateTime AddDate { set; get; }
    }
}