using CrystalDecisions.CrystalReports.Engine;
using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.CustomerReports.Controllers
{

    [Autintication]
    public class CustomerReportsController : Controller
    {
        WebServices webServices = new WebServices();
        List<SearchViewModel> searchViewModels = new List<SearchViewModel>();
        List<CustomerViewModel> venderViewModels = new List<CustomerViewModel>();
        List<CustomerViewModel> venderModels = new List<CustomerViewModel>();
        List<AccountsModel> accountsModels = new List<AccountsModel>();
        List<OrderReport> orderReports = new List<OrderReport>();


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UnpadInvoiceReportCustomer()
        {
            string pdfname = "";
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                SearchViewModel searchViewModel = new SearchViewModel();
                searchViewModel.Id = CompanyId;

                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReport");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }

                //AWFuel Company Id should be send here

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + 2);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);

                CustomerViewModel venderViewModel = new CustomerViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 
                List<CompnayModel> compnayModels1 = new List<CompnayModel>();
                if (result.Data != "[]")
                {
                    compnayModels1 = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(result.Data.ToString());

                    venderModels.Insert(0, new CustomerViewModel()
                    {
                        Name = compnayModels1[0].Name,
                        Address = compnayModels1[0].Address,
                        LandLine = compnayModels1[0].Phone,
                        Mobile = compnayModels1[0].Cell,
                        Email = compnayModels1[0].Email
                    });

                }
                venderViewModels.Insert(0, venderViewModel);

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Paid");
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
        public ActionResult UnpadInvoiceReportCustomerByDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                searchViewModel.Id = CompanyId;

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReportByDateCustomer");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }


                //AWFuel Company Id
                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + 2);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
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

                Report.SetParameterValue("IssuedReceived", "Paid");
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
        public ActionResult UnpadInvoiceReportFromDateToDateCustomer(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                searchViewModel.Id = CompanyId;
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/UnpadInvoiceReportFromDateToDateCustomer");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Pending Invoices";
                }
                               
                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
                              
                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(accountsModels);

                Report.SetParameterValue("IssuedReceived", "Paid");
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

        [HttpPost]
        public ActionResult OrderDeliverdReportByVehicle(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                searchViewModel.CompanyId = CompanyId;
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/OrderReport.rpt"));

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<OrderReport> SalePurchaseReport = new List<OrderReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/OrderDeliverdReportByVehicle");
                if (ExpDate.Data != "[]")
                {
                    orderReports = (new JavaScriptSerializer()).Deserialize<List<OrderReport>>(ExpDate.Data.ToString());
                    orderReports[0].Status = "All Deliver Order";
                }

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(orderReports);

                Report.SetParameterValue("IssuedReceived", "");
                Report.SetParameterValue("FromDates", "");
                Report.SetParameterValue("ToDate", "");

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = orderReports[0].OrderId + "-" + orderReports[0].VehicleNumber;
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
        public ActionResult OrderDeliverdReportFromDateToDate(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                searchViewModel.CompanyId = CompanyId;
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/OrderReport.rpt"));

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<OrderReport> SalePurchaseReport = new List<OrderReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/OrderDeliverdReportFromDateToDate");
                if (ExpDate.Data != "[]")
                {
                    orderReports = (new JavaScriptSerializer()).Deserialize<List<OrderReport>>(ExpDate.Data.ToString());
                    orderReports[0].Status = "All Deliver Order";
                }

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(orderReports);

                Report.SetParameterValue("IssuedReceived", "");
                Report.SetParameterValue("FromDates", searchViewModel.FromDate);
                Report.SetParameterValue("ToDate", searchViewModel.ToDate);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (ExpDate.Data != "[]")
                {
                    companyName = orderReports[0].OrderId + "-" + orderReports[0].VehicleNumber;
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

        [HttpGet]
        public ActionResult PaidInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                searchViewModel.Id = CompanyId;

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PaidInvoiceReportByCompanyId");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Paid Invoices";
                }

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());


                    //    venderModels.Insert(0, new VenderModel()
                    //    {
                    //        Name = venderViewModel.Name,
                    //        Address = venderViewModel.Address,
                    //        LandLine = venderViewModel.LandLine,
                    //        Mobile = venderViewModel.Mobile,
                    //        Email = venderViewModel.Email
                    //    });

                }
                //venderViewModels.Insert(0, venderViewModel);

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

        [HttpGet]
        public ActionResult PartailPaidInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                searchViewModel.Id = CompanyId;

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/PartailPaidInvoiceReportByCompanyId");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Partial Paid Invoices";
                }

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());


                    //    venderModels.Insert(0, new VenderModel()
                    //    {
                    //        Name = venderViewModel.Name,
                    //        Address = venderViewModel.Address,
                    //        LandLine = venderViewModel.LandLine,
                    //        Mobile = venderViewModel.Mobile,
                    //        Email = venderViewModel.Email
                    //    });

                }
                //venderViewModels.Insert(0, venderViewModel);

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

        [HttpGet]
        public ActionResult OverDueInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/Accounts.rpt"));

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                searchViewModel.Id = CompanyId;

                List<CompnayModel> compnayModels = new List<CompnayModel>();
                List<SalePurchaseReport> SalePurchaseReport = new List<SalePurchaseReport>();

                var ExpDate = webServices.Post(searchViewModel, "Report/OverDueInvoiceReportByCompanyId");
                if (ExpDate.Data != "[]")
                {
                    accountsModels = (new JavaScriptSerializer()).Deserialize<List<AccountsModel>>(ExpDate.Data.ToString());
                    accountsModels[0].ReportHeading = "All Over Due Invoices";
                }

                var companyData = webServices.Post(new CompnayModel(), "Company/Edit/" + CompanyId);
                if (companyData.Data != "[]")
                {
                    compnayModels = (new JavaScriptSerializer()).Deserialize<List<CompnayModel>>(companyData.Data.ToString());
                }

                var result = webServices.Post(new VenderViewModel(), "Vender/Edit/" + searchViewModel.Id);

                VenderViewModel venderViewModel = new VenderViewModel();
                //List<VenderModel> venderModels = new List<VenderModel>(); 

                if (result.Data != "[]")
                {
                    venderViewModel = (new JavaScriptSerializer()).Deserialize<VenderViewModel>(result.Data.ToString());


                    //    venderModels.Insert(0, new VenderModel()
                    //    {
                    //        Name = venderViewModel.Name,
                    //        Address = venderViewModel.Address,
                    //        LandLine = venderViewModel.LandLine,
                    //        Mobile = venderViewModel.Mobile,
                    //        Email = venderViewModel.Email
                    //    });

                }
                //venderViewModels.Insert(0, venderViewModel);

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
    }
}