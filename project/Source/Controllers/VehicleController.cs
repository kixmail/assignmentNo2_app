using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleCRUD.Models;

namespace VehicleCRUD.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleCRUDContext _context;

        public VehicleController(VehicleCRUDContext context)
        {
            _context = context;
        }
        // GET: VehicleController

        /// <summary>
        /// Displying all avialble vehicle on home page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            // return View();

            var vehicles = _context.VehicleInfo.ToList();

            if (_context.VehicleDetailInfo != null && _context.VehicleDetailInfo.Count() > 0)
            {
                foreach (var item in vehicles)
                {
                    var vehicleDetails = (from o in _context.VehicleDetailInfo where o.FkVehicleId == item.VehicleId select o).ToList();

                    item.VehicleDetailInfo = vehicleDetails;
                }
            }
            return View(vehicles);
        }



        // GET: VehicleController/Create
        //AddOrEdit Get Method
        /// <summary>
        /// fetch the vehicle basic and detail information and load the data on edit
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult AddOrEdit(int? vehicleId)
        {
            ViewBag.PageName = vehicleId == null ? "Create Vehcile" : "Edit Vehicle";
            ViewBag.IsEdit = vehicleId == null ? false : true;
            if (vehicleId == null)
            {
                return View();
            }
            else
            {
                var vehicle = _context.VehicleInfo.Find(vehicleId);
                VehicleDetailInfo vehicleDetailInfo = new VehicleDetailInfo();

                vehicleDetailInfo = (from o in _context.VehicleDetailInfo where o.FkVehicleId == vehicle.VehicleId select o).FirstOrDefault();


                VehicleMappingData vehicleMapData = MappVehileData(vehicle, vehicleDetailInfo);
                if (vehicle == null)
                {
                    return NotFound();
                }
                return View(vehicleMapData);
            }

            static VehicleMappingData MappVehileData(VehicleInfo vehicle, VehicleDetailInfo vehicleDetailInfo)
            {
                VehicleMappingData vehicleMapData = new VehicleMappingData();
                vehicleMapData.VehicleId = vehicle.VehicleId;
                vehicleMapData.ChassisNo = vehicle.ChassisNo;
                vehicleMapData.DrivingType = vehicle.DrivingType;

                vehicleMapData.Make = vehicle.Make;
                vehicleMapData.Model = vehicle.Model;
                vehicleMapData.PassengerCapacity = vehicle.PassengerCapacity;
                vehicleMapData.Year = vehicle.Year;

                if (vehicleDetailInfo != null)
                {
                    vehicleMapData.Transmission = vehicleDetailInfo.Transmission;
                    vehicleMapData.Grade = vehicleDetailInfo.Grade;
                    vehicleMapData.Fuel = vehicleDetailInfo.Fuel;
                    vehicleMapData.ExtrasInfo = vehicleDetailInfo.ExtrasInfo;
                    vehicleMapData.Engine = vehicleDetailInfo.Engine;
                    vehicleMapData.Colour = vehicleDetailInfo.Colour;
                }

                return vehicleMapData;
            }
        }
        /// <summary>
        /// add or update vehicle information
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="vehicleData"></param>
        /// <returns></returns>
        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddOrEdit(int vehicleId, [Bind("VehicleId,Make,Model,ChassisNo,DrivingType,PassengerCapacity,Year,Colour,Grade,Engine,Fuel,Transmission,ExtrasInfo")] VehicleMappingData vehicleData)
        {
            bool IsVehicleExist = false;

            VehicleInfo vehicle = _context.VehicleInfo.Find(vehicleId);

            VehicleDetailInfo vehicleDetailInfo = new VehicleDetailInfo();



            if (vehicle != null)
            {
                IsVehicleExist = true;
                vehicleDetailInfo = (from o in _context.VehicleDetailInfo where o.FkVehicleId == vehicle.VehicleId select o).FirstOrDefault();
            }
            else
            {
                vehicle = new VehicleInfo();


            }
            if (vehicleDetailInfo == null) vehicleDetailInfo = new VehicleDetailInfo();
            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.Make = vehicleData.Make;
                    vehicle.Model = vehicleData.Model;
                    vehicle.ChassisNo = vehicleData.ChassisNo;
                    vehicle.Year = vehicleData.Year;
                    vehicle.PassengerCapacity = vehicleData.PassengerCapacity;
                    vehicle.DrivingType = vehicleData.DrivingType;

                    vehicleDetailInfo.Colour = vehicleData.Colour;
                    vehicleDetailInfo.Engine = vehicleData.Engine;
                    vehicleDetailInfo.ExtrasInfo = vehicleData.ExtrasInfo;
                    vehicleDetailInfo.FkVehicleId = vehicleData.VehicleId;
                    vehicleDetailInfo.Grade = vehicleData.Grade;
                    vehicleDetailInfo.Transmission = vehicleData.Transmission;
                    if (IsVehicleExist)
                    {
                        _context.Update(vehicle);
                        _context.Update(vehicleDetailInfo);

                    }
                    else
                    {
                        _context.Add(vehicle);

                    }
                    _context.SaveChanges();

                    //adding detail record
                    if (!IsVehicleExist)
                    {
                        try
                        {
                            vehicleDetailInfo.FkVehicleId = _context.VehicleInfo.ToList().LastOrDefault().VehicleId; //newly added record
                            _context.Add(vehicleDetailInfo);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleData);
        }

        // display Vehicle Details
        [Authorize]
        public IActionResult Details(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return NotFound();
            }
            var vehicle = _context.VehicleInfo.FirstOrDefault(m => m.VehicleId == vehicleId);
            vehicle.VehicleDetailInfo.Clear();
            vehicle.VehicleDetailInfo.Add((from o in _context.VehicleDetailInfo where o.FkVehicleId == vehicle.VehicleId select o).FirstOrDefault());
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }
        // GET: load vehicle info on delete screen
        [Authorize]
        public IActionResult Delete(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return NotFound();
            }
            var vehicle = _context.VehicleInfo.FirstOrDefault(m => m.VehicleId == vehicleId);

            return View(vehicle);
        }

        // POST: Vehicle/Delete/1 delete selected vehicle record
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int vehicleId)
        {
            

            VehicleDetailInfo vehicleDetailInfo = (from o in _context.VehicleDetailInfo where o.FkVehicleId == vehicleId select o).FirstOrDefault();
            //delete child record i.e vehicle detail
            if (vehicleDetailInfo != null)
            {
                var detailInfo = _context.VehicleDetailInfo.Find(vehicleDetailInfo.VehicleDetailId);
                _context.VehicleDetailInfo.Remove(detailInfo);
                _context.SaveChanges();
            }

            //deleting parent record i.e vehicle info

            var vehicle = _context.VehicleInfo.Find(vehicleId);
            _context.VehicleInfo.Remove(vehicle);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
