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

        public string HandleSrc()
        {
            var file = this;

            if (file.Embed)
            {
                return file.EmbedUrl;
            }
            if (file.FileType == FileType.Image)
            {
                var ret = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(file.Bytes));
                return ret;
            }
            if (file.FileType == FileType.Audio)
            {
                var ret = String.Format("data:audio/mpeg;base64,{0}", Convert.ToBase64String(file.Bytes));
                return ret;
            }

            return "";
        }

        public string HtmlString()
        {
            var file = this;
            var src = HandleSrc();
            if (file.FileType == FileType.Image)
            {
                return "<img src=\"" + src + "\"alt=\"\" class=\"img-fluid\">";
            }
            if (file.FileType == FileType.Audio)
            {
                return "<audio controls autoplay> <source src=\""+src+"\"type=\"audio/mpeg\">Your browser does not support the audio element.</audio>";
            }
            return "";
        }

    }
}
