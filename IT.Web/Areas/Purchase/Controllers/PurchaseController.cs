using CrystalDecisions.CrystalReports.Engine;
using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using IT.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Purchase.Controllers
{

    [Autintication]
    public class PurchaseController : Controller
    {
        WebServices webServices = new WebServices();
        List<ProductViewModel> ProductViewModel = new List<ProductViewModel>();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        LPOInvoiceViewModel lPOInvoiceViewModel = new LPOInvoiceViewModel();
        List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
               
        public ActionResult Index()
        {
            try
            {

                var results = webServices.Post(new LPOInvoiceViewModel(), "LPO/LPOUnconvertedALL");
                lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(results.Data.ToString());
                lPOInvoiceViewModels.Insert(0, new LPOInvoiceViewModel() { Id = 0, PONumber = "Select LPO" });

                ViewBag.LPO = lPOInvoiceViewModels;

                return View();
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

                //int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["PurchaseData"] != null)
                {
                    lPOInvoiceViewModels = HttpContext.Cache["PurchaseData"] as List<LPOInvoiceViewModel>;
                }
                else
                {
                    var result = webServices.Post(new VehicleViewModel(), "Purchase/All");
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                    HttpContext.Cache["PurchaseData"] = lPOInvoiceViewModels;
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
                       DDate = x.DDate
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
                            DDate = x.DDate


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

        public ActionResult Create()
        {
            try
            {
                PurchaseNumber purchaseNumber = new PurchaseNumber();
                string SerailNO = purchaseNumber.PurchaseNumbers();

                var result = webServices.Post(new ProductViewModel(), "Product/All");
                ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                ViewBag.Product = ProductViewModel;

                var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                ViewBag.ProductUnit = productUnitViewModels;

                var Res = webServices.Post(new DriverViewModel(), "Vender/All");
                venderViewModels = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(Res.Data.ToString());
                venderViewModels.Insert(0, new VenderViewModel() { Id = 0, Name = "Select Vender" });

                ViewBag.Vender = venderViewModels;

                ViewBag.titles = "Purchase";

                ViewBag.PO = SerailNO;

                LPOInvoiceViewModel lPOInvoiceVModel = new LPOInvoiceViewModel
                {
                    FromDate = System.DateTime.Now,
                    DueDate = System.DateTime.Now
                };
                return View(lPOInvoiceVModel);

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
                lPOInvoiceViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                var result = webServices.Post(lPOInvoiceViewModel, "Purchase/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("LPOData");
                        TempData["Id"] = Res;

                        return Json(Res, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Purchase/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    lPOInvoiceViewModel.FromDate = lPOInvoiceViewModel.FromDate.AddDays(1);
                    lPOInvoiceViewModel.DueDate = lPOInvoiceViewModel.DueDate.AddDays(1);


                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;
                    lPOInvoiceViewModel.Heading = "Purchase";
                    var Results = webServices.Post(new LPOInvoiceDetails(), "Purchase/EditDetails/" + Id);

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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Purchase/Edit/" + Id);


                var result = webServices.Post(new ProductViewModel(), "Product/All");
                ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                ViewBag.Product = ProductViewModel;

                var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                ViewBag.ProductUnit = productUnitViewModels;


                List<VatModel> model = new List<VatModel> {
                    new VatModel() { Id = 0, VAT = 0 },
                    new VatModel() { Id = 5, VAT = 5 }
                };
                ViewBag.VatDrop = model;

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));

                    lPOInvoiceViewModel.FromDate = lPOInvoiceViewModel.FromDate.AddDays(1);
                    lPOInvoiceViewModel.DueDate = lPOInvoiceViewModel.DueDate.AddDays(1);

                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    var Results = webServices.Post(new LPOInvoiceDetails(), "Purchase/EditDetails/" + Id);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        lPOInvoiceViewModel.Heading = "Purchase";
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

        public ActionResult CreateFromLPO(int Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "LPO/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    var result = webServices.Post(new ProductViewModel(), "Product/All");
                    ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                    ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                    ViewBag.Product = ProductViewModel;

                    var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                    productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                    productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                    ViewBag.ProductUnit = productUnitViewModels;

                    List<VatModel> model = new List<VatModel> {
                        new VatModel() { Id = 0, VAT = 0 },
                        new VatModel() { Id = 5, VAT = 5 }
                    };
                    ViewBag.VatDrop = model;

                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    lPOInvoiceViewModel.Heading = "Purchase";

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
                        PurchaseNumber purchaseNumber = new PurchaseNumber();

                        lPOInvoiceViewModel.PONumber = purchaseNumber.PurchaseNumbers();

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
        public ActionResult CreateFromLPO(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                lPOInvoiceViewModel.FDate = System.DateTime.Now.ToShortDateString();
                lPOInvoiceViewModel.DDate = System.DateTime.Now.ToShortDateString();


                var result = webServices.Post(lPOInvoiceViewModel, "Purchase/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("LPOData");
                        TempData["Id"] = Res;

                        return Json(Res, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult DeleteLPurchaseDetailsRow(int Id, int detailId, int VAT, decimal RowTotal)
        {
            try
            {
                VAT = 5;
                decimal ResultVAT = CalculateVat(VAT, RowTotal);

                lPOInvoiceViewModel.lPOInvoiceDetailsList = new List<LPOInvoiceDetails>();

                var LPOData = webServices.Post(new LPOInvoiceViewModel(), "Purchase/Edit/" + Id);
                lPOInvoiceViewModel = (new JavaScriptSerializer()).Deserialize<LPOInvoiceViewModel>(LPOData.Data.ToString());

                lPOInvoiceViewModel.Total = lPOInvoiceViewModel.Total - RowTotal;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - ResultVAT;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - RowTotal;
                lPOInvoiceViewModel.VAT = lPOInvoiceViewModel.VAT - ResultVAT;
                lPOInvoiceViewModel.detailId = detailId;

                var result = webServices.Post(lPOInvoiceViewModel, "Purchase/DeleteDetailsRow");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    return Json("Success", JsonRequestBehavior.AllowGet);


                    //var deletequtation = webServices.Post(lPOInvoiceViewModel, "LPO/DeleteDeatlsRow");
                    //if (deletequtation.IsSuccess == true)
                    //{
                    //    return Json("Success", JsonRequestBehavior.AllowGet);
                    //}
                    //return new JsonResult { Data = new { Status = "Success" } };

                }
                return new JsonResult { Data = new { Status = "Fail" } };
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Status = "Fail" } };
            }
        }

        public static decimal CalculateVat(decimal vat, decimal Total)
        {
            decimal Result = 0;
            try
            {
                Result = Convert.ToDecimal((Total / 100) * vat);
                return Result;
            }
            catch (Exception)
            {
                return Result;
            }
        }

        [HttpPost]
        public ActionResult Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {

                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                lPOInvoiceViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(lPOInvoiceViewModel, "Purchase/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("LPOData");

                        if (lPOInvoiceViewModel.IsDownload != null)
                        {
                            TempData["Download"] = "True";
                        }

                        TempData["Id"] = Res;

                        int Download = UploadFileToFolder(Res);

                        return Json(Res, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public int UploadFileToFolder(int Id)
        {
            string pdfname = "";

            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/LPOReport.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<Models.LPOInvoiceModel>();
                List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
                List<VenderModel> venderModels = new List<VenderModel>();

                var LPOInvoice = webServices.Post(new IT.Core.ViewModels.LPOInvoiceModel(), "Purchase/EditReport/" + Id);
                if (LPOInvoice.Data != "[]")
                {
                    lPOInvoiceModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.LPOInvoiceModel>>(LPOInvoice.Data.ToString());
                }
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                var LPOInvoiceDetails = webServices.Post(new LPOInvoiceDetails(), "Purchase/EditDetails/" + Id);
                if (LPOInvoiceDetails.Data != "[]")
                {
                    lPOInvoiceDetails = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceDetails>>(LPOInvoiceDetails.Data.ToString());
                }
                VenderViewModel venderViewModel = new VenderViewModel();

                if (lPOInvoiceModels.Count > 0)
                {
                    var VenderData = webServices.Post(new VenderViewModel(), "Vender/Edit/" + lPOInvoiceModels[0].VenderId);
                    if (VenderData.Data != "[]")
                    {
                        venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(VenderData.Data.ToString());
                    }
                }

                venderModels.Add(new VenderModel()
                {

                    Name = venderViewModel.Name,
                    Address = venderViewModel.Address,
                    Representative = venderViewModel.Representative,
                    LandLine = venderViewModel.LandLine,
                    Mobile = venderViewModel.Mobile,
                    Title = venderViewModel.Title,
                    TRN = venderViewModel.TRN,
                    UserName = "Vender Info:"
                });

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(lPOInvoiceModels);
                Report.Database.Tables[2].SetDataSource(lPOInvoiceDetails);
                Report.Database.Tables[3].SetDataSource(venderModels);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = Id + "-" + lPOInvoiceModels[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                return 1;
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult PrintLPO(int Id)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/LPOReport.rpt"));


                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
                List<VenderModel> venderModels = new List<VenderModel>();

                var LPOInvoice = webServices.Post(new IT.Core.ViewModels.LPOInvoiceModel(), "Purchase/EditReport/" + Id);
                lPOInvoiceModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.LPOInvoiceModel>>(LPOInvoice.Data.ToString());

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());



                var LPOInvoiceDetails = webServices.Post(new LPOInvoiceDetails(), "Purchase/EditDetails/" + Id);
                lPOInvoiceDetails = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceDetails>>(LPOInvoiceDetails.Data.ToString());

                VenderViewModel venderViewModel = new VenderViewModel();
                var VenderData = webServices.Post(new VenderViewModel(), "Vender/Edit/" + lPOInvoiceModels[0].VenderId);
                venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(VenderData.Data.ToString());


                venderModels.Add(new VenderModel()
                {

                    Name = venderViewModel.Name,
                    Address = venderViewModel.Address,
                    Representative = venderViewModel.Representative,
                    LandLine = venderViewModel.LandLine,
                    Mobile = venderViewModel.Mobile,
                    Title = venderViewModel.Title,
                    TRN = venderViewModel.TRN,
                    UserName = "Vender Info:"
                });

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(lPOInvoiceModels);
                Report.Database.Tables[2].SetDataSource(lPOInvoiceDetails);
                Report.Database.Tables[3].SetDataSource(venderModels);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = Id + "-" + lPOInvoiceModels[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                // Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                // stram.Seek(0, SeekOrigin.Begin);

                // return new FileStreamResult(stram, "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CheckISFileExist(int Id)
        {
            try
            {
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                var LPOInvoice = webServices.Post(new IT.Core.ViewModels.LPOInvoiceModel(), "Purchase/EditReport/" + Id);
                lPOInvoiceModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.LPOInvoiceModel>>(LPOInvoice.Data.ToString());

                string FileName = Id + "-" + lPOInvoiceModels[0].PONumber + ".pdf";

                if (System.IO.File.Exists(Server.MapPath("~/PDF/" + FileName)))
                {
                    TempData["Id"] = Id;
                    TempData["FileName"] = FileName;
                    return Json("Exist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int Res = UploadFileToFolder(Id);
                    if (Res > 0)
                    {
                        TempData["Id"] = Id;
                        TempData["FileName"] = FileName;

                        return Json("Exist", JsonRequestBehavior.AllowGet);
                    }

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