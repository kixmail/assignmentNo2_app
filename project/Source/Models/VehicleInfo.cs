using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VehicleCRUD.Models
{
  
    public partial class VehicleInfo
    {
        public VehicleInfo()
        {
            VehicleDetailInfo = new HashSet<VehicleDetailInfo>();
        }

        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ChassisNo { get; set; }
        public string DrivingType { get; set; }
        public int? PassengerCapacity { get; set; }
        public int? Year { get; set; }

        public virtual ICollection<VehicleDetailInfo> VehicleDetailInfo { get; set; }
    }

    public class VehicleValidator
    {
        [Required]
        
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Chassis Number")]
        public string ChassisNo { get; set; }
    }
}
