using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 更新密碼需填入資料
    /// </summary>
    public class UpdatePassword
    {
        public string Account { get; set; }
        public string NewPassword { get; set; }
    }
}