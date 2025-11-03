using System.ComponentModel.DataAnnotations;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class MoneyDonation
    {
        [Key]
        [Required]
        public int MONEY_DONATION_ID { get; set; }

        // FIX: Initialize with empty string
        public string USERNAME { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? DATE { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal AMOUNT { get; set; }

        [Display(Name = "Donor")]
        public string? DONOR { get; set; }
    }
}