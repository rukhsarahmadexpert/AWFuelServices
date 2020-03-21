using IT.Core.ViewModels;
using IT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class AboutUsController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        public HttpResponseMessage Index()
        {
            try
            {
                var AboutUs = unitOfWork.GetRepositoryInstance<AboutUsViewModel>().ReadStoredProcedure("AboutUsTop1"
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AboutUs));
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
