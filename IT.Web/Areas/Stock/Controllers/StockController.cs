using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Stock.Controllers
{
    [Autintication]
    public class StockController : Controller
    {
        WebServices webServices = new WebServices();
        List<SiteViewModel> siteViewModel = new List<SiteViewModel>();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        StockViewModel StockViewModel = new StockViewModel();
        List<StockViewModel> stockViewModels = new List<StockViewModel>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
        List<ProductViewModel> productViewModels = new List<ProductViewModel>();


        public ActionResult Index()
        {
            try
            {

                var result = webServices.Post(new SiteViewModel(), "Site/All");
                if (result.Data != "[]")
                {
                    siteViewModel = ((new JavaScriptSerializer()).Deserialize<List<SiteViewModel>>(result.Data.ToString()));
                    if (siteViewModel[0].SiteName != "Select Site")
                    {
                        siteViewModel.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });
                    }
                }                

                var results = webServices.Post(new LPOInvoiceViewModel(), "LPO/LPOUnconvertedALL");

                if (results.Data != "[]")
                {
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(results.Data.ToString());
                    if (lPOInvoiceViewModels[0].PONumber != "Select LPO")
                    {
                        lPOInvoiceViewModels.Insert(0, new LPOInvoiceViewModel() { Id = 0, PONumber = "Select LPO" });
                    }
                }


                var resultProduct = webServices.Post(new ProductViewModel(), "Product/All");
                if (resultProduct.Data != "[]")
                {
                    productViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(resultProduct.Data.ToString());
                    if (productViewModels[0].Name != "Select Item")
                    {
                        productViewModels.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                    }
                }
                ViewBag.Product = productViewModels;
                ViewBag.LPO = lPOInvoiceViewModels;
                ViewBag.Site = siteViewModel;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Create(StockViewModel stockViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    stockViewModel.CreateBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(stockViewModel, "Stock/Add");

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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public JsonResult Edit(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new StockViewModel(), "Stock/Edit/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            StockViewModel = (new JavaScriptSerializer()).Deserialize<StockViewModel>(result.Data.ToString());
                        }
                        return Json(StockViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(StockViewModel stockViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    stockViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(stockViewModel, "Stock/Update");

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

                var result = webServices.Post(new DriverViewModel(), "Stock/All");

                stockViewModels = (new JavaScriptSerializer()).Deserialize<List<StockViewModel>>(result.Data.ToString());


                if (parm.sSearch != null)
                {

                    totalCount = stockViewModels.Where(x => x.VenderName.ToLower().Contains(parm.sSearch.ToLower())
                                 || x.BillNo.ToString().Contains(parm.sSearch)
                                 || x.Total.ToString().Contains(parm.sSearch)
                    ).Count();

                    stockViewModels = stockViewModels.ToList()
                        .Where(x =>
                               x.VenderName.ToLower().Contains(parm.sSearch.ToLower())
                               || x.BillNo.ToString().Contains(parm.sSearch)
                               || x.Total.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new StockViewModel
                   {

                       BillNo = x.BillNo,
                       Id = x.Id,
                       RefrenceNo = x.RefrenceNo,
                       VenderName = x.VenderName,
                       UserName = x.UserName,
                       Quantity = x.Quantity,
                       Rate = x.Rate,
                       VAT = x.VAT,
                       Total = x.Total,
                       CreatedDates = x.CreatedDates

                   }).ToList();

                }
                else
                {
                    totalCount = stockViewModels.Count();

                    stockViewModels = stockViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                        .Select(x => new StockViewModel
                        {

                            BillNo = x.BillNo,
                            Id = x.Id,
                            RefrenceNo = x.RefrenceNo,
                            VenderName = x.VenderName,
                            UserName = x.UserName,
                            Quantity = x.Quantity,
                            Rate = x.Rate,
                            VAT = x.VAT,
                            Total = x.Total,
                            CreatedDates = x.CreatedDates

                        }).ToList();
                }

                return Json(
                    new
                    {
                        aaData = stockViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = stockViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

                //return Json(driverViewModels.ToList(), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult GetAvailibleQuantity(StockViewModel stockViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(stockViewModel, "Stock/GetAvailibleQuantity");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            StockViewModel = (new JavaScriptSerializer()).Deserialize<StockViewModel>(result.Data.ToString());
                        }
                        return Json(StockViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
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