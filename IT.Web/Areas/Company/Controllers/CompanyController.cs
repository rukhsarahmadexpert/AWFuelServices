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

namespace IT.Web.Areas.Company.Controllers
{
    public class CompanyController : Controller
    {

        WebServices webServices = new WebServices();
        CompanyViewModel CompanyViewModel = new CompanyViewModel();
       
        List<TypeOwnerReprentative> typeOwnerReprentatives = new List<TypeOwnerReprentative>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CompanyViewModel companyViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    var timestamp = DateTime.Now.TimeOfDay.Ticks;
                    companyViewModel.UID = timestamp.ToString();

                    int count = 0;
                    foreach (var file in files)
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/uploads/Company-" + companyViewModel.UID));
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            if (count == 0)
                            {

                                fileName = "LogoUrl-" + fileName;
                                companyViewModel.LogoUrl = fileName;
                            }
                            else if (count == 1)
                            {

                                fileName = "TradeLicense-" + fileName;
                                companyViewModel.TradeLicense = fileName;
                            }
                            else if (count == 2)
                            {

                                fileName = "VATCertificate-" + fileName;
                                companyViewModel.VATCertificate = fileName;
                            }
                            else if (count == 3)
                            {

                                fileName = "PassportFirstPage-" + fileName;
                                companyViewModel.PassportFirstPage = fileName;
                            }
                            else if (count == 4)
                            {

                                fileName = "PassportLastPage-" + fileName;
                                companyViewModel.PassportLastPage = fileName;
                            }
                            else if (count == 5)
                            {
                                fileName = "IDCardUAE-" + fileName;
                                companyViewModel.IDCardUAE = fileName;
                            }
                            var path = Path.Combine(Server.MapPath("~/Uploads/uploads/Company-" + companyViewModel.UID), fileName);
                            file.SaveAs(path);
                        }
                        count = count + 1;
                    }
                }

                companyViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var result = webServices.Post(companyViewModel, "Company/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    int Result = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                    int Id = Result;

                    return Redirect("/Company-Details/" + Id);
                }
                else
                {
                    return View("Index", companyViewModel);
                }
            }
            else
            {
                return View("Index", companyViewModel);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id, string Flag)
        {
            try
            {
                var result = webServices.Post(new UserViewModel(), "Company/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {

                        CompanyViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                    else
                    {
                        ViewBag.Massage = "Sorry, No data availible";
                    }
                }

                typeOwnerReprentatives.Add(new TypeOwnerReprentative() { Id = "0", OwnerRepresentaive = "Select  Owner/Representative" });
                typeOwnerReprentatives.Add(new TypeOwnerReprentative() { Id = "Owner", OwnerRepresentaive = "Owner" });
                typeOwnerReprentatives.Add(new TypeOwnerReprentative() { Id = "Representative", OwnerRepresentaive = "Representative" });


                ViewBag.OwnerRepresentaive = typeOwnerReprentatives;

                ViewBag.Flag = Flag;
                return View(CompanyViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult CopnayInfoById(int? Id)
        {
            try
            {
                var result = webServices.Post(new UserViewModel(), "Company/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {

                        CompanyViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(result.Data.ToString()).FirstOrDefault();

                        return Json(CompanyViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Massage = "Sorry, No data availible";
                    }
                }

                return View(CompanyViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(CompanyViewModel companyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(companyViewModel, "Company/Update");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (companyViewModel.Flag != null)
                        {
                            return Redirect("/CustomerProfile/" + companyViewModel.Id);
                        }
                        else
                        {
                            return Redirect("/Company-Details/" + companyViewModel.Id);
                        }

                    }

                    return View(companyViewModel);
                }
                else
                {
                    return View(companyViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Details(int? Id)
        {
            try
            {

                List<CompanyImages> mylist = new List<CompanyImages>();

                //int Id = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new UserViewModel(), "Company/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    CompanyViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(result.Data.ToString()).FirstOrDefault();
                }

                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.LogoUrl });
                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.TradeLicense });
                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.VATCertificate });
                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.PassportFirstPage });
                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.PassportLastPage });
                mylist.Add(new CompanyImages { ImagesUrl = CompanyViewModel.IDCardUAE });

                ViewBag.Images = mylist;

                return View(CompanyViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateImage(CompanyImages companyImages)
        {
            if (Request.Files.Count > 0)
            {
                try
                {


                    companyImages.ImageName = companyImages.ImagesUrl.Split('/').Last();
                    companyImages.Flage = companyImages.ImageName.Split('-').Skip(0).FirstOrDefault();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string companyName = companyImages.CompanyName;

                        fname = companyImages.ImageName;

                        if (CommonFile.DeleteFile(companyImages.ImagesUrl))
                        {
                            fname = Path.Combine(Server.MapPath("~/Uploads/Uploads/" + companyName), fname);

                            file.SaveAs(fname);
                            // Get the complete folder path and store the file inside it.  
                            companyImages.ImageName = "";
                        }
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
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
                    var result = webServices.Post(companyImages, "Company/DeleteImage");

                    return Json(result.StatusCode);
                }
                else
                {
                    return Json("Failed to delete, try again later");
                }
            }
            catch (Exception ex)
            {
                return Json("Failed to Delete"+ex.ToString());
            }
        }

        [HttpGet]
        public ActionResult CustomerProfile(int Id)
        {
            try
            {
                try
                {
                    var result = webServices.Post(new UserViewModel(), "Company/Edit/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {

                            CompanyViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(result.Data.ToString()).FirstOrDefault();
                        }
                        else
                        {
                            ViewBag.Massage = "Sorry, No data availible";
                        }
                    }
                    return View(CompanyViewModel);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}