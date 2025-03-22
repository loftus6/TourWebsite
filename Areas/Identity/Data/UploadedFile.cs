using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Areas.Identity.Data
{
    public class UploadedFile

    {

        public UploadedFile()
        {
            Id = Guid.NewGuid().ToString();
            Tags = new List<string>(); //list initialize
        }

        public string Id { get; set; }

        public byte[] Bytes { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public bool Embed { get; set; }

        public string EmbedUrl { get; set; }
        [Required]
        public List<string> Tags { get; set; }

    }
}
