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
    public class ProductUnitController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";


        [HttpPost]
        public HttpResponseMessage Add([FromBody] ProductUnitViewModel productUnitViewModel)
        {
            try
            {
                SingleIntegerValueResult d = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProductUnitAdd @Name,@CreatedBy",
                      new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = productUnitViewModel.Name == null ? (object)DBNull.Value : productUnitViewModel.Name }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = productUnitViewModel.CreatedBy }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(d.Result));
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
                var ProductUnitList = unitOfWork.GetRepositoryInstance<ProductUnitViewModel>().ReadStoredProcedure("ProductUnitAll"
                     ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ProductUnitList));
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
                var productUnit = unitOfWork.GetRepositoryInstance<ProductUnitViewModel>().ReadStoredProcedure("ProductUnitById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(productUnit));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                var productUnit = unitOfWork.GetRepositoryInstance<ProductUnitViewModel>().ReadStoredProcedure("ProductUnitDisable @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(productUnit));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(ProductUnitViewModel productUnitViewModel)
        {
            try
            {

                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProductUnitUpdate @Id,@Name,@UpdatedBy"
                 , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = productUnitViewModel.Id }
                 , new SqlParameter("Name", System.Data.SqlDbType.NVarChar) { Value = productUnitViewModel.Name == null ? (object)DBNull.Value : productUnitViewModel.Name }
                 , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = productUnitViewModel.UpdatedBy }

                 ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
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
