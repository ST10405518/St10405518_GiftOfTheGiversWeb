using System.ComponentModel.DataAnnotations;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class GoodsDonation
    {
        [Key]
        [Required]
        public int GOODS_DONATION_ID { get; set; }

        // FIX: Initialize with empty string
        public string USERNAME { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        public DateTime? DATE { get; set; }

        [Required]
        [Display(Name = "Number of items")]
        public int ITEM_COUNT { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? CATEGORY { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? DESCRIPTION { get; set; }

        [Display(Name = "Donor")]
        public string? DONOR { get; set; }
    }
}