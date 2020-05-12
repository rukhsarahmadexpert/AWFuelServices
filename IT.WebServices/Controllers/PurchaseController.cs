using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
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
    public class PurchaseController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";


        [HttpPost]
        public HttpResponseMessage PurchaseNumber()
        {
            try
            {
                var QuotNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("PurchaseNumber"
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(QuotNumberData.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }


        [HttpPost]
        public HttpResponseMessage All()
        {
            try
            {
                var LpoList = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("PurchaseAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LpoList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {

                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                var PURID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseAdd @LPOId,@VenderId, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @CreatedBy",
                      new SqlParameter("LPOId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.LPOId }
                    , new SqlParameter("VenderId", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.VenderId }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition ?? (object)DBNull.Value  }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote ?? (object)DBNull.Value  }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber ?? (object)DBNull.Value  }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                   ).FirstOrDefault();

                int PurchaseId = Convert.ToInt32(PURID.Result);

                if (PurchaseId > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseDetailsAdd @PurchaseId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                          new SqlParameter("PurchaseId", System.Data.SqlDbType.Int) { Value = PurchaseId }
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

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PURID.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Edit(int Id)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceViewModel>().ReadStoredProcedure("PurchaseGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditDetails(int Id)
        {
            try
            {
                var LPODetailsData = unitOfWork.GetRepositoryInstance<LPOInvoiceDetails>().ReadStoredProcedure("PurchaseDetailsById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPODetailsData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDetailsRow(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseUpdate @Id, @Total, @VAT, @GrandTotal,@LPODetaiRowId"
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
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditReport(int Id)
        {
            try
            {
                var LPOData = unitOfWork.GetRepositoryInstance<LPOInvoiceModel>().ReadStoredProcedure("PurchaseGetById @Id"
                , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(LPOData));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Exception((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                DateTime DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                var PURID = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseUpdateAll @Id, @Total, @VAT, @GrandTotal, @TermCondition, @CustomerNote,@FromDate, @DueDate, @PONumber, @CreatedBy,@ReasonUpdated",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.Id }
                    , new SqlParameter("Total", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.Total }
                    , new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.VAT }
                    , new SqlParameter("GrandTotal", System.Data.SqlDbType.Money) { Value = lPOInvoiceViewModel.GrandTotal }
                    , new SqlParameter("TermCondition", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.TermCondition ?? (object)DBNull.Value }
                    , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.CustomerNote ?? (object)DBNull.Value }
                    , new SqlParameter("FromDate", System.Data.SqlDbType.DateTime) { Value = FromDate }
                    , new SqlParameter("DueDate", System.Data.SqlDbType.DateTime) { Value = DueDate }
                    , new SqlParameter("PONumber", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.PONumber }
                    , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = lPOInvoiceViewModel.CreatedBy }
                    , new SqlParameter("ReasonUpdated", System.Data.SqlDbType.NVarChar) { Value = lPOInvoiceViewModel.ReasonUpdated ?? (object)DBNull.Value  }
                   ).FirstOrDefault();

                int PURIDs = Convert.ToInt32(PURID.Result);

                if (PURIDs > 0)
                {
                    foreach (LPOInvoiceDetails DetailsList in lPOInvoiceViewModel.lPOInvoiceDetailsList)
                    {
                        if (DetailsList.Id == 0)
                        {
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseDetailsAdd @PurchaseId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("PurchaseId", System.Data.SqlDbType.Int) { Value = PURIDs }
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
                            var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("PurchaseDetailsUpdate @Id, @PurchaseId, @ItemId, @UnitId, @Description, @UnitPrice, @Qunatity,@Total, @VAT, @SubTotal",
                              new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = DetailsList.Id }
                            , new SqlParameter("PurchaseId", System.Data.SqlDbType.Int) { Value = PURIDs }
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

                if (lPOInvoiceViewModel.ReasonUpdated != null)
                {
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel = new UpdateReasonDescriptionViewModel();

                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.ReasonDescription = lPOInvoiceViewModel.ReasonUpdated;
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.CreatedBy = lPOInvoiceViewModel.CreatedBy;
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.Flag = "Invoice";
                    lPOInvoiceViewModel.updateReasonDescriptionViewModel.Id = PURIDs;
                }

                if (lPOInvoiceViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if (lPOInvoiceViewModel.Id > 0)
                    {
                        var result = updateReason.Add(lPOInvoiceViewModel.updateReasonDescriptionViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(PURID.Result));
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
