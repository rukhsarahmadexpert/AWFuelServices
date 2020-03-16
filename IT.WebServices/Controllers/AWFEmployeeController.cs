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
    public class AWFEmployeeController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All(int Id)
        {
            try
            {
                var employeeList = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("EmployeeAllAWFuel @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employeeList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add(EmployeeViewModel employeeViewModel)
        {

            try
            {
                var employeeAdd = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().WriteStoredProcedure("EmployeeAddAWFuel @Name,@Designation,@Contact,@Email,@Facebook,@Comments,@CreatedBy,@PassportFront,@PassportBack, @UID,@BasicSalary,@CompanyId,@ProjectId",
                  new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = employeeViewModel.Name == null ? (object)DBNull.Value : employeeViewModel.Name }
                , new SqlParameter("Designation", System.Data.SqlDbType.Int) { Value = employeeViewModel.Designation }
                , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Contact == null ? (object)DBNull.Value : employeeViewModel.Contact }
                , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Email == null ? (object)DBNull.Value : employeeViewModel.Email }
                , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Facebook == null ? (object)DBNull.Value : employeeViewModel.Facebook }
                , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Comments == null ? (object)DBNull.Value : employeeViewModel.Comments }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = employeeViewModel.CreatedBy }
                , new SqlParameter("PassportFront", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.PassportFront == null ? (object)DBNull.Value : employeeViewModel.PassportFront }
                , new SqlParameter("PassportBack", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.PassportBack == null ? (object)DBNull.Value : employeeViewModel.PassportBack }
                , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.UID == null ? (object)DBNull.Value : employeeViewModel.UID }
                , new SqlParameter("BasicSalary", System.Data.SqlDbType.Money) { Value = employeeViewModel.BasicSalary == 0 ? (object)DBNull.Value : employeeViewModel.BasicSalary }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = employeeViewModel.CompanyId }
                , new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = employeeViewModel.ProjectId }

                
                );
                userRepsonse.Success(new JavaScriptSerializer().Serialize(employeeAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        [HttpPost]
        public HttpResponseMessage Edit(int Id)
        {
            try
            {
                var employee = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("EmployeeByIdAWFuel @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employee));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(EmployeeViewModel employeeViewModel)
        {

            try
            {
                var employeeUpdate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("EmployeeUpdateAWFuel @Id, @Name,@Designation,@Contact,@Email,@Facebook,@Comments,@BasicSalary,@UpdayedBy,@ProjectId",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = employeeViewModel.Id }
                , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = employeeViewModel.Name == null ? (object)DBNull.Value : employeeViewModel.Name }
                , new SqlParameter("Designation", System.Data.SqlDbType.Int) { Value = employeeViewModel.Designation }
                , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Contact == null ? (object)DBNull.Value : employeeViewModel.Contact }
                , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Email == null ? (object)DBNull.Value : employeeViewModel.Email }
                , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Facebook == null ? (object)DBNull.Value : employeeViewModel.Facebook }
                , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Comments == null ? (object)DBNull.Value : employeeViewModel.Comments }
                , new SqlParameter("BasicSalary", System.Data.SqlDbType.Money) { Value = employeeViewModel.BasicSalary == 0 ? (object)DBNull.Value : employeeViewModel.BasicSalary }
                , new SqlParameter("UpdayedBy", System.Data.SqlDbType.Int) { Value = employeeViewModel.UpdatedBy }
                , new SqlParameter("ProjectId", System.Data.SqlDbType.Int) { Value = employeeViewModel.ProjectId }

                );
                userRepsonse.Success(new JavaScriptSerializer().Serialize(employeeUpdate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        [HttpPost]
        public HttpResponseMessage Delete(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var employee = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("EmployeeDeleteAWFuel @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = employeeViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employee));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

    }
}
