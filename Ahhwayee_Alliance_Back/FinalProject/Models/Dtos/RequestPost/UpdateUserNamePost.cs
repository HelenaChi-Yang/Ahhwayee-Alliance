using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 更新使用者名稱需填入資料
    /// </summary>
    public class UpdateUserNamePost
    {
        public string Account { get; set; }
        public string NewUserName { get; set; }
    }
}
