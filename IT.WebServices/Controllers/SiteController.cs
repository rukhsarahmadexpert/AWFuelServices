using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
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

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add(SiteViewModel siteViewModel)
        {
            try
            {

                var Res = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteAdd @SiteName,@ContactPersonName,@ContactPhone,@SiteCell,@FaceBook,@SiteEmail,@Address,@CreatedBy,@longitude,@latitude,@locationFullUrl,@pickingPoint",
                  new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName == null ? (object)DBNull.Value : siteViewModel.SiteName }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName == null ? (object)DBNull.Value : siteViewModel.ContactPersonName }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone == null ? (object)DBNull.Value : siteViewModel.ContactPhone }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell == null ? (object)DBNull.Value : siteViewModel.SiteCell }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook == null ? (object)DBNull.Value : siteViewModel.FaceBook }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail == null ? (object)DBNull.Value : siteViewModel.SiteEmail }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address == null ? (object)DBNull.Value : siteViewModel.Address }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.CreatedBy }
                , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude == null ? (object)DBNull.Value : siteViewModel.longitude }
                , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude == null ? (object)DBNull.Value : siteViewModel.latitude }
                , new SqlParameter("locationFullUrl", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl == null ? (object)DBNull.Value : siteViewModel.locationFullUrl }
                , new SqlParameter("pickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint == null ? (object)DBNull.Value : siteViewModel.PickingPoint }

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
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var SiteList = unitOfWork.GetRepositoryInstance<SiteViewModel>().ReadStoredProcedure("SiteAll"
                    ).ToList();

                int count = SiteList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    SiteList = SiteList.Where(x => x.SiteName.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
                }
                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = SiteList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                if (items.Count > 0)
                {
                    items[0].TotalRows = TotalCount;
                }

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                if (SiteList.Count < 1)
                {
                    userRepsonse.Success(null);
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                }

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
                , new SqlParameter("SiteName", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteName == null ? (object)DBNull.Value : siteViewModel.SiteName }
                , new SqlParameter("ContactPersonName", System.Data.SqlDbType.VarChar) { Value = siteViewModel.ContactPersonName == null ? (object)DBNull.Value : siteViewModel.ContactPersonName }
                , new SqlParameter("ContactPhone", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.ContactPhone == null ? (object)DBNull.Value : siteViewModel.ContactPhone }
                , new SqlParameter("SiteCell", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteCell == null ? (object)DBNull.Value : siteViewModel.SiteCell }
                , new SqlParameter("FaceBook", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.FaceBook == null ? (object)DBNull.Value : siteViewModel.FaceBook }
                , new SqlParameter("SiteEmail", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.SiteEmail == null ? (object)DBNull.Value : siteViewModel.SiteEmail }
                , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.Address == null ? (object)DBNull.Value : siteViewModel.Address }
                , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = siteViewModel.UpdateBy }
                , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.longitude == null ? (object)DBNull.Value : siteViewModel.longitude }
                , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.latitude == null ? (object)DBNull.Value : siteViewModel.latitude }
                , new SqlParameter("locationFullUrl", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.locationFullUrl == null ? (object)DBNull.Value : siteViewModel.locationFullUrl }
                , new SqlParameter("pickingPoint", System.Data.SqlDbType.NVarChar) { Value = siteViewModel.PickingPoint == null ? (object)DBNull.Value : siteViewModel.PickingPoint }

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
