namespace KendoUI11.Models
{
    public class BlogComment
    {
        public int Id { set; get; }

        public string Body { set; get; }

        public int? ParentId { get; set; }

        // مخصوص کندو یو آی هستند
        public bool HasChildren { get; set; }
        public string imageUrl { get; set; }
    }
}