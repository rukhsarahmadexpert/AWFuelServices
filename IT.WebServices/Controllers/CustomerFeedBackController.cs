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
    public class CustomerFeedBackController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";


        [HttpPost]
        public HttpResponseMessage Add([FromBody] FeedBackViewModel feedBackViewModel)
        {
            try
            {
                var vehicleAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerFeedBackAdd @FeedBack, @CompanyId, @CreatedBy",
                 new SqlParameter("FeedBack", System.Data.SqlDbType.NVarChar) { Value = feedBackViewModel.FeedBack }
               , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = feedBackViewModel.CompanyId }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = feedBackViewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize("Success"));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

    }
}
