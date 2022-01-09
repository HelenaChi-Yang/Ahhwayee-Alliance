using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class VerifyForm
    {
        public string Account { get; set; }
        public string VerifyCode { get; set; }
    }
}
