using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
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
    public class FuelTransferController : ApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage Add([FromBody] FuelTransferViewModel fuelTransferViewModel)
        {
            try
            {
                DateTime FuelTransferDate = Convert.ToDateTime(fuelTransferViewModel.FuelTransferDate);

                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("FuelTransferAdd @SiteId,@VehicleId,@FuelTransferDate,@CreatedBy,@Quantity,@Description",
                       new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.SiteId }
                     , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.VehicleId }
                     , new SqlParameter("FuelTransferDate", System.Data.SqlDbType.DateTime) { Value = FuelTransferDate }
                     , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.CreatedBy }
                     , new SqlParameter("Quantity", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.Quantity }
                     , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = fuelTransferViewModel.Description == null ? (object)DBNull.Value : fuelTransferViewModel.Description }

                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(res.Result));
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
                var FuelTransferAll = unitOfWork.GetRepositoryInstance<FuelTransferViewModel>().ReadStoredProcedure("FuelTransferAll"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(FuelTransferAll));
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
                var FuelTransferAll = unitOfWork.GetRepositoryInstance<FuelTransferViewModel>().ReadStoredProcedure("FuelTransferById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(FuelTransferAll));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] FuelTransferViewModel fuelTransferViewModel)
        {
            try
            {
                DateTime FuelTransferDate = Convert.ToDateTime(fuelTransferViewModel.FuelTransferDate);

                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("FuelTransferUpdate @Id, @SiteId,@VehicleId,@FuelTransferDate,@Quantity,@UpdatedBy,@Reason",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.Id }
                     , new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.SiteId }
                     , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.VehicleId }
                     , new SqlParameter("FuelTransferDate", System.Data.SqlDbType.DateTime) { Value = FuelTransferDate }
                     , new SqlParameter("Quantity", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.Quantity }
                     , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = fuelTransferViewModel.CreatedBy }
                     , new SqlParameter("Reason", System.Data.SqlDbType.NVarChar) { Value = fuelTransferViewModel.Reason == null ? (object)DBNull.Value : fuelTransferViewModel.Reason }

                    ).FirstOrDefault();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(res.Result));
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
                var FuelTransferAll = unitOfWork.GetRepositoryInstance<FuelTransferViewModel>().ReadStoredProcedure("FuelTransferDetailsById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(FuelTransferAll));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //for bragi

        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupTransferFromDriverAdd(TransferFromDriverViewModel transferFromDriverViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<TransferFromDriverViewModel>().ReadStoredProcedure("CustomerOrderGroupTransferFromDriverAdd @OrderId,@Qunantity,@From,@To,@CreatedBy,@IsFullOrder,@Descriptions,@CompanyId,@OrderTransferRequestId",
                       new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.OrderId }
                     , new SqlParameter("Qunantity", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.Qunantity }
                     , new SqlParameter("From", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.From }
                     , new SqlParameter("To", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.To }
                     , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.CreatedBy }
                     , new SqlParameter("IsFullOrder", System.Data.SqlDbType.Bit) { Value = transferFromDriverViewModel.IsFullOrder }
                     , new SqlParameter("Descriptions", System.Data.SqlDbType.NVarChar) { Value = transferFromDriverViewModel.Descriptions == null ? (object)DBNull.Value : transferFromDriverViewModel.Descriptions }
                     , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.CompanyId }
                     , new SqlParameter("OrderTransferRequestId", System.Data.SqlDbType.Int) { Value = transferFromDriverViewModel.OrderTransferRequestId }

                    ).FirstOrDefault();

                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
                CustomerOrderController customerOrderController = new CustomerOrderController();

                customerOrderListViewModel.NotificationCode = "DRV-003";
                customerOrderListViewModel.Title = "Admin Transfer Order";
                customerOrderListViewModel.Message = "Admin Transfer Order to Driver";
                customerOrderListViewModel.RequestedQuantity = transferFromDriverViewModel.Qunantity;
                customerOrderListViewModel.email = Result.EmailTo;

                int Res1 = customerOrderController.DriverNotification(customerOrderListViewModel);

                customerOrderListViewModel.NotificationCode = "DRV-004";
                customerOrderListViewModel.Title = "Admin Transfer Order";
                customerOrderListViewModel.Message = "Admin Transfer Order From Driver";
                customerOrderListViewModel.RequestedQuantity = transferFromDriverViewModel.Qunantity;
                customerOrderListViewModel.email = Result.EmailFrom;

                //Send Notification
                int Res = customerOrderController.DriverNotification(customerOrderListViewModel);

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
        public HttpResponseMessage OrderTransferRequestsAdd(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            List<StorageViewModel> storageViewModels = new List<StorageViewModel>();

            try
            {
                var Result = unitOfWork.GetRepositoryInstance<OrderTransferRequestsViewModel>().ReadStoredProcedure("OrderTransferRequestsAdd @DriverId,@RequestType,@Description,@OrderId,@IsFullOrPartistial,@PartialQuantity,@CompanyId",
                       new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.DriverId }
                     , new SqlParameter("RequestType", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.RequestType }
                     , new SqlParameter("Description", System.Data.SqlDbType.NChar) { Value = orderTransferRequestsViewModel.Description }
                     , new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.OrderId }
                     , new SqlParameter("IsFullOrPartistial", System.Data.SqlDbType.NChar) { Value = orderTransferRequestsViewModel.IsFullOrPartial }
                     , new SqlParameter("PartialQuantity", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.PartialQuantity }
                     , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.CompanyId }
                    ).FirstOrDefault();
                
                CustomerOrderController customerOrderController = new CustomerOrderController();
                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
                //Send Notification
                customerOrderListViewModel.NotificationCode = "ADM-007";
                customerOrderListViewModel.Title = "Transfer Request";               
                customerOrderListViewModel.Message = "Order Transfer Request Created ....";
                
                int Res = customerOrderController.AdminNotificaton(customerOrderListViewModel);

                StorageController storageController = new StorageController();
              
                if (Result.Id > 0)
                { 
                    var uniqueId = System.DateTime.UtcNow.ToString();
                    for (int i = 0; i < 2; i++)
                    {
                        StorageViewModel storageViewModel = new StorageViewModel();
                        storageViewModel.CreatedBy = Result.Id;
                        storageViewModel.Source = "admin vehicle";
                        storageViewModel.SiteId = 0;
                        storageViewModel.ClientVehicleId = 0;
                        storageViewModel.LPOId = 0;
                        storageViewModel.Decription = "Transfer From Vehcile";
                        storageViewModel.ProductId = 1;
                        storageViewModel.uniques = uniqueId;

                        if (i == 0)
                        {
                            storageViewModel.Action = true;
                            storageViewModel.StockOut = 0;
                            storageViewModel.StockIn = Result.TransferdQuantity;
                            storageViewModel.VehicleId = Result.ToVehicleId;
                        }
                        else
                        {
                            storageViewModel.Action = false;
                            storageViewModel.StockOut = Result.TransferdQuantity;
                            storageViewModel.StockIn = 0;
                            storageViewModel.VehicleId = Result.FromVehicleId;
                        }

                        storageViewModels.Add(storageViewModel);
                    }
                }
                
                if (Result.Description == "request already exist")
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("request already exist"));
                }
                else
                {
                    var Results = storageController.StorageAddNew(storageViewModels);   
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
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
        public HttpResponseMessage DriverTransferRequestTypeAll()
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<DriverTransferRequestType>().ReadStoredProcedure("DriverTransferRequestTypeAll"
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
        public HttpResponseMessage OrderTransferRequestsAll(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<OrderTransferRequestsViewModel>().ReadStoredProcedure("OrderTransferRequestsAll @CompanyId",
                            new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
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
        public HttpResponseMessage OrderTransferRequestsAdminAcceptOrReject(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<OrderTransferRequestsViewModel>().ReadStoredProcedure("OrderTransferRequestsAdminAcceptOrReject @Id,@IsAccepted,@AcceptedBy,@IsOpen",
                            new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.Id },
                            new SqlParameter("IsAccepted", System.Data.SqlDbType.Bit) { Value = orderTransferRequestsViewModel.IsAccepted },
                            new SqlParameter("AcceptedBy", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.AcceptBy },
                            new SqlParameter("IsOpen", System.Data.SqlDbType.Bit) { Value = orderTransferRequestsViewModel.IsOpen }
                    ).FirstOrDefault();


                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
                CustomerOrderController customerOrderController = new CustomerOrderController();

                if (orderTransferRequestsViewModel.IsAccepted == true)
                {
                    customerOrderListViewModel.NotificationCode = "DRV-005";
                    customerOrderListViewModel.Title = "Request Accepted";
                    customerOrderListViewModel.Message = "Please wait to Transfer Order";
                    customerOrderListViewModel.RequestedQuantity = Result.PartialQuantity;
                    customerOrderListViewModel.email = Result.Email;
                }
                else
                {

                    customerOrderListViewModel.NotificationCode = "DRV-005";
                    customerOrderListViewModel.Title = "Request Reject";
                    customerOrderListViewModel.Message = "Please wait, Admin will Conatct you";
                    customerOrderListViewModel.RequestedQuantity = Result.PartialQuantity;
                    customerOrderListViewModel.email = Result.Email;
                }

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
        public HttpResponseMessage CustomerOrderGroupTransferFromDriverAll(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<TransferFromDriverViewModel>().ReadStoredProcedure("CustomerOrderGroupTransferFromDriverAll @CompnayId",
                            new SqlParameter("CompnayId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }
                    ).ToList();

                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    Result = Result.Where(x => x.FromDriver.ToLower().Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                               x.ToDriver.Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                               x.CreatedDate.Contains(pagingparametermodel.SerachKey)
                                               ).ToList();
                }


                int count = Result.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = Result.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if (Result.Count < 1)
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
        public HttpResponseMessage OrderTransferRequestsAllByDriverId(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<OrderTransferRequestsViewModel>().ReadStoredProcedure("OrderTransferRequestsAllByDriverId @DriverId",
                            new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.DriverId }
                    ).ToList();


                if (pagingparametermodel.SerachKey != null && pagingparametermodel.SerachKey != "")
                {
                    Result = Result.Where(
                                                    x => x.DriverName.ToLower().Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                                         x.RequestTypeName.Contains(pagingparametermodel.SerachKey.ToLower()) ||
                                                         x.CreatedDate.Contains(pagingparametermodel.SerachKey)
                                                    ).ToList();
                }


                int count = Result.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = Result.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if (Result.Count < 1)
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
        public HttpResponseMessage CustomerOrderGroupTransferDriverByToDriverId(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<OrderTransferRequestsViewModel>().ReadStoredProcedure("CustomerOrderGroupTransferDriverByToDriverId @DriverId",
                            new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.DriverId }
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
        public HttpResponseMessage CustomerOrderGroupTransferComplete(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderGroupTransferComplete @OrderTransferRequestId,@TransferdQuantity,@DriverId",
                            new SqlParameter("OrderTransferRequestId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.OrderTransferRequestId },
                            new SqlParameter("TransferdQuantity", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.TransferdQuantity },
                            new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = orderTransferRequestsViewModel.DriverId }
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

    }
}
