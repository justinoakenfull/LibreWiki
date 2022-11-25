namespace LibreWiki.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorThoughts { get; set; }
        public int StarRating { get; set; }
        public bool IsRecommended { get; set; }
        public int CategoryId { get; set; }
    }
}
