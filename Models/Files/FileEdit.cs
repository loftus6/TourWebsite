namespace TourWebsite.Models.Files
{
    public class FileEdit
    {
        public bool EmbedOrUpload { get; set; }
        public IFormFile? NewFile { get; set; }

        public string EmbedUrl { get; set; }
    }
}
