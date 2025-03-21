using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Models.Tours
{
    public class CoordPair
    {
        public double Longitude { get; set; }

        public double Lattitude { get; set; }
    }
}
