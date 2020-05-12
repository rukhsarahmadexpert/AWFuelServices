using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Product.Controllers
{

    [Autintication]
    public class ProductController : Controller
    {

        WebServices webServices = new WebServices();

        List<ProductViewModel> productViewModels = new List<ProductViewModel>();
        ProductViewModel ProductViewModel = new ProductViewModel();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();

        // GET: Product/Product
        public ActionResult Index()
        {

            if (HttpContext.Cache["ProductUnitData"] != null)
            {
                productUnitViewModels = HttpContext.Cache["ProductUnitData"] as List<ProductUnitViewModel>;
            }
            else
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All/" + CompanyId);

                productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(result.Data.ToString());
            }

            ViewBag.Unit = productUnitViewModels;
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

                if (HttpContext.Cache["ProductData"] != null)
                {
                    productViewModels = HttpContext.Cache["ProductData"] as List<ProductViewModel>;
                }
                else
                {
                    var result = webServices.Post(new ProductViewModel(), "Product/All/");

                    productViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());

                    HttpContext.Cache["ProductData"] = productViewModels;
                }
                if (parm.sSearch != null)
                {

                    totalCount = productViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower())
                              ).Count();

                    productViewModels = productViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new ProductViewModel
                   {

                       Name = x.Name,
                       Id = x.Id,
                       Description = x.Description,
                       UnitName = x.UnitName,
                       UserName = x.UserName,
                       IsActive = x.IsActive

                   }).ToList();

                }
                else
                {
                    totalCount = productViewModels.Count();

                    productViewModels = productViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                         .Select(x => new ProductViewModel
                         {

                             Name = x.Name,
                             Id = x.Id,
                             Description = x.Description,
                             UnitName = x.UnitName,
                             UserName = x.UserName,
                             IsActive = x.IsActive

                         }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = productViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = productViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }
        
        [HttpPost]
        public ActionResult Add(ProductViewModel productViewModel)
        {
            try
            {
                var Result = webServices.Post(productViewModel, "Product/Add");

                if (Result.Data != "[]")
                {
                    int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));

                    HttpContext.Cache.Remove("ProductData");

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
        
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                var Result = webServices.Post(new ProductViewModel(), "Product/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    ProductViewModel = (new JavaScriptSerializer().Deserialize<ProductViewModel>(Result.Data.ToString()));
                    return Json(ProductViewModel, JsonRequestBehavior.AllowGet);
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
        
        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            try
            {
                productViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                var Result = webServices.Post(productViewModel, "Product/Update");
                if (Result.Data != "[]")
                {
                    int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));

                    HttpContext.Cache.Remove("ProductData");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json("Failed", JsonRequestBehavior.AllowGet);

            }
        }
        
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var Result = webServices.Post("", "Product/Delete/" + Id);

                if (Result.Data != "[]")
                {
                    int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));

                    HttpContext.Cache.Remove("ProductData");
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
        
        [HttpPost]
        public ActionResult GetProdctFromLPODetailsByLPOID(int Id)
        {
            try
            {
                var Result = webServices.Post(new ProductViewModel(), "Product/GetProdctFromLPODetailsByLPOID/" + Id);

                if (Result.Data != "[]")
                {
                    productViewModels = (new JavaScriptSerializer().Deserialize<List<ProductViewModel>>(Result.Data.ToString()));
                    
                    if(productViewModels.Count > 0)
                    {
                        if (productViewModels[0].Name != "Salect Item")
                        {
                            productViewModels.Insert(0, new ProductViewModel() {Id=0, Name= "Salect Item"});
                        }
                    }

                    return Json(productViewModels, JsonRequestBehavior.AllowGet);
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
    }
}