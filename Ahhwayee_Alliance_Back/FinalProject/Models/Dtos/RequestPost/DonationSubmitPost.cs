using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 捐點需填入資料
    /// </summary>
    public class DonationSubmitPost
    {
        public string Account{ get; set; }
        public int DonationPoints { get; set; }
        public int ShelterId{ get; set; }
        public int PurposeId { get; set; }
    }
}
