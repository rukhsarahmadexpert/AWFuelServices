using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Accounts.Controllers
{
    public class AccountsController : Controller
    {
        WebServices webServices = new WebServices();
        List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
        List<CompanyViewModel> companyViewModels = new List<CompanyViewModel>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
        AccountStatisticsViewModel AccountStatisticsViewModel = new AccountStatisticsViewModel();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        AccountViewModel AccountViewModel = new AccountViewModel();


        public ActionResult Index()
        {
            int CompanyId = Convert.ToInt32(Session["CompanyId"]);

            try
            {

                var Res = webServices.Post(new CompanyViewModel(), "Company/CompayAll");
                if (Res.Data != "[]")
                {
                    companyViewModels = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(Res.Data.ToString());
                    companyViewModels.Insert(0, new CompanyViewModel() { Id = 0, Name = "Select Customer Name" });
                }
                ViewBag.customers = companyViewModels;


                var result = webServices.Post(new DriverViewModel(), "Vender/All");
                if (result.Data != "[]")
                {
                    venderViewModels = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(result.Data.ToString());
                    venderViewModels.Insert(0, new VenderViewModel() { Id = 0, Name = "Select Vender Name" });
                }
                ViewBag.Vender = venderViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }


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

                if (HttpContext.Cache["AccountData"] != null)
                {
                    accountViewModels = HttpContext.Cache["AccountData"] as List<AccountViewModel>;
                }
                else
                {
                    var result = webServices.Post(new AccountViewModel(), "Accounts/All");
                    if (result.Data != "[]")
                    {
                        accountViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["AccountData"] = accountViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = accountViewModels.Where(x => x.PayedPersonName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.Paid.ToString().Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower())).Count();

                    accountViewModels = accountViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.Paid.ToString().Contains(parm.sSearch) ||
                               x.PayedPersonName.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new AccountViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Paid = x.Paid,
                       Received = x.Received,
                       PayedPersonName = x.PayedPersonName,
                       UserName = x.UserName,
                       CreatedDates = x.CreatedDates
                   }).ToList();
                }
                else
                {
                    totalCount = accountViewModels.Count();

                    accountViewModels = accountViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                       .Select(x => new AccountViewModel
                       {
                           Id = x.Id,
                           Name = x.Name,
                           Paid = x.Paid,
                           Received = x.Received,
                           PayedPersonName = x.PayedPersonName,
                           UserName = x.UserName,
                           CreatedDates = x.CreatedDates
                       }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = accountViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = accountViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet]
        public JsonResult ChequePendingAll(DataTablesParm parm)
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

                if (HttpContext.Cache["AccountChequePendingAllData"] != null)
                {
                    accountViewModels = HttpContext.Cache["AccountChequePendingAllData"] as List<AccountViewModel>;
                }
                else
                {
                    var result = webServices.Post(new AccountViewModel(), "Accounts/ChequePendingAll");
                    if (result.Data != "[]")
                    {
                        accountViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["AccountChequePendingAllData"] = accountViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = accountViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.Paid.ToString().Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower())).Count();

                    accountViewModels = accountViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.Paid.ToString().Contains(parm.sSearch) ||
                               x.PayedPersonName.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new AccountViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       BankName = x.BankName,
                       AccountNumber = x.AccountNumber,
                       CheckNumber = x.CheckNumber,
                       Received = x.Received,
                       UserName = x.UserName,
                       CreatedDates = x.CreatedDates,
                       PostedDates = x.PostedDates
                   }).ToList();
                }
                else
                {
                    totalCount = accountViewModels.Count();

                    accountViewModels = accountViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                       .Select(x => new AccountViewModel
                       {
                           Id = x.Id,
                           Name = x.Name,
                           BankName = x.BankName,
                           AccountNumber = x.AccountNumber,
                           CheckNumber = x.CheckNumber,
                           Received = x.Received,
                           UserName = x.UserName,
                           CreatedDates = x.CreatedDates,
                           PostedDates = x.PostedDates
                       }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = accountViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = accountViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult ReceivedCheques()
        {
            return View();
        }

        public JsonResult AccountChequeCashedAll(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }
                if (HttpContext.Cache["AccountChequeCashedAll"] != null)
                {
                    accountViewModels = HttpContext.Cache["AccountChequeCashedAll"] as List<AccountViewModel>;
                }
                else
                {
                    var result = webServices.Post(new AccountViewModel(), "Accounts/AccountChequeCashedAll");
                    if (result.Data != "[]")
                    {
                        accountViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["AccountChequeCashedAll"] = accountViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = accountViewModels
                        .Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.PostedDates.Contains(parm.sSearch) ||
                               x.BankName.Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower())).Count();

                    accountViewModels = accountViewModels.ToList()
                        .Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.PostedDates.Contains(parm.sSearch) ||
                               x.BankName.Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new AccountViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       BankName = x.BankName,
                       AccountNumber = x.AccountNumber,
                       CheckNumber = x.CheckNumber,
                       Received = x.Received,
                       UserName = x.UserName,
                       CreatedDates = x.CreatedDates,
                       PostedDates = x.PostedDates
                   }).ToList();
                }
                else
                {
                    totalCount = accountViewModels.Count();

                    accountViewModels = accountViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                       .Select(x => new AccountViewModel
                       {
                           Id = x.Id,
                           Name = x.Name,
                           BankName = x.BankName,
                           AccountNumber = x.AccountNumber,
                           CheckNumber = x.CheckNumber,
                           Received = x.Received,
                           UserName = x.UserName,
                           CreatedDates = x.CreatedDates,
                           PostedDates = x.PostedDates
                       }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = accountViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = accountViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ChequeOverDue()
        {
            return View();
        }

        public JsonResult ChequeOverDueList(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }
                if (HttpContext.Cache["AccountChequeOverDuedAll"] != null)
                {
                    accountViewModels = HttpContext.Cache["AccountChequeOverDuedAll"] as List<AccountViewModel>;
                }
                else
                {
                    var result = webServices.Post(new AccountViewModel(), "Accounts/ChequeOverDue");
                    if (result.Data != "[]")
                    {
                        accountViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountViewModel>>(result.Data.ToString());
                    }
                    HttpContext.Cache["AccountChequeOverDuedAll"] = accountViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = accountViewModels
                        .Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.PostedDates.Contains(parm.sSearch) ||
                               x.BankName.Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower())).Count();

                    accountViewModels = accountViewModels.ToList()
                        .Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Received.ToString().Contains(parm.sSearch) ||
                               x.PostedDates.Contains(parm.sSearch) ||
                               x.BankName.Contains(parm.sSearch) ||
                               x.Name.ToLower().Contains(parm.sSearch.ToLower()))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new AccountViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       BankName = x.BankName,
                       AccountNumber = x.AccountNumber,
                       CheckNumber = x.CheckNumber,
                       Received = x.Received,
                       UserName = x.UserName,
                       CreatedDates = x.CreatedDates,
                       PostedDates = x.PostedDates
                   }).ToList();
                }
                else
                {
                    totalCount = accountViewModels.Count();

                    accountViewModels = accountViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                       .Select(x => new AccountViewModel
                       {
                           Id = x.Id,
                           Name = x.Name,
                           BankName = x.BankName,
                           AccountNumber = x.AccountNumber,
                           CheckNumber = x.CheckNumber,
                           Received = x.Received,
                           UserName = x.UserName,
                           CreatedDates = x.CreatedDates,
                           PostedDates = x.PostedDates
                       }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = accountViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = accountViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult OverDueChequeDetails(int Id)
        {
            try
            {
                var result = webServices.Post(new AccountViewModel(), "Accounts/PendingChequeDetails/" + Id);

                if (result.Data != "[]")
                {
                    AccountViewModel = (new JavaScriptSerializer()).Deserialize<AccountViewModel>(result.Data.ToString());

                    var resultDet = webServices.Post(new AccountViewModel(), "Accounts/AccountChequeDetailsById/" + Id);
                    if (resultDet.Data != "[]")
                    {
                        List<AccountDetailsViewModel> accountDetailsViewModels = new List<AccountDetailsViewModel>();

                        accountDetailsViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountDetailsViewModel>>(resultDet.Data.ToString());

                        AccountViewModel.accountDetailsViewModels = accountDetailsViewModels;
                    }
                }
                return View(AccountViewModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PendingChequeDetails(int Id)
        {
            try
            {
                var result = webServices.Post(new AccountViewModel(), "Accounts/PendingChequeDetails/" + Id);

                if (result.Data != "[]")
                {
                    AccountViewModel = (new JavaScriptSerializer()).Deserialize<AccountViewModel>(result.Data.ToString());

                    var resultDet = webServices.Post(new AccountViewModel(), "Accounts/AccountChequeDetailsById/" + Id);
                    if (resultDet.Data != "[]")
                    {
                        List<AccountDetailsViewModel> accountDetailsViewModels = new List<AccountDetailsViewModel>();

                        accountDetailsViewModels = (new JavaScriptSerializer()).Deserialize<List<AccountDetailsViewModel>>(resultDet.Data.ToString());

                        AccountViewModel.accountDetailsViewModels = accountDetailsViewModels;
                    }
                }
                return View(AccountViewModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PendingCheque()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UnpadInvoice(int Id)
        {
            try
            {
                var result = webServices.Post(new LPOInvoiceViewModel(), "Accounts/UnpadInvoice/" + Id);

                if (result.Data != "[]")
                {
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());
                }
                return Json(lPOInvoiceViewModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UnpadBill(int Id)
        {
            try
            {
                var result = webServices.Post(new LPOInvoiceViewModel(), "Accounts/UnpadBill/" + Id);

                if (result.Data != "[]")
                {
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());
                }
                return Json(lPOInvoiceViewModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        [HttpPost]
        public JsonResult AccountCustomerStatistics(int Id)
        {
            try
            {
                var result = webServices.Post(new AccountStatisticsViewModel(), "Accounts/AccountCustomerStatistics/" + Id);

                if (result.Data != "[]")
                {
                    AccountStatisticsViewModel = (new JavaScriptSerializer()).Deserialize<AccountStatisticsViewModel>(result.Data.ToString());
                }
                return Json(AccountStatisticsViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AccountVenderStatistics(int Id)
        {
            try
            {
                var result = webServices.Post(new AccountStatisticsViewModel(), "Accounts/AccountVenderStatistics/" + Id);

                if (result.Data != "[]")
                {
                    AccountStatisticsViewModel = (new JavaScriptSerializer()).Deserialize<AccountStatisticsViewModel>(result.Data.ToString());
                }
                return Json(AccountStatisticsViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AmountReceived(AccountViewModel accountViewModel)
        {
            try
            {

                accountViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(accountViewModel, "Accounts/Add");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    HttpContext.Cache.Remove("AccountData");

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AmountIssued(AccountViewModel accountViewModel)
        {

            try
            {

                accountViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(accountViewModel, "Accounts/AmountIssued");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    HttpContext.Cache.Remove("AccountData");

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteTransiction(int Id)
        {
            try
            {
                var result = webServices.Post("", "Accounts/DeleteTransiction/" + Id);

                if (result.Data != "[]")
                {
                    HttpContext.Cache.Remove("AccountData");
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChequeIssued(AccountViewModel accountViewModel)
        {
            try
            {
                accountViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(accountViewModel, "Accounts/ChequeIssued");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    HttpContext.Cache.Remove("AccountChequePendingAllData");
                    HttpContext.Cache.Remove("AccountChequeCashedAll");

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChequeReceived(AccountViewModel accountViewModel)
        {
            try
            {
                accountViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(accountViewModel, "Accounts/ChequeReceived");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    HttpContext.Cache.Remove("AccountChequePendingAllData");
                    HttpContext.Cache.Remove("AccountChequeCashedAll");

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AccountPaymentReceiveFromCheque(AccountViewModel accountViewModel)
        {
            try
            {

                accountViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(accountViewModel, "Accounts/AccountPaymentReceiveFromCheque");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    HttpContext.Cache.Remove("AccountData");
                    HttpContext.Cache.Remove("AccountChequePendingAllData");

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
    }
}