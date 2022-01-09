using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class EvaluateCost
    {
        public int ShelterId { get; set; }
        public int PurposeId { get; set; }
        public int AverageCostOnOnePetPerMonth { get; set; }
    }
}
