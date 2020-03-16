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
    public class WebSitesImagesController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add([FromBody] WebSiteImagesviewModel webSiteImagesviewModel)
        {
            try
            {
                var WebsiteImageAdd = unitOfWork.GetRepositoryInstance<WebSiteImagesviewModel>().ReadStoredProcedure("WebSiteImagesAdd @WebImageUrl, @AboutUsId, @HomeId, @ContactUsId, @ServicesId, @PrivacyPolicyId,@TermConditionId,@FrequentlyAskQuestionId,@CreatedBy",
                 new SqlParameter("WebImageUrl", System.Data.SqlDbType.NVarChar) { Value = webSiteImagesviewModel.WebImageUrl ?? (object)DBNull.Value }
               , new SqlParameter("AboutUsId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.AboutUsId }
               , new SqlParameter("HomeId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.HomeId }
               , new SqlParameter("ContactUsId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.ContactUsId }
               , new SqlParameter("ServicesId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.ServicesId }
               , new SqlParameter("PrivacyPolicyId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.PrivacyPolicyId }
               , new SqlParameter("TermConditionId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.TermConditionId }
               , new SqlParameter("FrequentlyAskQuestionId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.FrequentlyAskQuestionId }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(WebsiteImageAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Update([FromBody] WebSiteImagesviewModel webSiteImagesviewModel)
        {
            try
            {
                var WebsiteImageUpdate = unitOfWork.GetRepositoryInstance<WebSiteImagesviewModel>().ReadStoredProcedure("WebSiteImagesUpdate @Id, @WebImageUrl, @AboutUsId, @HomeId, @ContactUsId, @ServicesId, @PrivacyPolicyId,@TermConditionId,@FrequentlyAskQuestionId,@UpdatedBy",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.Id }
               , new SqlParameter("WebImageUrl", System.Data.SqlDbType.NVarChar) { Value = webSiteImagesviewModel.WebImageUrl ?? (object)DBNull.Value }
               , new SqlParameter("AboutUsId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.AboutUsId }
               , new SqlParameter("HomeId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.HomeId }
               , new SqlParameter("ContactUsId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.ContactUsId }
               , new SqlParameter("ServicesId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.ServicesId }
               , new SqlParameter("PrivacyPolicyId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.PrivacyPolicyId }
               , new SqlParameter("TermConditionId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.TermConditionId }
               , new SqlParameter("FrequentlyAskQuestionId", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.FrequentlyAskQuestionId }
               , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = webSiteImagesviewModel.UpdatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(WebsiteImageUpdate));
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
