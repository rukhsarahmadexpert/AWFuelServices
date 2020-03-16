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
    public class CompanyPersonalController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";


        [HttpPost]
        public HttpResponseMessage CompanyInforamtion()
        {
            try
            {
                var CompanyInfo = unitOfWork.GetRepositoryInstance<CompanyPersonalInformation>().ReadStoredProcedure("CompanyPersonalInformationGet").FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyInfo));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }


        [HttpPost]
        public HttpResponseMessage AboutInforamtion()
        {
            try
            {
                var CompanyInfo = unitOfWork.GetRepositoryInstance<CompanyAboutUs>().ReadStoredProcedure("CompanyAboutUSGet").FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyInfo));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }


        [HttpPost]
        public HttpResponseMessage Contactforamtion()
        {
            try
            {
                var CompanyInfo = unitOfWork.GetRepositoryInstance<CompanyContactUs>().ReadStoredProcedure("CompanyContactUsGet").FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyInfo));
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
