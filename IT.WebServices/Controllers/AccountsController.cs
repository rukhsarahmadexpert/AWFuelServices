using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
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
    public class AccountsController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {

                var AccountList = unitOfWork.GetRepositoryInstance<AccountViewModel>().ReadStoredProcedure("AccountReceivedAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChequePendingAll()
        {
            try
            {

                var AccountList = unitOfWork.GetRepositoryInstance<AccountViewModel>().ReadStoredProcedure("AccountChequePendingAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AccountChequeCashedAll()
        {
            try
            {

                var ChequeChashedList = unitOfWork.GetRepositoryInstance<AccountViewModel>().ReadStoredProcedure("AccountChequeCashedAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ChequeChashedList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChequeOverDue()
        {
            try
            {
                var AccountChequeOverDue = unitOfWork.GetRepositoryInstance<AccountViewModel>().ReadStoredProcedure("AccountChequeOverDue"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountChequeOverDue));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage PendingChequeDetails(int Id)
        {
            try
            {

                var PendingChequeDetails = unitOfWork.GetRepositoryInstance<AccountViewModel>().ReadStoredProcedure("PendingChequeDetails @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PendingChequeDetails));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AccountChequeDetailsById(int Id)
        {
            try
            {

                var PendingChequeDetails = unitOfWork.GetRepositoryInstance<AccountDetailsViewModel>().ReadStoredProcedure("AccountChequeDetailsById @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PendingChequeDetails));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AccountPaymentReceiveFromCheque(AccountViewModel accountViewModel)
        {
            try
            {

                var PendingChequeDetails = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountPaymentReceiveFromCheque @Id,@CreatedBy,@VoucharNo",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = accountViewModel.Id },
                 new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy },
                 new SqlParameter("VoucharNo", System.Data.SqlDbType.Int) { Value = accountViewModel.Vouchar == null ? (object)DBNull.Value : accountViewModel.Vouchar }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PendingChequeDetails.Result));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadInvoice(int Id)
        {
            try
            {

                var AccountList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("UnpadInvoice @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnpadBill(int Id)
        {
            try
            {

                var AccountList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("UnpadBill @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AccountCustomerStatistics(int Id)
        {
            try
            {

                var AccountStatistics = unitOfWork.GetRepositoryInstance<AccountStatisticsViewModel>().ReadStoredProcedure("AccountCustomerStatistics @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountStatistics));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AccountVenderStatistics(int Id)
        {
            try
            {

                var AccountStatistics = unitOfWork.GetRepositoryInstance<AccountStatisticsViewModel>().ReadStoredProcedure("AccountVenderStatistics @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AccountStatistics));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AccountViewModel accountViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountPaymentReceive @CustomerId, @Received, @PaymentTerm, @PayedPersonName, @BankName, @AccountNumber, @CheckNumber, @CreatedBy,@Vouchar",
                      new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = accountViewModel.CustomerId }
                    , new SqlParameter("Received", System.Data.SqlDbType.Money) { Value = accountViewModel.Received }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = accountViewModel.PaymentTerm }
                    , new SqlParameter("PayedPersonName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PayedPersonName == null ? (Object)DBNull.Value : accountViewModel.PayedPersonName }
                    , new SqlParameter("BankName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.BankName == null ? (Object)DBNull.Value : accountViewModel.BankName }
                    , new SqlParameter("AccountNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.AccountNumber == null ? (Object)DBNull.Value : accountViewModel.AccountNumber }
                    , new SqlParameter("CheckNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.CheckNumber == null ? (Object)DBNull.Value : accountViewModel.CheckNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy }
                    , new SqlParameter("Vouchar", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Vouchar == null ? (object)DBNull.Value : accountViewModel.Vouchar }
                   ).FirstOrDefault();

                int AcountId = Res.Result;

                if (AcountId > 0)
                {
                    foreach (AccountDetailsViewModel accountDetailsView in accountViewModel.accountDetailsViewModels)
                    {
                        var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountReceivedDetails @AccountId,@InvoiceId, @ReceivedAmount",
                          new SqlParameter("AccountId", System.Data.SqlDbType.Int) { Value = AcountId }
                        , new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = accountDetailsView.InvoiceId }
                        , new SqlParameter("ReceivedAmount", System.Data.SqlDbType.Money) { Value = accountDetailsView.ReceivedAmount }
                       ).FirstOrDefault();
                    }
                }
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(accountViewModel.CustomerId));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChequeReceived([FromBody] AccountViewModel accountViewModel)
        {
            try
            {

                DateTime PostDate = Convert.ToDateTime(accountViewModel.PostedDates);
                accountViewModel.AccountNumber = accountViewModel.Vouchar;

                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountChequeReceivedAdd @Vouchar, @BankName, @ChequeNo, @CustomerId, @PostedDate, @Description, @CreatedBy,@Received,@AccountNumber,@PayedBy,@PayemntTerm",
                      new SqlParameter("Vouchar", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Vouchar == null ? (object)DBNull.Value : accountViewModel.Vouchar }
                    , new SqlParameter("BankName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.BankName == null ? (object)DBNull.Value : accountViewModel.BankName }
                    , new SqlParameter("ChequeNo", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.CheckNumber == null ? (object)DBNull.Value : accountViewModel.CheckNumber }
                    , new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = accountViewModel.CustomerId }
                    , new SqlParameter("PostedDate", System.Data.SqlDbType.Date) { Value = PostDate }
                    , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Description == null ? (Object)DBNull.Value : accountViewModel.Description }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy }
                    , new SqlParameter("Received", System.Data.SqlDbType.Money) { Value = accountViewModel.Received }
                    , new SqlParameter("AccountNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.AccountNumber == null ? (object)DBNull.Value : accountViewModel.AccountNumber }
                    , new SqlParameter("PayedBy", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PayedPersonName == null ? (object)DBNull.Value : accountViewModel.PayedPersonName }
                    , new SqlParameter("PayemntTerm", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PaymentTerms == null ? (object)DBNull.Value : accountViewModel.PaymentTerms }

                    ).FirstOrDefault();

                int AcountId = Res.Result;

                if (AcountId > 0)
                {
                    foreach (AccountDetailsViewModel accountDetailsView in accountViewModel.accountDetailsViewModels)
                    {
                        var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountChequeReceivedDetails @AccountId,@InvoiceId, @ReceivedAmount",
                          new SqlParameter("AccountId", System.Data.SqlDbType.Int) { Value = AcountId }
                        , new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = accountDetailsView.InvoiceId }
                        , new SqlParameter("ReceivedAmount", System.Data.SqlDbType.Money) { Value = accountDetailsView.ReceivedAmount }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(accountViewModel.CustomerId));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChequeIssued([FromBody] AccountViewModel accountViewModel)
        {
            try
            {

                DateTime PostDate = Convert.ToDateTime(accountViewModel.PostedDates);
                accountViewModel.AccountNumber = accountViewModel.Vouchar;

                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountChequeIssuedAdd @Vouchar, @BankName, @ChequeNo, @VenderId, @PostedDate, @Description, @CreatedBy,@Received,@AccountNumber,@PayedBy,@PayemntTerm",
                      new SqlParameter("Vouchar", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Vouchar == null ? (object)DBNull.Value : accountViewModel.Vouchar }
                    , new SqlParameter("BankName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.BankName == null ? (object)DBNull.Value : accountViewModel.BankName }
                    , new SqlParameter("ChequeNo", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.CheckNumber == null ? (object)DBNull.Value : accountViewModel.CheckNumber }
                    , new SqlParameter("VenderId", System.Data.SqlDbType.Int) { Value = accountViewModel.VenderId }
                    , new SqlParameter("PostedDate", System.Data.SqlDbType.Date) { Value = PostDate }
                    , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Description == null ? (Object)DBNull.Value : accountViewModel.Description }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy }
                    , new SqlParameter("Received", System.Data.SqlDbType.Money) { Value = accountViewModel.Received }
                    , new SqlParameter("AccountNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.AccountNumber == null ? (object)DBNull.Value : accountViewModel.AccountNumber }
                    , new SqlParameter("PayedBy", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PayedPersonName == null ? (object)DBNull.Value : accountViewModel.PayedPersonName }
                    , new SqlParameter("PayemntTerm", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PaymentTerms == null ? (object)DBNull.Value : accountViewModel.PaymentTerms }

                    ).FirstOrDefault();

                int AcountId = Res.Result;

                if (AcountId > 0)
                {
                    foreach (AccountDetailsViewModel accountDetailsView in accountViewModel.accountDetailsViewModels)
                    {
                        var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ChequeAccountIssuedDetails @AccountId,@BillId, @IssuedAmount",
                          new SqlParameter("AccountId", System.Data.SqlDbType.Int) { Value = AcountId }
                        , new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = accountDetailsView.InvoiceId }
                        , new SqlParameter("IssuedAmount", System.Data.SqlDbType.Money) { Value = accountDetailsView.ReceivedAmount }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(accountViewModel.VenderId));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AmountIssued([FromBody] AccountViewModel accountViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AmountIssued @VenderId, @Payed, @PaymentTerm, @PayedPersonName, @BankName, @AccountNumber, @CheckNumber, @CreatedBy,@Vouchar",
                      new SqlParameter("VenderId", System.Data.SqlDbType.Int) { Value = accountViewModel.VenderId }
                    , new SqlParameter("Payed", System.Data.SqlDbType.Money) { Value = accountViewModel.Received }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = accountViewModel.PaymentTerm }
                    , new SqlParameter("PayedPersonName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.PayedPersonName == null ? (Object)DBNull.Value : accountViewModel.PayedPersonName }
                    , new SqlParameter("BankName", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.BankName == null ? (Object)DBNull.Value : accountViewModel.BankName }
                    , new SqlParameter("AccountNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.AccountNumber == null ? (Object)DBNull.Value : accountViewModel.AccountNumber }
                    , new SqlParameter("CheckNumber", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.CheckNumber == null ? (Object)DBNull.Value : accountViewModel.CheckNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy }
                    , new SqlParameter("Vouchar", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.Vouchar == null ? (object)DBNull.Value : accountViewModel.Vouchar }
                   ).FirstOrDefault();

                int AcountId = Res.Result;

                if (AcountId > 0)
                {
                    foreach (AccountDetailsViewModel accountDetailsView in accountViewModel.accountDetailsViewModels)
                    {
                        var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AccountIssuedDetails @AccountId,@BillId, @IssuededAmount",
                          new SqlParameter("AccountId", System.Data.SqlDbType.Int) { Value = AcountId }
                        , new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = accountDetailsView.InvoiceId }
                        , new SqlParameter("IssuededAmount", System.Data.SqlDbType.Money) { Value = accountDetailsView.ReceivedAmount }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(accountViewModel.VenderId));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteTransiction(int Id)
        {
            try
            {

                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DeleteTransiction @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
    }
}
