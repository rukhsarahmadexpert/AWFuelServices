using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
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
    public class LocationsController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json"; 
        
        [HttpPost]
        public HttpResponseMessage CustomerOrderLocationAdd([FromBody] CustomerOrderLocationViewModel customerOrderLocationViewModel)
        {
            try
            {

                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("CustomerOrderLocationAdd @OrderId,@longitude,@latitude,@LocationFullUrl,@PickingPoint,@CreatedBy",
                       new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.OrderId }
                     , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.longitude == null ? (object)DBNull.Value : customerOrderLocationViewModel.longitude }
                     , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.latitude == null ? (object)DBNull.Value : customerOrderLocationViewModel.latitude }
                     , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.LocationFullUrl == null ? (object)DBNull.Value : customerOrderLocationViewModel.LocationFullUrl }
                     , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.PickingPoint == null ? (object)DBNull.Value : customerOrderLocationViewModel.PickingPoint }
                     , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.CreatedBy }

                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DriverContiniousLocationAdd([FromBody] CustomerOrderLocationViewModel customerOrderLocationViewModel)
        {
            try
            {

                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("DriverContiniousLocationAdd @latitude,@longitude,@LocationFullUrl,@DriverId,@OrderId",
                      new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.latitude == null ? (object)DBNull.Value : customerOrderLocationViewModel.latitude }
                     , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.longitude == null ? (object)DBNull.Value : customerOrderLocationViewModel.longitude }
                     , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.LocationFullUrl == null ? (object)DBNull.Value : customerOrderLocationViewModel.LocationFullUrl }
                     , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.DriverId }
                     , new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.OrderId }                     

                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DriverContiniousLocationList(CustomerOrderLocationViewModel customerOrderLocationViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("DriverContiniousLocationList @DriverId"
                   , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.DriverId }).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public HttpResponseMessage DriverLastContiniosLocation()
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("DriverLastContiniosLocation"
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
