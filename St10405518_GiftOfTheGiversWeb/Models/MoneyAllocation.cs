﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class MoneyAllocation
    {
        [Key]
        public int MoneyAllocationId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal AllocationAmount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Allocation Date")]
        public DateTime AllocationDate { get; set; } = DateTime.Now;

        public string? AidType { get; set; } 

        // Add any other properties you need for MoneyAllocation
        [NotMapped]
        public int DisasterId { get; set; }

    }
}