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

namespace IT.Web.Areas.AWDriver.Controllers
{
    [Autintication]
    public class AWFDriverController : Controller
    {
        DriverViewModel driverViewModel = new DriverViewModel();
        List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        CustomerNoteOrderViewModel CustomerNoteOrderViewModel = new CustomerNoteOrderViewModel();
        List<DriverViewModel> driverViewModelss = new List<DriverViewModel>();
        WebServices webServices = new WebServices();

     
        public ActionResult Index()
        {
            var results = webServices.Post(new CountryViewModel(), "Country/All");
            if (results.Data != "[]")
            {
                 CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                if(CountryViewModel[0].CountryName != "Select Country")
                {
                    CountryViewModel.Insert(0, new CountryViewModel() { Id = 0, CountryName = "Select Country" });
                }
            }

            

            ViewBag.CountryList = CountryViewModel;
            return View(driverViewModel);
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

                //if (HttpContext.Cache["AWFuelDriverData"] != null)
                //{
                //    driverViewModelss = HttpContext.Cache["AWFuelDriverData"] as List<DriverViewModel>;
                //}
                //else
                //{
                    var result = webServices.Post(new DriverViewModel(), "AWFDriver/All/" + CompanyId);
                    driverViewModelss = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());

                    HttpContext.Cache["AWFuelDriverData"] = driverViewModelss;

               // }
                if (parm.sSearch != null)
                {

                    totalCount = driverViewModelss.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch)).Count();

                    driverViewModelss = driverViewModelss.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new DriverViewModel
                   {
                       Name = x.Name,
                       Id = x.Id,
                       Contact = x.Contact,
                       Email = x.Email,
                       UserName = x.UserName,
                       IsActive = x.IsActive

                   }).ToList();

                }
                else
                {
                    totalCount = driverViewModelss.Count();

                    driverViewModelss = driverViewModelss.Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new DriverViewModel
                         {

                             Name = x.Name,
                             Id = x.Id,
                             Contact = x.Contact,
                             Email = x.Email,
                             UserName = x.UserName,
                             IsActive = x.IsActive

                         }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = driverViewModelss,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = driverViewModelss,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult Create()
        {

            try
            {

                var results = webServices.Post(new CountryViewModel(), "Country/All");
                CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                //CountryViewModel.Sort();
                ViewBag.CountryList = CountryViewModel;

                return View(new DriverViewModel());
            }
            catch (Exception)
            {
                ViewBag.CountryList = CountryViewModel;

                return View(new DriverViewModel());
            }
        }

        [HttpPost]
        public ActionResult Create(DriverViewModel driverViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            driverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            driverViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
          

            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    var timestamp = DateTime.Now.TimeOfDay.Ticks;
                    driverViewModel.UID = timestamp.ToString();

                    int count = 0;
                    foreach (var file in files)
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/uploads/Driver-" + timestamp));
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            if (count == 0)
                            {

                                fileName = "PassportCopy-" + fileName;
                                driverViewModel.PassportCopy = fileName;
                            }
                            else if (count == 2)
                            {

                                fileName = "VisaCopy-" + fileName;
                                driverViewModel.VisaCopy = fileName;
                            }
                            else if (count == 3)
                            {

                                fileName = "IDUAECopyFront-" + fileName;
                                driverViewModel.IDUAECopyFront = fileName;
                            }
                            else if (count == 4)
                            {

                                fileName = "IDUAECopyBack-" + fileName;
                                driverViewModel.IDUAECopyBack = fileName;
                            }
                            else if (count == 5)
                            {

                                fileName = "DrivingLicenseFront-" + fileName;
                                driverViewModel.DrivingLicenseFront = fileName;
                            }
                            else if (count == 6)
                            {
                                fileName = "DrivingLicenseBack-" + fileName;
                                driverViewModel.DrivingLicenseBack = fileName;
                            }
                            var path = Path.Combine(Server.MapPath("~/Uploads/uploads/Driver-" + timestamp), fileName);
                            file.SaveAs(path);
                        }
                        count = count + 1;
                    }
                }
                

                var result = webServices.Post(driverViewModel, "AWFDriver/Add");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                   int results = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                    HttpContext.Cache.Remove("AWFuelDriverData");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return View(driverViewModel);
            }
            else
            {
                //model state not valid
                var results = webServices.Post(new CountryViewModel(), "Country/All");
                CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                ViewBag.CountryList = CountryViewModel;

                return Json("Success", JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult Details(int? Id)
        {
            try
            {
                driverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                driverViewModel.Id = Convert.ToInt32(Id);

                var result = webServices.Post(driverViewModel, "AWFDriver/Edit");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        driverViewModel = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                }

                List<CompanyImages> mylist = new List<CompanyImages>();
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.PassportCopy });
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.VisaCopy });
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.IDUAECopyFront });
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.IDUAECopyBack });
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.DrivingLicenseFront });
                mylist.Add(new CompanyImages { ImagesUrl = driverViewModel.DrivingLicenseBack });

                ViewBag.Images = mylist;

                var results = webServices.Post(new CountryViewModel(), "Country/All");
                List<CountryViewModel> CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                ViewBag.CountryList = CountryViewModel;

                return View(driverViewModel);
            }
            catch (Exception)
            {

                return Json("Failed");
            }
        }

        [HttpGet]
        public JsonResult Edit(int? Id)
        {
            try
            {
                driverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                driverViewModel.Id = Convert.ToInt32(Id);

                var result = webServices.Post(driverViewModel, "AWFDriver/Edit");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        driverViewModel = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                }

                List<LicenseTypeViewModel> types = new List<LicenseTypeViewModel>();
                types.Add(new LicenseTypeViewModel { Id = 1, Name = "Heavy" });
                types.Add(new LicenseTypeViewModel { Id = 2, Name = "Light" });

                ViewBag.LicenseType = types;

                var results = webServices.Post(new CountryViewModel(), "Country/All");
                List<CountryViewModel> CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                ViewBag.CountryList = CountryViewModel;

                return Json(driverViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(DriverViewModel driverViewModel)
        {
            try
            {
                //driverViewModel.DrivingLicenseExpiryDate = System.DateTime.Now;
                driverViewModel.LicenseExpiry = System.DateTime.Now.ToShortDateString();

                var result = webServices.Post(driverViewModel, "AWFDriver/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("AWFuelDriverData");
                    return Json(result.StatusCode, JsonRequestBehavior.AllowGet);
                }
                //return Redirect("/Driver-Details/" + driverViewModel.Id);
                else
                    return View(driverViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteImage(CompanyImages companyImages)
        {

            try
            {

                companyImages.ImageName = companyImages.ImagesUrl.Split('/').Skip(4).FirstOrDefault();

                companyImages.Flage = companyImages.ImageName.Split('-').Skip(0).FirstOrDefault();

                if (CommonFile.DeleteFile(companyImages.ImagesUrl))
                {
                    var result = webServices.Post(companyImages, "AWFDriver/DeleteImage");

                    return Json(result.StatusCode);
                }
                else
                {
                    return Json("Failed to delete, try again later");
                }
            }
            catch (Exception ex)
            {
                return Json("Failed to Delete");
            }
        }

        [HttpPost]
        public ActionResult Delete(DriverViewModel driverViewModel)
        {
            try
            {
                var result = webServices.Post(driverViewModel, "AWFDriver/Delete");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("AWFuelDriverData");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult IsDriverTakinVehicle(SearchViewModel searchViewModel)
        {
            try
            {
                var result = webServices.Post(searchViewModel, "AWFDriver/IsDriverTakinVehicle");
                int Res = 0;
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    Res = (new JavaScriptSerializer().Deserialize<int>(result.Data));
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult DriverAWFInfoByEmail(SearchViewModel searchViewModel)
        {
            try
            {
                var result = webServices.Post(searchViewModel, "AWFDriver/DriverAWFInfoByEmail");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        driverViewModel = (new JavaScriptSerializer().Deserialize<DriverViewModel>(result.Data.ToString()));
                        Session["DriverId"] = driverViewModel.DriverLoginId;
                    }

                    return Json(driverViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("Failed" + ex, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult DriverLoginHistory(DriverLoginHistoryViewModel driverLoginHistoryViewModel)
        {
            try
            {
                driverLoginHistoryViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(driverLoginHistoryViewModel, "AWFDriver/DriverLoginHistory");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return Json("Success", JsonRequestBehavior.AllowGet);
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult DriverAllOnline()
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new DriverViewModel(), "AWFDriver/DriverAllOnline/" + CompanyId);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    driverViewModels = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());
                    return Json(driverViewModels, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult CustomerOrderAsignToDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                customerNoteOrderViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var result = webServices.Post(customerNoteOrderViewModel, "AWFDriver/CustomerOrderAsignToDriver");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int k = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult DriverViewOrder(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(customerNoteOrderViewModel, "AWFDriver/DriverViewOrder");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        CustomerNoteOrderViewModel = (new JavaScriptSerializer()).Deserialize<CustomerNoteOrderViewModel>(result.Data.ToString());
                    }
                    return Json(CustomerNoteOrderViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult CustomerOrderAcceptDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                int Res;
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(customerNoteOrderViewModel, "AWFDriver/CustomerOrderAcceptDriver");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }


                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}