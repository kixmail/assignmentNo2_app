using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleCRUD.Models
{
    /// <summary>
    /// System is using this class for add or update vehicle data
    /// </summary>
    [ModelMetadataType(typeof(VehicleValidator))]
    public partial class VehicleMappingData
    {

        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ChassisNo { get; set; }
        public string DrivingType { get; set; }
        public int? PassengerCapacity { get; set; }
        public int? Year { get; set; }
        public string Colour { get; set; }
        public string Grade { get; set; }
        public string Engine { get; set; }
        public string Fuel { get; set; }
        public string Transmission { get; set; }
        public string ExtrasInfo { get; set; }

        public virtual ICollection<VehicleDetailInfo> VehicleDetailInfo { get; set; }
    }
}
