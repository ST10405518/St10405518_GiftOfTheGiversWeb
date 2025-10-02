using System.ComponentModel.DataAnnotations;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class VolunteerTask
    {
        [Key]
        public int TaskID { get; set; }

        [Required, StringLength(100)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(50)]
        public required string Category { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Location")]
        public required string TaskLocation { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Required Volunteers")]
        public int RequiredVolunteers { get; set; }

        [Display(Name = "Current Volunteers")]
        public int CurrentVolunteers { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "Open";

        [StringLength(500)]
        [Display(Name = "Skills Required")]
        public string? SkillsRequired { get; set; }

        public virtual ICollection<TaskAssignment>? Assignments { get; set; }
    }
}