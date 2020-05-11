using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
using IT.WebServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class SiteController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add(SiteViewModel siteViewModel)
        {
            try
            {

                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteAdd @SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@CreatedBy,@longitude,@latitude,@locationFullUrl,@pickingPoint",
                  new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone ?? (object)DBNull.Value }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell ?? (object)DBNull.Value }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook ?? (object)DBNull.Value }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail ?? (object)DBNull.Value }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address ?? (object)DBNull.Value }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.CreatedBy }
                , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude ?? (object)DBNull.Value }
                , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude ?? (object)DBNull.Value }
                , new SqlParameter("locationFullUrl", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl ?? (object)DBNull.Value }
                , new SqlParameter("pickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint ?? (object)DBNull.Value }

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
        public HttpResponseMessage All()
        {
            try
            {
                var SiteList = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteAll"
                    ).ToList();
                               
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(SiteList));
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
                var siteDate = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();

                var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
              , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
              , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "AWFSite" }
              ).ToList();

                siteDate.updateReasonDescriptionViewModels = updatereason;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(siteDate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(SiteViewModel siteViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteUpdate @Id,@SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@UpdatedBy,@longitude,@latitude,@locationFullUrl,@pickingPoint",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = siteViewModel.Id }
                , new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone ?? (object)DBNull.Value }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell ?? (object)DBNull.Value }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook ?? (object)DBNull.Value }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail ?? (object)DBNull.Value }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address ?? (object)DBNull.Value }
                , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.UpdateBy }
                , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude ?? (object)DBNull.Value }
                , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude ?? (object)DBNull.Value }
                , new SqlParameter("locationFullUrl", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl ?? (object)DBNull.Value }
                , new SqlParameter("pickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint ?? (object)DBNull.Value  }

                ).FirstOrDefault();

                if(siteViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if(siteViewModel.Id > 0)
                    {
                       var result = updateReason.Add(siteViewModel.updateReasonDescriptionViewModel);
                    }
                }

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
        public HttpResponseMessage ChangeStatus(SiteViewModel siteViewModel)
        {
            try
            {
                var siteDate = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("DeleteSite @Id,@IsActive"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = siteViewModel.Id }
                , new SqlParameter("IsActive", System.Data.SqlDbType.Bit) { Value = siteViewModel.IsActive }
                ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(siteDate));
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
