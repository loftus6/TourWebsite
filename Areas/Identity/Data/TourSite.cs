using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TourWebsite.Areas.Identity.Data;


namespace TourWebsite.Models
{
    public enum VisibilityType : ushort
    {
        Private=0,
        Restricted=1,
        Public=2
    };
    public class TourSite
    {
        public TourSite()
        {
            Id = Guid.NewGuid().ToString();
            Tags = new List<string>(); //list initialize
            ApprovedEditUsers = new List<string>();
            ApprovedUsers = new List<string>();
        }
        public string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Range(-180, 180)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

        public VisibilityType Visibility { get; set; }
        public List<string> ApprovedEditUsers { get; set; }
        public List<string> ApprovedUsers { get; set; }

        public string? ThumbnailID { get; set; }

        public string? AudioID { get; set; }

        public string IconColor { get; set; }
        public string IconBorderColor { get; set; }


        public string? NextTourSiteID { get; set; }
        public string? LastTourSiteID { get; set; }

        [Required]
        public List<string> Tags { get; set; }

        //Analytics 
        public int UniqueClicks { get; set; } //If viewed
        public int UniqueVisitors { get; set; } //If actually in person
        public int Likes { get; set; } //How many people gave this site a thumbs up so to speak
        public float VisitorSeconds { get; set; } //Time spent by users at site, in seconds





        public string TagsToString()
        {
            var toRet = "";
            for (var i = 0; i < Tags.Count(); i++)
            {
                toRet += Tags[i];

                if (i < Tags.Count() - 1)
                    toRet += ", ";

            }

            return toRet;
        }
    }
}
