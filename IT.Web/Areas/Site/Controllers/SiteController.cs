using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Site.Controllers
{

    [Autintication]
    public class SiteController : Controller
    {
        List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
        SiteViewModel siteViewModel = new SiteViewModel();

        WebServices webServices = new WebServices();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SiteViewModel siteViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    siteViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(siteViewModel, "Site/Add");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Res = (new JavaScriptSerializer().Deserialize<int>(result.Data));
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
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


                var result = webServices.Post(new CustomerOrderViewModel(), "Site/All");
                siteViewModels = (new JavaScriptSerializer()).Deserialize<List<SiteViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = siteViewModels.Where(x => x.SiteName.Contains(parm.sSearch.ToLower()) ||
                               x.ContactPersonName.Contains(parm.sSearch) ||
                               x.ContactPhone.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.SiteCell.ToString().Contains(parm.sSearch)).Count();

                    siteViewModels = siteViewModels.ToList()
                        .Where(x => x.SiteName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.ContactPersonName.Contains(parm.sSearch) ||
                               x.ContactPhone.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.SiteCell.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new SiteViewModel
                   {
                       Id = x.Id,
                       SiteName = x.SiteName,
                       ContactPersonName = x.ContactPersonName,
                       ContactPhone = x.ContactPhone,
                       SiteCell = x.SiteCell,
                       SiteEmail = x.SiteEmail,
                       UserName = x.UserName,
                       CreatedDates = x.CreatedDates

                   }).ToList();
                }
                else
                {
                    totalCount = siteViewModels.Count();

                    siteViewModels = siteViewModels.Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new SiteViewModel
                          {
                              Id = x.Id,
                              SiteName = x.SiteName,
                              ContactPersonName = x.ContactPersonName,
                              ContactPhone = x.ContactPhone,
                              SiteCell = x.SiteCell,
                              SiteEmail = x.SiteEmail,
                              UserName = x.UserName,
                              CreatedDates = x.CreatedDates


                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = siteViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = siteViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public JsonResult Edit(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new SiteViewModel(), "Site/Edit/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            siteViewModel = (new JavaScriptSerializer()).Deserialize<SiteViewModel>(result.Data.ToString());
                        }
                        return Json(siteViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(SiteViewModel siteViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    siteViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(siteViewModel, "Site/Update");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Res = (new JavaScriptSerializer().Deserialize<int>(result.Data));
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public ActionResult Details(int Id)
        {

            try
            {
                var result = webServices.Post(new SiteViewModel(), "Site/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        siteViewModel = (new JavaScriptSerializer()).Deserialize<SiteViewModel>(result.Data.ToString());
                    }
                    return View(siteViewModel);
                }
                else
                {
                    return View(siteViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}