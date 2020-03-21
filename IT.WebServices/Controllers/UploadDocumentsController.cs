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
    public class UploadDocumentsController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public async Task<HttpResponseMessage> UploadDocumentsAdd()
        {
            UploadDocumentsViewModel uploadDocumentsViewModel = new UploadDocumentsViewModel();
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

                        if (file1.Headers.ContentDisposition.Name == "\"FileUrl\"" || file1.Headers.ContentDisposition.DispositionType == "FileUrl")
                        {
                            uploadDocumentsViewModel.FileUrl = thisFileName;
                        }                      
                        string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                        URL = DocsPath + thisFileName;
                    }

                    //Directory.CreateDirectory(@directoryName);  
                    if (filename != null && filename != "")
                    {
                        using (Stream file = File.OpenWrite(filename))
                        {
                            input.CopyTo(file);
                            //close file  
                            file.Close();
                        }
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("DocsUrl", URL);
                }

                uploadDocumentsViewModel.InvoiceId = Convert.ToInt32(HttpContext.Current.Request["InvoiceId"]);
                uploadDocumentsViewModel.QuotationId = Convert.ToInt32(HttpContext.Current.Request["QuotationId"]);
                uploadDocumentsViewModel.BillId = Convert.ToInt32(HttpContext.Current.Request["BillId"]);
                uploadDocumentsViewModel.DriverId = Convert.ToInt32(HttpContext.Current.Request["DriverId"]);
                uploadDocumentsViewModel.EmployeeId = Convert.ToInt32(HttpContext.Current.Request["EmployeeId"]);
                uploadDocumentsViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                uploadDocumentsViewModel.BookingId = Convert.ToInt32(HttpContext.Current.Request["BookingId"]);
                uploadDocumentsViewModel.OrderId = Convert.ToInt32(HttpContext.Current.Request["OrderId"]);
                uploadDocumentsViewModel.VehicleId = Convert.ToInt32(HttpContext.Current.Request["VehicleId"]);
                uploadDocumentsViewModel.StorageId = Convert.ToInt32(HttpContext.Current.Request["StorageId"]);
                uploadDocumentsViewModel.LPOId = Convert.ToInt32(HttpContext.Current.Request["LPOId"]);
                uploadDocumentsViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                uploadDocumentsViewModel.FilesName = HttpContext.Current.Request["FilesName"];
              
                var uploadDocAdd = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsAdd @FileUrl, @InvoiceId, @QuotationId, @BillId, @DriverId, @EmployeeId,@CompanyId,@BookingId,@OrderId,@VehicleId,@StorageId,@CreatedBy,@FilesName,@LPOId",
                 new SqlParameter("FileUrl", System.Data.SqlDbType.NVarChar) { Value = uploadDocumentsViewModel.FileUrl }
               , new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.InvoiceId }
               , new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.QuotationId }
               , new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.BillId }
               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.DriverId }
               , new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.EmployeeId }
               , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.CompanyId}
               , new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.BookingId }
               , new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.OrderId }
               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.VehicleId }
               , new SqlParameter("StorageId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.StorageId  }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.CreatedBy }
               , new SqlParameter("FilesName", System.Data.SqlDbType.NVarChar) { Value = uploadDocumentsViewModel.FilesName }
               , new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.LPOId }
    
                 ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(uploadDocAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage UploadDocumentsDeleteByid(UploadDocumentsViewModel uploadDocumentsViewModel)
        {
            try
            {
                var RemaingList = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsDeleteByid @Id,@InvoiceId, @QuotationId, @BillId, @DriverId, @EmployeeId,@CompanyId,@BookingId,@OrderId,@VehicleId,@StorageId",
                 new SqlParameter("Id", System.Data.SqlDbType.NVarChar) { Value = uploadDocumentsViewModel.Id }
               , new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.InvoiceId }
               , new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.QuotationId }
               , new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.BillId }
               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.DriverId }
               , new SqlParameter("EmployeeId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.EmployeeId }
               , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.CompanyId }
               , new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.BookingId }
               , new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.OrderId }
               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.VehicleId }
               , new SqlParameter("StorageId", System.Data.SqlDbType.Int) { Value = uploadDocumentsViewModel.StorageId }
              ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(RemaingList));
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
