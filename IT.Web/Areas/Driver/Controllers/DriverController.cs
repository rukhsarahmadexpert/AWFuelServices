using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Driver.Controllers
{

   // [Autintication]
    public class DriverController : Controller
    {
        WebServices webServices = new WebServices();

        DriverViewModel driverViewModel = new DriverViewModel();
        List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();


        public ActionResult Index()
        {
            var results = webServices.Post(new CountryViewModel(), "Country/All");
            if (results.Data != "[]")
            {
                 CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                if(CountryViewModel[0].CountryName != "Select Country")
                {
                    CountryViewModel.Insert(0, new Core.ViewModels.CountryViewModel() { Id=0,CountryName = "Select Country" });
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

                if (HttpContext.Cache["DriverDatas"] != null)
                {
                    driverViewModels = HttpContext.Cache["DriverDatas"] as List<DriverViewModel>;
                }
                else
                {

                    var result = webServices.Post(new DriverViewModel(), "Driver/All/" + CompanyId);
                    if (result.Data != "[]")
                    {
                        driverViewModels = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["DriverDatas"] = driverViewModels;
                }
                if (parm.sSearch != null)
                {

                    totalCount = driverViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch)).Count();

                    driverViewModels = driverViewModels.ToList()
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
                    totalCount = driverViewModels.Count();

                    driverViewModels = driverViewModels
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

                return Json(
                    new
                    {
                        aaData = driverViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = driverViewModels,
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
                if (results.Data != "[]")
                {
                    CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                }
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
            driverViewModel.LicenseExpiry = System.DateTime.Now.ToShortDateString();

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


                var result = webServices.Post(driverViewModel, "Driver/Add");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    }
                    HttpContext.Cache.Remove("DriverDatas");

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return View(driverViewModel);
            }
            else
            {
                //model state not valid
                var results = webServices.Post(new CountryViewModel(), "Country/All");
                if (results.Data != "[]")
                {
                    CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                }
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

                var result = webServices.Post(driverViewModel, "Driver/Edit");
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
                if (results.Data != "[]")
                {
                    List<CountryViewModel> CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                }
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

                var result = webServices.Post(driverViewModel, "Driver/Edit");
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
                if (results.Data != "[]")
                {
                    List<CountryViewModel> CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                }
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
                driverViewModel.LicenseExpiry = System.DateTime.Now.ToShortDateString();

                var result = webServices.Post(driverViewModel, "Driver/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {                  
                    HttpContext.Cache.Remove("DriverDatas");

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

        [HttpPost]
        public ActionResult DeleteImage(CompanyImages companyImages)
        {
            try
            {
                companyImages.ImageName = companyImages.ImagesUrl.Split('/').Skip(4).FirstOrDefault();

                companyImages.Flage = companyImages.ImageName.Split('-').Skip(0).FirstOrDefault();

                if (CommonFile.DeleteFile(companyImages.ImagesUrl))
                {
                    var result = webServices.Post(companyImages, "Driver/DeleteImage");

                    return Json(result.StatusCode);
                }
                else
                {
                    return Json("Failed to delete, try again later");
                }
            }
            catch (Exception ex)
            {
                return Json("Failed to Delete," +ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Delete(DriverViewModel driverViewModel)
        {
            try
            {
                var result = webServices.Post(driverViewModel, "Driver/Delete");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("DriverDatas");

                    HttpContext.Cache.Remove("DriverDatas");

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


        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public async Task SendAsync(HttpPostedFileBase filePath)
        {
            string url = "http://localhost:64299/api/Driver/Add";

            MultipartFormDataContent content = new MultipartFormDataContent();

            string urls = Path.GetFullPath(filePath.FileName);

            var fileContent = new StreamContent(System.IO.File.OpenRead(urls));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(@"PassportCopy");
            fileContent.Headers.ContentDisposition.FileName = "file.txt";
            fileContent.Headers.ContentDisposition.Name = "file";
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            content.Add(fileContent);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(url, content);
            }
        }

    }
}