using IT.Core.ViewModels;
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
    public class StorageController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var StorageList = unitOfWork.GetRepositoryInstance<StorageViewModel>().ReadStoredProcedure("StorageAll @SiteId",
                    new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id }
                    ).ToList();

                int count = StorageList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = StorageList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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


                userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage StorageAdd(List<StorageViewModel> storageViewModels)
        {
            try
            {
                var Result = StorageAddNew(storageViewModels);

                userRepsonse.Success(new JavaScriptSerializer().Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [NonAction]
        public StorageViewModel StorageAddNew(List<StorageViewModel> storageViewModels)
        {
            var Res = new StorageViewModel();
            try
            {                
                if (storageViewModels.Count > 0)
                {
                    foreach (var storageViewModel in storageViewModels)
                    {
                        Res = unitOfWork.GetRepositoryInstance<StorageViewModel>().ReadStoredProcedure("StorageAdd @StockIn,@StockOut,@VehicleId,@CreatedBy,@Source,@SiteId,@Action,@ClientVehicleId,@LPOId,@Decription,@ProductId,@uniques",
                          new SqlParameter("StockIn", System.Data.SqlDbType.Float) { Value = storageViewModel.StockIn }
                        , new SqlParameter("StockOut", System.Data.SqlDbType.Float) { Value = storageViewModel.StockOut }
                        , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = storageViewModel.VehicleId }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = storageViewModel.CreatedBy }
                        , new SqlParameter("Source", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.Source == null ? (object)DBNull.Value : storageViewModel.Source }
                        , new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = storageViewModel.SiteId }
                        , new SqlParameter("Action", System.Data.SqlDbType.Bit) { Value = storageViewModel.Action }
                        , new SqlParameter("ClientVehicleId", System.Data.SqlDbType.Int) { Value = storageViewModel.ClientVehicleId }
                        , new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = storageViewModel.LPOId }
                        , new SqlParameter("Decription", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.Decription ?? (object)DBNull.Value}
                        , new SqlParameter("ProductId", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.ProductId }
                        , new SqlParameter("uniques", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.uniques ?? (object)DBNull.Value}
                        ).FirstOrDefault();
                    }
                }
                else
                {
                    Res.Decription = "No Data Received To Insert";
                    return Res;
                }
                return Res;
            }
            catch(Exception ex)
            {
                Res.Decription = "exception occured" + ex.Message;
                return Res;
            }
        }

        [HttpPost]
        public HttpResponseMessage StorageUpdate(List<StorageViewModel> storageViewModels)
        {
            try
            {
                var Res = new StorageViewModel();
                foreach (var storageViewModel in storageViewModels)
                {
                    Res = unitOfWork.GetRepositoryInstance<StorageViewModel>().ReadStoredProcedure("StorageUpdate @Id,@StockIn,@StockOut,@VehicleId,@CreatedBy,@Source,@SiteId,@Action,@ClientVehicleId,@LPOId,@Decription,@ProductId",
                          new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = storageViewModel.Id }
                        , new SqlParameter("StockIn", System.Data.SqlDbType.Float) { Value = storageViewModel.StockIn }
                        , new SqlParameter("StockOut", System.Data.SqlDbType.Float) { Value = storageViewModel.StockOut }
                        , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = storageViewModel.VehicleId }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = storageViewModel.CreatedBy }
                        , new SqlParameter("Source", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.Source == null ? (object)DBNull.Value : storageViewModel.Source }
                        , new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = storageViewModel.SiteId }
                        , new SqlParameter("Action", System.Data.SqlDbType.Bit) { Value = storageViewModel.Action }
                        , new SqlParameter("ClientVehicleId", System.Data.SqlDbType.Int) { Value = storageViewModel.ClientVehicleId }
                        , new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = storageViewModel.LPOId }
                        , new SqlParameter("Decription", System.Data.SqlDbType.NVarChar) { Value = storageViewModel.Decription }
                        , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = storageViewModel.ProductId }
                    ).FirstOrDefault();
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
        public HttpResponseMessage Edit(SearchViewModel searchViewModel)
        {
            try
            {
                var StorageEditList = unitOfWork.GetRepositoryInstance<StorageViewModel>().ReadStoredProcedure("StorageTwoRowBy @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).ToList();

                
                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                  , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                  , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Storage" }
                  ).ToList();

                StorageEditList[0].uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(StorageEditList));
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
