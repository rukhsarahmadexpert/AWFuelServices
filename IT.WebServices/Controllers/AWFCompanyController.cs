using IT.Core.ViewModels;
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
    public class AWFCompanyController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Edit(SearchViewModel searchViewModel)
        {
            try
            {
                var AwfCompanyInfo = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyByIdAWFuel @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                ).FirstOrDefault();

                var Documents = new List<UploadDocumentsViewModel>();
                //var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                //, new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                //, new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Company" }
                //).ToList();

                AwfCompanyInfo.uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AwfCompanyInfo));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Update()
        {
            try
            {
                CompanyViewModel companyViewModel = new CompanyViewModel();

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

                        if (file1.Headers.ContentDisposition.Name == "\"LogoUrl\"" || file1.Headers.ContentDisposition.DispositionType == "LogoUrl")
                        {
                            companyViewModel.LogoUrl = thisFileName;
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

                companyViewModel.Id = Convert.ToInt32(HttpContext.Current.Request["Id"]);
                companyViewModel.Name = HttpContext.Current.Request["Name"];
                companyViewModel.OwnerRepresentaive = HttpContext.Current.Request["OwnerRepresentaive"];
                companyViewModel.Street = HttpContext.Current.Request["Street"];
                companyViewModel.Postcode = HttpContext.Current.Request["Postcode"];
                companyViewModel.City = HttpContext.Current.Request["City"];
                companyViewModel.State = HttpContext.Current.Request["State"];
                companyViewModel.Country = HttpContext.Current.Request["Country"];
                companyViewModel.Phone = HttpContext.Current.Request["Phone"];
                companyViewModel.Cell = HttpContext.Current.Request["Cell"];
                companyViewModel.Email = HttpContext.Current.Request["Email"];
                companyViewModel.Web = HttpContext.Current.Request["Web"];
                companyViewModel.UpdatedBy = Convert.ToInt32(HttpContext.Current.Request["UpdatedBy"]);
                companyViewModel.Address = HttpContext.Current.Request["Address"];
                companyViewModel.TRN = HttpContext.Current.Request["TRN"];

                var CompanyUpdate = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyUpdateAWFuel @Id, @Name,@OwnerRepresentaive, @Street, @Postcode, @City, @State, @Country, @Phone, @Cell, @Email, @Web, @UpdatedBy,@Address,@TRN,@LogoUrl",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyViewModel.Id }
                   , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Name == null ? (object)DBNull.Value : companyViewModel.Name }
                   , new SqlParameter("OwnerRepresentaive", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.OwnerRepresentaive == null ? (object)DBNull.Value : companyViewModel.OwnerRepresentaive }
                   , new SqlParameter("Street", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Street == null ? (Object)DBNull.Value : companyViewModel.Street }
                   , new SqlParameter("Postcode", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Postcode == null ? (Object)DBNull.Value : companyViewModel.Postcode }
                   , new SqlParameter("City", System.Data.SqlDbType.VarChar) { Value = companyViewModel.City == null ? (Object)DBNull.Value : companyViewModel.City }
                   , new SqlParameter("State", System.Data.SqlDbType.VarChar) { Value = companyViewModel.State == null ? (Object)DBNull.Value : companyViewModel.State }
                   , new SqlParameter("Country", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Country == null ? (Object)DBNull.Value : companyViewModel.Country }
                   , new SqlParameter("Phone", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Phone == null ? (Object)DBNull.Value : companyViewModel.Phone }
                   , new SqlParameter("Cell", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Cell == null ? (Object)DBNull.Value : companyViewModel.Cell }
                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Email == null ? (Object)DBNull.Value : companyViewModel.Email }
                   , new SqlParameter("Web", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Web == null ? (Object)DBNull.Value : companyViewModel.Web }
                   , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = 1 }
                   , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Address == null ? (object)DBNull.Value : companyViewModel.Address }
                   , new SqlParameter("TRN", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.TRN == null ? (object)DBNull.Value : companyViewModel.TRN }
                   , new SqlParameter("LogoUrl", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.LogoUrl == null ? (object)DBNull.Value : companyViewModel.LogoUrl }

                   ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyUpdate));
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
