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
    public class BillController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";          

        [HttpPost]
        public HttpResponseMessage Add([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);

                var LPOID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BIllAdd @FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@Bill_Id,@BillNumber,@Total, @VAT, @GrandTotal,@VenderId,@IsFromLpo"

                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.PONumber }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.RefrenceNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("Bill_Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.LPOId }
                    , new SqlParameter("BillNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.BillNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.BillNumber }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("VenderId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.VenderId }
                    , new SqlParameter("IsFromLpo", System.Data.SqlDbType.Bit) { Value = lPOInvoiceViewModel.IsFromLpo }
                   ).FirstOrDefault();

                if (LPOID.Result > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillDetailsAdd @BillId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                          new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = LPOID.Result }
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

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOID.Result));
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
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("BillAll"
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
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillDelete @Id"
                   , new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Details(int Id)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("BillById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault(); 

                 var BillDetails = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("BillDetailsById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();


                var CompanyModel = unitOfWork.GetRepositoryInstance<VenderViewModel>().ReadStoredProcedure("CompanyById @CompanyId"
               , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = LPOData.VenderId }
               ).ToList();

                var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
               , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Bill" }
               ).ToList();

                LPOData.uploadDocumentsViewModels = Documents;
                LPOData.lPOInvoiceDetailsList = BillDetails;
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
        public HttpResponseMessage BillNumber()
        {
            try
            {
                var QuotNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("BillNumber"
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
        public HttpResponseMessage DeleteDetailsRow(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillUpdate @Id, @Total, @VAT, @GrandTotal,@LPODetaiRowId"
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
        public HttpResponseMessage Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate).AddDays(1);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate).AddDays(1);


                var LPOID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillUpdateAll @Id, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @RefrenceNumber, @CreatedBy,@ReasonUpdated",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition == null ? (object)DBNull.Value : lPOInvoiceViewModel.TermCondition }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote == null ? (object)DBNull.Value : lPOInvoiceViewModel.CustomerNote }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber ?? (object)DBNull.Value }
                    , new SqlParameter("RefrenceNumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.RefrenceNumber == null ? (object)DBNull.Value : lPOInvoiceViewModel.RefrenceNumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("ReasonUpdated", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.ReasonUpdated == null ? (object)DBNull.Value : lPOInvoiceViewModel.ReasonUpdated }
                   ).FirstOrDefault();

                int LPOId = Convert.ToInt32(LPOID.Result);

                if (LPOId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        if (DetailsList.Id == 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillDetailsAdd @BillId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                               new SqlParameter("BillId", System.Data.SqlDbType.Int) { Value = LPOID.Result }
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
                        else if (DetailsList.Id > 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("BillDetailsUpdate @Id, @LPOId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = DetailsList.Id }
                            , new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = LPOId }
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
        public HttpResponseMessage EditReport(LPOInvoiceModel lPOInvoiceModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("BillById  @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceModel.Id }
                ).FirstOrDefault();


                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("BillDetailsById @Id"
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

        [HttpPost]
        public HttpResponseMessage LPOGetRemainingDetails(SearchViewModel searchViewModel)
        {
            try
            {
                var LPORemainingData = unitOfWork.GetRepositoryInstance<LpoRemainingQuantityViewModel>().ReadStoredProcedure("LPOGetRemainingDetails  @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPORemainingData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public HttpResponseMessage BillAllByLpoId(SearchViewModel searchViewModel)
        {
            try
            {
                var BillList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("BillAllByLpoId @Id"
                    , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(BillList));
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
