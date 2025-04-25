namespace TourWebsite.Models
{
    public class PageEdit
    {
        public string Id { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailID { get; set; }

        public string? Title { get; set; }

        public string? BackgroundColor { get; set; }

        public string? AccentColor1 { get; set; }

        public bool RemoveThumbnail { get; set; }
    }
}
