using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
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
    public class AWFDriverController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {

                var driverList = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverAllAWFuel @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }

                    ).ToList();

                if(pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
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
                var items = driverList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                if (items.Count > 0)
                {
                    items[0].TotalRows = TotalCount;
                }
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
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
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

                        if (file1.Headers.ContentDisposition.Name == "\"DriverImageUrl\"" || file1.Headers.ContentDisposition.DispositionType == "DriverImageUrl" )
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
                driverViewModel.FullName = HttpContext.Current.Request["FullName"];
                driverViewModel.Facebook = HttpContext.Current.Request["Facebook"];
                driverViewModel.Comments = HttpContext.Current.Request["Comments"];
                driverViewModel.Nationality = HttpContext.Current.Request["Nationality"];
                driverViewModel.LicenseExpiry = HttpContext.Current.Request["DrivingLicenseExpiryDate"];
              
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

                var userIsAlreadyAvailible = new SingleIntegerValueResult();

                if (driverViewModel.Email != null)
                {
                     userIsAlreadyAvailible = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CheckUser @UserName"
                         , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.Email }
                        ).FirstOrDefault();
                }
                if (userIsAlreadyAvailible.Result > 0)
                {
                     userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("User Already Availible"));
                     return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }                
                else
                {
                    var DriverResult = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverAddAWFuel @FullName, @Contact, @Email, @Facebook, @Comments, @PassportCopy, @VisaCopy, @IDUAECopyFront,@IDUAECopyBack,@DrivingLicenseFront, @DrivingLicenseBack,@Nationality, @DrivingLicenseExpiryDate,@CompanyId, @CreatedBy,@UID,@LicenseType,@LicenseType2,@LicenseType3,@DriverImageUrl,@PassportBack",
                          new SqlParameter("FullName", System.Data.SqlDbType.VarChar) { Value = driverViewModel.FullName == null ? (object)DBNull.Value : driverViewModel.FullName }
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

                    if (DriverResult.Name == "Failed to add Data")
                    {
                        userRepsonse.Success((new JavaScriptSerializer()).Serialize("Transiction Failed"));
                    }
                    else
                    {
                        userRepsonse.Success((new JavaScriptSerializer()).Serialize(DriverResult));
                    }
                }
               
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Edit(DriverViewModel driverViewModel)
        {
            try
            {
                var driverData = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverByIdAWFuel @Id, @CompanyId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = driverViewModel.CompanyId }
                ).FirstOrDefault();

                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Driver" }
               ).ToList();

                driverData.uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(driverData));
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

                    driverViewModel.Id = Convert.ToInt32(HttpContext.Current.Request["Id"]);                             
                    driverViewModel.UpdatedBy = Convert.ToInt32(HttpContext.Current.Request["UpdatedBy"]);

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
                    driverViewModel.FullName = HttpContext.Current.Request["FullName"];
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

                 var driverData = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverUpdateAWFuel @Id,@FullName, @Contact,@Email,@Facebook,@Comments,@PassportCopy,@VisaCopy,@IDUAECopyFront,@IDUAECopyBack,@DrivingLicenseFront,@DrivingLicenseBack,@Nationality,@DrivingLicenseExpiryDate,@LicenseType,@LicenseType2,@LicenseType3,@DriverImageUrl,@PassportBack,@updatedBy"
                    , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id }
                    , new SqlParameter("FullName", System.Data.SqlDbType.VarChar) { Value = driverViewModel.FullName == null ? (object)DBNull.Value : driverViewModel.FullName }
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
                    , new SqlParameter("LicenseType", System.Data.SqlDbType.Int) { Value = a }
                    , new SqlParameter("LicenseType2", System.Data.SqlDbType.Int) { Value = b }
                    , new SqlParameter("LicenseType3", System.Data.SqlDbType.Int) { Value = c }
                    , new SqlParameter("DriverImageUrl", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.DriverImageUrl == null ? (object)DBNull.Value : driverViewModel.DriverImageUrl }
                    , new SqlParameter("PassportBack", System.Data.SqlDbType.NVarChar) { Value = driverViewModel.PassportBack == null ? (object)DBNull.Value : driverViewModel.PassportBack }
                    , new SqlParameter("updatedBy", System.Data.SqlDbType.Int) { Value = driverViewModel.UpdatedBy }

                 ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(driverData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DeleteImage(CompanyImages companyImages)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("DriverDeleteImageAWFuel @Id, @Flage",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyImages.Id }
                     , new SqlParameter("Flage", System.Data.SqlDbType.NVarChar)
                     {
                         Value = companyImages.Flage == null ? (Object)DBNull.Value : companyImages.Flage
                     });
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ChangeStatus(DriverViewModel driverViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("DeleteDriverAWFuel @Id,@IsActive",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = driverViewModel.Id },
                       new SqlParameter("IsActive", System.Data.SqlDbType.Bit) { Value = driverViewModel.IsActive }
                    );
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage IsDriverTakinVehicle(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverLoginHistoryViewModel>().ReadStoredProcedure("IsDriverTakinVehicle @email",
                    new SqlParameter("email", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                    ).FirstOrDefault();

                if (Result.Id > 0)
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Id));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DriverAWFInfoByEmail(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("driverAWFuelInfoByEmail @email",
                    new SqlParameter("email", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.searchkey }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DriverLoginHistoryWithAsignVehicle(DriverLoginHistoryViewModel driverLoginHistoryViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverLoginHistoryViewModel>().ReadStoredProcedure("DriverLoginHistoryLoginIn @VehicleId,@UserId,@CompanyId,@userName",
                    new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = driverLoginHistoryViewModel.VehicleId },
                    new SqlParameter("UserId", System.Data.SqlDbType.Int) { Value = driverLoginHistoryViewModel.DriverId },
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = driverLoginHistoryViewModel.CompanyId },
                    new SqlParameter("userName", System.Data.SqlDbType.NVarChar) { Value = driverLoginHistoryViewModel.userName }
                    ).FirstOrDefault();

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ReleaseVehicle(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("ReleaseVehicle @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).FirstOrDefault();
                                              
                    CustomerOrderController customerOrderController = new CustomerOrderController();
                    CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel
                    { 
                        NotificationCode = "ADM-005",
                        Title = "Driver Logout",
                        Message = Result.FullName + " Is logOut"
                     };

                    int Res = customerOrderController.AdminNotificaton(customerOrderListViewModel);

                //    customerOrderListViewModel.email = Result.Email;
                //    customerOrderListViewModel.NotificationCode = "DRV-002";
                //    customerOrderListViewModel.Title = "Logout Info";
                //    customerOrderListViewModel.Message = "Admin logOut your account";   

                //int ResDr = customerOrderController.DriverNotification(customerOrderListViewModel);

                var responselist = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DeleteDriverDevicelToken @Email"
                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = Result.Email }
                   ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DriverAllOnline(int Id)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverAWFAllOnline @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderAsignToDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("customerOrderToDriverAsign @DriverId,@OrderId,@CreatedBy,@LocationLink,@Note",
                    new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.DriverId },
                    new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId },
                    new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.CreatedBy },
                    new SqlParameter("LocationLink", System.Data.SqlDbType.NVarChar) { Value = customerNoteOrderViewModel.LocationLink == null ? (object)DBNull.Value : customerNoteOrderViewModel.LocationLink },
                    new SqlParameter("Note", System.Data.SqlDbType.NVarChar) { Value = customerNoteOrderViewModel.Note == null ? (object)DBNull.Value : customerNoteOrderViewModel.Note }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DriverViewOrder(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("DriverViewOrder @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage CustomerOrderAcceptDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderAcceptDriver @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId }
                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DriverLoginHistoryAllForAdmin(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var DriverList = unitOfWork.GetRepositoryInstance<DriverLoginHistoryViewModelForAdmin>().ReadStoredProcedure("DriverLoginHistoryAllForAdmin @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id }
                    ).ToList();

                int count = DriverList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = DriverList.OrderByDescending(x => x.DriverId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if (DriverList.Count < 1)
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
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DriverLogouByAdmin(SearchViewModel searchViewModel)
        {

            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverViewModel>().ReadStoredProcedure("DriverLogouByAdmin @LoginHistoryId",
                    new SqlParameter("LoginHistoryId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).FirstOrDefault();


                CustomerOrderController customerOrderController = new CustomerOrderController();
                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel
                {
                    NotificationCode = "ADM-005",
                    Title = "Driver Logout",
                    Message = Result.FullName + " Is logOut"
                };
                int Res = customerOrderController.AdminNotificaton(customerOrderListViewModel);

                customerOrderListViewModel.email = Result.Email;
                customerOrderListViewModel.NotificationCode = "DRV-002";
                customerOrderListViewModel.Title = "Logout Info";
                customerOrderListViewModel.Message = "Admin logOut your account";

                int ResDr = customerOrderController.DriverNotification(customerOrderListViewModel);

                var responselist = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("DeleteDriverDevicelToken @Email"
                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = Result.Email }
                   ).FirstOrDefault();


                userRepsonse.Success((new JavaScriptSerializer()).Serialize(1));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }
        
        [HttpPost]
        public HttpResponseMessage AdminDriverListWithoutPagination(SearchViewModel searchViewModel)
        {
            try
            {
                var DriverList = unitOfWork.GetRepositoryInstance<DriverModel>().ReadStoredProcedure("AdminDriverListWithoutPagination @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).ToList();

                 userRepsonse.Success((new JavaScriptSerializer()).Serialize(DriverList));
               
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
