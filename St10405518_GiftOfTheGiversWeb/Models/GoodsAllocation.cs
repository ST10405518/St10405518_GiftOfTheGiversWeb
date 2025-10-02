﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class GoodsAllocation
    {
        [Key]
        public int GoodsAllocationId { get; set; }

        [Required]
        [Display(Name = "Number of items")]
        public int ITEM_COUNT { get; set; }

        [Display(Name = "Category")]
        public String? CATEGORY { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Allocation Date")]
        public DateTime? AllocationDate { get; set; } = DateTime.Now;

        public string? AidType { get; set; }

        // Add any other properties you need for MoneyAllocation
        [NotMapped]
        public int DisasterId { get; set; }
    }
}