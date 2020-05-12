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

namespace IT.Web.Areas.AWFEmployee.Controllers
{

    [Autintication]
    public class AWFEmployeeController : Controller
    {

        WebServices webServices = new WebServices();
        List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();
        List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();

        // GET: AWFEmployee/AWFEmployee
        public ActionResult Index()
        {

            var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
            if (resultDes.Data != "[]")
            {
                designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());
                if(designationViewModels[0].Designation != "Select Designation")
                {
                    designationViewModels.Insert(0, new DesignationViewModel() {Id= 0, Designation = "Select Designation" });
                }
            }
            ViewBag.DesignationList = designationViewModels;

            var results = webServices.Post(new CountryViewModel(), "Country/All");
            if (results.Data != "[]")
            {
                CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
                if(CountryViewModel[0].CountryName != "Select Country")
                {
                    CountryViewModel.Insert(0, new Core.ViewModels.CountryViewModel() {Id=0, CountryName ="Select Country" });
                }
            }
            ViewBag.CountryList = CountryViewModel;
            
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if(projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            return View(new EmployeeViewModel());
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
            catch (Exception)
            {

                throw;
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

                    var result = webServices.Post(employeeViewModel, "AWFEmployee/Add");
                    int rs = (new JavaScriptSerializer()).Deserialize<int>(result.Data);


                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                        designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());

                        ViewBag.DesignationList = designationViewModels;

                        HttpContext.Cache.Remove("AWFuelEmployeeData");

                        return Json("Success");
                    }
                    else
                    {
                        var resultDes = webServices.Post(new DesignationViewModel(), "Designation/All");
                        designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(resultDes.Data.ToString());

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

        }

        [HttpGet]
        public JsonResult GetAll(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                               
                //if (HttpContext.Cache["AWFuelEmployeeData"] != null)
                //{
                //    employeeViewModels = HttpContext.Cache["AWFuelEmployeeData"] as List<EmployeeViewModel>;
                //}
                //else
                //{
                    var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        HttpContext.Cache["AWFuelEmployeeData"] = employeeViewModels;

                    }
               // }
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

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/Edit/" + Id);
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (results.Data != "[]")
                {
                    employeeViewModel = (new JavaScriptSerializer()).Deserialize<EmployeeViewModel>(results.Data.ToString());
                    return Json(employeeViewModel, JsonRequestBehavior.AllowGet);
                }
            }
           
            return Json("Failed", JsonRequestBehavior.AllowGet);
                        
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel employeeViewModel)
        {
            var results = webServices.Post(employeeViewModel, "AWFEmployee/Update");
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                HttpContext.Cache.Remove("AWFuelEmployeeData");
                return Json("Success", JsonRequestBehavior.AllowGet);
            }

            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int Id)
        {
            var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/Edit/" + Id);
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (results.Data != "[]")
                {
                    employeeViewModel = (new JavaScriptSerializer()).Deserialize<EmployeeViewModel>(results.Data.ToString());
                }
            }

            return View(employeeViewModel);
        }

        [HttpPost]
        public ActionResult Delete(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var results = webServices.Post(employeeViewModel, "AWFEmployee/Delete");
                if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("AWFuelEmployeeData");
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
    }
}