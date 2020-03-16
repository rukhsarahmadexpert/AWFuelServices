using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.AWFEmployeeSalary.Controllers
{
    public class AWFEmployeeSalaryController : Controller
    {
        WebServices webServices = new WebServices();
        
        List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
     

        public ActionResult Index()
        {
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if (projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            return View();
        }


        [HttpGet]
        public JsonResult AllEmployeeByProjectId(int Id)
        {
            try
            {
                var ProjectResults = webServices.Post(new EmployeeViewModel(), "AWFuelSalary/AllEmployeeByProjectId/"+Id);
                if (ProjectResults.Data != "[]")
                {
                    employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(ProjectResults.Data.ToString());

                    return Json(employeeViewModels, JsonRequestBehavior.AllowGet);
                }

                return Json("Failed",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult GeneratSalary(AccountViewModel accountViewModel)
        {
            try
            {

                accountViewModel.CreatedBy = Convert.ToInt32(Session["Userid"]);

                accountViewModel.CreatedDate = System.DateTime.Now;

                var ProjectResults = webServices.Post(accountViewModel, "AWFuelSalary/Add/");

                if (ProjectResults.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public ActionResult IsSalaryGenerated(AccountViewModel accountViewModel)
        {
            try
            {
                var Res = webServices.Post(accountViewModel, "AWFuelSalary/IsSalaryGenerated");
                if (Res.Data != "[]")
                {
                    int Result = (new JavaScriptSerializer()).Deserialize<int>(Res.Data);

                    return Json(Result, JsonRequestBehavior.AllowGet);
                }

                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }      


        public ActionResult EmployeeLoadIssued()
        {
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if (projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            AWFuelSalaryViewModel aWFuelSalaryViewModel = new AWFuelSalaryViewModel();
            aWFuelSalaryViewModel.IssuedDate = System.DateTime.Now;

            return View(aWFuelSalaryViewModel);
        }

        [HttpPost]
        public ActionResult EmployeeLoadIssued(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);


                var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
                if (ProjectResults.Data != "[]")
                {
                    projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                    if (projectViewModels[0].ProjectTitle != "Select Project")
                    {
                        projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                    }
                }
                ViewBag.projectViewModels = projectViewModels;



                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/AddEmployeeLoad");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes",JsonRequestBehavior.AllowGet);
                }               
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult EmployeeLoanReturn(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);


                var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
                if (ProjectResults.Data != "[]")
                {
                    projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                    if (projectViewModels[0].ProjectTitle != "Select Project")
                    {
                        projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                    }
                }
                ViewBag.projectViewModels = projectViewModels;
                

                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/EmployeeLoanReturn");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

                     
        [HttpPost]
        public ActionResult EmployeeStatistics(int Id)
        {
            try
            {
                AWFuelSalaryViewModel aWFuelSalaryViewModel = new AWFuelSalaryViewModel();


                var ProjectResults = webServices.Post(new AWFuelSalaryViewModel(), "AWFuelSalary/EmployeeStatistics/"+Id);
                if (ProjectResults.Data != "[]")
                {
                    aWFuelSalaryViewModel = (new JavaScriptSerializer()).Deserialize<AWFuelSalaryViewModel>(ProjectResults.Data.ToString());

                    return Json(aWFuelSalaryViewModel, JsonRequestBehavior.AllowGet);
                }

                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }


       [HttpGet]
       public ActionResult EmployeeDediction()
       {
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if (projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            AWFuelSalaryViewModel aWFuelSalaryViewModel = new AWFuelSalaryViewModel();
            aWFuelSalaryViewModel.IssuedDate = System.DateTime.Now;

            return View(aWFuelSalaryViewModel);
        }


       [HttpPost]
       public ActionResult SaveDeduction(AWFuelSalaryViewModel aWFuelSalaryViewModel)
       {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);
                
                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/SaveDeduction");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       public ActionResult EmployeeAllowansis()
        {
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if (projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            AWFuelSalaryViewModel aWFuelSalaryViewModel = new AWFuelSalaryViewModel();
            aWFuelSalaryViewModel.IssuedDate = System.DateTime.Now;

            return View(aWFuelSalaryViewModel);
        }

       [HttpPost]
       public ActionResult EmployeeAllowanceSaved(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);

                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/EmployeeAllowanceSaved");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       [HttpGet]
       public ActionResult IssueEmployeeSalary()
       {
            var ProjectResults = webServices.Post(new CountryViewModel(), "Project/All");
            if (ProjectResults.Data != "[]")
            {
                projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(ProjectResults.Data.ToString());

                if (projectViewModels[0].ProjectTitle != "Select Project")
                {
                    projectViewModels.Insert(0, new ProjectViewModel() { Id = 0, ProjectTitle = "Select Project" });
                }
            }
            ViewBag.projectViewModels = projectViewModels;

            AWFuelSalaryViewModel aWFuelSalaryViewModel = new AWFuelSalaryViewModel();
            aWFuelSalaryViewModel.IssuedDate = System.DateTime.Now;

            return View(aWFuelSalaryViewModel);
        }


        [HttpPost]
        public ActionResult IssueEmployeeSalary(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);

                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/IssueEmployeeSalary");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost] 
        public ActionResult AllowanceTypeAdd(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                aWFuelSalaryViewModel.IssuedBy = Convert.ToInt32(Session["Userid"]);

                var ProjectResult = webServices.Post(aWFuelSalaryViewModel, "AWFuelSalary/AllowanceTypeAdd");

                if (ProjectResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Succes", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}