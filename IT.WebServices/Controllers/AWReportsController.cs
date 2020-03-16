using IT.Core.ViewModels;
using IT.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class AWReportsController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();


        string contentType = "application/json";


        #region Purchase


        [HttpPost]
        public HttpResponseMessage PurchaseAll()
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("PurchaseAllReport"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PurchaseFromDateToDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("PurchaseAllFromDateToDate @FromDate,@ToDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PurchaseAllByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("PurchaseAllByDate @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PurchaseByVenderANDDateRang(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("PurchaseByVenderANDDateRang @FromDate,@ToDate, @Id",
                     new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                     new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate },
                     new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }

                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PurchaseByVenderReport(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("PurchaseByVenderReport @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region Bill        
        [HttpPost]
        public HttpResponseMessage BillAll()
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("BillAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage BillFromDateToDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("BillFromDateToDateReport @FromDate,@ToDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage BillAllByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("BillAllByDateReport @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage BillAllByVender(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("BillAllByVender @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }


        #endregion

        #region Sale

        [HttpPost]
        public HttpResponseMessage SaleAll()
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("SaleAllReport"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaleReportByDateRang(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("SaleReportByDateRang @FromDate,@ToDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaleAllReportByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var SalesList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("SaleAllReportByDate @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(SalesList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaleAllReportByCustomer(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("SaleAllReportByCustomer @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaleAllReportByCustomerAndDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("SaleAllReportByCustomerAndDate @FromDate,@ToDate, @Id",
                     new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                     new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate },
                     new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }

                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region Expense
        [HttpPost]
        public HttpResponseMessage ExpenseAllReport()
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("ExpenseAllReport"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExpenseAllReportDateRange(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("ExpenseAllReportDateRange @FromDate,@ToDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExpenseAllReportByEmployee(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("ExpenseAllReportByEmployee @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExpenseAllReportByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var SalesList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("ExpenseAllReportByDate @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(SalesList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region Receivables
        [HttpPost]
        public HttpResponseMessage CustomerBalance(SearchViewModel searchViewModel)
        {
            try
            {
                var SalesList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("CustomerBalance"

                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(SalesList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        #endregion

        #region LPO
        [HttpPost]
        public HttpResponseMessage LPOAllReport()
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOAllReport"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOReportNotConverted()
        {
            try
            {
                var LPOReportNotConverted = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOReportNotConverted"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOReportNotConverted));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOAllNotConverted()
        {
            try
            {
                var LPOAllNotConverted = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOAllNotConverted"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOAllNotConverted));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOAllConverted()
        {
            try
            {
                var LPOAllConverted = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOAllConverted"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOAllConverted));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOAllByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                // DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var LPOAllByDate = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOAllByDate @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOFromDateToDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var PurchaseList = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("LPOAllByDateRange @FromDate,@ToDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadBillByVender(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<AccountsModel>().ReadStoredProcedure("UnpadBill @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region Invoice

        [HttpPost]
        public HttpResponseMessage UnpadInvoiceReport(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<AccountsModel>().ReadStoredProcedure("UnpadInvoiceReport @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadInvoiceReportByDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);

                var InvoiceAllByDate = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("UnpadInvoiceReportByDate @FromDate",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvoiceAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadInvoiceReportFromDateToDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var InvoiceAllByDate = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("UnpadInvoiceReportFromDateToDate @FromDate,@ToDate",
                 new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                 new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvoiceAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadInvoiceReportByDateCustomer(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);

                var InvoiceAllByDate = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("UnpadInvoiceReportByDateCustomer @FromDate,@Id",
                new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }

                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvoiceAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadInvoiceReportFromDateToDateCustomer(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var InvoiceAllByDate = unitOfWork.GetRepositoryInstance<SalePurchaseReport>().ReadStoredProcedure("UnpadInvoiceReportFromDateToDateCustomer @FromDate,@ToDate,@Id",
                 new SqlParameter("FromDate", System.Data.SqlDbType.VarChar) { Value = FromDate },
                 new SqlParameter("ToDate", System.Data.SqlDbType.VarChar) { Value = ToDate },
                 new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvoiceAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderDeliverdReportByVehicle(SearchViewModel searchViewModel)
        {
            try
            {

                var InvoiceAllByDate = unitOfWork.GetRepositoryInstance<OrderReport>().ReadStoredProcedure("OrderDeliverdReportByVehicle @Id,@CompanyId,@OrderProgress",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                 new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId },
                 new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvoiceAllByDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderDeliverdReportFromDateToDate(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime ToDate = Convert.ToDateTime(searchViewModel.ToDate);

                var OrderDeliverdReportList = unitOfWork.GetRepositoryInstance<OrderReport>().ReadStoredProcedure("OrderDeliverdReportFromDateToDate @FromDate,@ToDate,@Id",
                 new SqlParameter("FromDate", System.Data.SqlDbType.Date) { Value = FromDate },
                 new SqlParameter("ToDate", System.Data.SqlDbType.Date) { Value = ToDate },
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderDeliverdReportList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        #endregion

        #region Accounts
        
        [HttpPost]
        public HttpResponseMessage PaidInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<AccountsModel>().ReadStoredProcedure("PaidInvoiceReportByCompanyId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PartailPaidInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<AccountsModel>().ReadStoredProcedure("PartailPaidInvoiceReportByCompanyId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage OverDueInvoiceReportByCompanyId(SearchViewModel searchViewModel)
        {
            try
            {
                var PurchaseList = unitOfWork.GetRepositoryInstance<AccountsModel>().ReadStoredProcedure("OverDueInvoiceReportByCompanyId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.VarChar) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PurchaseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        #endregion
        
        #region New Reports API GROUPs WITH DATES for Customer For Brage
        
        //Report No:1
        [HttpPost]
        public HttpResponseMessage ReportByVehicleAndDates(SearchViewModel searchViewModel)
        {
            List<ReportsByDatesViewModel> reportsByDatesViewModels = new List<ReportsByDatesViewModel>();


            string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
            string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

            DateTime FromDate = Convert.ToDateTime(FDate);
            DateTime ToDate = Convert.ToDateTime(ToDates);

            try
            {
                var OrderDates = unitOfWork.GetRepositoryInstance<ReportsByDatesViewModel>().ReadStoredProcedure("CustomerOrderByDateAndVehicle @FromDate,@ToDate,@VehicleId,@CurrentStatus,@CompanyId",
                    new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                    new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                    new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                    new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }

                    ).ToList();

                var OrderDatesDetailsList = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderDetailsByDateAndVehicle @FromDate,@ToDate,@VehicleId,@CurrentStatus,@CompanyId",
                   new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                   new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                   ).ToList();


                foreach (var item in OrderDates)
                {
                    ReportsByDatesViewModel reportsByDatesViewModel = new ReportsByDatesViewModel();

                    reportsByDatesViewModel.Id = item.Id;
                    reportsByDatesViewModel.OrderDates = item.OrderDates;
                    reportsByDatesViewModel.VehicleId = searchViewModel.Id;
                    reportsByDatesViewModel.reportsByDatesVehicleDetails = OrderDatesDetailsList.Where(x => x.OrderDates == item.OrderDates).ToList();

                    if (reportsByDatesViewModel.reportsByDatesVehicleDetails.Count > 0)
                    {
                        reportsByDatesViewModels.Add(reportsByDatesViewModel);
                    }

                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportsByDatesViewModels));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Report No:2
        [HttpPost]
        public HttpResponseMessage ReportByVehicleAndDateGroupByVehicle(SearchViewModel searchViewModel)
        {
            List<ReportsByDatesViewModel> reportsByDatesViewModels = new List<ReportsByDatesViewModel>();


            string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
            string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

            DateTime FromDate = Convert.ToDateTime(FDate);
            DateTime ToDate = Convert.ToDateTime(ToDates);

            try
            {
                var OrderDates = unitOfWork.GetRepositoryInstance<ReportsByDatesViewModel>().ReadStoredProcedure("CustomerOrderByDateAndVehicleAll @FromDate,@ToDate,@VehicleId,@CurrentStatus,@CompanyId",
                    new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                    new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                    new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                    new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }

                    ).ToList();

                var OrderDatesDetailsList = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderDetailsByDateAndVehicleAll @FromDate,@ToDate,@VehicleId,@CurrentStatus,@CompanyId",
                   new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                   new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                   ).ToList();


                foreach (var item in OrderDates)
                {
                    ReportsByDatesViewModel reportsByDatesViewModel = new ReportsByDatesViewModel();

                    reportsByDatesViewModel.Id = item.Id;
                    reportsByDatesViewModel.OrderDates = item.OrderDates;
                    reportsByDatesViewModel.VehicleId = item.VehicleId;
                    reportsByDatesViewModel.TraficPlateNumber = item.TraficPlateNumber;
                    reportsByDatesViewModel.reportsByDatesVehicleDetails = OrderDatesDetailsList.Where(x => x.VehicleId == item.VehicleId).ToList();

                    if (reportsByDatesViewModel.reportsByDatesVehicleDetails.Count > 0)
                    {
                        reportsByDatesViewModels.Add(reportsByDatesViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportsByDatesViewModels));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        //Report No:3 AND No:2 in Admin Same Report
        [HttpPost]
        public HttpResponseMessage RepoOrdersByDates(SearchViewModel searchViewModel)
        {
            List<ReportsByDatesViewModel> reportsByDatesViewModels = new List<ReportsByDatesViewModel>();


            string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
            string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

            DateTime FromDate = Convert.ToDateTime(FDate);
            DateTime ToDate = Convert.ToDateTime(ToDates);

            try
            {
                var OrderDatesGroup = unitOfWork.GetRepositoryInstance<ReportsByDatesViewModel>().ReadStoredProcedure("CustomerOrderByDate @FromDate,@ToDate,@CurrentStatus,@CompanyId",
                    new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                    new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                    new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }

                    ).ToList();

                var OrderDatesNonGroupList = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderByDateWithOutGroup @FromDate,@ToDate,@CurrentStatus,@CompanyId",
                   new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                   new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                   ).ToList();


                foreach (var item in OrderDatesGroup)
                {
                    ReportsByDatesViewModel reportsByDatesViewModel = new ReportsByDatesViewModel();

                    reportsByDatesViewModel.Id = item.Id;
                    reportsByDatesViewModel.OrderDates = item.OrderDates;
                    reportsByDatesViewModel.VehicleId = item.VehicleId;
                    reportsByDatesViewModel.reportsByDatesVehicleDetails = OrderDatesNonGroupList.Where(x => x.OrderDates == item.OrderDates).ToList();

                    if (reportsByDatesViewModel.reportsByDatesVehicleDetails.Count > 0)
                    {
                        reportsByDatesViewModels.Add(reportsByDatesViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportsByDatesViewModels));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region New Reports API GROUPs WITH DATES for Admin For Brage
        [HttpPost]
        public HttpResponseMessage ReportCustomerOrderOrderDetailsAll(SearchViewModel searchViewModel)
        {

            try
            {

                List<ReportAdminViewModel> reportAdminViewModel = new List<ReportAdminViewModel>();

                string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
                string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

                DateTime FromDate = Convert.ToDateTime(FDate);
                DateTime ToDate = Convert.ToDateTime(ToDates);

                var reportAdminViewModele = unitOfWork.GetRepositoryInstance<ReportAdminViewModel>().ReadStoredProcedure("ReportCustomerOrderOrderAllAdmin @FromDate,@ToDate,@OrderProgress",
                     new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                     new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                     new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                   ).ToList();

                var OrderDatesDetailsListAdmin = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderDetailsByDateAdmin @FromDate,@ToDate,@CurrentStatus",
                  new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                  new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                  ).ToList();


                if (reportAdminViewModele.Count > 0)
                {
                    foreach (var item in reportAdminViewModele)
                    {
                        ReportAdminViewModel reportAdminViewModeles = new ReportAdminViewModel();

                        reportAdminViewModeles.reportsByDatesCompanyDetails = OrderDatesDetailsListAdmin.Where(x => x.CustomerId == item.companyId).ToList();
                        reportAdminViewModeles.companyId = item.companyId;
                        reportAdminViewModeles.CompanyName = item.CompanyName;


                        if (reportAdminViewModeles.reportsByDatesCompanyDetails.Count > 0)
                        {
                            reportAdminViewModel.Add(reportAdminViewModeles);
                        }
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportAdminViewModel));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ReportBYCompanyAndDateAdmin(SearchViewModel searchViewModel)
        {
            try
            {

                List<ReportAdminViewModel> reportAdminViewModel = new List<ReportAdminViewModel>();

                string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
                string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

                DateTime FromDate = Convert.ToDateTime(FDate);
                DateTime ToDate = Convert.ToDateTime(ToDates);

                var reportAdminViewModele = unitOfWork.GetRepositoryInstance<ReportAdminViewModel>().ReadStoredProcedure("ReportCustomerOrderOrderAllAdmin @FromDate,@ToDate,@OrderProgress",
                     new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                     new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                     new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                   ).ToList();

                var OrderDatesDetailsListAdmin = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderDetailsByDateAdmin @FromDate,@ToDate,@CurrentStatus",
                  new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                  new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                  ).ToList();


                if (reportAdminViewModele.Count > 0)
                {
                    foreach (var item in reportAdminViewModele)
                    {
                        ReportAdminViewModel reportAdminViewModeles = new ReportAdminViewModel();

                        reportAdminViewModeles.reportsByDatesCompanyDetails = OrderDatesDetailsListAdmin.Where(x => x.CustomerId == item.companyId).ToList();
                        reportAdminViewModeles.companyId = item.companyId;
                        reportAdminViewModeles.CompanyName = item.CompanyName;


                        if (reportAdminViewModeles.reportsByDatesCompanyDetails.Count > 0)
                        {
                            reportAdminViewModel.Add(reportAdminViewModeles);
                        }
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportAdminViewModel));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ReportByAdminDriver(SearchViewModel searchViewModel)
        {

            {
                try
                {

                    List<ReportAdminViewModel> reportAdminViewModel = new List<ReportAdminViewModel>();

                    string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
                    string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

                    DateTime FromDate = Convert.ToDateTime(FDate);
                    DateTime ToDate = Convert.ToDateTime(ToDates);

                    var reportAdminViewModele = unitOfWork.GetRepositoryInstance<ReportAdminViewModel>().ReadStoredProcedure("ReportCustomerOrderOrderAllAdminDriver @FromDate,@ToDate,@OrderProgress,@DriverId",
                         new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                         new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                         new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey },
                         new SqlParameter("DriverId", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.Id }
                       ).ToList();

                    var OrderDatesDetailsListAdmin = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderDetailsByDateAdminDriver @FromDate,@ToDate,@CurrentStatus",
                      new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                      new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                       new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                      ).ToList();


                    if (reportAdminViewModele.Count > 0)
                    {
                        foreach (var item in reportAdminViewModele)
                        {
                            ReportAdminViewModel reportAdminViewModeles = new ReportAdminViewModel();

                            reportAdminViewModeles.reportsByDatesCompanyDetails = OrderDatesDetailsListAdmin.Where(x => x.CustomerId == item.companyId).ToList();
                            reportAdminViewModeles.companyId = item.companyId;
                            reportAdminViewModeles.CompanyName = item.CompanyName;


                            if (reportAdminViewModeles.reportsByDatesCompanyDetails.Count > 0)
                            {
                                reportAdminViewModel.Add(reportAdminViewModeles);
                            }
                        }
                    }

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportAdminViewModel));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception ex)
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage ReportCustomerOrderAdminByStatus(SearchViewModel searchViewModel)
        {
            try
            {

                List<ReportsByDatesViewModel> reportsByDatesViewModels = new List<ReportsByDatesViewModel>();

                string FDate = Convert.ToDateTime(searchViewModel.FromDate).ToString("yyyy-MM-dd h:mm tt");
                string ToDates = Convert.ToDateTime(searchViewModel.ToDate).ToString("yyyy-MM-dd h:mm tt");

                DateTime FromDate = Convert.ToDateTime(FDate);
                DateTime ToDate = Convert.ToDateTime(ToDates);

                var reportAdminViewModele = unitOfWork.GetRepositoryInstance<ReportsByDatesViewModel>().ReadStoredProcedure("ReportCustomerOrderAdminByStatus @FromDate,@ToDate,@OrderProgress",
                     new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                     new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                     new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                   ).ToList();

                var OrderDatesNonGroupList = unitOfWork.GetRepositoryInstance<ReportsByDatesVehicleDetails>().ReadStoredProcedure("CustomerOrderByDateWithOutGroupAdmin @FromDate,@ToDate,@CurrentStatus",
                   new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate },
                   new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = ToDate },
                   new SqlParameter("CurrentStatus", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                   
                   ).ToList();


                foreach (var item in reportAdminViewModele)
                {
                    ReportsByDatesViewModel reportsByDatesViewModel = new ReportsByDatesViewModel();

                    reportsByDatesViewModel.Id = item.Id;
                    reportsByDatesViewModel.OrderDates = item.OrderDates;
                    reportsByDatesViewModel.VehicleId = item.VehicleId;
                    reportsByDatesViewModel.reportsByDatesVehicleDetails = OrderDatesNonGroupList.Where(x => x.OrderDates == item.OrderDates).ToList();

                    if (reportsByDatesViewModel.reportsByDatesVehicleDetails.Count > 0)
                    {
                        reportsByDatesViewModels.Add(reportsByDatesViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(reportsByDatesViewModels));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

    }
}
