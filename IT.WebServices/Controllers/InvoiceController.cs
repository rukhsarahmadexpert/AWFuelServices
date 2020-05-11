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
    public class InvoiceController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";
        
        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var InvoiceList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("InvoiceAll"
                    ).ToList();

                int count = InvoiceList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    InvoiceList = InvoiceList.Where(x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
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
                var items = InvoiceList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage AllByCustomer(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var InvoiceList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("InvoiceAllByCustomer @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }
                    ).ToList();
                int count = InvoiceList.Count();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    InvoiceList = InvoiceList.Where(x => x.Name.ToLower().Contains(pagingparametermodel.SerachKey.ToLower())).ToList();
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
                var items = InvoiceList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
               
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);

                var InvocieID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceAdd @CustomerId, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy",
                      new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CustomerId }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition == null ? (object)DBNull.Value : lPOInvoiceViewModel.TermCondition }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote == null ? (object)DBNull.Value : lPOInvoiceViewModel.CustomerNote }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.PONumber }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.RefrenceNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                   ).FirstOrDefault();

                int InvoceID = Convert.ToInt32(InvocieID.Result);

                if (InvoceID > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceDetailsAdd @InvoiceId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                          new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = InvoceID }
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

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(InvocieID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage InvoiceNumber()
        {
            try
            {
                var QuotNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("InvoiceNumber"
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(QuotNumberData.Result));
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
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("InvoiceById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();

                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("InvoiceDetailsById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               ).ToList();


                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Invoice" }
               ).ToList();

                LPOData.lPOInvoiceDetailsList = LPODetailsData;
                LPOData.uploadDocumentsViewModels = Documents;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditDetails(int Id)
        {
            try
            {
                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("InvoiceDetailsById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPODetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
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

                var invoiceID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceUpdateAll @Id, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@ReasonUpdated",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition?? (object)DBNull.Value }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote?? (object)DBNull.Value  }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber ?? (object)DBNull.Value }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber ?? (object)DBNull.Value }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("ReasonUpdated", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.ReasonUpdated ?? (object)DBNull.Value  }
                   ).FirstOrDefault();

                int InvId = Convert.ToInt32(invoiceID.Result);

                if (InvId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        if (DetailsList.Id == 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceDetailsAdd @InvoiceId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = InvId }
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
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceDetailsUpdate @Id, @InvoiceId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = DetailsList.Id }
                            , new SqlParameter("InvoiceId", System.Data.SqlDbType.Int) { Value = InvId }
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

                if (lPOInvoiceViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if (lPOInvoiceViewModel.Id > 0)
                    {
                        var result = updateReason.Add(lPOInvoiceViewModel.updateReasonDescriptionViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(invoiceID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDeatlsRow(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceUpdate @Id, @Total, @VAT, @GrandTotal,@LPODetaiRowId"
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
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditReport(LPOInvoiceModel lPOInvoiceModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("InvoiceGetById  @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
                ).FirstOrDefault();


                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("InvoiceDetailsById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
               ).ToList();

                var CompanyModel = unitOfWork.GetRepositoryInstance<VenderViewModel>().ReadStoredProcedure("CompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.VenderId }
                ).ToList();

                var AWFCompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("AWFCompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.detailId}
                ).ToList();
                
                LPOData.lPOInvoiceDetailsList = LPODetailsData;
                LPOData.compnays = AWFCompanyModel;
                LPOData.venders = CompanyModel;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddFromQuotation([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FDate);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DDate);

                var LPOID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("InvoiceAddFromQuotation @FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@QuotationId"

                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.PONumber }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.RefrenceNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.LPOId }
                   ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOID.Result));
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
