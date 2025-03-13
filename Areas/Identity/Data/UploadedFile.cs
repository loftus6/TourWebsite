using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Areas.Identity.Data
{
    public class UploadedFile

    {

        public UploadedFile()
        {
            Id = Guid.NewGuid().ToString();

        }

        public string Id { get; set; }

        [Required]
        public byte[] Bytes { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public string FileName { get; set; }

    }
}
