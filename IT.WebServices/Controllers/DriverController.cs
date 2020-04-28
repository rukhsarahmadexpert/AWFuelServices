using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
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
using IT.WebServices.Models;
using Newtonsoft.Json;

namespace IT.WebServices.Controllers
{
    public class DriverController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {

                var driverList = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverAll @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }

                    ).ToList();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    driverList = driverList.Where(
                                                    x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                                         x.Nationality.ToLower().Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                                         x.Contact.Contains(pagingparametermodel.SerachKey)
                                                    ).ToList();
                }

                int count = driverList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = driverList.OrderByDescending(x=>x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                if (driverList.Count < 1)
                {
                    userRepsonse.Success("[]");
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                }

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            try
            {
                DriverViewModel driverViewModel = new DriverViewModel();

                //if (!Request.Content.IsMimeMultipartContent())
                //{  
                //    userRepsonse.BadRequest("media type not supported Note: only MimeMultipartContent accepted");
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);

                //}

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

                        if (file1.Headers.ContentDisposition.Name == "\"DriverImageUrl\"" || file1.Headers.ContentDisposition.DispositionType == "DriverImageUrl")
                        {
                            driverViewModel.DriverImageUrl = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"DrivingLicenseBack\"" || file1.Headers.ContentDisposition.DispositionType == "DrivingLicenseBack")
                        {
                            driverViewModel.DrivingLicenseBack = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"DrivingLicenseFront\"" || file1.Headers.ContentDisposition.DispositionType == "DrivingLicenseFront")
                        {
                            driverViewModel.DrivingLicenseFront = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"PassportCopy\"" || file1.Headers.ContentDisposition.DispositionType == "PassportCopy")
                        {
                            driverViewModel.PassportCopy = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"VisaCopy\"" || file1.Headers.ContentDisposition.DispositionType == "VisaCopy")
                        {
                            driverViewModel.VisaCopy = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"PassportBack\"" || file1.Headers.ContentDisposition.DispositionType == "PassportBack")
                        {
                            driverViewModel.PassportBack = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"IDUAECopyFront\"" || file1.Headers.ContentDisposition.DispositionType == "IDUAECopyFront")
                        {
                            driverViewModel.IDUAECopyFront = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"IDUAECopyBack\"" || file1.Headers.ContentDisposition.DispositionType == "IDUAECopyBack")
                        {
                            driverViewModel.IDUAECopyBack = thisFileName;
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


                //driverViewModel.Name = HttpContext.Current.Request["Name"]; ;
                driverViewModel.UID = HttpContext.Current.Request["UID"];
                driverViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                driverViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);

                string listofIDs = HttpContext.Current.Request["LicenseTypes"];

                if (listofIDs == "[1]")
                {
                    List<int> myValues = new List<int>() { 1 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[2]")
                {
                    List<int> myValues = new List<int>() { 2 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[3]")
                {
                    List<int> myValues = new List<int>() { 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,2]" || listofIDs == "[2,1]")
                {
                    List<int> myValues = new List<int>() { 1, 2 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,3]" || listofIDs == "[3,1]")
                {
                    List<int> myValues = new List<int>() { 1, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[2,3]" || listofIDs == "[3,2]")
                {
                    List<int> myValues = new List<int>() { 2, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,2,3]" || listofIDs == "[3,2,1]" || listofIDs == "[2,1,3]" || listofIDs == "[1,3,2]" || listofIDs == "[2,3,1]")
                {
                    List<int> myValues = new List<int>() { 1, 2, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }

                driverViewModel.LicenseExpiry = HttpContext.Current.Request["LicenseExpiry"];
                driverViewModel.Contact = HttpContext.Current.Request["Contact"];
                driverViewModel.Email = HttpContext.Current.Request["Email"];
                driverViewModel.Name = HttpContext.Current.Request["FullName"];
                driverViewModel.Facebook = HttpContext.Current.Request["Facebook"];
                driverViewModel.Comments = HttpContext.Current.Request["Comments"];
                driverViewModel.Nationality = HttpContext.Current.Request["Nationality"];
                driverViewModel.LicenseExpiry = HttpContext.Current.Request["DrivingLicenseExpiryDate"];

                // driverViewModel.DrivingLicenseExpiryDate = Convert.ToDateTime(driverViewModel.LicenseExpiry == null ? System.DateTime.Now.ToShortDateString() : driverViewModel.LicenseExpiry);

                int a = 0, b = 0, c = 0;
                if (driverViewModel.LicenseTypes != null && driverViewModel.LicenseTypes.Count > 0)
                {
                    for (int i = 0; i < driverViewModel.LicenseTypes.Count; i++)
                    {
                        if (driverViewModel.LicenseTypes[i] == 1)
                        {
                            a = driverViewModel.LicenseTypes[i];
                        }
                        else if (driverViewModel.LicenseTypes[i] == 2)
                        {
                            b = driverViewModel.LicenseTypes[i];
                        }
                        else
                        {
                            c = driverViewModel.LicenseTypes[i];
                        }
                    }
                }
                var userIsAlreadyAvailible = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("IsDriverEmailAvailible @Email"
                    , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Email }
                   ).FirstOrDefault();
                if (userIsAlreadyAvailible.Result > 0)
                {
                    userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("Email Already Availible"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {

                    var Res = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverAdd @Name, @Contact, @Email, @Facebook, @Comments, @PassportCopy, @VisaCopy, @IDUAECopyFront,@IDUAECopyBack,@DrivingLicenseFront, @DrivingLicenseBack,@Nationality, @DrivingLicenseExpiryDate,@CompanyId, @CreatedBy,@UID,@LicenseType,@LicenseType2,@LicenseType3,@DriverImageUrl,@PassportBack",
                          new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = driverViewModel.Name == null ? (object)DBNull.Value : driverViewModel.Name }
                        , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Contact == null ? (Object)DBNull.Value : driverViewModel.Contact }
                        , new SqlParameter("Email   ", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Email == null ? (Object)DBNull.Value : driverViewModel.Email }
                        , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Facebook == null ? (Object)DBNull.Value : driverViewModel.Facebook }
                        , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Comments == null ? (Object)DBNull.Value : driverViewModel.Comments }
                        , new SqlParameter("PassportCopy", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.PassportCopy == null ? (Object)DBNull.Value : driverViewModel.PassportCopy }
                        , new SqlParameter("VisaCopy", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.VisaCopy == null ? (Object)DBNull.Value : driverViewModel.VisaCopy }
                        , new SqlParameter("IDUAECopyFront", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.IDUAECopyFront == null ? (Object)DBNull.Value : driverViewModel.IDUAECopyFront }
                        , new SqlParameter("IDUAECopyBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.IDUAECopyBack == null ? (Object)DBNull.Value : driverViewModel.IDUAECopyBack }
                        , new SqlParameter("DrivingLicenseFront", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DrivingLicenseFront == null ? (Object)DBNull.Value : driverViewModel.DrivingLicenseFront }
                        , new SqlParameter("DrivingLicenseBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DrivingLicenseBack == null ? (Object)DBNull.Value : driverViewModel.DrivingLicenseBack }
                        , new SqlParameter("Nationality", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Nationality == null ? (Object)DBNull.Value : driverViewModel.Nationality }
                        , new SqlParameter("DrivingLicenseExpiryDate", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.LicenseExpiry == null ? (Object)DBNull.Value : driverViewModel.LicenseExpiry }
                        , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = driverViewModel.CompanyId }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = driverViewModel.CreatedBy }
                        , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.UID == null ? (object)DBNull.Value : driverViewModel.UID }
                        , new SqlParameter("LicenseType", System.Data.SqlDbType.Int) { Value = a }
                        , new SqlParameter("LicenseType2", System.Data.SqlDbType.Int) { Value = b }
                        , new SqlParameter("LicenseType3", System.Data.SqlDbType.Int) { Value = c }
                        , new SqlParameter("DriverImageUrl", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DriverImageUrl == null ? (object)DBNull.Value : driverViewModel.DriverImageUrl }
                        , new SqlParameter("PassportBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.PassportBack == null ? (object)DBNull.Value : driverViewModel.PassportBack }
                        ).FirstOrDefault();
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));                
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Edit(DriverViewModel driverViewModel)
        {
            try
            {
                var driverData = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverById @Id, @CompanyId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = driverViewModel.CompanyId }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(driverData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public async Task<HttpResponseMessage> Update()
        {
            try
            {
                DriverViewModel driverViewModel = new DriverViewModel();


                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;

                //access files  
                IList<HttpContent> files = provider.Files;

                if (files.Count > 0)
                {

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

                            if (file1.Headers.ContentDisposition.Name == "\"DriverImageUrl\"" || file1.Headers.ContentDisposition.DispositionType == "DriverImageUrl")
                            {
                                driverViewModel.DriverImageUrl = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"DrivingLicenseBack\"" || file1.Headers.ContentDisposition.DispositionType == "DrivingLicenseBack")
                            {
                                driverViewModel.DrivingLicenseBack = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"DrivingLicenseFront\"" || file1.Headers.ContentDisposition.DispositionType == "DrivingLicenseFront")
                            {
                                driverViewModel.DrivingLicenseFront = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"PassportCopy\"" || file1.Headers.ContentDisposition.DispositionType == "PassportCopy")
                            {
                                driverViewModel.PassportCopy = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"VisaCopy\"" || file1.Headers.ContentDisposition.DispositionType == "VisaCopy")
                            {
                                driverViewModel.VisaCopy = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"PassportBack\"" || file1.Headers.ContentDisposition.DispositionType == "PassportBack")
                            {
                                driverViewModel.PassportBack = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"IDUAECopyFront\"" || file1.Headers.ContentDisposition.DispositionType == "IDUAECopyFront")
                            {
                                driverViewModel.IDUAECopyFront = thisFileName;
                            }
                            else if (file1.Headers.ContentDisposition.Name == "\"IDUAECopyBack\"" || file1.Headers.ContentDisposition.DispositionType == "IDUAECopyBack")
                            {
                                driverViewModel.IDUAECopyBack = thisFileName;
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
                }

                driverViewModel.Id = Convert.ToInt32(HttpContext.Current.Request["Id"]);
                driverViewModel.Name = HttpContext.Current.Request["FullName"];
                driverViewModel.UID = HttpContext.Current.Request["UID"];
                driverViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                driverViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);

                string listofIDs = HttpContext.Current.Request["LicenseTypes"];
                
                if (listofIDs == "[1]")
                {
                    List<int> myValues = new List<int>() { 1 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[2]")
                {
                    List<int> myValues = new List<int>() { 2 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[3]")
                {
                    List<int> myValues = new List<int>() { 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,2]" || listofIDs == "[2,1]")
                {
                    List<int> myValues = new List<int>() { 1, 2 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,3]" || listofIDs == "[3,1]")
                {
                    List<int> myValues = new List<int>() { 1, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[2,3]" || listofIDs == "[3,2]")
                {
                    List<int> myValues = new List<int>() { 2, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }
                if (listofIDs == "[1,2,3]" || listofIDs == "[3,2,1]" || listofIDs == "[2,1,3]" || listofIDs == "[1,3,2]" || listofIDs == "[2,3,1]")
                {
                    List<int> myValues = new List<int>() { 1, 2, 3 };
                    driverViewModel.LicenseTypes = myValues;
                }

                int a = 0, b = 0, c = 0;
                if (driverViewModel.LicenseTypes != null && driverViewModel.LicenseTypes.Count > 0)
                {
                    for (int i = 0; i < driverViewModel.LicenseTypes.Count; i++)
                    {
                        if (driverViewModel.LicenseTypes[i] == 1)
                        {
                            a = driverViewModel.LicenseTypes[i];
                        }
                        else if (driverViewModel.LicenseTypes[i] == 2)
                        {
                            b = driverViewModel.LicenseTypes[i];
                        }
                        else
                        {
                            c = driverViewModel.LicenseTypes[i];
                        }
                    }
                }



                driverViewModel.LicenseExpiry = HttpContext.Current.Request["LicenseExpiry"];
                driverViewModel.Contact = HttpContext.Current.Request["Contact"];
                driverViewModel.Email = HttpContext.Current.Request["Email"];
                driverViewModel.Facebook = HttpContext.Current.Request["Facebook"];
                driverViewModel.Comments = HttpContext.Current.Request["Comments"];
                driverViewModel.Nationality = HttpContext.Current.Request["Nationality"];
                driverViewModel.DrivingLicenseExpiryDate = HttpContext.Current.Request["DrivingLicenseExpiryDate"];

                // driverViewModel.DrivingLicenseExpiryDate = Convert.ToDateTime(driverViewModel.LicenseExpiry == null ? System.DateTime.Now.ToShortDateString() : driverViewModel.LicenseExpiry);
                
                var Res = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverUpdate @Id, @Name, @Contact, @Email, @Facebook, @Comments, @PassportCopy, @VisaCopy, @IDUAECopyFront,@IDUAECopyBack,@DrivingLicenseFront, @DrivingLicenseBack,@Nationality, @DrivingLicenseExpiryDate,@CompanyId, @CreatedBy,@UID,@LicenseType,@LicenseType2,@LicenseType3,@DriverImageUrl,@PassportBack",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id }
                    , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = driverViewModel.Name == null ? (object)DBNull.Value : driverViewModel.Name }
                    , new SqlParameter("Contact", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Contact == null ? (Object)DBNull.Value : driverViewModel.Contact }
                    , new SqlParameter("Email   ", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Email == null ? (Object)DBNull.Value : driverViewModel.Email }
                    , new SqlParameter("Facebook", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Facebook == null ? (Object)DBNull.Value : driverViewModel.Facebook }
                    , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Comments == null ? (Object)DBNull.Value : driverViewModel.Comments }
                    , new SqlParameter("PassportCopy", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.PassportCopy == null ? (Object)DBNull.Value : driverViewModel.PassportCopy }
                    , new SqlParameter("VisaCopy", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.VisaCopy == null ? (Object)DBNull.Value : driverViewModel.VisaCopy }
                    , new SqlParameter("IDUAECopyFront", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.IDUAECopyFront == null ? (Object)DBNull.Value : driverViewModel.IDUAECopyFront }
                    , new SqlParameter("IDUAECopyBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.IDUAECopyBack == null ? (Object)DBNull.Value : driverViewModel.IDUAECopyBack }
                    , new SqlParameter("DrivingLicenseFront", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DrivingLicenseFront == null ? (Object)DBNull.Value : driverViewModel.DrivingLicenseFront }
                    , new SqlParameter("DrivingLicenseBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DrivingLicenseBack == null ? (Object)DBNull.Value : driverViewModel.DrivingLicenseBack }
                    , new SqlParameter("Nationality", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Nationality == null ? (Object)DBNull.Value : driverViewModel.Nationality }
                    , new SqlParameter("DrivingLicenseExpiryDate", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DrivingLicenseExpiryDate == null ? (Object)DBNull.Value : driverViewModel.DrivingLicenseExpiryDate }
                    , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = driverViewModel.CompanyId }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = driverViewModel.CreatedBy }
                    , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.UID == null ? (object)DBNull.Value : driverViewModel.UID }
                    , new SqlParameter("LicenseType", System.Data.SqlDbType.Int) { Value = a }
                    , new SqlParameter("LicenseType2", System.Data.SqlDbType.Int) { Value = b }
                    , new SqlParameter("LicenseType3", System.Data.SqlDbType.Int) { Value = c }
                    , new SqlParameter("DriverImageUrl", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DriverImageUrl == null ? (object)DBNull.Value : driverViewModel.DriverImageUrl }
                    , new SqlParameter("PassportBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.PassportBack == null ? (object)DBNull.Value : driverViewModel.PassportBack }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteImage(CompanyImages companyImages)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("DriverDeleteImage @Id, @Flage",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyImages.Id }
                     , new SqlParameter("Flage", System.Data.SqlDbType.NVarChar)
                     {
                         Value = companyImages.Flage == null ? (Object)DBNull.Value : companyImages.Flage
                     });
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChangeStatus(DriverViewModel driverViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("DeleteDriver @Id,@IsActive",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id },
                       new SqlParameter("IsActive", System.Data.SqlDbType.Bit) { Value = driverViewModel.IsActive }
                    );
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        [Route("api/DocumentUpload/MediaUpload")]
        public async Task<HttpResponseMessage> MediaUpload()
        {
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            //access files  
            IList<HttpContent> files = provider.Files;

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

            ////-------------------------------------For testing----------------------------------  
            //to append any text in filename.  
            //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"') + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToDo: Uncomment this after UAT as per Jeeevan  

            //List<string> tempFileName = thisFileName.Split('.').ToList();  
            //int counter = 0;  
            //foreach (var f in tempFileName)  
            //{  
            //    if (counter == 0)  
            //        thisFileName = f;  

            //    if (counter > 0)  
            //    {  
            //        thisFileName = thisFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + f;  
            //    }  
            //    counter++;  
            //}  

            ////-------------------------------------For testing----------------------------------  

            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

            if (formData["ClientDocs"] == "ClientDocs")
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string DDTT = unixTimestamp.ToString();

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "ClientDocument");
                filename = System.IO.Path.Combine(directoryName, DDTT + thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
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
            return response;
        }
        
        [HttpPost]
        public HttpResponseMessage BulkDriver()
        {
            try
            {
                var driverData = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("BulkDriver"
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(driverData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
    }
}
