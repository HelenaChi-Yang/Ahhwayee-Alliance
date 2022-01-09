using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 註冊需填入資料
    /// </summary>
    public class RegisterPost
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string VerifyCode { get; set; }
    }
}
