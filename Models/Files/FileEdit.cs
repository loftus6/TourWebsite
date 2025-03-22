using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Files
{
    public class FileEdit
    {

        //For create
        public bool EmbedOrUpload { get; set; }
        public IFormFile? NewFile { get; set; }

        public string EmbedUrl { get; set; }

        public string FileName { get; set; }

        public FileType FileType { get; set; }


        //For edit
        public UploadedFile FileToEdit { get; set; }
        public string FileToEditId { get; set; }
    }
}
