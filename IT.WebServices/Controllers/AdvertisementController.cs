using IT.Core.ViewModels;
using IT.Repository;
using IT.WebServices.Models;
using Newtonsoft.Json;
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
    public class AdvertisementController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var advertisementList = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationAll"
                    ).ToList();


                if (advertisementList.Count < 1)
                {
                    userRepsonse.Success("[]");
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(advertisementList));
                }

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            try
            {

                CustomerNotification customerNotification = new CustomerNotification();

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

                        if (file1.Headers.ContentDisposition.FileName != null && file1.Headers.ContentDisposition.FileName != "")
                        {
                            customerNotification.ImageUrl = thisFileName;
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

                customerNotification.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                customerNotification.MessageTitle = HttpContext.Current.Request["MessageTitle"];
                customerNotification.MessageDescription = HttpContext.Current.Request["MessageDescription"];
              
                //CustomerNotification

                var AdvertisementAdd = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationAdd @ImageUrl, @MessageTitle, @MessageDescription,@CreatedBy",
                          new SqlParameter("ImageUrl", System.Data.SqlDbType.NVarChar) { Value = customerNotification.ImageUrl == null ? (object)DBNull.Value : customerNotification.ImageUrl }
                        , new SqlParameter("MessageTitle", System.Data.SqlDbType.NVarChar) { Value = customerNotification.MessageTitle == null ? (Object)DBNull.Value : customerNotification.MessageTitle }
                        , new SqlParameter("MessageDescription", System.Data.SqlDbType.NVarChar) { Value = customerNotification.MessageDescription == null ? (Object)DBNull.Value : customerNotification.MessageDescription }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerNotification.CreatedBy }
                          ).FirstOrDefault();
                 userRepsonse.Success((new JavaScriptSerializer()).Serialize(AdvertisementAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public async Task<HttpResponseMessage> Update()
        {
            try
            {

                CustomerNotification customerNotification = new CustomerNotification();

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

                        if (file1.Headers.ContentDisposition.FileName != null && file1.Headers.ContentDisposition.FileName != "")
                        {
                            customerNotification.ImageUrl = thisFileName;
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

                customerNotification.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                customerNotification.Id = Convert.ToInt32(HttpContext.Current.Request["Id"]);
                customerNotification.MessageTitle = HttpContext.Current.Request["MessageTitle"];
                customerNotification.MessageDescription = HttpContext.Current.Request["MessageDescription"];

                //CustomerNotification

                var AdvertisementAdd = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationUpdate @Id,@ImageUrl, @MessageTitle, @MessageDescription,@UpdatedBy,@IsActive",
                          new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerNotification.Id }
                        , new SqlParameter("ImageUrl", System.Data.SqlDbType.NVarChar) { Value = customerNotification.ImageUrl == null ? (object)DBNull.Value : customerNotification.ImageUrl }
                        , new SqlParameter("MessageTitle", System.Data.SqlDbType.NVarChar) { Value = customerNotification.MessageTitle == null ? (Object)DBNull.Value : customerNotification.MessageTitle }
                        , new SqlParameter("MessageDescription", System.Data.SqlDbType.NVarChar) { Value = customerNotification.MessageDescription == null ? (Object)DBNull.Value : customerNotification.MessageDescription }
                        , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = customerNotification.UpdatedBy }
                        , new SqlParameter("IsActive", System.Data.SqlDbType.Bit) { Value = customerNotification.IsActive }
                          ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(AdvertisementAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Edit(SearchViewModel searchViewModel)
        {
            try
            {

                var advertisementList = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationGetById @Id",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }

                    ).FirstOrDefault();

                       userRepsonse.Success((new JavaScriptSerializer()).Serialize(advertisementList));                
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
