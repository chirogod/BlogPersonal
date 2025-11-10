namespace BlogPersonal.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateOnly PublishedDate { get; set; }
        public string Content { get; set; }

    }
}
