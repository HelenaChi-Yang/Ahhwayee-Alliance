using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 登入需填入資料
    /// </summary>
    public class LoginPost
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
