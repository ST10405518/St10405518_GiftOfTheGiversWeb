using System.Collections.Generic;

namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class IncomingDataModel
    {
        // FIX: Initialize all collections to avoid null reference warnings
        public IEnumerable<Disaster> Disasters { get; set; } = new List<Disaster>();
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; } = new List<GoodsDonation>();
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; } = new List<MoneyDonation>();
    }
}