using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class EmployeeController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All(int Id)
        {
            try
            {
                var employeeList = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("EmployeeAll @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employeeList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;

                //access files  
                IList<HttpContent> files = provider.Files;

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string DDTT = unixTimestamp.ToString();

                for (int i = 0; i < files.Count; i++)
                {

                    HttpContent file1 = files[i];

                    var thisFileName = DDTT + file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = String.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = String.Empty;
                    string URL = String.Empty;
                    string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                    if (formData["ClientDocs"] == "ClientDocs")
                    {
                        var path = HttpRuntime.AppDomainAppPath;
                        directoryName = System.IO.Path.Combine(path, "ClientDocument");
                        filename = System.IO.Path.Combine(directoryName, thisFileName);

                        //Deletion exists file  
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }

                        if (file1.Headers.ContentDisposition.Name == "\"PassportFront\"" || file1.Headers.ContentDisposition.DispositionType == "PassportFront")
                        {
                            employeeViewModel.PassportFront = thisFileName;
                        }
                        else if(file1.Headers.ContentDisposition.Name == "\"PassportBack\"" || file1.Headers.ContentDisposition.DispositionType == "PassportBack")
                        {
                            employeeViewModel.PassportBack = thisFileName;
                        }
                        string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                        URL = DocsPath + thisFileName;

                    }

                    //Directory.CreateDirectory(@directoryName);  
                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        //close file  
                        file.Close();
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("DocsUrl", URL);
                }

                employeeViewModel.Name = HttpContext.Current.Request["Name"];
                employeeViewModel.Designation = Convert.ToInt32(HttpContext.Current.Request["Designation"]);
                employeeViewModel.Contact = HttpContext.Current.Request["Contact"];
                employeeViewModel.Email = HttpContext.Current.Request["Email"];
                employeeViewModel.Facebook = HttpContext.Current.Request["Facebook"];
                employeeViewModel.Comments = HttpContext.Current.Request["Comments"];
                employeeViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                employeeViewModel.UID = HttpContext.Current.Request["UID"];
                if (HttpContext.Current.Request["BasicSalary"] != null)
                {
                    employeeViewModel.BasicSalary = Convert.ToDecimal(HttpContext.Current.Request["BasicSalary"]);
                }
                if (HttpContext.Current.Request["CompanyId"] != null)
                {
                    employeeViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                }

                var employeeAdd = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().WriteStoredProcedure("EmployeeAdd @Name,@Designation,@Contact,@Email,@Facebook,@Comments,@CreatedBy,@PassportFront,@PassportBack, @UID,@BasicSalary,@CompanyId",
                  new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = employeeViewModel.Name == null ? (object)DBNull.Value : employeeViewModel.Name }
                , new SqlParameter("Designation", System.Data.SqlDbType.Int) { Value = employeeViewModel.Designation }
                , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Contact == null ? (object)DBNull.Value : employeeViewModel.Contact }
                , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Email == null ? (object)DBNull.Value : employeeViewModel.Email }
                , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Facebook == null ? (object)DBNull.Value : employeeViewModel.Facebook }
                , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Comments == null ? (object)DBNull.Value : employeeViewModel.Comments }
                , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = employeeViewModel.CreatedBy }
                , new SqlParameter("PassportFront", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.PassportFront == null ? (object)DBNull.Value : employeeViewModel.PassportFront }
                , new SqlParameter("PassportBack", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.PassportBack == null ? (object)DBNull.Value : employeeViewModel.PassportBack }
                , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.UID == null ? (object)DBNull.Value : employeeViewModel.UID }
                , new SqlParameter("BasicSalary", System.Data.SqlDbType.Money) { Value = employeeViewModel.BasicSalary == 0 ? (object)DBNull.Value : employeeViewModel.BasicSalary }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = employeeViewModel.CompanyId }

                );
                userRepsonse.Success(new JavaScriptSerializer().Serialize(employeeAdd));
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
                var employee = unitOfWork.GetRepositoryInstance<EmployeeViewModel>().ReadStoredProcedure("EmployeeById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                   , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Employee" }
                   ).ToList();

                employee.uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employee));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(EmployeeViewModel employeeViewModel)
        {

            try
            {

                var employeeUpdate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("EmployeeUpdate @Id, @Name,@Designation,@Contact,@Email,@Facebook,@Comments,@BasicSalary,@UpdayedBy",
                  new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = employeeViewModel.Id }
                , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = employeeViewModel.Name == null ? (object)DBNull.Value : employeeViewModel.Name }
                , new SqlParameter("Designation", System.Data.SqlDbType.Int) { Value = employeeViewModel.Designation }
                , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Contact == null ? (object)DBNull.Value : employeeViewModel.Contact }
                , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Email == null ? (object)DBNull.Value : employeeViewModel.Email }
                , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Facebook == null ? (object)DBNull.Value : employeeViewModel.Facebook }
                , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = employeeViewModel.Comments == null ? (object)DBNull.Value : employeeViewModel.Comments }
                , new SqlParameter("BasicSalary", System.Data.SqlDbType.Money) { Value = employeeViewModel.BasicSalary == 0 ? (object)DBNull.Value : employeeViewModel.BasicSalary }
                , new SqlParameter("UpdayedBy", System.Data.SqlDbType.Int) { Value = employeeViewModel.UpdatedBy }

                );
                userRepsonse.Success(new JavaScriptSerializer().Serialize(employeeUpdate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        [HttpPost]
        public HttpResponseMessage Delete(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var employee = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("EmployeeDelete @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = employeeViewModel.Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(employee.Result));
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
