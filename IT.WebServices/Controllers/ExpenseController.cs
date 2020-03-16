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
    public class ExpenseController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var ExpenseList = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("ExpenseAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpenseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExpenseNumber()
        {
            try
            {
                var ExpNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("ExpenseNumber"
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpNumberData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ExpenseTypeAll()
        {
            try
            {
                var ExpenseTypeData = unitOfWork.GetRepositoryInstance<ExpenseTypeViewModel>().ReadStoredProcedure("ExpenseTypeAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpenseTypeData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ExpenseForAll()
        {
            try
            {
                var ExpenseForData = unitOfWork.GetRepositoryInstance<ExpenseForVIewModel>().ReadStoredProcedure("ExpenseForAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpenseForData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Add([FromBody] ExpenseViewModel expenseViewModel)
        {
            try
            {

                var EXPID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseAdd @EmployeeId, @ExpenseNumber, @RequestedAmount, @IssuedAmount, @PaymentTerm, @Total,@VAT, @GrandTotal, @CreatedBy,@Category,@ItemRefrenceId,@ExpensePadNumber",
                      new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = expenseViewModel.EmployeeId }
                    , new SqlParameter("ExpenseNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpenseNumber == null ? (object)DBNull.Value : expenseViewModel.ExpenseNumber }
                    , new SqlParameter("RequestedAmount", System.Data.SqlDbType.Money) { Value = expenseViewModel.RequestedAmount }
                    , new SqlParameter("IssuedAmount", System.Data.SqlDbType.Money) { Value = 0 }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = 1 }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = expenseViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expenseViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = expenseViewModel.GrandTotal }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = expenseViewModel.CreatedBy }
                    , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.Category == null ? (object)DBNull.Value : expenseViewModel.Category }
                    , new SqlParameter("ItemRefrenceId", System.Data.SqlDbType.Int) { Value = expenseViewModel.ItemRefrenceId }
                    , new SqlParameter("ExpensePadNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpensePadNumber == null ? (object)DBNull.Value : expenseViewModel.ExpensePadNumber }

                   ).FirstOrDefault();

                int ExID = Convert.ToInt32(EXPID.Result);

                if (ExID > 0)
                {
                    foreach (ExpenseDetailsViewModel expDetails in expenseViewModel.expenseDetailsList)
                    {

                        DateTime FromDate = expDetails.OnDates.AddDays(1);

                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseDetailsAdd @ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                          new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                        , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                        , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                        , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                        , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                        , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                        , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                        , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category == null ? (object)DBNull.Value : expDetails.Category }
                        , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(EXPID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Edit(int Id)
        {
            try
            {
                var ExpData = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("ExpenseGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage GeneralExpenseAll()
        {
            try
            {
                var GenExpOData = unitOfWork.GetRepositoryInstance<GeneralExpenseViewModel>().ReadStoredProcedure("GeneralExpenseAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(GenExpOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage EditExpenseDetails(int Id)
        {
            try
            {
                var ExpDetailsData = unitOfWork.GetRepositoryInstance<ExpenseDetailsViewModel>().ReadStoredProcedure("ExpenseDetailsByExpenseId @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpDetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] ExpenseViewModel expenseViewModel)
        {
            try
            {

                var EXPID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseUpdate @Id, @ExpenseNumber, @RequestedAmount, @IssuedAmount, @PaymentTerm, @Total,@VAT, @GrandTotal, @UpdatedBy,@Category,@ItemRefrenceId",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expenseViewModel.Id }
                    , new SqlParameter("ExpenseNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpenseNumber == null ? (object)DBNull.Value : expenseViewModel.ExpenseNumber }
                    , new SqlParameter("RequestedAmount", System.Data.SqlDbType.Money) { Value = expenseViewModel.RequestedAmount }
                    , new SqlParameter("IssuedAmount", System.Data.SqlDbType.Money) { Value = 0 }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = 1 }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = expenseViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expenseViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = expenseViewModel.GrandTotal }
                    , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = expenseViewModel.UpdatedBy }
                    , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.Category == null ? (object)DBNull.Value : expenseViewModel.Category }
                    , new SqlParameter("ItemRefrenceId", System.Data.SqlDbType.Int) { Value = expenseViewModel.ItemRefrenceId }

                   ).FirstOrDefault();

                int ExID = Convert.ToInt32(EXPID.Result);

                if (ExID > 0)
                {
                    foreach (ExpenseDetailsViewModel expDetails in expenseViewModel.expenseDetailsList)
                    {

                        DateTime FromDate = expDetails.OnDates.AddDays(1);

                        if (expDetails.Id == 0)
                        {

                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseDetailsAdd @ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                              new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                            , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                            , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                            , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                            , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category == null ? (object)DBNull.Value : expDetails.Category }
                            , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                           ).FirstOrDefault();
                        }
                        else
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseDetailsUpdate @Id,@ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                             new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expDetails.Id }
                           , new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                           , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                           , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                           , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                           , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                           , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                           , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                           , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category }
                           , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                          ).FirstOrDefault();
                        }
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(EXPID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDetailsRow(ExpenseViewModel expenseViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpUpdate @Id, @Total, @VAT, @GrandTotal,@ExpDetaiRowId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expenseViewModel.Id }
                , new SqlParameter("Total", System.Data.SqlDbType.Int) { Value = expenseViewModel.Total }
                , new SqlParameter("VAT", System.Data.SqlDbType.Int) { Value = expenseViewModel.VAT }
                , new SqlParameter("GrandTotal", System.Data.SqlDbType.Int) { Value = expenseViewModel.GrandTotal }
                , new SqlParameter("@ExpDetaiRowId", System.Data.SqlDbType.Int) { Value = expenseViewModel.detailId }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditReport(int Id)
        {
            try
            {
                var ExpeData = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("ExpenseGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpeData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #region Customer Expense

        [HttpPost]
        public HttpResponseMessage AllCustomerExpense()
        {
            try
            {
                var ExpenseList = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("CustomerExpenseAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpenseList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LoadGeneralExpenseCustomer()
        {
            try
            {
                var GenExpOData = unitOfWork.GetRepositoryInstance<GeneralExpenseViewModel>().ReadStoredProcedure("GeneralExpenseAllCusomer"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(GenExpOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddCustomerExpense([FromBody] ExpenseViewModel expenseViewModel)
        {
            try
            {

                var EXPID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseAddCustomer @EmployeeId, @ExpenseNumber, @RequestedAmount, @IssuedAmount, @PaymentTerm, @Total,@VAT, @GrandTotal, @CreatedBy,@Category,@ItemRefrenceId,@ExpensePadNumber",
                      new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = expenseViewModel.EmployeeId }
                    , new SqlParameter("ExpenseNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpenseNumber == null ? (object)DBNull.Value : expenseViewModel.ExpenseNumber }
                    , new SqlParameter("RequestedAmount", System.Data.SqlDbType.Money) { Value = expenseViewModel.RequestedAmount }
                    , new SqlParameter("IssuedAmount", System.Data.SqlDbType.Money) { Value = 0 }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = 1 }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = expenseViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expenseViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = expenseViewModel.GrandTotal }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = expenseViewModel.CreatedBy }
                    , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.Category == null ? (object)DBNull.Value : expenseViewModel.Category }
                    , new SqlParameter("ItemRefrenceId", System.Data.SqlDbType.Int) { Value = expenseViewModel.ItemRefrenceId }
                    , new SqlParameter("ExpensePadNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpensePadNumber == null ? (object)DBNull.Value : expenseViewModel.ExpensePadNumber }

                   ).FirstOrDefault();

                int ExID = Convert.ToInt32(EXPID.Result);

                if (ExID > 0)
                {
                    foreach (ExpenseDetailsViewModel expDetails in expenseViewModel.expenseDetailsList)
                    {
                        //DateTime ExpDate = Convert.ToDateTime(expDetails.ExpDates);

                        DateTime FromDate = System.DateTime.Now;

                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseDetailsAddCsutomer @ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                          new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                        , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                        , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                        , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                        , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                        , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                        , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                        , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category }
                        , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(EXPID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditExpenseCustomer(int Id)
        {
            try
            {
                var ExpData = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("ExpenseCustomerGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditExpenseCustomerDetails(int Id)
        {
            try
            {
                var ExpDetailsData = unitOfWork.GetRepositoryInstance<ExpenseDetailsViewModel>().ReadStoredProcedure("EditExpenseCustomerDetails @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpDetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateCustomerExpense([FromBody] ExpenseViewModel expenseViewModel)
        {
            try
            {
                var EXPID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerExpenseUpdate @Id, @ExpenseNumber, @RequestedAmount, @IssuedAmount, @PaymentTerm, @Total,@VAT, @GrandTotal, @UpdatedBy,@Category,@ItemRefrenceId",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expenseViewModel.Id }
                    , new SqlParameter("ExpenseNumber", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.ExpenseNumber == null ? (object)DBNull.Value : expenseViewModel.ExpenseNumber }
                    , new SqlParameter("RequestedAmount", System.Data.SqlDbType.Money) { Value = expenseViewModel.RequestedAmount }
                    , new SqlParameter("IssuedAmount", System.Data.SqlDbType.Money) { Value = 0 }
                    , new SqlParameter("PaymentTerm", System.Data.SqlDbType.Int) { Value = 1 }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = expenseViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expenseViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = expenseViewModel.GrandTotal }
                    , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = expenseViewModel.UpdatedBy }
                    , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expenseViewModel.Category == null ? (object)DBNull.Value : expenseViewModel.Category }
                    , new SqlParameter("ItemRefrenceId", System.Data.SqlDbType.Int) { Value = expenseViewModel.ItemRefrenceId }

                   ).FirstOrDefault();

                int ExID = Convert.ToInt32(EXPID.Result);

                if (ExID > 0)
                {
                    foreach (ExpenseDetailsViewModel expDetails in expenseViewModel.expenseDetailsList)
                    {
                        DateTime FromDate = System.DateTime.Now;

                        if (expDetails.Id == 0)
                        {

                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ExpenseDetailsAddCsutomer @ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                              new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                            , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                            , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                            , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                            , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category }
                            , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                           ).FirstOrDefault();
                        }
                        else
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CsutomerExpenseDetailsUpdate @Id,@ExpenseId, @Description, @ExpenseType, @SubTotal, @VAT, @NetTotal,@OnDates,@Category,@ExpenseRefrenceId",
                             new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expDetails.Id }
                           , new SqlParameter("ExpenseId", System.Data.SqlDbType.Int) { Value = ExID }
                           , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = expDetails.Description == null ? (object)DBNull.Value : expDetails.Description }
                           , new SqlParameter("ExpenseType", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseType }
                           , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = expDetails.SubTotal }
                           , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = expDetails.VAT }
                           , new SqlParameter("NetTotal", System.Data.SqlDbType.Money) { Value = expDetails.NetTotal }
                           , new SqlParameter("OnDates", System.Data.SqlDbType.Date) { Value = FromDate }
                           , new SqlParameter("Category", System.Data.SqlDbType.NVarChar) { Value = expDetails.Category }
                           , new SqlParameter("ExpenseRefrenceId", System.Data.SqlDbType.Int) { Value = expDetails.ExpenseRefrenceId }
                          ).FirstOrDefault();
                        }
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(EXPID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditReportExpenseCustomer(int Id)
        {
            try
            {
                var ExpeData = unitOfWork.GetRepositoryInstance<ExpenseViewModel>().ReadStoredProcedure("CustomerExpenseGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpeData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditCustomerExpenseDetails(int Id)
        {
            try
            {
                var ExpDetailsData = unitOfWork.GetRepositoryInstance<ExpenseDetailsViewModel>().ReadStoredProcedure("CustomerExpenseDetailsByExpenseId @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpDetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerExpenseNumber()
        {
            try
            {
                var ExpNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("CustomerExpenseNumber"
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ExpNumberData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteCustomerDetailsRow(ExpenseViewModel expenseViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerExpUpdate @Id, @Total, @VAT, @GrandTotal,@ExpDetaiRowId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = expenseViewModel.Id }
                , new SqlParameter("Total", System.Data.SqlDbType.Int) { Value = expenseViewModel.Total }
                , new SqlParameter("VAT", System.Data.SqlDbType.Int) { Value = expenseViewModel.VAT }
                , new SqlParameter("GrandTotal", System.Data.SqlDbType.Int) { Value = expenseViewModel.GrandTotal }
                , new SqlParameter("@ExpDetaiRowId", System.Data.SqlDbType.Int) { Value = expenseViewModel.detailId }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData.Result));
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
