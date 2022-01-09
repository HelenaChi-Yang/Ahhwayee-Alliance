using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class TransferDonation
    {
        public int ShelterId { get; set; }
        public DateTime TransferDate { get; set; }
        public int PurposeId { get; set; }
        public int DonationAmount { get; set; }
    }
}
