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

namespace IT.Web.Areas.CustomerExpenses.Controllers
{

    [Autintication]
    public class CustomerExpensesController : Controller
    {
        WebServices webServices = new WebServices();
        LPOInvoiceViewModel lPOInvoiceViewModel = new LPOInvoiceViewModel();
        List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
        List<ExpenseTypeViewModel> expenseTypeViewModels = new List<ExpenseTypeViewModel>();
        List<ExpenseForVIewModel> expenseForVIewModels = new List<ExpenseForVIewModel>();
        List<VehicleViewModel> VehicleViewModels = new List<VehicleViewModel>();
        List<GeneralExpenseViewModel> generalExpenseViewModels = new List<GeneralExpenseViewModel>();
        ExpenseViewModel ExpenseViewModel = new ExpenseViewModel();
        List<ExpenseDetailsViewModel> expenseDetailsViewModels = new List<ExpenseDetailsViewModel>();
        List<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();

        public ActionResult Index()
        {
            return View();
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

                if (HttpContext.Cache["CustomerExpenseData"] != null)
                {
                    expenseViewModels = HttpContext.Cache["CustomerExpenseData"] as List<ExpenseViewModel>;
                }
                else
                {
                    var result = webServices.Post(new ExpenseViewModel(), "Expense/AllCustomerExpense");
                    expenseViewModels = (new JavaScriptSerializer()).Deserialize<List<ExpenseViewModel>>(result.Data.ToString());

                    HttpContext.Cache["CustomerExpenseData"] = expenseViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = expenseViewModels.Where(x => x.EmployeeName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.ExpenseNumber.ToString().Contains(parm.sSearch)).Count();

                    expenseViewModels = expenseViewModels.ToList()
                        .Where(x => x.EmployeeName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.ExpenseNumber.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                  .Select(x => new ExpenseViewModel
                  {
                      Id = x.Id,
                      EmployeeName = x.EmployeeName,
                      Total = x.Total,
                      VAT = x.VAT,
                      GrandTotal = x.GrandTotal,
                      UserName = x.UserName,
                      ExpenseNumber = x.ExpenseNumber,
                      CreatedDates = x.CreatedDates

                  }).ToList();
                }
                else
                {
                    totalCount = expenseViewModels.Count();

                    expenseViewModels = expenseViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                        .Select(x => new ExpenseViewModel
                        {
                            Id = x.Id,
                            EmployeeName = x.EmployeeName,
                            Total = x.Total,
                            VAT = x.VAT,
                            GrandTotal = x.GrandTotal,
                            UserName = x.UserName,
                            ExpenseNumber = x.ExpenseNumber,
                            CreatedDates = x.CreatedDates

                        }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = expenseViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = expenseViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Create()
        {
            try
            {
                CustomerExpenseNumber expenseNumber = new CustomerExpenseNumber();

                var CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var Result = webServices.Post(new EmployeeViewModel(), "Employee/All/" + 1);
                var Results = webServices.Post(new ExpenseTypeViewModel(), "Expense/ExpenseTypeAll");
                var ResultExpenseFor = webServices.Post(new ExpenseForVIewModel(), "Expense/ExpenseForAll");

                if (ResultExpenseFor.Data != "[]")
                {
                    expenseForVIewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseForVIewModel>>(ResultExpenseFor.Data.ToString()));
                    expenseForVIewModels.Insert(0, new ExpenseForVIewModel() { Id = 0, ExpenseFor = "Expense For" });
                }

                ViewBag.ExpenseFor = expenseForVIewModels;

                if (Results.Data != "[]")
                {
                    expenseTypeViewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseTypeViewModel>>(Results.Data.ToString()));
                    expenseTypeViewModels.Insert(0, new ExpenseTypeViewModel() { Id = 0, ExpensType = "Select Type" });
                }

                ViewBag.ExpenseType = expenseTypeViewModels;

                if (Result.Data != "[]")
                {
                    employeeViewModels = (new JavaScriptSerializer().Deserialize<List<EmployeeViewModel>>(Result.Data.ToString()));
                    employeeViewModels.Insert(0, new EmployeeViewModel() { Id = 0, Name = "Select Employee" });

                    ViewBag.Employee = employeeViewModels;

                    ExpenseViewModel.ExpenseNumber = expenseNumber.ExNumber();

                    ViewBag.titles = "Expense";
                    ViewBag.ExpenseViewModel = ExpenseViewModel;


                    return View();
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
        public ActionResult Create(ExpenseViewModel expenseViewModel)
        {
            try
            {
                expenseViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(expenseViewModel, "Expense/AddCustomerExpense");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        //HttpContext.Cache.Remove("LPOData");
                        TempData["Id"] = Res;
                        HttpContext.Cache.Remove("ExpenseDataCustomer");
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
        public ActionResult LoadCustomerVehicle()
        {
            try
            {
                var CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["LoadCustomerVehicle"] != null)
                {
                    VehicleViewModels = HttpContext.Cache["LoadCustomerVehicle"] as List<VehicleViewModel>;
                }
                else
                {
                    var result = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
                    VehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());

                    HttpContext.Cache["LoadCustomerVehicle"] = VehicleViewModels;
                    // VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle Number" });
                }

                if (VehicleViewModels[0].TraficPlateNumber != "Select Vehicle Number")
                {
                    VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle Number" });
                }
                return Json(VehicleViewModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult LoadEmployee()
        {
            try
            {
                var CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["EmployeeData"] != null)
                {
                    employeeViewModels = HttpContext.Cache["EmployeeData"] as List<EmployeeViewModel>;
                }
                else
                {
                    var results = webServices.Post(new EmployeeViewModel(), "Employee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        HttpContext.Cache["EmployeeData"] = employeeViewModels;
                    }
                }
                if (employeeViewModels.Count > 0)
                {
                    if (employeeViewModels[0].Name != "select Employee")
                    {
                        employeeViewModels.Insert(0, new EmployeeViewModel() { Id = 0, Name = "select Employee" });
                    }
                }
                return Json(employeeViewModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult LoadGeneralExpenseCustomer()
        {
            try
            {

                if (HttpContext.Cache["CustomerGenralExpenseData"] != null)
                {
                    generalExpenseViewModels = HttpContext.Cache["CustomerGenralExpenseData"] as List<GeneralExpenseViewModel>;

                }
                else
                {
                    var results = webServices.Post(new EmployeeViewModel(), "Expense/LoadGeneralExpenseCustomer");
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        generalExpenseViewModels = (new JavaScriptSerializer()).Deserialize<List<GeneralExpenseViewModel>>(results.Data.ToString());

                        HttpContext.Cache["CustomerGenralExpenseData"] = generalExpenseViewModels;
                    }
                }

                if (generalExpenseViewModels[0].ExpenseName != "select Expense")
                {
                    generalExpenseViewModels.Insert(0, new GeneralExpenseViewModel() { Id = 0, ExpenseName = "select Expense" });
                }
                return Json(generalExpenseViewModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Details(int Id)
        {
            try
            {
                var CompanyId = Convert.ToInt32(Session["CompanyId"]);


                var ResultExp = webServices.Post(new ExpenseViewModel(), "Expense/EditExpenseCustomer/" + Id);


                if (ResultExp.Data != "[]")
                {
                    ExpenseViewModel = (new JavaScriptSerializer().Deserialize<ExpenseViewModel>(ResultExp.Data.ToString()));

                    var Result = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + ExpenseViewModel.EmployeeId);
                    if (Result.Data != "[]")
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(Result.Data.ToString());
                    }
                    ViewBag.employeeViewModels = employeeViewModels;

                    if (Result.Data != "[]")
                    {
                        EmployeeViewModel employee = (new JavaScriptSerializer().Deserialize<List<EmployeeViewModel>>(Result.Data.ToString()).FirstOrDefault());

                        ViewBag.Employee = employee;
                        ViewBag.titles = "Expense";
                        ViewBag.ExpenseViewModel = ExpenseViewModel;
                    }
                }

                var ResultExpDetais = webServices.Post(new ExpenseDetailsViewModel(), "Expense/EditExpenseCustomerDetails/" + Id);

                if (ResultExpDetais.Data != "[]")
                {
                    expenseDetailsViewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseDetailsViewModel>>(ResultExpDetais.Data.ToString()));
                    ViewBag.expenseDetailsViewModels = expenseDetailsViewModels;

                    ExpenseViewModel.expenseDetailsList = expenseDetailsViewModels;

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
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult Edit(int Id)
        {
            try
            {
                var CompanyId = Convert.ToInt32(Session["CompanyId"]);


                var Results = webServices.Post(new ExpenseTypeViewModel(), "Expense/ExpenseTypeAll");
                var ResultExpenseFor = webServices.Post(new ExpenseForVIewModel(), "Expense/ExpenseForAll");

                if (ResultExpenseFor.Data != "[]")
                {
                    expenseForVIewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseForVIewModel>>(ResultExpenseFor.Data.ToString()));
                    expenseForVIewModels.Insert(0, new ExpenseForVIewModel() { Id = 0, ExpenseFor = "Expense For" });
                }


                ViewBag.ExpenseFor = expenseForVIewModels;

                if (Results.Data != "[]")
                {
                    expenseTypeViewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseTypeViewModel>>(Results.Data.ToString()));
                    expenseTypeViewModels.Insert(0, new ExpenseTypeViewModel() { Id = 0, ExpensType = "Select Type" });
                }

                ViewBag.ExpenseType = expenseTypeViewModels;


                var ResultExp = webServices.Post(new ExpenseViewModel(), "Expense/EditExpenseCustomer/" + Id);

                List<VatModel> model = new List<VatModel>();
                model.Add(new VatModel() { Id = 0, VAT = 0 });
                model.Add(new VatModel() { Id = 5, VAT = 5 });
                ViewBag.VatDrop = model;


                if (ResultExp.Data != "[]")
                {

                    var result = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
                    if (result.Data != "[]")
                    {
                        VehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());
                    }
                    ViewBag.VehicleViewModels = VehicleViewModels;


                    var results = webServices.Post(new EmployeeViewModel(), "Employee/All/" + CompanyId);
                    if (results.Data != "[]")
                    {
                        ExpenseViewModel = (new JavaScriptSerializer().Deserialize<ExpenseViewModel>(ResultExp.Data.ToString()));

                        var Result = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + ExpenseViewModel.EmployeeId);
                        if (Result.Data != "[]")
                        {
                            employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());
                        }
                        ViewBag.employeeViewModels = employeeViewModels;

                        if (Result.Data != "[]")
                        {
                            EmployeeViewModel employee = (new JavaScriptSerializer().Deserialize<List<EmployeeViewModel>>(Result.Data.ToString()).FirstOrDefault()
);

                            ViewBag.Employee = employee;
                            ViewBag.titles = "Expense";
                            ViewBag.ExpenseViewModel = ExpenseViewModel;
                        }

                    }
                    ViewBag.ExpenseViewModel = ExpenseViewModel;


                    var resultsExp = webServices.Post(new EmployeeViewModel(), "Expense/LoadGeneralExpenseCustomer");
                    if (resultsExp.Data != "[]")
                    {
                        generalExpenseViewModels = (new JavaScriptSerializer()).Deserialize<List<GeneralExpenseViewModel>>(resultsExp.Data.ToString());
                    }

                    ViewBag.generalExpenseViewModels = generalExpenseViewModels;
                    var ResultExpDetais = webServices.Post(new ExpenseDetailsViewModel(), "Expense/EditExpenseCustomerDetails/" + Id);

                    if (ResultExpDetais.Data != "[]")
                    {
                        expenseDetailsViewModels = (new JavaScriptSerializer().Deserialize<List<ExpenseDetailsViewModel>>(ResultExpDetais.Data.ToString()));
                        ViewBag.expenseDetailsViewModels = expenseDetailsViewModels;

                        ExpenseViewModel.expenseDetailsList = expenseDetailsViewModels;

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

                throw;
            }
        }

        [HttpPost]
        public ActionResult Update(ExpenseViewModel expenseViewModel)
        {
            try
            {
                expenseViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(expenseViewModel, "Expense/UpdateCustomerExpense");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        //HttpContext.Cache.Remove("LPOData");
                        TempData["Id"] = Res;
                        HttpContext.Cache.Remove("ExpenseData");
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
        public ActionResult PrintExpense(int Id)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ExpenseReport.rpt"));


                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<ExpenseModel> expenseModel = new List<ExpenseModel>();
                List<ExpenseDetailsModel> expenseDetailsModel = new List<ExpenseDetailsModel>();
                List<EmployeeModel> employeeModels = new List<EmployeeModel>();


                var ExpDate = webServices.Post(new ExpenseModel(), "Expense/EditReportExpenseCustomer/" + Id);
                expenseModel = (new JavaScriptSerializer()).Deserialize<List<ExpenseModel>>(ExpDate.Data.ToString());

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                var EXpDetails = webServices.Post(new LPOInvoiceDetails(), "Expense/EditCustomerExpenseDetails/" + Id);
                expenseDetailsModel = (new JavaScriptSerializer()).Deserialize<List<ExpenseDetailsModel>>(EXpDetails.Data.ToString());

                EmployeeViewModel employeeViewModel = new EmployeeViewModel();
                var EmpData = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + expenseModel[0].EmployeeId);
                if (EmpData.Data != "[]")
                {
                    employeeViewModel = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(EmpData.Data.ToString()).FirstOrDefault();
                }

                List<ExpenseDetailsModel> ExpenseDetailsModel = new List<ExpenseDetailsModel>();

                foreach (var item in expenseDetailsModel)
                {
                    ExpenseDetailsModel expenseDetailsModels = new ExpenseDetailsModel();

                    expenseDetailsModels.Id = item.Id;
                    if (item.Name == null && item.TraficPlateNumber == null)
                    {
                        expenseDetailsModels.ExpenseName = item.ExpenseName;
                    }
                    else if (item.ExpenseName == null && item.Name == null)
                    {
                        expenseDetailsModels.ExpenseName = item.TraficPlateNumber;
                    }
                    else
                    {
                        expenseDetailsModels.ExpenseName = item.Name;
                    }
                    expenseDetailsModels.SubTotal = item.SubTotal;
                    expenseDetailsModels.VAT = item.VAT;
                    expenseDetailsModels.NetTotal = item.NetTotal;
                    expenseDetailsModels.Category = item.Category;
                    expenseDetailsModels.ExpDates = item.ExpDates;

                    ExpenseDetailsModel.Add(expenseDetailsModels);
                }

                employeeModels.Add(new EmployeeModel()
                {

                    Name = employeeViewModel.Name,
                    Contact = employeeViewModel.Contact,
                    Email = employeeViewModel.Email
                });

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(employeeModels);
                Report.Database.Tables[2].SetDataSource(ExpenseDetailsModel);
                Report.Database.Tables[3].SetDataSource(expenseModel);


                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName;
                if (expenseModel.Count > 0)
                {
                     companyName = Id + "-" + expenseModel[0].ExpenseNumber;
                }
                else
                {
                     companyName = "Data Not Found.pdf";
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

                //Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stram.Seek(0, SeekOrigin.Begin);

                //return new FileStreamResult(stram, "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CheckIExpenseExist(int Id)
        {
            try
            {
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                var ExpenseData = webServices.Post(new ExpenseViewModel(), "Expense/EditReportExpenseCustomer/" + Id);
                expenseViewModels = (new JavaScriptSerializer()).Deserialize<List<ExpenseViewModel>>(ExpenseData.Data.ToString());

                string FileName;

                if (expenseViewModels.Count > 0)
                {
                     FileName = Id + "-" + expenseViewModels[0].ExpenseNumber + ".pdf";
                }
                else
                {
                     FileName = "Data Not Found" +".pdf";
                }
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

        [HttpPost]
        public int UploadFileToFolder(int Id)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/ExpenseReport.rpt"));


                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<ExpenseModel> expenseModel = new List<ExpenseModel>();
                List<ExpenseDetailsModel> expenseDetailsModel = new List<ExpenseDetailsModel>();
                List<EmployeeModel> employeeModels = new List<EmployeeModel>();


                var ExpDate = webServices.Post(new ExpenseModel(), "Expense/EditReportExpenseCustomer/" + Id);
                expenseModel = (new JavaScriptSerializer()).Deserialize<List<ExpenseModel>>(ExpDate.Data.ToString());

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var companyData = webServices.Post(new IT.Core.ViewModels.CompnayModel(), "Company/Edit/" + CompanyId);
                compnayModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.CompnayModel>>(companyData.Data.ToString());

                var EXpDetails = webServices.Post(new LPOInvoiceDetails(), "Expense/EditExpenseCustomerDetails/" + Id);
                expenseDetailsModel = (new JavaScriptSerializer()).Deserialize<List<ExpenseDetailsModel>>(EXpDetails.Data.ToString());

                EmployeeViewModel employeeViewModel = new EmployeeViewModel();
                var EmpData = webServices.Post(new EmployeeViewModel(), "Employee/Edit/" + expenseModel[0].EmployeeId);
                if (EmpData.Data != "[]")
                {
                    employeeViewModel = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(EmpData.Data.ToString()).FirstOrDefault();
                }

                List<ExpenseDetailsModel> ExpenseDetailsModel = new List<ExpenseDetailsModel>();

                foreach (var item in expenseDetailsModel)
                {
                    ExpenseDetailsModel expenseDetailsModels = new ExpenseDetailsModel();

                    expenseDetailsModels.Id = item.Id;
                    if (item.Name == null && item.TraficPlateNumber == null)
                    {
                        expenseDetailsModels.ExpenseName = item.ExpenseName;
                    }
                    else if (item.ExpenseName == null && item.Name == null)
                    {
                        expenseDetailsModels.ExpenseName = item.TraficPlateNumber;
                    }
                    else
                    {
                        expenseDetailsModels.ExpenseName = item.Name;
                    }
                    expenseDetailsModels.SubTotal = item.SubTotal;
                    expenseDetailsModels.VAT = item.VAT;
                    expenseDetailsModels.NetTotal = item.NetTotal;
                    expenseDetailsModels.Category = item.Category;
                    expenseDetailsModels.ExpDates = item.ExpDates;

                    ExpenseDetailsModel.Add(expenseDetailsModels);
                }

                employeeModels.Add(new EmployeeModel()
                {

                    Name = employeeViewModel.Name,
                    Contact = employeeViewModel.Contact,
                    Email = employeeViewModel.Email
                });

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(employeeModels);
                Report.Database.Tables[2].SetDataSource(ExpenseDetailsModel);
                Report.Database.Tables[3].SetDataSource(expenseModel);


                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = Id + "-" + expenseModel[0].ExpenseNumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult DeleteExpDetailsRow(DeleteRowViewModel deleteRowViewModel)
        {
            try
            {

                decimal ResultVAT = CalculateVat(deleteRowViewModel.VAT, deleteRowViewModel.RowTotal);

                lPOInvoiceViewModel.lPOInvoiceDetailsList = new List<LPOInvoiceDetails>();

                var ResultExp = webServices.Post(new ExpenseViewModel(), "Expense/EditExpenseCustomer/" + deleteRowViewModel.Id);
                ExpenseViewModel = (new JavaScriptSerializer()).Deserialize<ExpenseViewModel>(ResultExp.Data.ToString());

                ExpenseViewModel.Total = ExpenseViewModel.Total - deleteRowViewModel.RowTotal;
                ExpenseViewModel.GrandTotal = ExpenseViewModel.GrandTotal - ResultVAT;
                ExpenseViewModel.GrandTotal = ExpenseViewModel.GrandTotal - deleteRowViewModel.RowTotal;
                ExpenseViewModel.VAT = ExpenseViewModel.VAT - ResultVAT;
                ExpenseViewModel.detailId = deleteRowViewModel.detailId;
                ExpenseViewModel.Id = deleteRowViewModel.Id;

                var result = webServices.Post(ExpenseViewModel, "Expense/DeleteCustomerDetailsRow");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                return Result;
            }
        }
    }

}
