using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.FuelTransfer.Controllers
{

    [Autintication]
    public class FuelTransferController : Controller
    {
        WebServices webServices = new WebServices();
        List<VehicleViewModel> VehicleViewModels = new List<VehicleViewModel>();
        FuelTransferViewModel fuelTransferViewModel = new FuelTransferViewModel();
        List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
        List<FuelTransferViewModel> fuelTransferViewModels = new List<FuelTransferViewModel>();


        public ActionResult Index()
        {
            try
            {

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var result = webServices.Post(new VehicleViewModel(), "AWFVehicle/All/" + CompanyId);
                if (result.Data != "[]")
                {
                    VehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());
                }
                if (VehicleViewModels.Count > 0)
                {
                    if (VehicleViewModels[0].TraficPlateNumber != "Select Vehicle")
                    {
                        VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle" });
                    }

                    ViewBag.VehicleViewModels = VehicleViewModels;
                }


                var results = webServices.Post(new CustomerOrderViewModel(), "Site/All");
                if (results.Data != "[]")
                {
                    siteViewModels = (new JavaScriptSerializer()).Deserialize<List<SiteViewModel>>(results.Data.ToString());
                }
                if (siteViewModels.Count > 0)
                {
                    if (siteViewModels[0].SiteName != "Select Site")
                    {
                        siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });
                    }
                }
                ViewBag.siteViewModels = siteViewModels;

                return View(fuelTransferViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public JsonResult GetAll(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["FuelTransferViewModel"] != null)
                {
                    fuelTransferViewModels = HttpContext.Cache["FuelTransferViewModel"] as List<FuelTransferViewModel>;
                }
                else
                {
                    var result = webServices.Post(new FuelTransferViewModel(), "FuelTransfer/All");

                    fuelTransferViewModels = (new JavaScriptSerializer()).Deserialize<List<FuelTransferViewModel>>(result.Data.ToString());

                    HttpContext.Cache["FuelTransferViewModel"] = fuelTransferViewModels;
                }
                if (parm.sSearch != null)
                {

                    totalCount = fuelTransferViewModels.Where(x => x.SiteName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.Contains(parm.sSearch) ||
                               x.UserName.Contains(parm.sSearch)).Count();

                    fuelTransferViewModels = fuelTransferViewModels.ToList()
                        .Where(x => x.SiteName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.Contains(parm.sSearch) ||
                               x.UserName.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)

                   .Select(x => new FuelTransferViewModel
                   {
                       SiteName = x.SiteName,
                       Id = x.Id,
                       TraficPlateNumber = x.TraficPlateNumber,
                       Quantity = x.Quantity,
                       UserName = x.UserName,
                       IsActive = x.IsActive,
                       FuelTransferDate = x.FuelTransferDate,

                   }).ToList();

                }
                else
                {
                    totalCount = fuelTransferViewModels.Count();

                    fuelTransferViewModels = fuelTransferViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new FuelTransferViewModel
                         {

                             SiteName = x.SiteName,
                             Id = x.Id,
                             TraficPlateNumber = x.TraficPlateNumber,
                             Quantity = x.Quantity,
                             UserName = x.UserName,
                             IsActive = x.IsActive,
                             FuelTransferDate = x.FuelTransferDate,

                         }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = fuelTransferViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = fuelTransferViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult Create(FuelTransferViewModel fuelTransferViewModel)
        {
            try
            {
                fuelTransferViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(fuelTransferViewModel, "FuelTransfer/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("FuelTransferViewModel");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult Edit(int Id)
        {
            try
            {
                var result = webServices.Post(new FuelTransferViewModel(), "FuelTransfer/Edit/" + Id);
                if (result.Data != "[]")
                {
                    fuelTransferViewModel = (new JavaScriptSerializer()).Deserialize<FuelTransferViewModel>(result.Data.ToString());
                }
                return Json(fuelTransferViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public ActionResult Edit(FuelTransferViewModel fuelTransferViewModel)
        {

            try
            {
                fuelTransferViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(fuelTransferViewModel, "FuelTransfer/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("FuelTransferViewModel");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public ActionResult Details(int Id)
        {
            try
            {
                var result = webServices.Post(new FuelTransferViewModel(), "FuelTransfer/Details/" + Id);
                if (result.Data != "[]")
                {
                    fuelTransferViewModel = (new JavaScriptSerializer()).Deserialize<FuelTransferViewModel>(result.Data.ToString());
                }
                return View(fuelTransferViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public ActionResult LoadVehicleFuelTransfer()
        {
            int CompanyId = Convert.ToInt32(Session["CompanyId"]);

            var result = webServices.Post(new VehicleViewModel(), "AWFVehicle/All/" + CompanyId);
            if (result.Data != "[]")
            {
                VehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());
            }
            if (VehicleViewModels.Count > 0)
            {
                if (VehicleViewModels[0].TraficPlateNumber != "Select Vehicle")
                {
                    VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle" });
                }

                return Json(VehicleViewModels,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult LoadSiteFuelTransfer()
        {
            try
            {
                var results = webServices.Post(new CustomerOrderViewModel(), "Site/All");
                if (results.Data != "[]")
                {
                    siteViewModels = (new JavaScriptSerializer()).Deserialize<List<SiteViewModel>>(results.Data.ToString());
                }
                if (siteViewModels.Count > 0)
                {
                    if (siteViewModels[0].SiteName != "Select Site")
                    {
                        siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });
                        return Json(siteViewModels, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}