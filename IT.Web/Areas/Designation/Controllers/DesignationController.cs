using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Designation.Controllers
{

    [Autintication]
    public class DesignationController : Controller
    {
        List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        WebServices webServices = new WebServices();

        DesignationViewModel DesignationViewModel = new DesignationViewModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new DesignationViewModel());
        }

        [HttpPost]
        public ActionResult Create(DesignationViewModel designationViewModel)
        {
            try
            {
                var result = webServices.Post(designationViewModel, "Designation/Add");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("DesignationData");

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

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            try
            {
                var result = webServices.Post(new DesignationViewModel(), "Designation/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        DesignationViewModel = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                    return Json(DesignationViewModel, JsonRequestBehavior.AllowGet);
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
                if (HttpContext.Cache["DesignationData"] != null)
                {
                    designationViewModels = HttpContext.Cache["DesignationData"] as List<DesignationViewModel>;
                }
                else
                {

                    var result = webServices.Post(new DesignationViewModel(), "Designation/All");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(result.Data.ToString());

                        HttpContext.Cache["DesignationData"] = designationViewModels;
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
                    totalCount = designationViewModels.Where(x => x.Designation.ToLower().Contains(parm.sSearch.ToLower())).Count();

                    designationViewModels = designationViewModels.ToList()
                        .Where(x => x.Designation.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new DesignationViewModel
                   {
                       Designation = x.Designation,
                       UserName = x.UserName,
                       Id = x.Id
                   }).ToList();
                }
                else
                {
                    totalCount = designationViewModels.Count();

                    designationViewModels = designationViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new DesignationViewModel
                         {
                             Designation = x.Designation,
                             UserName = x.UserName,
                             Id = x.Id
                         }).ToList();

                }
                return Json(
                    new
                    {
                        aaData = designationViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = designationViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var result = webServices.Post("", "Designation/Delete/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        if (Res > 0)
                        {
                            HttpContext.Cache.Remove("DesignationData");

                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                return Json("Failed", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public ActionResult Update(DesignationViewModel designationViewModel)
        {
            try
            {
                designationViewModel.CreatedBy = 1;

                   var result = webServices.Post(designationViewModel, "Designation/Update");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("DesignationData");

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

    }
}