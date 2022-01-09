using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class Donation
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public DateTime DonationTime { get; set; }
        public int DonationPoints { get; set; }
        public int ShelterId { get; set; }
        public int PurposeId { get; set; }
    }
}
