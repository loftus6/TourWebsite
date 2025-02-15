using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Models.Roles
{
    public class RoleModification
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }



        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string? AddEmail { get; set; }

        public string[]? DeleteIds { get; set; }
    }
}
