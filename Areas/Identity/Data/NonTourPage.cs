using System.ComponentModel;

namespace TourWebsite.Areas.Identity.Data
{
    public class NonTourPage
    {


        public NonTourPage()
        {
            Id = Guid.NewGuid().ToString();
            ColorName1 = "";
            ColorName2 = "";
        }
        public string Id { get; set; }
        public string? Description { get; set; }

        public string? Title { get; set; }


        [DisplayName("Main Color")]
        public string? MainColor { get; set; }

        [DisplayName("Accent Color")]
        public string? AccentColor { get; set; }

        public string ColorName1 { get; set; }
        public string ColorName2 { get; set; }

    }
}
