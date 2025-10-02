using System.ComponentModel.DataAnnotations;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, Phone]
        public required string PhoneNumber { get; set; }

        [Required, StringLength(200)]
        public required string Address { get; set; }

        [Required, StringLength(500)]
        [Display(Name = "Skills & Qualifications")]
        public required string Skills { get; set; }

        [StringLength(200)]
        public string? Availability { get; set; }

        [StringLength(100)]
        [Display(Name = "Emergency Contact")]
        public string? EmergencyContact { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public virtual ICollection<TaskAssignment>? Tasks { get; set; }
    }
}