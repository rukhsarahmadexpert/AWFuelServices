using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
using IT.WebServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class LPOController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage LPOUnconvertedALL()
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<UnconvertedLpoModel>().ReadStoredProcedure("LPOUnconvertedALL"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("LPOAll"
                    ).ToList();

                int count = LpoList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    LpoList = LpoList.Where(x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
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
                var items = LpoList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                if (LpoList.Count > 0)
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
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOAllCustomer(SearchViewModel searchViewModel)
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("LPOAllCustomer @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AllConverted(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("LPOAllConverted"
                    ).ToList();

                int count = LpoList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    LpoList = LpoList.Where(x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
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
                var items = LpoList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                if (LpoList.Count > 0)
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

                if (LpoList.Count < 1)
                {
                    userRepsonse.Success(null);
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                }

                 return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.PONumber = GetQuoNumber();

                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);

                var LPOID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPOAdd @VenderId, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@CompanyId,@IsForCustomer,@CustomerId",
                      new SqlParameter("VenderId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.VenderId }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition ?? (object)DBNull.Value }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote ?? (object)DBNull.Value }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber ?? (object)DBNull.Value }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber ?? (object)DBNull.Value }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CompanyId }
                    , new SqlParameter("IsForCustomer", System.Data.SqlDbType.Bit) { Value = lPOInvoiceViewModel.IsForCustomer }
                    , new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CustomerId }
                   ).FirstOrDefault();

                int LPOId = Convert.ToInt32(LPOID.Result);

                if (LPOId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPODetailsAdd @LPOId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                          new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = LPOId }
                        , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                        , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                        , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description ?? (object)DBNull.Value }
                        , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                        , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                        , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                        , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                        , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }
                       ).FirstOrDefault();
                    }
                }

                if(lPOInvoiceViewModel.IsForCustomer == true)
                {
                    //var Customer = unitOfWork.GetRepositoryInstance<Get>().ReadStoredProcedure("LPODetailsAdd").FirstOrDefault(),


                   CustomerOrderController customerOrderController = new CustomerOrderController();
                    var customerOrderListViewModel = new CustomerOrderListViewModel { 

                        NotificationCode = "CUS-003",
                        Title = "LPO Created",
                        Message = "Admin Created LPO for your company",
                        RequestedQuantity = 0,
                        CustomerId = lPOInvoiceViewModel.CustomerId
                    };
                //Send Notification
                var resultNotification = customerOrderController.CustomerNotification(customerOrderListViewModel);
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public string GetQuoNumber()
        {
            string SerailNO = "";
            var LPOData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("LPOGetPONumber"
                ).FirstOrDefault();

            if (LPOData != null)
            {
                if (LPOData.Result != null || LPOData.Result != "")
                {
                    SerailNO = LPOData.Result.Substring(4, 8);

                    SerailNO = LPOData.Result.ToString().Substring(0, 6);

                    string TotdayNumber = POClassLPO.PONumber().Substring(0, 6);
                    int Counts = 0;
                    if (SerailNO == TotdayNumber)
                    {
                        Counts = Convert.ToInt32(LPOData.Result.Substring(10, 2)) + 1;

                        if (Counts.ToString().Length == 1)
                        {
                            SerailNO = "LPO-" + TotdayNumber + "0" + Counts;
                        }
                        else
                        {
                            SerailNO = "LPO-" + TotdayNumber + Counts.ToString();
                        }
                    }
                    else
                    {
                        SerailNO = "LPO-" + POClassLPO.PONumber();
                    }
                }
            }
            else
            {
                SerailNO = "LPO-" + POClassLPO.PONumber();
            }

            return SerailNO;
        }

        [HttpPost]
        public HttpResponseMessage Edit(int Id)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("LPOGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                
                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("LPODetailsById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               ).ToList();
                
                var CompanyModel = new List<CompnayModel>();

                if (LPOData.IsForCustomer == true)
                {
                     CompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("CompanyById @CompanyId"
                       , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.CompanyId }
                       ).ToList();
                }
                else
                {
                     CompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("AWFCompanyById @CompanyId"
                   , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.CompanyId }
                   ).ToList();
                }

                var venderData = unitOfWork.GetRepositoryInstance<VenderViewModel>().ReadStoredProcedure("VenderById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = LPOData.VenderId }
               ).ToList();
                
                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "LPO" }
               ).ToList();

                LPOData.uploadDocumentsViewModels = Documents;
                LPOData.lPOInvoiceDetailsList = LPODetailsData;
                LPOData.compnays = CompanyModel;
                LPOData.venders = venderData;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditReport(LPOInvoiceModel lPOInvoiceModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("LPOGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
                ).FirstOrDefault();

                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("LPODetailsById @Id"
              , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
              ).ToList();

                var CompanyModel = new List<CompnayModel>();

                if (LPOData.IsForCustomer == true)
                {
                    CompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("CompanyById @CompanyId"
                      , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.CompanyId }
                      ).ToList();
                }
                else
                {
                    CompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("AWFCompanyById @CompanyId"
                  , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.CompanyId }
                  ).ToList();
                }

                var venderData = unitOfWork.GetRepositoryInstance<VenderViewModel>().ReadStoredProcedure("VenderById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = LPOData.VenderId }
               ).ToList();

                LPOData.lPOInvoiceDetailsList = LPODetailsData;
                LPOData.compnays = CompanyModel;
                LPOData.venders = venderData;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOGetPONumber()
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("LPOGetPONumber"
                ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditDetails(int Id)
        {
            try
            {
                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("LPODetailsById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPODetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);
                
                var LPOID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPOUpdateAll @Id, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@ReasonUpdated,@CustomerId",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition ?? (object)DBNull.Value }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote ?? (object)DBNull.Value }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber ?? "Unknown" }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber ?? (object)DBNull.Value }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("ReasonUpdated", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.ReasonUpdated ?? (object)DBNull.Value }
                    , new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CustomerId }
                   ).FirstOrDefault();

                int LPOId = Convert.ToInt32(LPOID.Result);

                if (LPOId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        if (DetailsList.Id == 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPODetailsAdd @LPOId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = LPOId }
                            , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                            , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description ?? (object)DBNull.Value}
                            , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                            , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                            , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }

                            ).FirstOrDefault();
                        }
                        else if (DetailsList.Id > 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPODetailsUpdate @Id, @LPOId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = DetailsList.Id }
                            , new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = LPOId }
                            , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                            , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description ?? (object)DBNull.Value}
                            , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                            , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                            , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }

                            ).FirstOrDefault();
                        }
                    }
                }

                if (lPOInvoiceViewModel.ReasonUpdated != null)
                {
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel = new UpdateReasonDescriptionViewModel();
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.ReasonDescription = lPOInvoiceViewModel.ReasonUpdated;
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.CreatedBy = lPOInvoiceViewModel.CreatedBy;
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.Flag = "LPO";
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.Id = LPOId;
                }

                if (lPOInvoiceViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if (lPOInvoiceViewModel.Id > 0)
                    {
                        var result = updateReason.Add(lPOInvoiceViewModel.updateReasonDescriptionViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDeatlsRow(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LPOUpdate @Id, @Total, @VAT, @GrandTotal,@LPODetaiRowId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                , new SqlParameter("Total", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Total }
                , new SqlParameter("VAT", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.VAT }
                , new SqlParameter("GrandTotal", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.GrandTotal }
                , new SqlParameter("LPODetaiRowId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.detailId }
                ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage LPOAllLByDateRange(SearchViewModel searchViewModel)
        {
            try
            {
                DateTime FDate = Convert.ToDateTime(searchViewModel.FromDate);
                DateTime TDate = Convert.ToDateTime(searchViewModel.ToDate);

                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("LPOAllLByDateRange @FromDate,@ToDate"
                      , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FDate }
                      , new SqlParameter("ToDate", System.Data.SqlDbType.DateTime) { Value = TDate }
                      ).ToList();

                var CompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("CompanyByIdAWFuel @CompanyId"
                    , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).ToList();

                if (LpoList.Count > 0)
                {
                    LpoList[0].compnays = CompanyModel;
                }
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.BadRequest((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
    }

    internal class POClassLPO
    {
        public static string PONumber()
        {
            string Day = System.DateTime.Now.Day.ToString();
            string Month = System.DateTime.Now.Month.ToString();
            string YY = System.DateTime.Now.Year.ToString();


            if (Day.Length == 1)
            {
                Day = "0" + Day;
            }
            if (Month.Length == 1)
            {
                Month = "0" + Month;
            }

            YY = YY.Substring(2, 2);

            string PONumber = Day + Month + YY + "01";


            return PONumber;
        }
    }
}
