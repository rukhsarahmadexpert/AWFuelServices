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
    public class ExceptionLogController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel serviceResponseModel = new ServiceResponseModel();
        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage AddException([FromBody] ExceptionLogViewModel exceptionLogViewModel)
        {
            try
            {
                var result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("LogException @ControllerName,@ActionName,@ExceptionType,@ExceptionDescription,@CompanyId,@userId,@ExceptionDatetime"
                    , new SqlParameter("ControllerName", System.Data.SqlDbType.VarChar) { Value = exceptionLogViewModel.ControllerName }
                    , new SqlParameter("ActionName", System.Data.SqlDbType.VarChar) { Value = exceptionLogViewModel.ActionName }
                    , new SqlParameter("ExceptionType", System.Data.SqlDbType.NVarChar) { Value = exceptionLogViewModel.ExceptionType }
                    , new SqlParameter("ExceptionDescription", System.Data.SqlDbType.NVarChar) { Value = exceptionLogViewModel.ExceptionDescription }
                    , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = exceptionLogViewModel.CompanyId }
                    , new SqlParameter("UserId", System.Data.SqlDbType.Int) { Value = exceptionLogViewModel.UserId }
                    , new SqlParameter("ExceptionDatetime", System.Data.SqlDbType.DateTime) { Value = exceptionLogViewModel.ExceptionDatetime == null ? (object)DBNull.Value : exceptionLogViewModel.ExceptionDatetime }
                    ).ToString();

                serviceResponseModel.Success((new JavaScriptSerializer()).Serialize(result));
                return Request.CreateResponse(HttpStatusCode.Accepted, serviceResponseModel, contentType);
            }
            catch (Exception exception)
            {
                serviceResponseModel.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, serviceResponseModel, contentType);
            }
        }

    }
}
