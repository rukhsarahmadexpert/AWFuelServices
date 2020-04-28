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
    public class AWFuelSalaryController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        //Generate Salary
        [HttpPost]
        public HttpResponseMessage Add([FromBody] AccountViewModel accountViewModel)
        {
            try
            {
                var Result =0;

                foreach (AccountDetailsViewModel aWFuelSalaryViewModel in accountViewModel.accountDetailsViewModels)
                {
                    var  Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AWFuelEmployeeSalaryAdd @VoucharNo, @ProjectId, @EmployeeId, @Receivable, @Received, @IssuedBy,@IssuedDate, @PaymentType,@SalaryMonth,@SalayYear",
                          new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = 0 }
                        , new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = accountViewModel.Id }
                        , new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.InvoiceId }
                        , new SqlParameter("Receivable", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.ReceivedAmount }
                        , new SqlParameter("Received", System.Data.SqlDbType.Money) { Value = 0 }
                        , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = accountViewModel.CreatedBy }
                        , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = accountViewModel.CreatedDate }
                        , new SqlParameter("PaymentType", System.Data.SqlDbType.NVarChar) { Value ="Salary Generated" }
                        , new SqlParameter("SalaryMonth", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.SalaryMonth }
                        , new SqlParameter("SalayYear", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.SalayYear }

                   ).FirstOrDefault();

                    Result = Res.Result;
                }               

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage AllEmployeeByProjectId(int Id)
        {
            try
            {
                var EmpList = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("AllEmployeeByProjectId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(EmpList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage GenerateSalary(AccountViewModel accountViewModel)
        {
            try
            {
                var Result = 0;

                foreach (AccountDetailsViewModel accountDetailsViewModel in accountViewModel.accountDetailsViewModels)
                {
                    var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("GeneratsSalary @ProjectId, @EmployeeId, @Receivable,@Payemnttype",
                      new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = accountViewModel.Id }
                    , new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = accountDetailsViewModel.InvoiceId }
                    , new SqlParameter("Receivable", System.Data.SqlDbType.Int) { Value = accountDetailsViewModel.ReceivedAmount }
                    , new SqlParameter("Payemnttype", System.Data.SqlDbType.Int) { Value = "Salary Genrated" }
                   ).FirstOrDefault();

                    Result = Res.Result;
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage IsSalaryGenerated(AccountViewModel accountViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("EmployeeSalaryIsGenerated @ProjectId, @SalaryMonth, @SalaryYear",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = accountViewModel.Id }
                     , new SqlParameter("SalaryMonth", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.SalaryMonth == null ?  (object)DBNull.Value : accountViewModel.SalaryMonth }
                     , new SqlParameter("SalaryYear", System.Data.SqlDbType.NVarChar) { Value = accountViewModel.SalayYear ==null ? (object)DBNull.Value : accountViewModel.SalayYear }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage AddEmployeeLoad(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("EmployeeLoanIssued @ProjectId, @EmloyeeId, @LoanAmount,@IssuedDate,@VoucharNo,@Description,@IssuedBy",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.ProjectId   }
                     , new SqlParameter("EmloyeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.EmployeeId  }
                     , new SqlParameter("LoanAmount", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.Received }
                     , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = aWFuelSalaryViewModel.IssuedDate == null ? (object)DBNull.Value : aWFuelSalaryViewModel.IssuedDate }
                     , new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.VoucharNo == null ? (object)DBNull.Value : aWFuelSalaryViewModel.VoucharNo }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.UpdateReason == null ? (object)DBNull.Value : aWFuelSalaryViewModel.UpdateReason }
                     , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.IssuedBy }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage EmployeeStatistics(int Id)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<AWFuelSalaryViewModel>().ReadStoredProcedure("EmployeeStatistics @Id",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage EmployeeLoanReturn(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("EmployeeLoanReturn @ProjectId, @EmloyeeId, @LoanAmount,@IssuedDate,@VoucharNo,@Description,@IssuedBy",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.ProjectId }
                     , new SqlParameter("EmloyeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.EmployeeId }
                     , new SqlParameter("LoanAmount", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.Received }
                     , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = aWFuelSalaryViewModel.IssuedDate == null ? (object)DBNull.Value : aWFuelSalaryViewModel.IssuedDate }
                     , new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.VoucharNo == null ? (object)DBNull.Value : aWFuelSalaryViewModel.VoucharNo }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.UpdateReason == null ? (object)DBNull.Value : aWFuelSalaryViewModel.UpdateReason }
                     , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.IssuedBy }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage SaveDeduction(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("SaveDeduction  @ProjectId, @EmloyeeId, @LoanAmount,@IssuedDate,@VoucharNo,@Description,@IssuedBy,@PaymentType",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.ProjectId }
                     , new SqlParameter("EmloyeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.EmployeeId }
                     , new SqlParameter("LoanAmount", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.Received }
                     , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = aWFuelSalaryViewModel.IssuedDate == null ? (object)DBNull.Value : aWFuelSalaryViewModel.IssuedDate }
                     , new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.VoucharNo == null ? (object)DBNull.Value : aWFuelSalaryViewModel.VoucharNo }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.UpdateReason == null ? (object)DBNull.Value : aWFuelSalaryViewModel.UpdateReason }
                     , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.IssuedBy }
                     , new SqlParameter("PaymentType", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.PaymentType }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
                
        [HttpPost]
        public HttpResponseMessage EmployeeAllowanceSaved(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("SaveAllowance  @ProjectId, @EmloyeeId, @LoanAmount,@IssuedDate,@VoucharNo,@Description,@IssuedBy,@PaymentType",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.ProjectId }
                     , new SqlParameter("EmloyeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.EmployeeId }
                     , new SqlParameter("LoanAmount", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.Received }
                     , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = aWFuelSalaryViewModel.IssuedDate == null ? (object)DBNull.Value : aWFuelSalaryViewModel.IssuedDate }
                     , new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.VoucharNo == null ? (object)DBNull.Value : aWFuelSalaryViewModel.VoucharNo }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.UpdateReason == null ? (object)DBNull.Value : aWFuelSalaryViewModel.UpdateReason }
                     , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.IssuedBy }
                     , new SqlParameter("PaymentType", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.PaymentType == null ? (object)DBNull.Value : aWFuelSalaryViewModel.PaymentType }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage IssueEmployeeSalary(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("IssueEmployeeSalary  @ProjectId, @EmloyeeId, @LoanAmount,@IssuedDate,@VoucharNo,@Description,@IssuedBy,@Month,@Year",
                       new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.ProjectId }
                     , new SqlParameter("EmloyeeId", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.EmployeeId }
                     , new SqlParameter("LoanAmount", System.Data.SqlDbType.Money) { Value = aWFuelSalaryViewModel.Received }
                     , new SqlParameter("IssuedDate", System.Data.SqlDbType.DateTime) { Value = aWFuelSalaryViewModel.IssuedDate == null ? (object)DBNull.Value : aWFuelSalaryViewModel.IssuedDate }
                     , new SqlParameter("VoucharNo", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.VoucharNo == null ? (object)DBNull.Value : aWFuelSalaryViewModel.VoucharNo }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.UpdateReason == null ? (object)DBNull.Value : aWFuelSalaryViewModel.UpdateReason }
                     , new SqlParameter("IssuedBy", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.IssuedBy }
                     , new SqlParameter("Month", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.Month == null ? (object)DBNull.Value : aWFuelSalaryViewModel.Month }
                     , new SqlParameter("Year", System.Data.SqlDbType.NVarChar) { Value = aWFuelSalaryViewModel.Year == null ? (object)DBNull.Value : aWFuelSalaryViewModel.Year }

                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
    }
}
