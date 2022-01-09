using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class UserImage
    {
        public int UserId { get; set; }
        public byte[] Image { get; set; }
    }
}
