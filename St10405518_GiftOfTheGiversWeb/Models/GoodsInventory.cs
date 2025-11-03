using System.ComponentModel.DataAnnotations;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class GoodsInventory
    {
        [Key]
        public int GOODS_INVENTORY_ID { get; set; }

        // FIX: Initialize with empty string
        public string CATEGORY { get; set; } = string.Empty;

        public int? ITEM_COUNT { get; set; }
    }
}