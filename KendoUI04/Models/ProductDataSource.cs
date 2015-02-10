using System.Collections.Generic;

namespace KendoUI04.Models
{
    /// <summary>
    /// منبع داده فرضی جهت سهولت دموی برنامه
    /// </summary>
    public static class ProductDataSource
    {
        private static readonly IList<Product> _cachedItems;
        static ProductDataSource()
        {
            _cachedItems = createProductsDataSource();
        }

        public static IList<Product> LatestProducts
        {
            get { return _cachedItems; }
        }

        /// <summary>
        /// هدف صرفا تهیه یک منبع داده آزمایشی ساده تشکیل شده در حافظه است
        /// </summary>
        private static IList<Product> createProductsDataSource()
        {
            var list = new List<Product>();
            for (var i = 0; i < 1500; i++)
            {
                list.Add(new Product
                {
                    Id = i + 1,
                    Name = "نام " + (i + 1),
                    IsAvailable = (i % 2 == 0),
                    Price = 1000 + i
                });
            }
            return list;
        }
    }
}