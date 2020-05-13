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
    public class ProjectController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage Add([FromBody] ProjectViewModel projectViewModel)
        {
            try
            {

                projectViewModel.StartDate = projectViewModel.StartDate.AddDays(1);
                projectViewModel.EndDate = projectViewModel.EndDate.AddDays(1);

                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProjectAdd @ProjectTitle, @Location, @Contact, @Email, @StartDate,@EndDate,@CreatedBy"

                    , new SqlParameter("ProjectTitle", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.ProjectTitle }
                    , new SqlParameter("Location", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Location }
                    , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Contact == null ? (object)DBNull.Value : projectViewModel.Contact }
                    , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Email == null ? (object)DBNull.Value : projectViewModel.Email }
                    , new SqlParameter("StartDate", System.Data.SqlDbType.DateTime) { Value = projectViewModel.StartDate }
                    , new SqlParameter("EndDate", System.Data.SqlDbType.DateTime) { Value = projectViewModel.EndDate }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = projectViewModel.CreatedBy }
                   ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var ProjectData = unitOfWork.GetRepositoryInstance<ProjectViewModel>().ReadStoredProcedure("ProjectAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ProjectData));
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
                var ProjectDate = unitOfWork.GetRepositoryInstance<ProjectViewModel>().ReadStoredProcedure("ProjectById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(ProjectDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Update(ProjectViewModel projectViewModel)
        {
            try
            {
                projectViewModel.StartDate = projectViewModel.StartDate.AddDays(1);
                projectViewModel.EndDate = projectViewModel.EndDate.AddDays(1);

                var designationAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProjectUpdate @Id,@ProjectTitle,@Location,@Contact,@Email,@StartDate,@EndDate,@UpdatedBy,@UpdatedReason",
                      new SqlParameter("Id", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Id }
                    ,  new SqlParameter("ProjectTitle", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.ProjectTitle }
                    , new SqlParameter("Location", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Location }
                    , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Contact == null ? (object)DBNull.Value : projectViewModel.Contact }
                    , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.Email == null ? (object)DBNull.Value : projectViewModel.Email }
                    , new SqlParameter("StartDate", System.Data.SqlDbType.DateTime) { Value = projectViewModel.StartDate }
                    , new SqlParameter("EndDate", System.Data.SqlDbType.DateTime) { Value = projectViewModel.EndDate }
                    , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = projectViewModel.UpdatedBy }
                    , new SqlParameter("UpdatedReason", System.Data.SqlDbType.NVarChar) { Value = projectViewModel.UpdatedReason == null ? (object)DBNull.Value : projectViewModel.UpdatedReason }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designationAdd.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Details(int Id)
        {
            try
            {
                var ProjectDate = unitOfWork.GetRepositoryInstance<ProjectViewModel>().ReadStoredProcedure("ProjectDetailsById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(ProjectDate));
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
