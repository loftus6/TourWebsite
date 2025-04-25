namespace TourWebsite.Areas.Identity.Data
{
    public class NonTourPage
    {


        public NonTourPage()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailID { get; set; }

        public string? Title { get; set; }

        public string? BackgroundColor { get; set; }

        public string? AccentColor1 { get; set; }

    }
}
