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
    public class ProductController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var ProductList = unitOfWork.GetRepositoryInstance<ProductViewModel>().ReadStoredProcedure("ProductAll"
                     ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ProductList));
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
                var product = unitOfWork.GetRepositoryInstance<ProductViewModel>().ReadStoredProcedure("ProductById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(product));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Add([FromBody] ProductViewModel productViewModel)
        {
            try
            {
                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProductAdd @Name,@Description,@UPrice,@Unit,@CreatedBy",
                       new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = productViewModel.Name ?? (object)DBNull.Value }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = productViewModel.Description ?? (object)DBNull.Value }
                     , new SqlParameter("UPrice", System.Data.SqlDbType.Money) { Value = productViewModel.UPrice }
                     , new SqlParameter("Unit", System.Data.SqlDbType.Int) { Value = productViewModel.Unit }
                     , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = productViewModel.CreatedBy }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Update([FromBody] ProductViewModel productViewModel)
        {
            try
            {
                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProductUpdate @Id, @Name,@Description,@Unit,@UpdatedBy",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = productViewModel.Id }
                     , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = productViewModel.Name == null ? (object)DBNull.Value : productViewModel.Name }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = productViewModel.Description == null ? (object)DBNull.Value : productViewModel.Description }
                     , new SqlParameter("Unit", System.Data.SqlDbType.Int) { Value = productViewModel.Unit }
                     , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = productViewModel.UpdatedBy }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(res.Result));
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
                var product = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("ProductDisable @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(product.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage GetProdctFromLPODetailsByLPOID(int Id)
        {
            try
            {
                var productByLPO = unitOfWork.GetRepositoryInstance<ProductViewModel>().ReadStoredProcedure("GetProductIDFromLPODetailsBYLOPId @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(productByLPO));
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
