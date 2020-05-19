using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
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
    public class CompanyController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        //UserViewModel userViewModel = new UserViewModel();
        readonly string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var userList = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("UserAll"
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
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
                companyViewModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Request["CreatedBy"]);
                companyViewModel.Address = HttpContext.Current.Request["Address"];
                companyViewModel.TRN = HttpContext.Current.Request["TRN"];
                companyViewModel.Commentes = HttpContext.Current.Request["Commentes"];
                companyViewModel.Token = HttpContext.Current.Request["Token"];
                companyViewModel.DeviceId = HttpContext.Current.Request["DeviceId"];
                companyViewModel.Device = HttpContext.Current.Request["Device"];
                companyViewModel.IsCashCompany = Convert.ToBoolean(HttpContext.Current.Request["IsCashCompany"]);
               
                var CompanyAdd = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyAdd @Name,@OwnerRepresentaive, @Street, @Postcode, @City, @State, @Country, @Phone, @Cell, @Email, @Web, @Comments, @FindSource, @CreatedBy,@LogoURL,@TradeLicense,@PassportFirstPage,@VATCertificate, @PassportLastPage,@IDCardUAE,@UID,@TRN,@Address,@IsCashCompany",
                     new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Name  ?? (object)DBNull.Value }
                    , new SqlParameter("OwnerRepresentaive", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.OwnerRepresentaive  ?? (Object)DBNull.Value  }
                    , new SqlParameter("Street", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Street  ?? (Object)DBNull.Value  }
                    , new SqlParameter("Postcode", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Postcode  ?? (Object)DBNull.Value }
                    , new SqlParameter("City", System.Data.SqlDbType.VarChar) { Value = companyViewModel.City ?? (Object)DBNull.Value  }
                    , new SqlParameter("State", System.Data.SqlDbType.VarChar) { Value = companyViewModel.State  ?? (Object)DBNull.Value  }
                    , new SqlParameter("Country", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Country ?? (Object)DBNull.Value }
                    , new SqlParameter("Phone", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Phone  ?? (Object)DBNull.Value  }
                    , new SqlParameter("Cell", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Cell  ?? (Object)DBNull.Value  }
                    , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Email ?? (Object)DBNull.Value  }
                    , new SqlParameter("Web", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Web ?? (Object)DBNull.Value  }
                    , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Commentes ?? (Object)DBNull.Value  }
                    , new SqlParameter("FindSource", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.FindSource ?? (Object)DBNull.Value }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = companyViewModel.CreatedBy }
                    , new SqlParameter("LogoURL", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.LogoUrl  ?? (object)DBNull.Value  }
                    , new SqlParameter("TradeLicense", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.TradeLicense  ?? (object)DBNull.Value  }
                    , new SqlParameter("VATCertificate", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.VATCertificate  ?? (object)DBNull.Value  }
                    , new SqlParameter("PassportFirstPage", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.PassportFirstPage ?? (object)DBNull.Value  }
                    , new SqlParameter("PassportLastPage", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.PassportLastPage ?? (object)DBNull.Value  }
                    , new SqlParameter("IDCardUAE", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.IDCardUAE  ?? (object)DBNull.Value  }
                    , new SqlParameter("UID", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.UID ?? (object)DBNull.Value }
                    , new SqlParameter("TRN", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.TRN  ?? (object)DBNull.Value }
                    , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Address  ?? (object)DBNull.Value }
                    , new SqlParameter("IsCashCompany", System.Data.SqlDbType.Bit) { Value = companyViewModel.IsCashCompany }
                  
                    ).FirstOrDefault();

                if (CompanyAdd.Id > 0)
                {
                    if (companyViewModel.Token != null && companyViewModel.Token != "")
                    {
                        var notificationAddResponse = unitOfWork.GetRepositoryInstance<UserCompanyViewModel>().ReadStoredProcedure("NotificationInformationAdd @DeviceId,@DeviceToken,@Email,@Authority,@CompanyId,@Device"
                                                   , new SqlParameter("DeviceId", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.DeviceId ?? (object)DBNull.Value }
                                                   , new SqlParameter("DeviceToken", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Token ?? (object)DBNull.Value  }
                                                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = CompanyAdd.UserName }
                                                   , new SqlParameter("Authority", System.Data.SqlDbType.VarChar) { Value = CompanyAdd.Authority }
                                                   , new SqlParameter("CompanyId", System.Data.SqlDbType.VarChar) { Value = CompanyAdd.Id }
                                                   , new SqlParameter("Device", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Device ?? (object)DBNull.Value }
                                                   ).FirstOrDefault();
                    }
                }

                CustomerOrderController customerOrderController = new CustomerOrderController();

                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel
                { 
                    NotificationCode = "ADM-006",
                    Title = "New Company Registered",
                    Message = companyViewModel.Name + " Is Registered as new company",
                    RequestedQuantity = 0
                };

                int Res = customerOrderController.AdminNotificaton(customerOrderListViewModel);

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Edit(int Id)
        {
            try
            {
                var companyModel = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();

                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Company" }
                ).ToList();

                var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
                 , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                 , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "ClientCompany" }
                 ).ToList();

                companyModel.uploadDocumentsViewModels = Documents;
                companyModel.updateReasonDescriptionViewModels = updatereason;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(companyModel));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage CompanyById(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var companyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("CompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id }
                ).FirstOrDefault();


                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id }
                , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Company" }
                ).ToList();

                var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
              , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id }
              , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "ClientCompany" }
              ).ToList();

                companyModel.uploadDocumentsViewModels = Documents;
                companyModel.updateReasonDescriptionViewModels = updatereason;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(companyModel));
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
            UpdateReason updateReason = new UpdateReason();
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

                var CompanyUpdate = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompanyUpdate @Id, @Name,@OwnerRepresentaive, @Street, @Postcode, @City, @State, @Country, @Phone, @Cell, @Email, @Web, @UpdatedBy,@Address,@TRN,@LogoUrl",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyViewModel.Id }
                   , new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Name ?? (object)DBNull.Value  }
                   , new SqlParameter("OwnerRepresentaive", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.OwnerRepresentaive ?? (object)DBNull.Value  }
                   , new SqlParameter("Street", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Street ?? (Object)DBNull.Value  }
                   , new SqlParameter("Postcode", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Postcode  ?? (Object)DBNull.Value  }
                   , new SqlParameter("City", System.Data.SqlDbType.VarChar) { Value = companyViewModel.City ?? (Object)DBNull.Value  }
                   , new SqlParameter("State", System.Data.SqlDbType.VarChar) { Value = companyViewModel.State  ?? (Object)DBNull.Value }
                   , new SqlParameter("Country", System.Data.SqlDbType.VarChar) { Value = companyViewModel.Country ?? (Object)DBNull.Value  }
                   , new SqlParameter("Phone", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Phone  ?? (Object)DBNull.Value }
                   , new SqlParameter("Cell", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Cell  ?? (Object)DBNull.Value  }
                   , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Email  ?? (Object)DBNull.Value  }
                   , new SqlParameter("Web", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Web ==  (Object)DBNull.Value  }
                   , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = companyViewModel.UpdatedBy }
                   , new SqlParameter("Address", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.Address  ?? (object)DBNull.Value  }
                   , new SqlParameter("TRN", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.TRN  ?? (object)DBNull.Value }
                   , new SqlParameter("LogoUrl", System.Data.SqlDbType.NVarChar) { Value = companyViewModel.LogoUrl ?? (object)DBNull.Value  }

                   ).FirstOrDefault();

                if (companyViewModel.ReasonDescription != null)
                {
                    if (companyViewModel.Id > 0)
                    {
                        var updateReasonDes = new UpdateReasonDescriptionViewModel { 
                            Id = companyViewModel.Id,
                            ReasonDescription = companyViewModel.ReasonDescription,
                            CreatedBy = companyViewModel.UpdatedBy,
                            Flag = "ClientCompany"
                        };
                    var result = updateReason.Add(updateReasonDes);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyUpdate));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DeleteImage(CompanyImages companyImages)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CompanyDeleteImage @Id, @Flage",
                          new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = companyImages.Id }
                        , new SqlParameter("Flage", System.Data.SqlDbType.NVarChar) { Value = companyImages.Flage ?? (Object)DBNull.Value }
                        ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage CompayAll(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var CompanyList = unitOfWork.GetRepositoryInstance<CompanyViewModel>().ReadStoredProcedure("CompayAll"
                   ).ToList();
                //userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyList));
                //return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);

                int count = CompanyList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    CompanyList = CompanyList.Where(x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
                }
                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = CompanyList.OrderByDescending(x=>x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                //bool IsAscending = true;

                //if (pagingparametermodel.sortColumnDir != null)
                //{
                //    string direction = IsAscending ? " ASC" : " DESC";
                //    items = CompanyList.OrderBy(x => pagingparametermodel.sortColumn + pagingparametermodel.sortColumnDir).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                //}
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

               
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage CompanyFrezeOrBlackListByAdmin(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CompanyFrezeOrBlackListByAdmin @Id, @Flage",
                          new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id },
                          new SqlParameter("Flage", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.Flage }
                        ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CompayAllWithOutPagination(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var CompanyList = unitOfWork.GetRepositoryInstance<CompanyModel>().ReadStoredProcedure("CompayAllwithoutPagination"
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CompanyList));
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
