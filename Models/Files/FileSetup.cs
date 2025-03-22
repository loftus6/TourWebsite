using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Models.Files
{
    public class FileSetup
    {

        [Required]
        public FileType fileType { get; set; }


        [Required]
        public AttachType attachType { get; set; }
    }
}
