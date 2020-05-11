using IT.Core.ViewModels;
using IT.Repository;
using IT.WebServices.MISC;
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

        readonly string contentType = "application/json";

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

                var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
              , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
              , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "AWFCompany" }
              ).ToList();

                AwfCompanyInfo.uploadDocumentsViewModels = Documents;
                AwfCompanyInfo.updateReasonDescriptionViewModels = updatereason;

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
                companyViewModel.ReasonDescription = HttpContext.Current.Request["ReasonDescription"];

                var CompanyUpdate = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyUpdateAWFuel @Id, @Name,@OwnerRepresentaive, @Street, @Postcode, @City, @State, @Country, @Phone, @Cell, @Email, @Web, @UpdatedBy,@Address,@TRN,@LogoUrl",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyViewModel.Id }
                   , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Name ?? (object)DBNull.Value }
                   , new SqlParameter("OwnerRepresentaive", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.OwnerRepresentaive ?? (object)DBNull.Value }
                   , new SqlParameter("Street", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Street ?? (Object)DBNull.Value }
                   , new SqlParameter("Postcode", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Postcode ?? (Object)DBNull.Value }
                   , new SqlParameter("City", System.Data.SqlDbType.VarChar) { Value = companyViewModel.City ?? (Object)DBNull.Value }
                   , new SqlParameter("State", System.Data.SqlDbType.VarChar) { Value = companyViewModel.State ?? (Object)DBNull.Value }
                   , new SqlParameter("Country", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Country ?? (Object)DBNull.Value }
                   , new SqlParameter("Phone", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Phone ?? (Object)DBNull.Value }
                   , new SqlParameter("Cell", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Cell ?? (Object)DBNull.Value }
                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Email ?? (Object)DBNull.Value }
                   , new SqlParameter("Web", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Web ?? (Object)DBNull.Value }
                   , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = 1 }
                   , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Address ?? (object)DBNull.Value }
                   , new SqlParameter("TRN", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.TRN ?? (object)DBNull.Value }
                   , new SqlParameter("LogoUrl", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.LogoUrl ?? (object)DBNull.Value }

                   ).FirstOrDefault();

                if (companyViewModel.ReasonDescription != null)
                {
                    var updateReason = new UpdateReason();
                    if (companyViewModel.Id > 0)
                    {
                        var updateReasonDes = new UpdateReasonDescriptionViewModel {
                            Id = companyViewModel.Id,
                            ReasonDescription = companyViewModel.ReasonDescription,
                            CreatedBy = companyViewModel.UpdatedBy,
                            Flag = "AWFCompany"
                        };
                    var result = updateReason.Add(updateReasonDes);
                    }
                }


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
