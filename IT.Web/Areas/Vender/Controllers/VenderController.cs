using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Vender.Controllers
{

    [Autintication]
    public class VenderController : Controller
    {
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        VenderViewModel VenderViewModel = new VenderViewModel();
        WebServices webServices = new WebServices();

        // GET: Vender/Vender
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(VenderViewModel venderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    venderViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(venderViewModel, "Vender/Add");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Res = (new JavaScriptSerializer().Deserialize<int>(result.Data));
                            HttpContext.Cache.Remove("VenderDatas");
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
                if (HttpContext.Cache["VenderDatas"] != null)
                {
                    venderViewModels = HttpContext.Cache["VenderDatas"] as List<VenderViewModel>;
                }
                else
                {
                    var result = webServices.Post(new DriverViewModel(), "Vender/All");

                    venderViewModels = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(result.Data.ToString());

                    HttpContext.Cache["VenderDatas"] = venderViewModels;
                }
                if (parm.sSearch != null)
                {

                    totalCount = venderViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.LandLine.Contains(parm.sSearch) ||
                                x.TRN.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch)).Count();

                    venderViewModels = venderViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.LandLine.Contains(parm.sSearch) ||
                                x.TRN.Contains(parm.sSearch) ||
                               x.Email.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new VenderViewModel
                   {

                       Name = x.Name,
                       Id = x.Id,
                       Mobile = x.Mobile,
                       Email = x.Email,
                       UserName = x.UserName,
                       IsActive = x.IsActive,
                       LandLine = x.LandLine,
                       Representative = x.Representative,
                       TRN = x.TRN

                   }).ToList();

                }
                else
                {
                    totalCount = venderViewModels.Count();

                    venderViewModels = venderViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new VenderViewModel
                         {

                             Name = x.Name,
                             Id = x.Id,
                             Mobile = x.Mobile,
                             Email = x.Email,
                             UserName = x.UserName,
                             IsActive = x.IsActive,
                             LandLine = x.LandLine,
                             Representative = x.Representative,
                             TRN = x.TRN

                         }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = venderViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = venderViewModels,
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
        public JsonResult Edit(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            VenderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                        }
                        return Json(VenderViewModel, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(VenderViewModel venderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    venderViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(venderViewModel, "Vender/Update");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Res = (new JavaScriptSerializer().Deserialize<int>(result.Data));
                            HttpContext.Cache.Remove("VenderDatas");

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
        
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new VenderViewModel(), "Vender/Delete/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            VenderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());

                            HttpContext.Cache.Remove("VenderDatas");
                        }
                        return Json(VenderViewModel, JsonRequestBehavior.AllowGet);
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

    }
}