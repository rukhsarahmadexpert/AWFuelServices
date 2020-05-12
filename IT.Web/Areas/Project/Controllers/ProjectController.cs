using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Project.Controllers
{
    public class ProjectController : Controller
    {

        WebServices webServices = new WebServices();
        List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
        ProjectViewModel ProjectViewModel = new ProjectViewModel();
        
        public ActionResult Index()
        {
            return View();
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

                if (HttpContext.Cache["ProjectDatas"] != null)
                {
                    projectViewModels = HttpContext.Cache["ProjectDatas"] as List<ProjectViewModel>;
                }
                else
                {

                    var result = webServices.Post(new ProjectViewModel(), "Project/All");
                    if (result.Data != "[]")
                    {
                        projectViewModels = (new JavaScriptSerializer()).Deserialize<List<ProjectViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["ProjectDatas"] = projectViewModels;
                }
                if (parm.sSearch != null)
                {

                    totalCount = projectViewModels.Where(x => x.ProjectTitle.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch)).Count();

                    projectViewModels = projectViewModels.ToList()
                        .Where(x => x.ProjectTitle.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Contact.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new ProjectViewModel
                   {
                       ProjectTitle = x.ProjectTitle,
                       Id = x.Id,
                       Contact = x.Contact,
                       Email = x.Email,
                       UserName = x.UserName,
                       IsActive = x.IsActive

                   }).ToList();

                }
                else
                {
                    totalCount = projectViewModels.Count();

                    projectViewModels = projectViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                        .Select(x => new ProjectViewModel
                        {
                            ProjectTitle = x.ProjectTitle,
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
                        aaData = projectViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = projectViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
               
        public ActionResult  Create()
        {

            ProjectViewModel.StartDate = System.DateTime.Now;
            ProjectViewModel.EndDate= System.DateTime.Now;


            return View(ProjectViewModel);
        }
        
        [HttpPost]
        public ActionResult Create(ProjectViewModel projectViewModel)
        {

            projectViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

            try
            {
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(projectViewModel, "Project/Add");
                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                        }
                        HttpContext.Cache.Remove("ProjectDatas");

                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(projectViewModel);
                }
                else
                    return View(projectViewModel);
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
                var result = webServices.Post(new ProjectViewModel(), "Project/Edit/"+Id);
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        ProjectViewModel = (new JavaScriptSerializer()).Deserialize<ProjectViewModel>(result.Data.ToString());
                    }
                    HttpContext.Cache.Remove("ProjectDatas");

                    return View(ProjectViewModel);
                }
                else
                    return View(ProjectViewModel);
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }
        
        [HttpPost]
        public ActionResult Edit(ProjectViewModel projectViewModel)
        {
            projectViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

            try
            {
                if (projectViewModel.UpdatedReason == null)
                {
                    ModelState.AddModelError("UpdatedReason", "Please Provide Updated Reason!!!");

                    return View(projectViewModel);
                }
                else
                {

                    if (ModelState.IsValid)
                    {
                        var result = webServices.Post(projectViewModel, "Project/Update");
                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            if (result.Data != "[]")
                            {
                                int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                            }
                            HttpContext.Cache.Remove("ProjectDatas");

                            return RedirectToAction(nameof(Index));
                        }
                        else
                            return View(projectViewModel);
                    }
                    else
                        return View(projectViewModel);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        [HttpGet]
        public ActionResult Details(int Id)
        {
            try
            {
                var result = webServices.Post(new ProjectViewModel(), "Project/Details/" + Id);
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        ProjectViewModel = (new JavaScriptSerializer()).Deserialize<ProjectViewModel>(result.Data.ToString());
                    }
                    HttpContext.Cache.Remove("ProjectDatas");

                    return View(ProjectViewModel);
                }
                else
                    return View(ProjectViewModel);
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }

    }
}