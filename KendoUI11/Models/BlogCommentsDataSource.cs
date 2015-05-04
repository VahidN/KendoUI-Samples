using System.Collections.Generic;

namespace KendoUI11.Models
{
    /// <summary>
    /// منبع داده فرضی جهت سهولت دموی برنامه
    /// </summary>
    public static class BlogCommentsDataSource
    {
        private static readonly IList<BlogComment> _cachedItems;
        static BlogCommentsDataSource()
        {
            _cachedItems = createBlogCommentsDataSource();
        }

        public static IList<BlogComment> LatestComments
        {
            get { return _cachedItems; }
        }

        /// <summary>
        /// هدف صرفا تهیه یک منبع داده آزمایشی ساده تشکیل شده در حافظه است
        /// </summary>
        private static IList<BlogComment> createBlogCommentsDataSource()
        {
            var list = new List<BlogComment>();

            var comment1 = new BlogComment
            {
                Id = 1, Body = "نظر من این است که", HasChildren = true, ParentId = null
            };
            list.Add(comment1);

            var comment12 = new BlogComment
            {
                Id = 2, Body = "پاسخی به نظر اول", HasChildren = true, ParentId = 1
            };
            list.Add(comment12);

            var comment12A = new BlogComment
            {
                Id = 3, Body = "پاسخی دیگری به نظر اول", HasChildren = false, ParentId = 1
            };
            list.Add(comment12A);

            var comment121 = new BlogComment
            {
                Id = 4, Body = "پاسخی به پاسخ به نظر اول", HasChildren = false, ParentId = 2
            };
            list.Add(comment121);

            var comment2 = new BlogComment
            {
                Id = 5, Body = "نظر 2", HasChildren = true, ParentId = null, imageUrl= "images/search.png"
            };
            list.Add(comment2);

            var comment21 = new BlogComment
            {
                Id = 6, Body = "پاسخ به نظر 2", HasChildren = false, ParentId = 5
            };
            list.Add(comment21);

            return list;
        }
    }
}