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

namespace IT.Web.Areas.Employee.Controllers
{

    [Autintication]
    public class EmployeeController : Controller
    {
        WebServices webServices = new WebServices();
        List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();

        public ActionResult Index()
        {

            if (HttpContext.Cache["DesignationData"] != null)
            {
                designationViewModels = HttpContext.Cache["DesignationData"] as List<DesignationViewModel>;
            }
            else
            {
                var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                if (resultDes.Data != "[]")
                {
                    designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());
                }
                HttpContext.Cache["DesignationData"] = designationViewModels;
            }

            ViewBag.DesignationList = designationViewModels;


            if (HttpContext.Cache["CountryData"] != null)
            {
                CountryViewModel = HttpContext.Cache["CountryData"] as List<CountryViewModel>;
            }
            else
            {
                var results = webServices.Post(new CountryViewModel(), "Country/All");
                if (results.Data != "[]")
                {
                    CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                }
                HttpContext.Cache["CountryData"] = CountryViewModel;
            }

            ViewBag.CountryList = CountryViewModel;

            return View(employeeViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {

            try
            {
                var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());

                ViewBag.DesignationList = designationViewModels;

                var results = webServices.Post(new CountryViewModel(), "Country/All");
                CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

                ViewBag.CountryList = CountryViewModel;

                return View(employeeViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeeViewModel, IEnumerable<HttpPostedFileBase> files)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    employeeViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    employeeViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    if (files != null)
                    {
                        var timestamp = DateTime.Now.TimeOfDay.Ticks;
                        employeeViewModel.UID = timestamp.ToString();

                        int count = 0;
                        foreach (var file in files)
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Uploads/uploads/Employee-" + timestamp));
                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);

                                if (count == 0)
                                {

                                    fileName = "PassportFront-" + fileName;
                                    employeeViewModel.PassportFront = fileName;
                                }
                                else if (count == 1)
                                {

                                    fileName = "PassportBack-" + fileName;
                                    employeeViewModel.PassportBack = fileName;
                                }
                                var path = Path.Combine(Server.MapPath("~/Uploads/uploads/Employee-" + timestamp), fileName);
                                file.SaveAs(path);
                            }
                            count = count + 1;
                        }
                    }

                    var result = webServices.Post(employeeViewModel, "Employee/Add");
                    if(result.Data != "[]")
                    {
                        int rs = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                        HttpContext.Cache.Remove("DesignationData");
                        HttpContext.Cache.Remove("EmployeeDatas");
                    }
                  

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                        if (resultDes.Data != "[]")
                        {
                            designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());
                        }
                        ViewBag.DesignationList = designationViewModels;

                        return Json("Success");
                    }
                    else
                    {
                        var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                        if (resultDes.Data != "[]")
                        {
                            designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());
                        }
                        ViewBag.DesignationList = designationViewModels;
                        return Json("Failed");
                    }

                }
                else
                {
                    return Json("Failed");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            //var results = webServices.Post(new CountryViewModel(), "Country/All");
            //CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());

            //ViewBag.CountryList = CountryViewModel;

            //return View(employeeViewModel);
        }

        [HttpGet]
        public JsonResult GetAll(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["EmployeeDatas"] != null)
                {

                    employeeViewModels = HttpContext.Cache["EmployeeDatas"] as List<EmployeeViewModel>;
                }

                else
                {
                    var results = webServices.Post(new EmployeeViewModel(), "Employee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        HttpContext.Cache["EmployeeDatas"] = employeeViewModels;
                    }
                }
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }
                if (parm.sSearch != null)
                {
                    totalCount = employeeViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch)).Count();

                    employeeViewModels = employeeViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new EmployeeViewModel
                   {
                       Name = x.Name,
                       Id = x.Id,
                       Contact = x.Contact,
                       Email = x.Email,
                       UserName = x.UserName,
                       IsActive = x.IsActive,
                       BasicSalary = x.BasicSalary,
                       DesignationName = x.DesignationName

                   }).ToList();
                }
                else
                {
                    totalCount = employeeViewModels.Count();

                    employeeViewModels = employeeViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new EmployeeViewModel
                         {

                             Name = x.Name,
                             Id = x.Id,
                             Contact = x.Contact,
                             Email = x.Email,
                             UserName = x.UserName,
                             IsActive = x.IsActive,
                             BasicSalary = x.BasicSalary,
                             DesignationName = x.DesignationName

                         }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = employeeViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = employeeViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var results = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + Id);
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (results.Data != "[]")
                {
                    employeeViewModel = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString()).FirstOrDefault();
                }                
            }

            return Json(employeeViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel employeeViewModel)
        {
            var results = webServices.Post(employeeViewModel, "Employee/Update");
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {

                HttpContext.Cache.Remove("EmployeeDatas");
                return Json("Success", JsonRequestBehavior.AllowGet);
            }

            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int Id)
        {
            var results = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + Id);
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {

                employeeViewModel = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString()).FirstOrDefault();
            }

            return View(employeeViewModel);
        }

        [HttpPost]
        public ActionResult Delete(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var results = webServices.Post(employeeViewModel, "Employee/Delete");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("EmployeeDatas");
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

        [HttpPost]
        public ActionResult LoadEmployeeAll()
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["EmployeeDatas"] != null)
                {
                    employeeViewModels = HttpContext.Cache["EmployeeDatas"] as List<EmployeeViewModel>;
                }
                else
                {
                    var results = webServices.Post(new EmployeeViewModel(), "Employee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        HttpContext.Cache["EmployeeDatas"] = employeeViewModels;
                    }
                }
                return Json(employeeViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}