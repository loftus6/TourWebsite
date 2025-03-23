using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Files
{
    public static class FileStatics
    {

        public static string HandleSrc(UploadedFile file)
        {
            var test = file;
            if (file.FileType == FileType.Image)
            {
                var ret = file.Embed ? file.EmbedUrl : String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(file.Bytes));
                return ret;
            }

            return "";
        }
    }
}
