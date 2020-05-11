using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
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
    public class FuelPricesController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var fuelPricesData = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPricesAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(fuelPricesData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Add([FromBody] FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var FuelPricesAdd = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPricesAdd @Price, @Unit, @Month, @CreatedBy,@ProductId",
                 new SqlParameter("Price", System.Data.SqlDbType.Money) { Value = fuelPricesViewModel.Price }
               , new SqlParameter("Unit", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Unit }
               , new SqlParameter("Month", System.Data.SqlDbType.NVarChar) { Value = fuelPricesViewModel.Month }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.CreatedBy }
               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.ProductId }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(FuelPricesAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Update([FromBody] FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var FuelPricesUpdate = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPricesUpdate @Id, @Price, @Unit, @Month, @CreatedBy,@ProductId",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Id }
               , new SqlParameter("Price", System.Data.SqlDbType.Money) { Value = fuelPricesViewModel.Price }
               , new SqlParameter("Unit", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Unit }
               , new SqlParameter("Month", System.Data.SqlDbType.NVarChar) { Value = fuelPricesViewModel.Month ?? System.DateTime.Now.Month.ToString()}
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.CreatedBy }
               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.ProductId }
                 ).FirstOrDefault();

                if(fuelPricesViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    var res = updateReason.Add(fuelPricesViewModel.updateReasonDescriptionViewModel);
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(FuelPricesUpdate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Edit(FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var fuelPricesData = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPricesEdit @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Id }
                    ).FirstOrDefault();

                 var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Id }
                , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "FuelPrice" }
                ).ToList();

                fuelPricesData.updateReasonDescriptionViewModels = updatereason;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(fuelPricesData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage FuelPricesTopOne()
        {
            try
            {
                var fuelPricesData = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPricesTopOne"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(fuelPricesData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage FuelPriceUpdateStatus(FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var fuelPricesData = unitOfWork.GetRepositoryInstance<FuelPricesViewModel>().ReadStoredProcedure("FuelPriceUpdateStatus @Id,@IsActive",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = fuelPricesViewModel.Id }
                    ,new SqlParameter("IsActive", System.Data.SqlDbType.Bit) { Value = fuelPricesViewModel.IsActive }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(fuelPricesData));
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
 