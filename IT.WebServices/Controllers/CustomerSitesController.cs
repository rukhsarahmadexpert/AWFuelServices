using IT.Core.ViewModels;
using IT.Repository;
using IT.WebServices.MISC;
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
    public class CustomerSitesController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add(SiteViewModel siteViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("CustomerSiteAdd @SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@CreatedBy,@CompanyId,@Latitude,@Longnitude,@PickingPoint,@FullAddress",
                  new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName ?? (object)DBNull.Value  }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone ?? (object)DBNull.Value }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell ?? (object)DBNull.Value  }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook ?? (object)DBNull.Value  }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail ?? (object)DBNull.Value  }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address  ?? (object)DBNull.Value  }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.CreatedBy }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = siteViewModel.CompanyId }
                , new SqlParameter("Latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude ?? (object)DBNull.Value   }
                , new SqlParameter("Longnitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude   ?? (object)DBNull.Value   }
                , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint  ?? (object)DBNull.Value  }
                , new SqlParameter("FullAddress", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl ?? (object)DBNull.Value  }

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
        public HttpResponseMessage SiteAllCustomer(SiteViewModel siteViewModel)
        {
            try
            {
                var SiteList = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteAllCustomer @CompanyId"
                     , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = siteViewModel.CompanyId }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(SiteList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerSiteById(int Id)

        {
            try
            {
                var siteDate = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("CustomerSiteById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                
                var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
              , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
              , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "SIte" }
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
                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("CustomerSiteUpdate @Id,@SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@UpdatedBy,@Latitude,@Longnitude,@PickingPoint,@FullAddress",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = siteViewModel.Id }
                , new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName ?? (object)DBNull.Value }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone ?? (object)DBNull.Value }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell ?? (object)DBNull.Value }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook ?? (object)DBNull.Value }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail ?? (object)DBNull.Value }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address ?? (object)DBNull.Value }
                , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.UpdateBy }
                , new SqlParameter("Latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude ?? (object)DBNull.Value }
                , new SqlParameter("Longnitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude ?? (object)DBNull.Value }
                , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint ?? (object)DBNull.Value }
                , new SqlParameter("FullAddress", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl ?? (object)DBNull.Value }

                ).FirstOrDefault();

                if (siteViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if (siteViewModel.Id > 0)
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
                var siteDate = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("CustomerDeleteSite @Id,@IsActive"
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
