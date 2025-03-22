using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TourWebsite.Models.Files;

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
        [DisplayName("Extension")]
        public string FileExtension { get; set; }
        [Required]
        [DisplayName("Type")]
        public FileType FileType { get; set; }
        [Required]
        [DisplayName("Name")]
        public string FileName { get; set; }
        [Required]
        public bool Embed { get; set; }

        public string EmbedUrl { get; set; }
        [Required]
        public List<string> Tags { get; set; }

    }
}
