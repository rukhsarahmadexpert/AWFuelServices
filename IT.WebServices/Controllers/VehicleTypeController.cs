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
    public class VehicleTypeController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();
        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var vehicleTypeList = unitOfWork.GetRepositoryInstance<VehicleTypeViewModel>().ReadStoredProcedure("VehicleTypeAll @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = 1 }
                   ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleTypeList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
    }
}
