﻿using IT.Core.ViewModels;
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
    public class CustomerSitesController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add(SiteViewModel siteViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("CustomerSiteAdd @SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@CreatedBy,@CompanyId,@Latitude,@Longnitude,@PickingPoint,@FullAddress",
                  new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName == null ? (object)DBNull.Value : siteViewModel.SiteName }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName == null ? (object)DBNull.Value : siteViewModel.ContactPersonName }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone == null ? (object)DBNull.Value : siteViewModel.ContactPhone }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell == null ? (object)DBNull.Value : siteViewModel.SiteCell }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook == null ? (object)DBNull.Value : siteViewModel.FaceBook }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail == null ? (object)DBNull.Value : siteViewModel.SiteEmail }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address == null ? (object)DBNull.Value : siteViewModel.Address }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.CreatedBy }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = siteViewModel.CompanyId }
                , new SqlParameter("Latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude == null ? (object)DBNull.Value : siteViewModel.latitude }
                , new SqlParameter("Longnitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude == null ? (object)DBNull.Value : siteViewModel.longitude }
                , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint == null ? (object)DBNull.Value : siteViewModel.PickingPoint }
                , new SqlParameter("FullAddress", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl == null ? (object)DBNull.Value : siteViewModel.locationFullUrl }

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
                , new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName == null ? (object)DBNull.Value : siteViewModel.SiteName }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName == null ? (object)DBNull.Value : siteViewModel.ContactPersonName }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone == null ? (object)DBNull.Value : siteViewModel.ContactPhone }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell == null ? (object)DBNull.Value : siteViewModel.SiteCell }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook == null ? (object)DBNull.Value : siteViewModel.FaceBook }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail == null ? (object)DBNull.Value : siteViewModel.SiteEmail }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address == null ? (object)DBNull.Value : siteViewModel.Address }
                , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.UpdateBy }
                , new SqlParameter("Latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude == null ? (object)DBNull.Value : siteViewModel.latitude }
                , new SqlParameter("Longnitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude == null ? (object)DBNull.Value : siteViewModel.longitude }
                , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint == null ? (object)DBNull.Value : siteViewModel.PickingPoint }
                , new SqlParameter("FullAddress", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl == null ? (object)DBNull.Value : siteViewModel.locationFullUrl }

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
