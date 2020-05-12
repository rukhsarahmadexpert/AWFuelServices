using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.ProductUnit.Controllers
{

    [Autintication]
    public class ProductUnitController : Controller
    {

        WebServices webServices = new WebServices();

        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        ProductUnitViewModel ProductUnitViewModel = new ProductUnitViewModel();

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Add(ProductUnitViewModel productUnitViewModel)
        {
            try
            {
                var Result = webServices.Post(productUnitViewModel, "Productunit/Add");
                int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));
                if (Res > 0)
                {
                    HttpContext.Cache.Remove("ProductUnitData");
                    return Json(Res, JsonRequestBehavior.AllowGet);
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

                if (HttpContext.Cache["ProductUnitData"] != null)
                {
                    productUnitViewModels = HttpContext.Cache["ProductUnitData"] as List<ProductUnitViewModel>;
                }
                else
                {
                    int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    var result = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All/" + CompanyId);

                    productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(result.Data.ToString());

                    HttpContext.Cache["ProductUnitData"] = productUnitViewModels;
                }

                if (parm.sSearch != null)
                {

                    totalCount = productUnitViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower())
                              ).Count();

                    productUnitViewModels = productUnitViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new ProductUnitViewModel
                   {

                       Name = x.Name,
                       Id = x.Id,
                       UserName = x.UserName,
                       IsActive = x.IsActive

                   }).ToList();

                }
                else
                {
                    totalCount = productUnitViewModels.Count();

                    productUnitViewModels = productUnitViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new ProductUnitViewModel
                         {

                             Name = x.Name,
                             Id = x.Id,
                             UserName = x.UserName,
                             IsActive = x.IsActive

                         }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = productUnitViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = productUnitViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);


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
                var result = webServices.Post(ProductUnitViewModel, "ProductUnit/Edit/" + Id);
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        ProductUnitViewModel = (new JavaScriptSerializer()).Deserialize<ProductUnitViewModel>(result.Data.ToString());
                    }
                }
                return Json(ProductUnitViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            try
            {
                productViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(productViewModel, "ProductUnit/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("ProductUnitData");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                //return Redirect("/Driver-Details/" + driverViewModel.Id);
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
        public JsonResult Delete(int Id)
        {
            try
            {
                var result = webServices.Post(ProductUnitViewModel, "ProductUnit/Delete/" + Id);
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("ProductUnitData");
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
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