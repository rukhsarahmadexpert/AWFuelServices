using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Vehicle.Controllers
{

    //[Autintication]
    public class VehicleController : Controller
    {
        WebServices webServices = new WebServices();
        List<VehicleTypeViewModel> vehicleTypeViewModels = new List<VehicleTypeViewModel>();
        List<VehicleViewModel> VehicleViewModels = new List<VehicleViewModel>();
        VehicleViewModel vehicleViewModel = new VehicleViewModel();


        public ActionResult Index()
        {
            var results = webServices.Post(new VehicleTypeViewModel(), "VehicleType/GetAll");
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (results.Data != "[]")
                {
                    vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(results.Data.ToString());

                    if(vehicleTypeViewModels[0].VehicleType != "Select Vehicle Type")
                    {
                        vehicleTypeViewModels.Insert(0, new VehicleTypeViewModel() { Id = 0, VehicleType = "Select Vehicle Type" });
                    }
                }
            }

            ViewBag.vehicleType = vehicleTypeViewModels;

            return View(vehicleViewModel);
        }

        public JsonResult GetAll(DataTablesParm parm)
        {

            List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
                if (result.Data != "[]")
                {
                    VehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = VehicleViewModels.Where(x => x.VehicleTypeName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.Contains(parm.sSearch) ||
                               x.Color.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Brand.Contains(parm.sSearch)).Count();

                    vehicleViewModels = VehicleViewModels.ToList()
                        .Where(x => x.VehicleTypeName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.Contains(parm.sSearch) ||
                               x.Color.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Brand.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new VehicleViewModel
                   {
                       VehicleTypeName = x.VehicleTypeName,
                       Id = x.Id,
                       TraficPlateNumber = x.TraficPlateNumber,
                       Color = x.Color,
                       Brand = x.Brand,
                       MulkiaExpiry = x.MulkiaExpiry

                   }).ToList();
                }
                else
                {
                    totalCount = VehicleViewModels.Count();

                    vehicleViewModels = VehicleViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new VehicleViewModel
                         {

                             VehicleTypeName = x.VehicleTypeName,
                             Id = x.Id,
                             TraficPlateNumber = x.TraficPlateNumber,
                             Color = x.Color,
                             Brand = x.Brand,
                             MulkiaExpiry = x.MulkiaExpiry

                         }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = vehicleViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = vehicleViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex  ;
            }

        }

        [HttpGet]
        public ActionResult Create()
        {

            var results = webServices.Post(new VehicleTypeViewModel(), "VehicleType/GetAll");
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (results.Data != "[]")
                {
                    vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(results.Data.ToString());
                }
            }

            ViewBag.vehicleType = vehicleTypeViewModels;

            return View();
        }

        [HttpPost]
        public ActionResult Create(VehicleViewModel vehicleViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                vehicleViewModel.InsuranceExpiry = System.DateTime.Now.ToShortDateString();
                vehicleViewModel.MulkiaExpiry = System.DateTime.Now.ToShortDateString();
                vehicleViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                vehicleViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                if (files != null)
                {
                    var timestamp = DateTime.Now.TimeOfDay.Ticks;
                    vehicleViewModel.UID = timestamp.ToString();

                    int count = 0;
                    foreach (var file in files)
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/uploads/Vehicle-" + timestamp));
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            if (count == 0)
                            {

                                fileName = "MulkiaFront1-" + fileName;
                                vehicleViewModel.MulkiaFront1 = fileName;
                            }
                            else if (count == 1)
                            {

                                fileName = "MulkiaBack1-" + fileName;
                                vehicleViewModel.MulkiaBack1 = fileName;
                            }
                            else if (count == 2)
                            {

                                fileName = "MulkiaFront2-" + fileName;
                                vehicleViewModel.MulkiaFront2 = fileName;
                            }
                            else if (count == 3)
                            {

                                fileName = "MulkiaBack2-" + fileName;
                                vehicleViewModel.MulkiaBack2 = fileName;
                            }

                            var path = Path.Combine(Server.MapPath("~/Uploads/uploads/Vehicle-" + timestamp), fileName);
                            file.SaveAs(path);
                        }
                        count = count + 1;
                    }
                }

                var result = webServices.Post(vehicleViewModel, "Vehicle/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return Json("Success", JsonRequestBehavior.AllowGet);
                else
                    ViewBag.vehicleType = vehicleTypeViewModels;
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var results = webServices.Post(new VehicleTypeViewModel(), "VehicleType/GetAll");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (results.Data != "[]")
                    {
                        vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(results.Data.ToString());
                    }
                }
                ViewBag.vehicleType = vehicleTypeViewModels;

                return View(vehicleTypeViewModels);
            }

        }

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            try
            {
                vehicleViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                if (Id != 0)
                {
                    vehicleViewModel.Id = Convert.ToInt32(Id);
                }

                var result = webServices.Post(vehicleViewModel, "Vehicle/Edit");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        vehicleViewModel = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                }

                List<CompanyImages> mylist = new List<CompanyImages>();
                mylist.Add(new CompanyImages { ImagesUrl = vehicleViewModel.MulkiaFront1 });
                mylist.Add(new CompanyImages { ImagesUrl = vehicleViewModel.MulkiaBack1 });
                mylist.Add(new CompanyImages { ImagesUrl = vehicleViewModel.MulkiaFront2 });
                mylist.Add(new CompanyImages { ImagesUrl = vehicleViewModel.MulkiaBack2 });

                ViewBag.Images = mylist;

                var results = webServices.Post(new VehicleTypeViewModel(), "VehicleType/GetAll");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(results.Data.ToString());
                }

                ViewBag.vehicleType = vehicleTypeViewModels;


                return View(vehicleViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            try
            {
                vehicleViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                vehicleViewModel.Id = Convert.ToInt32(Id);
                var result = webServices.Post(vehicleViewModel, "Vehicle/Edit");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        vehicleViewModel = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                }

                var results = webServices.Post(new VehicleTypeViewModel(), "VehicleType/GetAll");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (results.Data != "[]")
                    {
                        vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(results.Data.ToString());
                    }
                }

                ViewBag.vehicleType = vehicleTypeViewModels;

                return Json(vehicleViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(VehicleViewModel vehicleViewModel)
        {
            try
            {
                vehicleViewModel.MulkiaExpiry = System.DateTime.Now.ToShortDateString();
                vehicleViewModel.InsuranceExpiry = System.DateTime.Now.ToShortDateString();


                var results = webServices.Post(vehicleViewModel, "Vehicle/Update");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    //singleIntegerValueResult = (new JavaScriptSerializer()).Deserialize<SingleIntegerValueResult>(results.Data);

                    //return Redirect("/Vehicle-Details/" + results.Data);

                    return Json("Success");
                }
                return View(vehicleViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DeleteImage(CompanyImages companyImages)
        {

            try
            {

                companyImages.ImageName = companyImages.ImagesUrl.Split('/').Skip(4).FirstOrDefault();

                companyImages.Flage = companyImages.ImageName.Split('-').Skip(0).FirstOrDefault();

                if (CommonFile.DeleteFile(companyImages.ImagesUrl))
                {
                    var result = webServices.Post(companyImages, "Vehicle/DeleteImage");

                    return Json(result.StatusCode);
                }
                else
                {
                    return Json("Failed to delete, try again later");
                }
            }
            catch (Exception ex)
            {
                return Json("Failed to Delete", ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Delete(VehicleTypeViewModel vehicleTypeViewModel)
        {
            try
            {
                var result = webServices.Post(vehicleTypeViewModel, "Vehicle/Delete");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
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



    }
}