using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.Dtos.RequestPost
{
    /// <summary>
    /// 更新頭貼url需填入資料
    /// </summary>
    public class UpdateImageUrl
    {
        public string Account { get; set; }
        public string NewImageUrl { get; set; }
    }
}
