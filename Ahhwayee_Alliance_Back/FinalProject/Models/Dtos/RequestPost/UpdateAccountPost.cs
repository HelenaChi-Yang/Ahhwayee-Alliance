using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 更新帳號需填入資料
    /// </summary>
    public class UpdateAccountPost
    {
        public string Account { get; set; }
        public string NewAccount { get; set; }
    }
}
