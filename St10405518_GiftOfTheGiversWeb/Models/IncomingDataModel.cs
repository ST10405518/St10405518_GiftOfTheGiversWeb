﻿namespace St10405518_GiftOfTheGiversWeb.Models
{
    public class IncomingDataModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; }
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; }
    }
}
