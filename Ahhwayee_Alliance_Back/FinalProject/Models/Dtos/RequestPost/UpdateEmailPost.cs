using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 更新信箱需填入資料
    /// </summary>
    public class UpdateEmailPost
    {
        public string Account { get; set; }
        public string NewEmailAddress { get; set; }
        public string VerifyCode { get; set; }

    }
}
