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
    public class PrivacyPolicyController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        public HttpResponseMessage Index()
        {
            try
            {
                var privacypolicy = unitOfWork.GetRepositoryInstance<PrivatePolicyViewModel>().ReadStoredProcedure("PrivacyPlicyAll"
                    ).FirstOrDefault();

                var privacypolicyDetails = unitOfWork.GetRepositoryInstance<PrivacyPolicyDetailsViewModel>().ReadStoredProcedure("PrivacyPlicyDetailsByPrivacyId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = privacypolicy.Id }
                    ).ToList();

                privacypolicy.privacyPolicyDetailsViewModels = privacypolicyDetails;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(privacypolicy));
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
