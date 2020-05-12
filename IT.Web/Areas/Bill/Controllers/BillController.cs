using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Bill.Controllers
{

    [Autintication]
    public class BillController : Controller
    {
        WebServices webServices = new WebServices();
        readonly List<ProductViewModel> ProductViewModel = new List<ProductViewModel>();
        readonly List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        readonly List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        LPOInvoiceViewModel lPOInvoiceViewModel = new LPOInvoiceViewModel();
        List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();


        // GET: Bill/Bill
        public ActionResult Index()
        {
            try
            {
                var result = webServices.Post(new VehicleViewModel(), "LPO/LPOUnconvertedALL");
                lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                lPOInvoiceViewModels.Insert(0, new LPOInvoiceViewModel() { Id = 0, PONumber = "select LPO Number" });

                ViewBag.LPO = lPOInvoiceViewModels;

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(int Id)
        {

            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "LPO/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    lPOInvoiceViewModel.Heading = "BILL";

                    var Results = webServices.Post(new LPOInvoiceDetails(), "LPO/EditDetails/" + Id);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        HttpContext.Cache.Remove("LPOData");

                        if (TempData["Success"] == null)
                        {
                            if (TempData["Download"] != null)
                            {
                                ViewBag.IsDownload = TempData["Download"].ToString();
                                ViewBag.Id = Id;
                            }
                        }
                        else
                        {
                            ViewBag.Success = TempData["Success"];
                        }

                        lPOInvoiceViewModel.RefrenceNumber = lPOInvoiceViewModel.PONumber;
                        BillPONumber billPONumber = new BillPONumber();

                        lPOInvoiceViewModel.PONumber = billPONumber.PnNumber();

                        LPOInvoiceViewModel lPOInvoiceVModel = new LPOInvoiceViewModel {

                            FromDate = System.DateTime.Now,
                            DueDate = System.DateTime.Now
                        };
                        return View(lPOInvoiceVModel);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public ActionResult Create(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                lPOInvoiceViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var result = webServices.Post(lPOInvoiceViewModel, "BILL/Add");
                int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                if (Res > 0)
                {
                    HttpContext.Cache.Remove("AWFuelBillData");
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

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["AWFuelBillData"] != null)
                {

                    lPOInvoiceViewModels = HttpContext.Cache["AWFuelBillData"] as List<LPOInvoiceViewModel>;
                }

                else
                {

                    var result = webServices.Post(new LPOInvoiceViewModel(), "Bill/All");
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                    HttpContext.Cache["AWFuelBillData"] = lPOInvoiceViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = lPOInvoiceViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.PONumber.ToString().Contains(parm.sSearch)).Count();

                    lPOInvoiceViewModels = lPOInvoiceViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.PONumber.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new LPOInvoiceViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Total = x.Total,
                       VAT = x.VAT,
                       GrandTotal = x.GrandTotal,
                       UserName = x.UserName,
                       PONumber = x.PONumber,
                       FDate = x.FDate,
                       DDate = x.DDate,
                       IsUpdated = x.IsUpdated,
                       RefrenceNumber = x.RefrenceNumber
                   }).ToList();
                }
                else
                {
                    totalCount = lPOInvoiceViewModels.Count();

                    lPOInvoiceViewModels = lPOInvoiceViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                        .Select(x => new LPOInvoiceViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Total = x.Total,
                            VAT = x.VAT,
                            GrandTotal = x.GrandTotal,
                            UserName = x.UserName,
                            PONumber = x.PONumber,
                            FDate = x.FDate,
                            DDate = x.DDate,
                            IsUpdated = x.IsUpdated,
                            RefrenceNumber = x.RefrenceNumber

                        }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = lPOInvoiceViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = lPOInvoiceViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult Details(int Id)
        {

            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Bill/Details/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    lPOInvoiceViewModel.Heading = "BILL";

                    var Results = webServices.Post(new LPOInvoiceDetails(), "LPO/EditDetails/" + lPOInvoiceViewModel.LPOId);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        if (TempData["Success"] == null)
                        {
                            if (TempData["Download"] != null)
                            {
                                ViewBag.IsDownload = TempData["Download"].ToString();
                                ViewBag.Id = Id;
                            }
                        }
                        else
                        {
                            ViewBag.Success = TempData["Success"];
                        }
                        ViewBag.RefrenceNumber = lPOInvoiceViewModel.RefrenceNumber;

                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

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
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Bill/Delete/" + Id);

                int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));
                if (Res > 0)
                {
                    HttpContext.Cache.Remove("AWFuelBillData");
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