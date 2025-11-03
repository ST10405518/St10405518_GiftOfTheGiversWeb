using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class Disaster
    {
        [Key]
        [Required]
        public int DISTATER_ID { get; set; }

        // FIX: Make nullable or initialize
        public string USERNAME { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Start Date")]
        public DateTime? STARTDATE { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "End Date")]
        public DateTime? ENDDATE { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string? LOCATION { get; set; }

        [Required]
        [Display(Name = "Aid Type")]
        public string? AID_TYPE { get; set; }

        [Display(Name = "Is Active")]
        public int IsActive { get; set; }
    }
}