using IT.Core.ViewModels;
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
    public class AWDepartmentController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {

                var AWDepartmentLists = unitOfWork.GetRepositoryInstance<AWDepartments>().ReadStoredProcedure("AWDepartmentsAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AWDepartmentLists));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }


        [HttpPost]
        public HttpResponseMessage Add(SearchViewModel searchViewModel)
        {
            try
            {

                var AWDepartmentLists = unitOfWork.GetRepositoryInstance<AWDepartments>().ReadStoredProcedure("AWDepartmentsAdd @Departments",
                      new SqlParameter("Departments", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.Title }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AWDepartmentLists));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }



        [HttpPost]
        public HttpResponseMessage AWFuelDepartmentDetailsByDepartmentId(SearchViewModel searchViewModel)
        {
            try
            {

                var AWDepartmentLists = unitOfWork.GetRepositoryInstance<AWFuelDepartmentDetailsViewModel>().ReadStoredProcedure("AWFuelDepartmentDetailsByDepartmentId @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).ToList();

               
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(AWDepartmentLists));
                
              
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

    }
}
