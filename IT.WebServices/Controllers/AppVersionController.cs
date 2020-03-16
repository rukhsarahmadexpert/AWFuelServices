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
    public class AppVersionController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage AppVersionAll()
        {
            try
            {
                var userList = unitOfWork.GetRepositoryInstance<AppVersionViewModel>().ReadStoredProcedure("AppVersionAll"
                   ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AppVersionAdd(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<AppVersionViewModel>().ReadStoredProcedure("AppVersionAdd @AppVersion,@AppType",
                  new SqlParameter("AppVersion", System.Data.SqlDbType.NVarChar) { Value = appVersionViewModel.AppVersion == null ? (object)DBNull.Value : appVersionViewModel.AppVersion.ToLower() }
                , new SqlParameter("AppType", System.Data.SqlDbType.NVarChar) { Value = appVersionViewModel.AppType == null ? (object)DBNull.Value : appVersionViewModel.AppType.ToLower() }
                 
                ).FirstOrDefault();
                userRepsonse.Success(new JavaScriptSerializer().Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        [HttpPost]
        public HttpResponseMessage AppVersionUpdate(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<AppVersionViewModel>().ReadStoredProcedure("AppVersionUpdate @Id,@AppVersion,@AppType",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = appVersionViewModel.Id },
                  new SqlParameter("AppVersion", System.Data.SqlDbType.NVarChar) { Value = appVersionViewModel.AppVersion == null ? (object)DBNull.Value : appVersionViewModel.AppVersion }
                , new SqlParameter("AppType", System.Data.SqlDbType.NVarChar) { Value = appVersionViewModel.AppType == null ? (object)DBNull.Value : appVersionViewModel.AppType }

                ).FirstOrDefault();
                userRepsonse.Success(new JavaScriptSerializer().Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }
        
        [HttpPost]
        public HttpResponseMessage AppVersionById(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<AppVersionViewModel>().ReadStoredProcedure("AppVersionById @Id",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = appVersionViewModel.Id }                
                ).FirstOrDefault();
                userRepsonse.Success(new JavaScriptSerializer().Serialize(Res));
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
