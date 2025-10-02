using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class TaskAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [ForeignKey("Volunteer")]
        public int VolunteerID { get; set; }

        [ForeignKey("VolunteerTask")]
        public int TaskID { get; set; }

        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Assigned";

        [Display(Name = "Hours Worked")]
        public decimal? HoursWorked { get; set; }

        [StringLength(500)]
        [Display(Name = "Volunteer Notes")]
        public string? VolunteerNotes { get; set; }

        [StringLength(500)]
        [Display(Name = "Admin Feedback")]
        public string? AdminFeedback { get; set; }

        public virtual Volunteer? Volunteer { get; set; }
        public virtual VolunteerTask? VolunteerTask { get; set; }
    }
}