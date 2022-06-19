using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VehicleCRUD.Models
{
    public partial class VehicleDetailInfo
    {
        public int VehicleDetailId { get; set; }
        public int FkVehicleId { get; set; }
        public string Colour { get; set; }
        public string Grade { get; set; }
        public string Engine { get; set; }
        public string Fuel { get; set; }
        public string Transmission { get; set; }
        public string ExtrasInfo { get; set; }

        public virtual VehicleInfo FkVehicle { get; set; }
    }
}
