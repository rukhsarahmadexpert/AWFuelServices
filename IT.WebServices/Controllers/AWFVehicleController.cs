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
    public class AWFVehicleController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var AWvehicleList = unitOfWork.GetRepositoryInstance<VehicleViewModel>().ReadStoredProcedure("VehicleAllAWFuel @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }
                    ).ToList();

                if(pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    AWvehicleList = AWvehicleList.Where(x => x.TraficPlateNumber.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
                }

                int count = AWvehicleList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = AWvehicleList.OrderByDescending(x=>x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if (AWvehicleList.Count < 1)
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
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            try
            {

                VehicleViewModel vehicleViewModel = new VehicleViewModel();


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

                        if (file1.Headers.ContentDisposition.Name == "\"MulkiaFront1\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaFront1")
                        {
                            vehicleViewModel.MulkiaFront1 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaBack1\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaBack1")
                        {
                            vehicleViewModel.MulkiaBack1 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaFront2\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaFront2")
                        {
                            vehicleViewModel.MulkiaFront2 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaBack2\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaBack2")
                        {
                            vehicleViewModel.MulkiaBack2 = thisFileName;
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


                vehicleViewModel.VehicleType = Convert.ToInt32(HttpContext.Current.Request["VehicleType"]);
                vehicleViewModel.TraficPlateNumber = HttpContext.Current.Request["TraficPlateNumber"];
                vehicleViewModel.TCNumber = HttpContext.Current.Request["TCNumber"];
                vehicleViewModel.Model = HttpContext.Current.Request["Model"];
                vehicleViewModel.Color = HttpContext.Current.Request["Color"];
                vehicleViewModel.MulkiaExpiry = HttpContext.Current.Request["MulkiaExpiry"];
                vehicleViewModel.InsuranceExpiry = HttpContext.Current.Request["InsuranceExpiry"];
                vehicleViewModel.RegisteredRegion = HttpContext.Current.Request["RegisteredRegion"];
                vehicleViewModel.Brand = HttpContext.Current.Request["Brand"];
                vehicleViewModel.Comments = HttpContext.Current.Request["Comments"];
                vehicleViewModel.UID = HttpContext.Current.Request["UID"];
                vehicleViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                vehicleViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                
                var userIsAlreadyAvailible = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AWFVehicleCheckTraficPlateNumber @TraficPlateNumber,@CompanyId"
                   , new SqlParameter("TraficPlateNumber", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.TraficPlateNumber }
                   , new SqlParameter("CompanyId", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.CompanyId }
                  ).FirstOrDefault();
                if (userIsAlreadyAvailible.Result > 0)
                {
                    userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("TraficPlateNumber Already Availible"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {

                    var vehicleAdd = unitOfWork.GetRepositoryInstance<VehicleViewModel>().ReadStoredProcedure("VehicleAddAWFuel @VehicleType, @TraficPlateNumber, @TCNumber, @Model, @Color, @MulkiaExpiry,@InsuranceExpiry, @RegisteredRegion, @Brand, @MulkiaFront1, @MulkiaBack1, @MulkiaFront2, @MulkiaBack2, @Comments,@CompanyId,@CreatedBy,@UID",
                         new SqlParameter("VehicleType", System.Data.SqlDbType.Int) { Value = vehicleViewModel.VehicleType }
                        , new SqlParameter("TraficPlateNumber", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.TraficPlateNumber == null ? (Object)DBNull.Value : vehicleViewModel.TraficPlateNumber }
                        , new SqlParameter("TCNumber", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.TCNumber == null ? (Object)DBNull.Value : vehicleViewModel.TCNumber }
                        , new SqlParameter("Model", System.Data.SqlDbType.VarChar) { Value = vehicleViewModel.Model == null ? (Object)DBNull.Value : vehicleViewModel.Model }
                        , new SqlParameter("Color", System.Data.SqlDbType.VarChar) { Value = vehicleViewModel.Color == null ? (Object)DBNull.Value : vehicleViewModel.Color }
                        , new SqlParameter("MulkiaExpiry", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaExpiry }
                        , new SqlParameter("InsuranceExpiry", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.InsuranceExpiry }
                        , new SqlParameter("RegisteredRegion", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.RegisteredRegion == null ? (Object)DBNull.Value : vehicleViewModel.RegisteredRegion }
                        , new SqlParameter("Brand", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.Brand == null ? (Object)DBNull.Value : vehicleViewModel.Brand }
                        , new SqlParameter("MulkiaFront1", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaFront1 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaFront1 }
                        , new SqlParameter("MulkiaBack1", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaBack1 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaBack1 }
                        , new SqlParameter("MulkiaFront2", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaFront2 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaFront2 }
                        , new SqlParameter("MulkiaBack2", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaBack2 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaBack2 }
                        , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.Comments == null ? (object)DBNull.Value : vehicleViewModel.Comments }
                        , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = vehicleViewModel.CompanyId == 0 ? (object)DBNull.Value : vehicleViewModel.CompanyId }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = vehicleViewModel.CreatedBy == 0 ? (object)DBNull.Value : vehicleViewModel.CreatedBy }
                        , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.UID == null ? (object)DBNull.Value : vehicleViewModel.UID }
                       );

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleAdd));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }




        }

        [HttpPost]
        public HttpResponseMessage Edit(VehicleViewModel vehicleViewModel)
        {
            try
            {
                var userList = unitOfWork.GetRepositoryInstance<VehicleViewModel>().ReadStoredProcedure("VehicleByIdAWFuel @Id,@CompanyId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = vehicleViewModel.Id }
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = vehicleViewModel.CompanyId }
                ).FirstOrDefault();
                
                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                  , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = vehicleViewModel.Id }
                  , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Vehicle" }
                ).ToList();

                userList.uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> update()
        {
            try
            {

                VehicleViewModel vehicleViewModel = new VehicleViewModel();

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

                        if (file1.Headers.ContentDisposition.Name == "\"MulkiaFront1\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaFront1")
                        {
                            vehicleViewModel.MulkiaFront1 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaBack1\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaBack1")
                        {
                            vehicleViewModel.MulkiaBack1 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaFront2\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaFront2")
                        {
                            vehicleViewModel.MulkiaFront2 = thisFileName;
                        }
                        else if (file1.Headers.ContentDisposition.Name == "\"MulkiaBack2\"" || file1.Headers.ContentDisposition.DispositionType == "MulkiaBack2")
                        {
                            vehicleViewModel.MulkiaBack2 = thisFileName;
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

                vehicleViewModel.Id = Convert.ToInt32(HttpContext.Current.Request["Id"]);
                vehicleViewModel.VehicleType = Convert.ToInt32(HttpContext.Current.Request["VehicleType"]);
                vehicleViewModel.TraficPlateNumber = HttpContext.Current.Request["TraficPlateNumber"];
                vehicleViewModel.TCNumber = HttpContext.Current.Request["TCNumber"];
                vehicleViewModel.Model = HttpContext.Current.Request["Model"];
                vehicleViewModel.Color = HttpContext.Current.Request["Color"];
                vehicleViewModel.MulkiaExpiry = HttpContext.Current.Request["MulkiaExpiry"];
                vehicleViewModel.InsuranceExpiry = HttpContext.Current.Request["InsuranceExpiry"];
                vehicleViewModel.RegisteredRegion = HttpContext.Current.Request["RegisteredRegion"];
                vehicleViewModel.Brand = HttpContext.Current.Request["Brand"];
                vehicleViewModel.Comments = HttpContext.Current.Request["Comments"];
                vehicleViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Request["CompanyId"]);
                vehicleViewModel.UpdateBy = Convert.ToInt32(HttpContext.Current.Request["UpdatedBy"]);


                var vehicleAdd = unitOfWork.GetRepositoryInstance<VehicleViewModel>().ReadStoredProcedure("VehicleUpdateAWFuel @Id, @VehicleType, @TraficPlateNumber, @TCNumber, @Model, @Color, @MulkiaExpiry,@InsuranceExpiry, @RegisteredRegion, @Brand, @MulkiaFront1, @MulkiaBack1, @MulkiaFront2, @MulkiaBack2, @Comments,@UpdatedBy",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = vehicleViewModel.Id }
                    , new SqlParameter("VehicleType", System.Data.SqlDbType.Int) { Value = vehicleViewModel.VehicleType }
                    , new SqlParameter("TraficPlateNumber", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.TraficPlateNumber == null ? (Object)DBNull.Value : vehicleViewModel.TraficPlateNumber }
                    , new SqlParameter("TCNumber", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.TCNumber == null ? (Object)DBNull.Value : vehicleViewModel.TCNumber }
                    , new SqlParameter("Model", System.Data.SqlDbType.VarChar) { Value = vehicleViewModel.Model == null ? (Object)DBNull.Value : vehicleViewModel.Model }
                    , new SqlParameter("Color", System.Data.SqlDbType.VarChar) { Value = vehicleViewModel.Color == null ? (Object)DBNull.Value : vehicleViewModel.Color }
                    , new SqlParameter("MulkiaExpiry", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaExpiry }
                    , new SqlParameter("InsuranceExpiry", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.InsuranceExpiry }
                    , new SqlParameter("RegisteredRegion", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.RegisteredRegion == null ? (Object)DBNull.Value : vehicleViewModel.RegisteredRegion }
                    , new SqlParameter("Brand", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.Brand == null ? (Object)DBNull.Value : vehicleViewModel.Brand }
                    , new SqlParameter("MulkiaFront1", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaFront1 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaFront1 }
                    , new SqlParameter("MulkiaBack1", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaBack1 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaBack1 }
                    , new SqlParameter("MulkiaFront2", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaFront2 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaFront2 }
                    , new SqlParameter("MulkiaBack2", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.MulkiaBack2 == null ? (Object)DBNull.Value : vehicleViewModel.MulkiaBack2 }
                    , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = vehicleViewModel.Comments == null ? (object)DBNull.Value : vehicleViewModel.Comments }
                    , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = vehicleViewModel.UpdateBy == 0 ? (object)DBNull.Value : vehicleViewModel.UpdateBy }
                   ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteImage(CompanyImages companyImages)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("VehicleDeleteImageAWFuel @Id, @Flage",
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
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChangeStatus(VehicleViewModel vehicleViewModel)
        {
            try
            {
                var result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("VehcileDisableAWFuel @Id,@IsActive",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = vehicleViewModel.Id },
                    new SqlParameter("IsActive", System.Data.SqlDbType.Int) { Value = vehicleViewModel.IsActive }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);

            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage VehicleGatAllUnAsigned(SearchViewModel searchViewModel)
        {
            try
            {
                var result = unitOfWork.GetRepositoryInstance<VehicleViewModel>().ReadStoredProcedure("VehcileAllUnAsigned @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);

            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AwfuelvehicleAll(VehicleViewModel vehicleViewModel)
        {
            try
            {
                var BulkVehicle = unitOfWork.GetRepositoryInstance<VehicleModel>().ReadStoredProcedure("AwfuelvehicleAll"
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(BulkVehicle));
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
