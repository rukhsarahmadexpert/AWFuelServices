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
    public class DailyTyreTestController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var TyreTestList = unitOfWork.GetRepositoryInstance<DailyTyreTestViewModel>().ReadStoredProcedure("DailyTyreTestAll"
                    ).ToList();

                int count = TyreTestList.Count();
                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = TyreTestList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).OrderByDescending(x => x.Id).ToList();

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
        public HttpResponseMessage DailyTyreTestAdd([FromBody] DailyTyreTestViewModel TyreTestViewModel)
        {
            try
            {
                DateTime TyreTestDate = Convert.ToDateTime(TyreTestViewModel.TyreTestDates ?? System.DateTime.Now.ToString());

                var TyreTestAdd = unitOfWork.GetRepositoryInstance<DailyTyreTestViewModel>().ReadStoredProcedure("DailyTyreTestAdd @VehicleId, @TyreTestDate, @HeadFrontTypeRight, @HeadFrontTypeLeft, @HeadRearTyreRight_1_Pear, @HeadRearTyreRight_2_Pear,@HeadRearTyreLeft_1_Pear,@HeadRearTyreLeft_2_Pear,@TankerTyreRight_1,@TankerTyreRight_2,@TankerTyreRight_3,@TankerTyreLeft_1,@TankerTyreLeft_2,@TankerTyreLeft_3,@Remarks,@TankerNumber,@CreatedBy",
                 new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = TyreTestViewModel.VehicleId }
               , new SqlParameter("TyreTestDate", System.Data.SqlDbType.DateTime) { Value = TyreTestDate }
               , new SqlParameter("HeadFrontTypeRight", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadFrontTypeRight }
               , new SqlParameter("HeadFrontTypeLeft", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadFrontTypeLeft ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreRight_1_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreRight_1_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreRight_2_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreRight_2_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreLeft_1_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreLeft_1_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreLeft_2_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreLeft_2_Pear ?? (object)DBNull.Value }
               , new SqlParameter("TankerTyreRight_1", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_1 ?? (object)DBNull.Value }
               , new SqlParameter("TankerTyreRight_2", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_2 }
               , new SqlParameter("TankerTyreRight_3", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_3 }
               , new SqlParameter("TankerTyreLeft_1", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_1 }
               , new SqlParameter("TankerTyreLeft_2", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_2 }
               , new SqlParameter("TankerTyreLeft_3", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_3 }
               , new SqlParameter("Remarks", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.Remarks ?? (object)DBNull.Value }
               , new SqlParameter("TankerNumber", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerNumber ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = TyreTestViewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(TyreTestAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DailyTyreTestUpdate([FromBody] DailyTyreTestViewModel TyreTestViewModel)
        {
            try
            {
                DateTime TyreTestDate = Convert.ToDateTime(TyreTestViewModel.TyreTestDates ?? System.DateTime.Now.ToString());

                var TyreTestUpdate = unitOfWork.GetRepositoryInstance<DailyTyreTestViewModel>().ReadStoredProcedure("DailyTyreTestUpdate @Id, @VehicleId, @TyreTestDate, @HeadFrontTypeRight, @HeadFrontTypeLeft, @HeadRearTyreRight_1_Pear, @HeadRearTyreRight_2_Pear,@HeadRearTyreLeft_1_Pear,@HeadRearTyreLeft_2_Pear,@TankerTyreRight_1,@TankerTyreRight_2,@TankerTyreRight_3,@TankerTyreLeft_1,@TankerTyreLeft_2,@TankerTyreLeft_3,@Remarks,@TankerNumber,@CreatedBy",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = TyreTestViewModel.Id }
               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = TyreTestViewModel.VehicleId }
               , new SqlParameter("TyreTestDate", System.Data.SqlDbType.DateTime) { Value = TyreTestDate }
               , new SqlParameter("HeadFrontTypeRight", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadFrontTypeRight }
               , new SqlParameter("HeadFrontTypeLeft", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadFrontTypeLeft ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreRight_1_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreRight_1_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreRight_2_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreRight_2_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreLeft_1_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreLeft_1_Pear ?? (object)DBNull.Value }
               , new SqlParameter("HeadRearTyreLeft_2_Pear", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.HeadRearTyreLeft_2_Pear ?? (object)DBNull.Value }
               , new SqlParameter("TankerTyreRight_1", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_1 ?? (object)DBNull.Value }
               , new SqlParameter("TankerTyreRight_2", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_2 }
               , new SqlParameter("TankerTyreRight_3", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreRight_3 }
               , new SqlParameter("TankerTyreLeft_1", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_1 }
               , new SqlParameter("TankerTyreLeft_2", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_2 }
               , new SqlParameter("TankerTyreLeft_3", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerTyreLeft_3 }
               , new SqlParameter("Remarks", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.Remarks ?? (object)DBNull.Value }
               , new SqlParameter("TankerNumber", System.Data.SqlDbType.NVarChar) { Value = TyreTestViewModel.TankerNumber ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = TyreTestViewModel.UpdatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(TyreTestUpdate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DailyTyreTestById(DailyTyreTestViewModel dailyTyreTestViewModel)
        {
            try
            {
                var TyreTestById = unitOfWork.GetRepositoryInstance<DailyTyreTestViewModel>().ReadStoredProcedure("DailyTyreTestById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = dailyTyreTestViewModel.Id }
                    ).FirstOrDefault();
                                
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(TyreTestById));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DailyTyreTestByDates(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime From = Convert.ToDateTime(searchViewModel.FromDate ?? System.DateTime.Now.ToString());
                DateTime To = Convert.ToDateTime(searchViewModel.ToDate ?? System.DateTime.Now.ToString());

                var TyreTestByDates = unitOfWork.GetRepositoryInstance<DailyTyreTestViewModel>().ReadStoredProcedure("DailyTyreTestByDates @From,@To",
                    new SqlParameter("From", System.Data.SqlDbType.DateTime) { Value = From },
                    new SqlParameter("To", System.Data.SqlDbType.DateTime) { Value = To }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(TyreTestByDates));
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
