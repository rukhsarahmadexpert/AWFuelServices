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

namespace IT.Web.Areas.Report.Controllers
{

    [Autintication]
    public class ReportController : Controller
    {
        WebServices webServices = new WebServices();
        List<SearchViewModel> searchViewModels = new List<SearchViewModel>();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        List<VenderModel> venderModels = new List<VenderModel>();
        List<AccountsModel> accountsModels = new List<AccountsModel>();


        public ActionResult Index()
        {
            var result = webServices.Post(new DriverViewModel(), "Vender/All");
            if (result.Data != "[]")
            {
                venderViewModels = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(result.Data.ToString());
            }
            venderViewModels.Insert(0, new VenderViewModel() { Id = 0, Name = "Select Vender Please" });
            ViewBag.Vender = venderViewModels;

            return View();
        }


        #region Purchase

        public ActionResult PurchaseAll()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/PurchaseAll/");
                SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                SalePurchaseReport[0].ReportHeading = "All Purchase";

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PurchaseFromDateToDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PurchaseFromDateToDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Purchase";
                }
                else
                {
                    SalePurchaseReport.Insert(0, new SalePurchaseReport() { ReportHeading = "No Data Found" });
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PurchaseAllByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PurchaseAllByDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Purchase";
                }


                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PurchaseByVenderANDDateRang(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PurchaseByVenderANDDateRang");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All Purchase";
                }
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PurchaseByVenderReport(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PurchaseByVenderReport");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Purchase";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Bill

        public ActionResult BillAll()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/BillAll");
                SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                SalePurchaseReport[0].ReportHeading = "All Bill";

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BillFromDateToDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/BillFromDateToDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All Bill";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BillAllByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/BillAllByDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Bill";
                }


                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BillAllByVender(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/BillAllByVender");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Bill";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UnpadBillByVender(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadBillByVender");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Bill Pending";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());


                    venderModels.Insert(0, new VenderModel()
                    {
                        Name = venderViewModel.Name,
                        Address = venderViewModel.Address,
                        LandLine = venderViewModel.LandLine,
                        Mobile = venderViewModel.Mobile,
                        Email = venderViewModel.Email
                    });

                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Issued");
                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");


                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = accountsModels[0].Id + "-" + accountsModels[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Sale

        public ActionResult SaleAllReport()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/SaleAll");
                SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                SalePurchaseReport[0].ReportHeading = "All Sales";

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Customer");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaleReportByDateRang(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/SaleReportByDateRang");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All Sale";
                }
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaleAllReportByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/SaleAllReportByDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Sales";
                }


                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaleAllReportByCustomer(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/SaleAllReportByCustomer");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Sales";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaleAllReportByCustomerAndDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/SaleAllReportByCustomerAndDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All Sales";
                }
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Expense

        public ActionResult ExpenseAllReport()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/ExpenseAllReport");
                SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                SalePurchaseReport[0].ReportHeading = "All Expenses";

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Employee Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ExpenseAllReportDateRange(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/ExpenseAllReportDateRange");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Expenses";
                }


                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);
                Report.SetParameterValue("CustomerOwner", "Employee Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ExpenseAllReportByEmployee(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ReportByCompany.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/ExpenseAllReportByEmployee");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Expense";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());
                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderViewModels);
                Report.Database.Tables[2].SetDataSource(SalePurchaseReport);


                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ExpenseAllReportByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/ExpenseAllReportByDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All Expense";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Employee Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region LPO        
        public ActionResult LPOAllReport()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/LPOAllReport");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All LPO";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LPOReportNotConverted()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/PendingLPO.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new SalePurchaseReport(), "Report/LPOReportNotConverted");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "Pending LPO";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Item");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LPOAllNotConverted()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/LPOAllNotConverted");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All LPO (Not Converted)";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LPOAllConverted()
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(new ExpenseModel(), "Report/LPOAllConverted");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All LPO (Converted)";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "Vender Name");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LPOAllByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/LPOAllByDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());

                    SalePurchaseReport[0].ReportHeading = "All LPO";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");
                Report.SetParameterValue("CustomerOwner", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult LPOFromDateToDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Purchase.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/LPOFromDateToDate");
                if (ExpDate.Data != "[]")
                {
                    SalePurchaseReport = (new JavaScriptSerializer()).Deserialize<List<SalePurchaseReport>>(ExpDate.Data.ToString());
                    SalePurchaseReport[0].ReportHeading = "All LPO";
                }


                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(SalePurchaseReport);

                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);
                Report.SetParameterValue("CustomerOwner", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = SalePurchaseReport[0].Id + "-" + SalePurchaseReport[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Invoice
        [HttpPost]
        public ActionResult UnpadInvoiceReport(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReport");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());


                    venderModels.Insert(0, new VenderModel()
                    {
                        Name = venderViewModel.Name,
                        Address = venderViewModel.Address,
                        LandLine = venderViewModel.LandLine,
                        Mobile = venderViewModel.Mobile,
                        Email = venderViewModel.Email
                    });

                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Received");
                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");


                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = accountsModels[0].Id + "-" + accountsModels[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UnpadInvoiceReportByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReportByDate");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());
                }

                //var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                //if (result.Data != "[]")
                //{
                //    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());

                //    venderModels.Insert(0, new VenderModel()
                //    {
                //        Name = venderViewModel.Name,
                //        Address = venderViewModel.Address,
                //        LandLine = venderViewModel.LandLine,
                //        Mobile = venderViewModel.Mobile,
                //        Email = venderViewModel.Email
                //    });

                //}
                //venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Received");
                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", "");


                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = "";
                if (ExpDate.Data != "[]")
                {
                    companyName = accountsModels[0].Id + "-" + accountsModels[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UnpadInvoiceReportFromDateToDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReportFromDateToDate");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                //Report.Database.Tables[0].SetDataSource(compnayModels);
                //Report.Database.Tables[1].SetDataSource(SalePurchaseReport);
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Received");
                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = accountsModels[0].Id + "-" + accountsModels[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
    }
}