using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 寄信需填入資料
    /// </summary>
    public class SendEmailPost
    {
        public string Account { get; set; }
        public string EmailAddress { get; set; }
    }
}
