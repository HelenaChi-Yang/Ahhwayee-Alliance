using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class UserInformation
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public int RemainingPoints { get; set; }
        public string ValidToken { get; set; }
        public string UserImageUrl { get; set; }
        public byte[] UserImage { get; set; }
    }
}
