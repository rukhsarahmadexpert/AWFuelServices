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
    public class VehicleServiceController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var vehicleService = unitOfWork.GetRepositoryInstance<VehicleServiceViewModel>().ReadStoredProcedure("VehicleServiceAll"

                    ).ToList();

                int count = vehicleService.Count();
                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = vehicleService.Skip((CurrentPage - 1) * PageSize).Take(PageSize).OrderByDescending(x => x.Id).ToList();

                if(items.Count > 0)
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
        public HttpResponseMessage VehicleServiceAdd([FromBody] VehicleServiceViewModel vehicleServiceViewModel)
        {
            try
            {
                DateTime serviceDate = Convert.ToDateTime(vehicleServiceViewModel.ServiceDates ?? System.DateTime.Now.ToString());

                var vehicleServiceAdd = unitOfWork.GetRepositoryInstance<VehicleServiceViewModel>().ReadStoredProcedure("VehicleServiceAdd @VehicleId, @ServiceDate, @Engine, @Mileage, @OliFilter, @DieselFilter,@GearOil,@BodyGreas,@Remarks,@CreatedBy",
                 new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = vehicleServiceViewModel.VehicleId }
               , new SqlParameter("ServiceDate", System.Data.SqlDbType.DateTime) { Value = serviceDate }
               , new SqlParameter("Engine", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Engine ?? (object)DBNull.Value }
               , new SqlParameter("Mileage", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Mileage ?? (object)DBNull.Value }
               , new SqlParameter("OliFilter", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.OilFilter ?? (object)DBNull.Value }
               , new SqlParameter("DieselFilter", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.DieselFilter ?? (object)DBNull.Value }
               , new SqlParameter("GearOil", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.GearOil ?? (object)DBNull.Value }
               , new SqlParameter("BodyGreas", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.BodyGreas ?? (object)DBNull.Value }
               , new SqlParameter("Remarks", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Remarks ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = vehicleServiceViewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleServiceAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage VehicleServiceByDates(SearchViewModel searchViewModel)
        {
            try
            {

                DateTime from = Convert.ToDateTime(searchViewModel.FromDate ?? System.DateTime.Now.ToString());
                DateTime To = Convert.ToDateTime(searchViewModel.ToDate ?? System.DateTime.Now.ToString());

                var vehicleServiceInDates = unitOfWork.GetRepositoryInstance<VehicleServiceViewModel>().ReadStoredProcedure("VehicleServiceByDates @VehicleId, @FromDate,@ToDateTime",
                     new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ,new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = from }
                    ,new SqlParameter("ToDateTime", System.Data.SqlDbType.DateTime) { Value = To}
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleServiceInDates));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage VehicleServiceById(SearchViewModel searchViewModel)
        {
            try
            {
                var vehicleServiceInDates = unitOfWork.GetRepositoryInstance<VehicleServiceViewModel>().ReadStoredProcedure("VehicleServiceById @Id",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleServiceInDates));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage VehicleServiceUpdate([FromBody] VehicleServiceViewModel vehicleServiceViewModel)
        {
            try
            {
                DateTime serviceDate = Convert.ToDateTime(vehicleServiceViewModel.ServiceDates ?? System.DateTime.Now.ToString());

                var vehicleServiceAdd = unitOfWork.GetRepositoryInstance<VehicleServiceViewModel>().ReadStoredProcedure("VehicleServiceUpdate @Id, @VehicleId, @ServiceDate, @Engine, @Mileage, @OliFilter, @DieselFilter,@GearOil,@BodyGreas,@Remarks,@CreatedBy",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = vehicleServiceViewModel.Id }
               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = vehicleServiceViewModel.VehicleId }
               , new SqlParameter("ServiceDate", System.Data.SqlDbType.DateTime) { Value = serviceDate }
               , new SqlParameter("Engine", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Engine ?? (object)DBNull.Value }
               , new SqlParameter("Mileage", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Mileage ?? (object)DBNull.Value }
               , new SqlParameter("OliFilter", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.OilFilter ?? (object)DBNull.Value }
               , new SqlParameter("DieselFilter", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.DieselFilter ?? (object)DBNull.Value }
               , new SqlParameter("GearOil", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.GearOil ?? (object)DBNull.Value }
               , new SqlParameter("BodyGreas", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.BodyGreas ?? (object)DBNull.Value }
               , new SqlParameter("Remarks", System.Data.SqlDbType.NVarChar) { Value = vehicleServiceViewModel.Remarks ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = vehicleServiceViewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleServiceAdd));
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
