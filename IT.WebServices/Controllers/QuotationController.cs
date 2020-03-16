using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class QuotationController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {

                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);

                var QuotId = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationAdd @CustomerId, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy",
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

                int QuotationId = Convert.ToInt32(QuotId.Result);

                if (QuotationId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationDetailsAdd @QuotationId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                         new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = QuotationId }
                        , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                        , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                        , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description == null ? (object)DBNull.Value : DetailsList.Description }
                        , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                        , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                        , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                        , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                        , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }
                       ).FirstOrDefault();
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(QuotId.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("QuotationAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage QuotationAllByCustomer(SearchViewModel searchViewModel)
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("QuotationAllByCustomer @Id"
                    , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage QuotaNumber()
        {
            try
            {
                var QuotNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("QuotNumber"
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
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("QuotationById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();

                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Quotation" }
               ).ToList();

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
                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("QuotationDetailsById @Id"
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

                var QuotationId = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationUpdateAll @Id, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@ReasonUpdated",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition == null ? (object)DBNull.Value : lPOInvoiceViewModel.TermCondition }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote == null ? (object)DBNull.Value : lPOInvoiceViewModel.CustomerNote }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.RefrenceNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("ReasonUpdated", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.ReasonUpdated == null ? (object)DBNull.Value : lPOInvoiceViewModel.ReasonUpdated }
                   ).FirstOrDefault();

                int QuotId = Convert.ToInt32(QuotationId.Result);

                if (QuotId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        if (DetailsList.Id == 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationDetailsAdd @QuotationId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = QuotId }
                            , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                            , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description }
                            , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                            , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                            , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }

                            ).FirstOrDefault();
                        }
                        else if (DetailsList.Id > 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationDetailsUpdate @Id, @QuotationId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = DetailsList.Id }
                            , new SqlParameter("QuotationId", System.Data.SqlDbType.Int) { Value = QuotId }
                            , new SqlParameter("ItemId", System.Data.SqlDbType.Int) { Value = DetailsList.ItemId }
                            , new SqlParameter("UnitId", System.Data.SqlDbType.Int) { Value = DetailsList.UnitId }
                            , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = DetailsList.Description }
                            , new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = DetailsList.UnitPrice }
                            , new SqlParameter("Qunatity", System.Data.SqlDbType.Int) { Value = DetailsList.Qunatity }
                            , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = DetailsList.Total }
                            , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = DetailsList.VAT }
                            , new SqlParameter("SubTotal", System.Data.SqlDbType.Money) { Value = DetailsList.SubTotal }

                            ).FirstOrDefault();
                        }
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(QuotationId.Result));
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
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("QuotationUpdate @Id, @Total, @VAT, @GrandTotal,@LPODetaiRowId"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                , new SqlParameter("Total", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Total }
                , new SqlParameter("VAT", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.VAT }
                , new SqlParameter("GrandTotal", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.GrandTotal }
                , new SqlParameter("@LPODetaiRowId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.detailId }
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
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("QuotationGetById  @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
                ).FirstOrDefault();

                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("QuotationDetailsById @Id"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
               ).ToList();

                var CompanyModel = unitOfWork.GetRepositoryInstance<VenderViewModel>().ReadStoredProcedure("CompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.VenderId }
                ).ToList();

                var AWFCompanyModel = unitOfWork.GetRepositoryInstance<CompnayModel>().ReadStoredProcedure("AWFCompanyById @CompanyId"
                , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.detailId }
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

    }
}
