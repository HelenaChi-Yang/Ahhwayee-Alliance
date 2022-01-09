using System;
using System.Collections.Generic;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class ShelterInformation
    {
        public int ShelterId { get; set; }
        public string ShelterName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ShelterPhoneNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public short ContainMaxNumber { get; set; }
        public short RealNumber { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ShelterImgUrl { get; set; }
        public string ShelterImgName { get; set; }
    }
}
