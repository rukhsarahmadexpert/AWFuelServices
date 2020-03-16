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
    public class DesignationController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var designation = unitOfWork.GetRepositoryInstance<DesignationViewModel>().ReadStoredProcedure("DesignatinAll @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = 1 }
                    ).ToList();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designation));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Add(DesignationViewModel designationViewModel)
        {
            try
            {
                var designationAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DesignationAdd @Designation,@CompanyId,@CreatedBy",
                    new SqlParameter("Designation", System.Data.SqlDbType.NVarChar) { Value = designationViewModel.Designation },
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = 1 },
                    new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = 1 }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designationAdd.Result));
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
                var designation = unitOfWork.GetRepositoryInstance<DesignationViewModel>().ReadStoredProcedure("DesignationGetbyId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.NVarChar) { Value = Id }
                    ).ToList();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designation));
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
                var designation = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DesignationDelete @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designation.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(DesignationViewModel designationViewModel)
        {
            try
            {
                var designationAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DesignationUpdate @Id,@Designation,@UpdatedBy",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = designationViewModel.Id },
                    new SqlParameter("Designation", System.Data.SqlDbType.NVarChar) { Value = designationViewModel.Designation },                   
                    new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = designationViewModel.CreatedBy }
                    ).FirstOrDefault();

                userRepsonse.Success(new JavaScriptSerializer().Serialize(designationAdd.Result));
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
