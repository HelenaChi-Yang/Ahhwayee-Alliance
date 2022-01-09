using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 儲值需填入資料
    /// </summary>
    public class PayInfoPost
    {
        public string Account { get; set; }

        public int Money { get; set; }
    }
}
