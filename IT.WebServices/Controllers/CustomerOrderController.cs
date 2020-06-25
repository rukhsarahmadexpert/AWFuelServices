using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
using IT.WebServices.Models;
using Newtonsoft.Json;
using SendingPushNotifications.Logics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class CustomerOrderController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage All(PagingParameterModel pagingparametermodel)
        {

            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("CustomerOrderAll @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingparametermodel.CompanyId }
                    ).ToList();

                int count = CustomerOrderList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = CustomerOrderList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
        public HttpResponseMessage Add([FromBody] CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {
                var vehicleAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderAdd @OrderQuantity, @DriverId, @VehicleId, @CompanyId, @RequestThrough, @CreatedBy",
                 new SqlParameter("OrderQuantity", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.OrderQuantity }
               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.DriverId }
               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
               , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.CompanyId }
               , new SqlParameter("RequestThrough", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.RequestThrough ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.CreatedBy }
                 ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(vehicleAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetTop()
        {
            try
            {
                var CustomerOrderNoteList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerOrder5UnRead"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderNoteList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllUnreadOrder()
        {
            try
            {
                var CustomerUnreadOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerUnreadOrderList"
                    ).ToList();

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = Int32.MaxValue
                };
                userRepsonse.Success(serializer.Serialize(CustomerUnreadOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Customer Order Viewed
        [HttpPost]
        public HttpResponseMessage OrderViewed(SearchViewModel searchViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("customerOrderViewed @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
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
        public HttpResponseMessage OrderById(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                var result = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("OrderById @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(result));
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
                var result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("customerOrderToDriverAsign @DriverId,@OrderId,@CreatedBy,@LocationLink,@Note",
                    new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.DriverId },
                    new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId },
                    new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.CreatedBy },
                    new SqlParameter("LocationLink", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.LocationLink },
                    new SqlParameter("Note", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.Note }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Customer Get Asigned Order
        [HttpPost]
        public HttpResponseMessage GetAsignedOrder(int Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("GetAsignedOrder @CompanyId",
                    new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //AWFuel Get Asigned Order
        [HttpPost]
        public HttpResponseMessage GetAsignedOrderAWFuel()
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("GetAsignedOrderAWFuel"
                    ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //AWFuel Admin
        [HttpPost]
        public HttpResponseMessage GetDeliverdOrder(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("CustomerOrderDeliverd"
                    ).ToList();


                int count = CustomerOrderList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = CustomerOrderList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
        public HttpResponseMessage GetDeliverdOrderDetails(int Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<OrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderDeliverdDetails @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ViewDeliveryInfo(string Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<OrderDetailsViewModel>().ReadStoredProcedure("ViewDeliveryInfo @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Deliverd Order By Company Id To each company
        [HttpPost]
        public HttpResponseMessage OrderReceived(int Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("OrderReceived @CompanyId",
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage ViewedNotifyCustomer(int Id)
        {
            try
            {
                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderNotifyViewd @OrderId",
                    new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = Id }
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
        public HttpResponseMessage GetAsignedOrderByDriver(int Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("GetAsignedOrderByDriver @DriverId",
                   new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetDeliverOrderByDriver(int Id)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("GetDeliverOrderByDriver @DriverId",
                   new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CustomerOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderDeliverdUpdate(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDeliverdUpdate @DriverId,@DeliverQuantity",
                   new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Id },
                   new SqlParameter("DeliverQuantity", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Quantity }
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
        public HttpResponseMessage CusOrderDelUpdateCusConfirmed(int Id)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CusOrderDelUpdateCusConfirmed @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
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

        #region Customer Group Order        
        //Direct sale Add
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupDirectSaleAdd([FromBody] CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {

                int OrderId = 0;
                var number = OrderNumber();
                if (number != null)
                {
                    int NumberNew = Convert.ToInt32(number) + 1;
                    customerOrderListViewModel.CustomerOrderNumber = NumberNew.ToString();
                }
                else
                {
                    number = "1001";
                    customerOrderListViewModel.CustomerOrderNumber = number;
                }
                try
                {
                    var ResDriver = new DriverModel();
                    if (customerOrderListViewModel.customerOrderViewModels[0].DriverId == 0)
                    {
                        ResDriver = unitOfWork.GetRepositoryInstance<DriverModel>().ReadStoredProcedure("DirectSaleDriverAdd @DriverName, @ContactNumber",
                         new SqlParameter("DriverName", System.Data.SqlDbType.VarChar) { Value = customerOrderListViewModel.DriverName ?? (object)DBNull.Value }
                        ,new SqlParameter("ContactNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.ContactNumber ?? (Object)DBNull.Value }
                       ).FirstOrDefault();

                        customerOrderListViewModel.customerOrderViewModels[0].DriverId = ResDriver.DriverId;
                    }

                    var OrderAdd = unitOfWork.GetRepositoryInstance<CustomerOrderListViewModel>().ReadStoredProcedure("CustomerOrderGroupDirectSaleAdd  @DeliverdQuantity, @CreatedBy,@CustomerNote,@CustomerOrderId,@CustomerOrderNumber,@RequestThrough",
                            //new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerId }
                            new SqlParameter("DeliverdQuantity", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.DeliverdQuantity }
                          , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }
                          , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerNote ?? (object)DBNull.Value }
                          , new SqlParameter("CustomerOrderId", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerOrderNumber ?? (object)DBNull.Value }
                          , new SqlParameter("CustomerOrderNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerOrderNumber ?? (object)DBNull.Value }
                          , new SqlParameter("RequestThrough", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.RequestThrough ?? (object)DBNull.Value }
                    ).FirstOrDefault();

                    OrderId = OrderAdd.Id;

                    if (OrderId > 0)
                    {
                        foreach (CustomerOrderViewModel customerOrderViewModel in customerOrderListViewModel.customerOrderViewModels)
                        {
                            try
                            {
                                var OrderDetailsAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsGroupDirectSaleAdd @OrderId, @VehicleId, @DriverId, @RequestedQuantity,@Comments,@ProductId,@UnitPrice,@VATPercentage,@VatAmount,@TotalAmount",
                                 new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
                               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.DriverId }
                               , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.OrderQuantity }
                               , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.ProductId }
                               , new SqlParameter("UnitPrice", System.Data.SqlDbType.Decimal) { Value = customerOrderViewModel.UnitPrice }
                               , new SqlParameter("VATPercentage", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VATPercentage }
                               , new SqlParameter("VatAmount", System.Data.SqlDbType.Money) { Value = customerOrderViewModel.VatAmount }
                               , new SqlParameter("TotalAmount", System.Data.SqlDbType.Money) { Value = customerOrderViewModel.TotalAmount }
                               ).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                var RemoveParrentOrder = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDelete @OrderId",
                                     new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                                    ).FirstOrDefault();

                                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                            }
                        }

                        //Send Notification
                        customerOrderListViewModel.Title = "Direct Sale";
                        customerOrderListViewModel.NotificationCode = "ADM-001";
                        customerOrderListViewModel.Message = "Direct Sale has been done.";

                        int Res = AdminNotificaton(customerOrderListViewModel);

                        var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("CustomerOrderLocationAdd @OrderId,@longitude,@latitude,@LocationFullUrl,@PickingPoint,@CreatedBy",
                          new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                        , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.longitude ?? (object)DBNull.Value }
                        , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.latitude ?? (object)DBNull.Value }
                        , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.LocationFullUrl ?? "" }
                        , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.PickingPoint ?? "" }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }

                       ).FirstOrDefault();

                        OrderAdd.latitude = Result.latitude;
                        OrderAdd.longitude = Result.longitude;
                        OrderAdd.LocationFullUrl = Result.LocationFullUrl;
                        OrderAdd.PickingPoint = Result.PickingPoint;

                        var OrderAddedDetails = unitOfWork.GetRepositoryInstance<CustomerOrderViewModel>().ReadStoredProcedure("OrderAddedDetails @OrderId",
                        new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                        ).ToList();

                        OrderAdd.customerOrderViewModels = OrderAddedDetails;

                        var checkIfDriverLoginGetVehicleId = unitOfWork.GetRepositoryInstance<DriverLoginHistoryViewModelForAdmin>().ReadStoredProcedure("AWFDriverGetVehicleIdByLogin @CreatedBy",
                            new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }
                         ).FirstOrDefault();

                        List<StorageViewModel> storageViewModels = new List<StorageViewModel>();

                        StorageController storageController = new StorageController();

                        var uniqueId = Guid.NewGuid().ToString();
                        for (int i = 0; i < 2; i++)
                        {
                            StorageViewModel storageViewModel = new StorageViewModel
                            {
                                CreatedBy = Result.CreatedBy,
                                SiteId = 0,
                                LPOId = 0,
                                Decription = "Direct Sale",
                                ProductId = customerOrderListViewModel.customerOrderViewModels[0].ProductId,
                                uniques = uniqueId
                            };
                            if (i == 0)
                            {
                                storageViewModel.Source = "client vehicle";
                                storageViewModel.Action = true;
                                storageViewModel.StockOut = 0;
                                storageViewModel.StockIn = customerOrderListViewModel.customerOrderViewModels[0].OrderQuantity;
                                storageViewModel.ClientVehicleId = customerOrderListViewModel.customerOrderViewModels[0].VehicleId;
                                storageViewModel.VehicleId = 0;
                            }
                            else
                            {
                                storageViewModel.Action = false;
                                storageViewModel.StockOut = customerOrderListViewModel.customerOrderViewModels[0].OrderQuantity;
                                storageViewModel.StockIn = 0;
                                storageViewModel.ClientVehicleId = 0;
                                if (checkIfDriverLoginGetVehicleId != null && checkIfDriverLoginGetVehicleId.VehicleId > 0)
                                {
                                    storageViewModel.Source = "admin vehicle";
                                    storageViewModel.VehicleId = checkIfDriverLoginGetVehicleId.VehicleId;
                                }
                                else
                                {
                                    storageViewModel.Source = "site";
                                    storageViewModel.VehicleId = 0;
                                    if (checkIfDriverLoginGetVehicleId != null)
                                    {
                                        storageViewModel.SiteId = checkIfDriverLoginGetVehicleId.DriverLoginId;
                                    }
                                    else
                                    {
                                        storageViewModel.SiteId = 1;
                                    }
                                }

                            }

                            storageViewModels.Add(storageViewModel);
                        }

                        var Results = storageController.StorageAddNew(storageViewModels);

                        userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                        return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    }
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception ex)
                {
                    var RemoveParrentOrder = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDelete @OrderId",
                     new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                     ).FirstOrDefault();

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerGroupOrderAdd([FromBody] CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                int OrderId = 0;
                var number = OrderNumber();
                if (number != null || number != "")
                {
                    int NumberNew = Convert.ToInt32(number) + 1;
                    customerOrderListViewModel.CustomerOrderNumber = NumberNew.ToString();
                }
                else
                {
                    number = "1001";
                    customerOrderListViewModel.CustomerOrderNumber = number;
                }

                try
                {
                    var OrderAdd = unitOfWork.GetRepositoryInstance<CustomerOrderListViewModel>().ReadStoredProcedure("CustomerOrderGroupAdd @CustomerId, @RequestedQuantity, @DeliverdQuantity, @OrderProgress,  @CreatedBy,@CustomerNote,@CustomerOrderId,@DeliveryNoteNumber,@CustomerOrderNumber,@RequestThrough,@SiteId,@IsBulk,@IsSelfPickup",
                            new SqlParameter("CustomerId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerId }
                          , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.RequestedQuantity }
                          , new SqlParameter("DeliverdQuantity", System.Data.SqlDbType.Int) { Value = 0 }
                          , new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = "Order Created" }
                          , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }
                          , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerNote ?? (object)DBNull.Value }
                          , new SqlParameter("CustomerOrderId", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerOrderNumber ?? (object)DBNull.Value }
                          , new SqlParameter("DeliveryNoteNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.DeliveryNoteNumber ?? (object)DBNull.Value }
                          , new SqlParameter("CustomerOrderNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerOrderNumber ?? (object)DBNull.Value }
                          , new SqlParameter("RequestThrough", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.RequestThrough ?? (object)DBNull.Value }
                          , new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.SiteId }
                          , new SqlParameter("IsBulk", System.Data.SqlDbType.Bit) { Value = customerOrderListViewModel.IsBulk }
                          , new SqlParameter("IsSelfPickup", System.Data.SqlDbType.Bit) { Value = customerOrderListViewModel.IsSelfPickup }
                    ).FirstOrDefault();

                    OrderId = OrderAdd.Id;

                    if (OrderId > 0)
                    {
                        foreach (CustomerOrderViewModel customerOrderViewModel in customerOrderListViewModel.customerOrderViewModels)
                        {
                            try
                            {
                                var OrderDetailsAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsGroupAdd @OrderId, @VehicleId, @DriverId, @RequestedQuantity,@Comments,@ProductId",
                                 new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
                               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.DriverId }
                               , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.OrderQuantity }
                               , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.ProductId }
                               ).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                var RemoveParrentOrder = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDelete @OrderId",
                                     new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                                    ).FirstOrDefault();

                                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                            }
                        }

                        if (customerOrderListViewModel.IsSend == true)
                        {
                            //Send Notification
                            customerOrderListViewModel.Title = "Order Created";
                            customerOrderListViewModel.NotificationCode = "ADM-001";
                            customerOrderListViewModel.Message = "new Order Created.";

                            int Res = AdminNotificaton(customerOrderListViewModel);
                        }

                        var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("CustomerOrderLocationAdd @OrderId,@longitude,@latitude,@LocationFullUrl,@PickingPoint,@CreatedBy",
                          new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                        , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.longitude ?? (object)DBNull.Value }
                        , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.latitude ?? (object)DBNull.Value }
                        , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.LocationFullUrl ?? (object)DBNull.Value }
                        , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.PickingPoint ?? (object)DBNull.Value }
                        , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }

                       ).FirstOrDefault();

                        OrderAdd.latitude = Result.latitude;
                        OrderAdd.longitude = Result.longitude;
                        OrderAdd.LocationFullUrl = Result.LocationFullUrl;
                        OrderAdd.PickingPoint = Result.PickingPoint;

                        userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                        return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    }
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception ex)
                {
                    var RemoveParrentOrder = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDelete @OrderId",
                     new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderId }
                     ).FirstOrDefault();

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string OrderNumber()
        {
            try
            {
                var ExpNumberData = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("CustomerOrderNumber"
                ).FirstOrDefault();

                if (ExpNumberData.Result != null)
                {
                    return ExpNumberData.Result.ToString();
                }
                else
                {
                    return "1001";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Remaining Booking can find here
        [HttpPost]
        public HttpResponseMessage CustomerGroupOrderById(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("No Data Exist on This Id"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {

                    var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerOrderGroupViewModel>().ReadStoredProcedure("CustomerOrderGroupById @Id",
                       new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                       ).FirstOrDefault();

                    var customerGroupOrderDetails = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderDetailsGroupByOrderId @Id",
                      new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                      ).ToList();

                    var Documents = unitOfWork.GetRepositoryInstance<UploadDocumentsViewModel>().ReadStoredProcedure("UploadDocumentsGetByRespectiveId @Id,@Flag"
                       , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                       , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "Order" }
                       ).ToList();

                    var customerRemainingBooking = unitOfWork.GetRepositoryInstance<CustomerRemainingBookingViewModel>().ReadStoredProcedure("CustomerBookingByCompanyId @CompanyId"
                       , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = customerGroupOrder.CompanyId }
                        ).ToList();

                    var updatereason = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionGet @Id,@Flag"
                      , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                      , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = "DeliveryOrder" }
                      ).ToList();

                    customerGroupOrder.updateReasonDescriptionViewModels = updatereason;
                    customerGroupOrder.customerRemainingBookingViewModels = customerRemainingBooking;
                    customerGroupOrder.uploadDocumentsViewModels = Documents;
                    customerGroupOrder.customerGroupOrderDetailsViewModels = customerGroupOrderDetails;

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //to Delete Record From Details
        [HttpPost]
        public HttpResponseMessage CustomerOrderDetailsDelete(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                var ExpNumberData = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsDelete @Id,@DetailsId,@NewQuantity",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Id },
                     new SqlParameter("DetailsId", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.OrderAsignId },
                     new SqlParameter("NewQuantity", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Quantity }
                ).FirstOrDefault();

                if (ExpNumberData.Result > 0)
                {
                    userRepsonse.Success(ExpNumberData.Result.ToString());
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("Failed"));
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
        public HttpResponseMessage CustomerGroupOrderDetailsByOrderId(int Id)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderDetailsGroupByOrderId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Get Accept and Deliverd Group order Details By Admin
        [HttpPost]
        public HttpResponseMessage GroupOrderAcDetDetailsByOrderId(int Id)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("GroupOrderAcDetDetailsByOrderId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderDetailsGroupAsignedByOrderId(int Id)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderDetailsGroupAsignedByOrderId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerGroupOrderUpdate([FromBody] CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                var OrderAdd = unitOfWork.GetRepositoryInstance<CustomerOrderListViewModel>().ReadStoredProcedure("CustomerOrderGroupUpdate @Id, @RequestedQuantity, @UpdatedBy,@CustomerNote",
                        new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.Id }
                      , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.RequestedQuantity }
                      , new SqlParameter("UpdatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }
                      , new SqlParameter("CustomerNote", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.CustomerNote ?? (object)DBNull.Value }
                      ).FirstOrDefault();

                if (OrderAdd.Id > 0)
                {
                    foreach (CustomerOrderViewModel customerOrderViewModel in customerOrderListViewModel.customerOrderViewModels)
                    {
                        try
                        {

                            if (customerOrderViewModel.Id > 0)
                            {
                                var OrderDetailsAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsGroupUpdate @Id, @VehicleId, @DriverId, @RequestedQuantity,@Comments,@ProductId",
                                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.Id }
                               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
                               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.DriverId }
                               , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.OrderQuantity }
                               , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.ProductId }

                               ).FirstOrDefault();
                            }
                            else
                            {
                                var OrderDetailsAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsGroupAdd @OrderId, @VehicleId, @DriverId, @RequestedQuantity,@Comments,@ProductId",
                                 new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = OrderAdd.Id }
                               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
                               , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.DriverId }
                               , new SqlParameter("RequestedQuantity", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.OrderQuantity }
                               , new SqlParameter("Comments", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                               , new SqlParameter("ProductId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.ProductId }
                               ).FirstOrDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                            return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                        }
                    }

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }

                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("CustomerOrderLocationUpdate @OrderId,@longitude,@latitude,@LocationFullUrl,@PickingPoint",
                     new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.Id }
                   , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.longitude ?? (object)DBNull.Value  }
                   , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.latitude ?? (object)DBNull.Value }
                   , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.LocationFullUrl ?? (object)DBNull.Value }
                   , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.PickingPoint ?? (object)DBNull.Value }

                  ).FirstOrDefault();

                OrderAdd.latitude = Result.latitude;
                OrderAdd.longitude = Result.longitude;
                OrderAdd.LocationFullUrl = Result.LocationFullUrl;
                OrderAdd.PickingPoint = Result.PickingPoint;

                if (customerOrderListViewModel.updateReasonDescriptionViewModel != null)
                {
                    UpdateReason updateReason = new UpdateReason();
                    if (customerOrderListViewModel.Id > 0)
                    {
                        var result = updateReason.Add(customerOrderListViewModel.updateReasonDescriptionViewModel);
                    }
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAdd));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderLocationUpdate(CustomerOrderLocationViewModel customerOrderLocationViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderLocationViewModel>().ReadStoredProcedure("CustomerOrderLocationUpdate @OrderId,@longitude,@latitude,@LocationFullUrl,@PickingPoint,@SiteId",
                        new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.OrderId }
                      , new SqlParameter("longitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.longitude ?? (object)DBNull.Value  }
                      , new SqlParameter("latitude", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.latitude ?? (object)DBNull.Value   }
                      , new SqlParameter("LocationFullUrl", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.LocationFullUrl ?? (object)DBNull.Value   }
                      , new SqlParameter("PickingPoint", System.Data.SqlDbType.NVarChar) { Value = customerOrderLocationViewModel.PickingPoint ?? (object)DBNull.Value }
                      , new SqlParameter("SiteId", System.Data.SqlDbType.Int) { Value = customerOrderLocationViewModel.SiteId }
                     ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Admin CustomerOrder Api with Bragi
        #region Admin Section

        [HttpPost]
        public HttpResponseMessage GetAllCustomerOrderGroupByAdmin(CustomerOrderGroupViewModel customerOrderGroupViewModel)
        {
            try
            {
                var CustomerUnreadOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerOrderGroupAll @OrderPrgress,@IsSend,@IsRead,@CompanyId,@IsTrue"
                      , new SqlParameter("OrderPrgress", System.Data.SqlDbType.NVarChar) { Value = customerOrderGroupViewModel.OrderProgress ?? (object)DBNull.Value }
                      , new SqlParameter("IsSend", System.Data.SqlDbType.Bit) { Value = customerOrderGroupViewModel.IsSend }
                      , new SqlParameter("IsRead", System.Data.SqlDbType.Bit) { Value = customerOrderGroupViewModel.IsRead }
                      , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = customerOrderGroupViewModel.CompanyId }
                      , new SqlParameter("IsTrue", System.Data.SqlDbType.Bit) { Value = customerOrderGroupViewModel.IsTrue }
                    ).ToList();

                if (customerOrderGroupViewModel.serachKey != null && customerOrderGroupViewModel.serachKey != "" && customerOrderGroupViewModel.SearchFlage != "" && customerOrderGroupViewModel.SearchFlage != null)
                {
                    if (customerOrderGroupViewModel.SearchFlage == "ByCompanyName")
                    {
                        CustomerUnreadOrderList = CustomerUnreadOrderList.Where(x => x.Company.ToLower().Contains(customerOrderGroupViewModel.serachKey.ToLower())).ToList();
                    }
                    else if (customerOrderGroupViewModel.SearchFlage == "ByDeliverNumber")
                    {
                        CustomerUnreadOrderList = CustomerUnreadOrderList.Where(x => x.DeliveryNoteNumber.ToLower().Contains(customerOrderGroupViewModel.serachKey.ToLower())).ToList();
                    }
                    else if (customerOrderGroupViewModel.SearchFlage == "ByOrderNumber")
                    {
                        CustomerUnreadOrderList = CustomerUnreadOrderList.Where(x => x.CustomerOrderNumber.ToLower().Contains(customerOrderGroupViewModel.serachKey.ToLower())).ToList();
                    }
                    else if (customerOrderGroupViewModel.SearchFlage == "ByDateRange")
                    {
                        if (customerOrderGroupViewModel.FromDate != null && customerOrderGroupViewModel.FromDate != "" && customerOrderGroupViewModel.ToDate != null && customerOrderGroupViewModel.ToDate != "")
                        {

                            CustomerUnreadOrderList = CustomerUnreadOrderList.Where(x => Convert.ToDateTime(x.CreateDates) >= Convert.ToDateTime(customerOrderGroupViewModel.FromDate) && Convert.ToDateTime(x.CreateDates) <= Convert.ToDateTime(customerOrderGroupViewModel.ToDate)).ToList();
                        }
                    }
                    else if (customerOrderGroupViewModel.SearchFlage == "ByDateRangeAndCompany")
                    {
                        if (customerOrderGroupViewModel.FromDate != null && customerOrderGroupViewModel.FromDate != "" && customerOrderGroupViewModel.ToDate != null && customerOrderGroupViewModel.ToDate != "")
                        {
                            CustomerUnreadOrderList = CustomerUnreadOrderList.Where(x => x.Company.ToLower().Contains(customerOrderGroupViewModel.serachKey.ToLower()) && Convert.ToDateTime(x.CreateDates) >= Convert.ToDateTime(customerOrderGroupViewModel.FromDate) && Convert.ToDateTime(x.CreateDates) <= Convert.ToDateTime(customerOrderGroupViewModel.ToDate)).ToList();
                        }
                    }
                }

                int count = CustomerUnreadOrderList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = customerOrderGroupViewModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = customerOrderGroupViewModel.PageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);


                // Returns List of Customer after applying Paging   
                var items = CustomerUnreadOrderList.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if (CustomerUnreadOrderList.Count < 1)
                {
                    userRepsonse.Success(null);
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));
                }
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //serializer.MaxJsonLength = Int32.MaxValue;
                //userRepsonse.Success(serializer.Serialize(CustomerUnreadOrderList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Get Direct Order List
        [HttpPost]
        public HttpResponseMessage CustomerOrderAllDirectSale(PagingParameterModel pagingParameterModel)
        {
            try
            {
                var customerOrderListDirectSale = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerOrderGroupDirectSale"
                                                 ).ToList();

                int count = customerOrderListDirectSale.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingParameterModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingParameterModel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
                
                // Returns List of Customer after applying Paging   
                var items = customerOrderListDirectSale.OrderByDescending(x => x.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(items));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage customerOrderGroupViewed(SearchViewModel searchViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("customerOrderGroupViewed @Id",
                    new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
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
        public HttpResponseMessage CustomerOrderRejectAcceptByAdmin(CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderRejectByAdmin @Id,@Status,@Description,@CreatedBy"
                   , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.Id }
                   , new SqlParameter("Status", System.Data.SqlDbType.Bit) { Value = customerOrderViewModel.Status }
                   , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                   , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.CreatedBy }
                   ).FirstOrDefault();

                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();

                if (customerOrderViewModel.Status == true)
                {
                    customerOrderListViewModel.NotificationCode = "CUS-001";
                    customerOrderListViewModel.Title = "Order Accepted";
                    customerOrderListViewModel.Message = "Admin has accepted your order successfully.";
                    customerOrderListViewModel.RequestedQuantity = 0;
                    customerOrderListViewModel.CustomerId = Result.Result;

                    //Send Notification
                    CustomerNotification(customerOrderListViewModel);
                }
                else
                {
                    customerOrderListViewModel.NotificationCode = "CUS-002";
                    customerOrderListViewModel.Title = "Unfortunately! Order Rejected";
                    customerOrderListViewModel.Message = "Your Order has been rejected by admin";
                    customerOrderListViewModel.RequestedQuantity = 0;
                    customerOrderListViewModel.CustomerId = Result.Result;

                    //Send Notification
                    CustomerNotification(customerOrderListViewModel);
                }

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
        public HttpResponseMessage CustomerOrderAcceptRejectedOrderByAdmin(CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderAcceptRejectedOrderByAdmin @Id,@Description,@CreatedBy"
                   , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.Id }
                   , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = customerOrderViewModel.Comments ?? (object)DBNull.Value }
                   , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.CreatedBy }
                   ).FirstOrDefault();

                CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel
                {
                    NotificationCode = "CUS-001",
                    Title = "Admin Accept Order",
                    Message = "Admin acccept order Rejected Order",
                    RequestedQuantity = 0,
                    CustomerId = Result.Result
                };
                //Send Notification
                CustomerNotification(customerOrderListViewModel);
                
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Result.Result));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Direct sale customer Order Group list
        public HttpResponseMessage CustomerOrderGroupDirectSale()
        {
            try
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(1));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //End with Bragi
        #endregion

        [HttpPost]
        public HttpResponseMessage rejectedOrderDetails([FromBody] RejectedOrderDetails rejectedOrderDetails)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("RejectedOrderDetailsAdd @OrderId, @Description, @CreatedBy",
                 new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = rejectedOrderDetails.OrderId }
               , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = rejectedOrderDetails.Description ?? (object)DBNull.Value }
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = rejectedOrderDetails.CreatedBy }
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
        public HttpResponseMessage CustomerOrderRejectDetailsById(int Id)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<RejectedOrderDetails>().ReadStoredProcedure("RejectedOrderDetailsById @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
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
        public HttpResponseMessage AcceptedOrdersAdmin()
        {
            try
            {
                var CustomerUnreadOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("AcceptedOrdersAdmin"
                    ).ToList();

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = Int32.MaxValue,
                };
                userRepsonse.Success(serializer.Serialize(CustomerUnreadOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeliverdOrderAdmin()
        {
            try
            {
                var CustomerUnreadOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("DeliverdOrderAdmin"
                    ).ToList();

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = Int32.MaxValue
                };
                userRepsonse.Success(serializer.Serialize(CustomerUnreadOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupAsignedDriverAdd([FromBody] CustomerOrderListViewModel customerOrderListViewModel)
        {

            bool Flage = true;

            var BookingId = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerBookingTopOneOpen @OrderId",
                               new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerOrderId }
                               ).FirstOrDefault();

            var checkQuantity = new SingleIntegerValueResult();
            if (BookingId.Result > 0)
            {
                checkQuantity = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerBookingQuantityToDeliver @BookingId,@DeliveryQuantity",
                              new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = BookingId.Result },
                              new SqlParameter("DeliveryQuantity", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.RequestedQuantity }
                              ).FirstOrDefault();

                if (checkQuantity.TotalCount < 0)
                {
                    if (checkQuantity.Id > 0)
                    {
                        Flage = false;
                        for (int i = 0; i < 2; i++)
                        {
                            int DeliverQuantity = 0; int BookingIds = 0;
                            if (i == 0)
                            {
                                BookingIds = BookingId.Result;
                                DeliverQuantity = customerOrderListViewModel.RequestedQuantity - Math.Abs(checkQuantity.TotalCount);
                            }
                            else
                            {
                                BookingIds = checkQuantity.Id;
                                DeliverQuantity = Math.Abs(checkQuantity.TotalCount);
                            }

                            var NewAutoBookingAndOrderCreation = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderGroupAddAuto @OrderId,@DeliveryQuantity,@DeliveryNoteNumber,@UnitPrice,@VAT,@TotalAmount,@BookingId",
                                       new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerOrderId },
                                       new SqlParameter("DeliveryQuantity", System.Data.SqlDbType.Int) { Value = DeliverQuantity },
                                       new SqlParameter("DeliveryNoteNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.DeliveryNoteNumber },
                                       new SqlParameter("UnitPrice", System.Data.SqlDbType.Money) { Value = customerOrderListViewModel.UnitPrice },
                                       new SqlParameter("VAT", System.Data.SqlDbType.Money) { Value = customerOrderListViewModel.RequestedQuantity },
                                       new SqlParameter("TotalAmount", System.Data.SqlDbType.Money) { Value = customerOrderListViewModel.RequestedQuantity },
                                       new SqlParameter("BookingId", System.Data.SqlDbType.Money) { Value = BookingIds }
                                       ).FirstOrDefault();

                            var SuccessBookingStatusChanges = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerBookingIsOpenToFalse @BookingId",
                                       new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = BookingIds }
                                       ).FirstOrDefault();
                        }                        
                    }
                    else
                    {
                        userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("Currently you have no booking availible"));
                        return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    }
                }
                int Count = 0;
                try
                {
                    var OrderAsignAdd = unitOfWork.GetRepositoryInstance<CustomerOrderListViewModel>().ReadStoredProcedure("CustomerOrderGroupAsignedDriverAdd @OrderId, @TotalQuantity, @DriverId,@CreatedBy,@VehicleId,@DeliveryNoteNumber,@BookingId,@IsforSite,@SiteId,@Flage",
                                new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerOrderId }
                              , new SqlParameter("TotalQuantity", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.RequestedQuantity }
                              , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.DriverId }
                              , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CreatedBy }
                              , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.VehicleId }
                              , new SqlParameter("DeliveryNoteNumber", System.Data.SqlDbType.NVarChar) { Value = customerOrderListViewModel.DeliveryNoteNumber ?? (object)DBNull.Value }
                              , new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = BookingId.Result }
                              , new SqlParameter("IsforSite", System.Data.SqlDbType.Bit) { Value = customerOrderListViewModel.IsforSite }
                              , new SqlParameter("SiteId", System.Data.SqlDbType.Bit) { Value = customerOrderListViewModel.SiteId }
                              , new SqlParameter("Flage", System.Data.SqlDbType.Bit) { Value = Flage }

                          ).FirstOrDefault();

                    var ResultId = OrderAsignAdd.Id;

                    if (ResultId > 0 && OrderAsignAdd.Note != "Order already asigned")
                    {
                        foreach (CustomerOrderViewModel customerOrderViewModel in customerOrderListViewModel.customerOrderViewModels)
                        {
                            try
                            {
                                var OrderDetailsAdd = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderGroupAsignedDetailsAdd @OrderDriverAsignId, @CustomerOrderId, @CustomerOrderDetailId,@VehicleId",
                                 new SqlParameter("OrderDriverAsignId", System.Data.SqlDbType.Int) { Value = ResultId }
                               , new SqlParameter("CustomerOrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerOrderId }
                               , new SqlParameter("CustomerOrderDetailId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.Id }
                               , new SqlParameter("VehicleId", System.Data.SqlDbType.Int) { Value = customerOrderViewModel.VehicleId }
                                ).FirstOrDefault();

                                var Success = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderOrderProgresChange @OrderId",
                                    new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerOrderListViewModel.CustomerOrderId }
                                    ).FirstOrDefault();
                                if (Flage == true)
                                {
                                    var SuccessBookingStatusChanges = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerBookingIsOpenToFalse @BookingId",
                                       new SqlParameter("BookingId", System.Data.SqlDbType.Int) { Value = BookingId.Result }
                                       ).FirstOrDefault();
                                }

                                Count = Success.TotalCount;

                                //customerOrderListViewModel.NotificationCode = "CUS-003";
                                //customerOrderListViewModel.Title = "Admin Assign Order";
                                //customerOrderListViewModel.Message = "Admin Assign Order to Driver";
                                //customerOrderListViewModel.RequestedQuantity = 0;
                                //customerOrderListViewModel.CustomerId = Success.Result;
                                ////Send Notification
                                //CustomerNotification(customerOrderListViewModel);
                            }
                            catch (Exception ex)
                            {
                                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                            }
                        }

                        if (Count == 0)
                        {
                            customerOrderListViewModel.Id = 0;
                        }
                        else
                        {
                            customerOrderListViewModel.Id = ResultId;
                        }

                        //  CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
                        if (customerOrderListViewModel.IsforSite == false)
                        {
                            customerOrderListViewModel.NotificationCode = "DRV-001";
                            customerOrderListViewModel.Title = "Assigned Order";
                            customerOrderListViewModel.Message = "Admin has Assign new Order to you!";
                            customerOrderListViewModel.RequestedQuantity = 0;
                            customerOrderListViewModel.email = OrderAsignAdd.email;

                            //Send Notification
                            int Res = DriverNotification(customerOrderListViewModel);
                        }
                        else
                        {
                            customerOrderListViewModel.NotificationCode = "ADM-009";
                            customerOrderListViewModel.Title = "Assigned Order";
                            customerOrderListViewModel.Message = "Admin has Assign new Order to Site!";
                            customerOrderListViewModel.RequestedQuantity = 0;
                            customerOrderListViewModel.email = OrderAsignAdd.email;

                            //Send Notification
                            int Res = AdminNotificaton(customerOrderListViewModel);

                            customerOrderListViewModel.NotificationCode = "CUS-005";
                            customerOrderListViewModel.Title = "Order Assigend";
                            customerOrderListViewModel.Message = "Your Order is assigned to site successfully, Please send your vehicle";
                            customerOrderListViewModel.RequestedQuantity = 0;
                            customerOrderListViewModel.CustomerId = BookingId.TotalCount;

                            //Send Notification to Customer
                            CustomerNotification(customerOrderListViewModel);

                        }
                        userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAsignAdd));
                        return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    }
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(OrderAsignAdd));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception ex)
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
                }
            }
            else
            {
                userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("Currently you have no booking availible"));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
        }

        #endregion

        #region Customer Order Group For Driver with Bragi

        //Customer Order Group List
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupBYDriverAsignedId(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerOrderGroupViewModel>().ReadStoredProcedure("CustomerOrderGroupBYDriverAsignedId @Id,@OrderProgress",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = pagingparametermodel.Id },
                   new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = pagingparametermodel.OrderProgress }
                   ).ToList();

                int count = customerGroupOrder.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = customerGroupOrder.OrderByDescending(x => x.CreatedDate).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

        //Site Assigned Order
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupAsignedForSite(PagingParameterModel pagingparametermodel)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerOrderSiteAssignedViewModel>().ReadStoredProcedure("CustomerOrderAssignedOrderToSites"
                    ).ToList();

                int count = customerGroupOrder.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = customerGroupOrder.OrderByDescending(x => x.CreatedDate).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

        //Customer Order Group asign by Id
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupAsignedByOrderId(SearchViewModel searchViewModel)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerOrderGroupViewModel>().ReadStoredProcedure("CustomerOrderGroupBYAsignedId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                   ).FirstOrDefault();

                var customerGroupOrderDetails = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderGroupAsignedDetailsByOrderId @Id",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                 ).ToList();

                customerGroupOrder.customerGroupOrderDetailsViewModels = customerGroupOrderDetails;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Customer Order Group Details asign by Id
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupAsignedDetailsByOrderId(int Id)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CustomerOrderGroupAsignedDetailsByOrderId @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerOrderDetailsGroupDeliveryByDriver(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
            var Storagelist = new List<StorageViewModel>();
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDetailsGroupUpDelQTY @Id,@DeliverdQuantity,@KiloMeter,@Note",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Id },
                   new SqlParameter("DeliverdQuantity", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Quantity },
                   new SqlParameter("KiloMeter", System.Data.SqlDbType.NVarChar) { Value = customerOrderDeliverVewModel.KiloMeter ?? (object)DBNull.Value },
                   new SqlParameter("Note", System.Data.SqlDbType.NVarChar) { Value = customerOrderDeliverVewModel.Note }
                   ).FirstOrDefault();

                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderGroupUpdateDeliverd @OrderDeatislId",
                   new SqlParameter("OrderDeatislId", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Id }
                    ).FirstOrDefault();

                var Result1 = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderProgresChangetoDeliverd @OrderDeatislId",
                    new SqlParameter("OrderDeatislId", System.Data.SqlDbType.Int) { Value = customerOrderDeliverVewModel.Id }
                    ).FirstOrDefault();

                if (Result1.TotalCount < 1)
                {
                    customerOrderListViewModel.NotificationCode = "ADM-003";
                    customerOrderListViewModel.Title = "Order Deliverd";
                    customerOrderListViewModel.Message = "Driver has deliverd order successfully";
                    customerOrderListViewModel.RequestedQuantity = 0;

                    //Send Notification
                    AdminNotificaton(customerOrderListViewModel);

                    customerOrderListViewModel.NotificationCode = "CUS-005";
                    customerOrderListViewModel.Title = "Order Deliverd";
                    customerOrderListViewModel.Message = "Driver has deliverd order successfully";
                    customerOrderListViewModel.RequestedQuantity = 0;
                    customerOrderListViewModel.CustomerId = Result1.Result;

                    //Send Notification
                    CustomerNotification(customerOrderListViewModel);
                }
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
        public HttpResponseMessage CustomerOrderSend(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = unitOfWork.GetRepositoryInstance<CustomerOrderListViewModel>().ReadStoredProcedure("CustomerOrderSend @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = searchViewModel.Id }
                   ).FirstOrDefault();

                if (Result != null)
                {
                    CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel {

                        NotificationCode = "ADM-001",
                        Title = "Order Created",
                        Message = "New customer order created!"
                    };
                    int Res = AdminNotificaton(customerOrderListViewModel);
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
        public HttpResponseMessage CustomerOrderAcceptedByDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();

            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderAccepted @Id,@AsignOrderId",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId },
                   new SqlParameter("AsignOrderId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderAssignId }
                   ).FirstOrDefault();


                var Result = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderProgresChangetoAccepted @OrderId",
                    new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId }
                    ).FirstOrDefault();


                customerOrderListViewModel.NotificationCode = "ADM-002";
                customerOrderListViewModel.Title = "Order Accepted";
                customerOrderListViewModel.Message = "Driver has acccepted asigned order, successfully";
                customerOrderListViewModel.RequestedQuantity = 0;

                //Send Notification to Admin
                AdminNotificaton(customerOrderListViewModel);

                if (Result.TotalCount < 1)
                {
                    customerOrderListViewModel.NotificationCode = "CUS-004";
                    customerOrderListViewModel.Title = "Order Assigend";
                    customerOrderListViewModel.Message = "Your Order is dispatched successfully";
                    customerOrderListViewModel.RequestedQuantity = 0;
                    customerOrderListViewModel.CustomerId = Res.Result;

                    //Send Notification to Customer
                    CustomerNotification(customerOrderListViewModel);
                }

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
        public HttpResponseMessage CustomerOrderGroupDetailsDriverView(int Id)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<CustomerOrderGroupViewModel>().ReadStoredProcedure("CustomerOrderGroupDetailsDriverView @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        public HttpResponseMessage CustomerOrderGroupRejectByDriver(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                var Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderGroupRejectByDriver @OrderId,@DriverId,@OrderAsignId,@Description",
                   new SqlParameter("OrderId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderId }
                  , new SqlParameter("DriverId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.DriverId }
                  , new SqlParameter("OrderAsignId", System.Data.SqlDbType.Int) { Value = customerNoteOrderViewModel.OrderAssignId }
                  , new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = customerNoteOrderViewModel.Note }
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

        #endregion

        #region Customer Group Order Status

        //CustomerOrder Group All Asigned To Driver
        [HttpPost]
        public HttpResponseMessage CustomerOrderGroupAllAsignedToDriver()
        {
            try
            {
                var CustomerUnreadOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerOrderGroupAllAsignedToDriver"
                    ).ToList();

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = Int32.MaxValue
                };
                userRepsonse.Success(serializer.Serialize(CustomerUnreadOrderList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //CustomerOrder Group All Company 
        [HttpPost]
        public HttpResponseMessage CustomerOrderAllByCompanyId(PagingParameterModel pagingParameterModel)
        {
            try
            {
                var CustomerOrderList = unitOfWork.GetRepositoryInstance<CustomerNoteOrderViewModel>().ReadStoredProcedure("CustomerOrderAllByCompanyId @CompanyId,@OrderProgress,@IsSent",
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = pagingParameterModel.CompanyId },
                   new SqlParameter("OrderProgress", System.Data.SqlDbType.NVarChar) { Value = pagingParameterModel.OrderProgress },
                   new SqlParameter("IsSent", System.Data.SqlDbType.Bit) { Value = pagingParameterModel.IsSend }
                   ).ToList();

                int count = CustomerOrderList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingParameterModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingParameterModel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = CustomerOrderList.OrderByDescending(x => x.OrderId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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

                if(CustomerOrderList.Count > 0)
                {
                    items[0].TotalRows = TotalCount;
                }

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
        public HttpResponseMessage CusOrderDetGrpByOdrIdForCustomer(int Id)
        {
            try
            {
                var customerGroupOrder = unitOfWork.GetRepositoryInstance<CustomerGroupOrderDetailsViewModel>().ReadStoredProcedure("CusOrderDetGrpByOdrIdForCustomer @Id",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
                   ).ToList();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerGroupOrder));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        #endregion

        #region Message
        [HttpPost]
        public HttpResponseMessage CustomerDeliverdOrderUpdate(int Id)
        {
            try
            {
                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerDeliverdOrderUpdate @Id",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
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
        #endregion

        [HttpPost]
        public HttpResponseMessage AllowanceTypeAdd(AWFuelSalaryViewModel aWFuelSalaryViewModel)
        {
            try
            {
                var res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerDeliverdOrderUpdate @Id",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = aWFuelSalaryViewModel.Id }
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

        //Get Driver And Vehicle list in single api
        [HttpPost]
        public HttpResponseMessage DriverandVehicellist(SearchViewModel searchViewModel)
        {
            DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();
            try
            {
                var driverList = unitOfWork.GetRepositoryInstance<DriverModel>().ReadStoredProcedure("DriverList @CompanyId",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }

                     ).ToList();

                var vehicleList = unitOfWork.GetRepositoryInstance<VehicleModel>().ReadStoredProcedure("VehicleList @CompanyId",
                   new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                   ).ToList();

                driverVehicelViewModel.driverModels = driverList;
                driverVehicelViewModel.vehicleModels = vehicleList;

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(driverVehicelViewModel));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CustomerStatistics(SearchViewModel searchViewModel)
        {
            DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();

            try
            {
                DateTime TodayDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

                var RequestedQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderRequestedQtyTodate @CompanyId,@CurrentDate",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId },
                       new SqlParameter("CurrentDate", System.Data.SqlDbType.DateTime) { Value = TodayDate }
                     ).FirstOrDefault();

                var DriverQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerDriverAll @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).FirstOrDefault();

                var VehicleQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerVehicleAll @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).FirstOrDefault();

                var DeliverdQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDeliverdQtyTodate @CompanyId,@CurrentDate",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId },
                       new SqlParameter("CurrentDate", System.Data.SqlDbType.DateTime) { Value = TodayDate }
                     ).FirstOrDefault();

                //Requested 7 Record
                List<CustomerOrderDateViewModel> reportsByDatesViewModelsRequested = new List<CustomerOrderDateViewModel>();
                var SevenDatesRequested = unitOfWork.GetRepositoryInstance<CustomerOrderDateViewModel>().ReadStoredProcedure("CustomerOrderSevenDays @CompanyId",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                       ).ToList();

                foreach (var item in SevenDatesRequested)
                {
                    CustomerOrderDateViewModel customerOrderDateViewModel = new CustomerOrderDateViewModel();
                    var CountByDate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderCountByDate @CompanyId,@CreatedDare",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId },
                       new SqlParameter("CreatedDare", System.Data.SqlDbType.DateTime) { Value = Convert.ToDateTime(item.CreatedDate) }
                       ).FirstOrDefault();

                    customerOrderDateViewModel.CreatedDate = item.CreatedDate;
                    customerOrderDateViewModel.Total = CountByDate.Result;
                    if (customerOrderDateViewModel.Total > 0)
                    {
                        reportsByDatesViewModelsRequested.Add(customerOrderDateViewModel);
                    }
                }

                //Deliverd 7 Record
                List<CustomerOrderDateViewModel> reportsByDatesViewModelsDeliverd = new List<CustomerOrderDateViewModel>();
                var SevenDatesDeliverd = unitOfWork.GetRepositoryInstance<CustomerOrderDateViewModel>().ReadStoredProcedure("CustomerOrderSevenDaysDeliverd @CompanyId",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                       ).ToList();

                foreach (var item in SevenDatesDeliverd)
                {
                    CustomerOrderDateViewModel customerOrderDateViewModel = new CustomerOrderDateViewModel();
                    var CountByDate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderCountByDateDeliverd @CompanyId,@CreatedDare",
                       new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId },
                       new SqlParameter("CreatedDare", System.Data.SqlDbType.DateTime) { Value = Convert.ToDateTime(item.CreatedDate) }
                       ).FirstOrDefault();

                    customerOrderDateViewModel.CreatedDate = item.CreatedDate;
                    customerOrderDateViewModel.Total = CountByDate.Result;

                    if (customerOrderDateViewModel.Total > 0)
                    {
                        reportsByDatesViewModelsDeliverd.Add(customerOrderDateViewModel);
                    }
                }

                var CustomerNotificationGetActive = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationGetActive"
                     ).ToList();

                CustomerOrderStatistics customerOrderStatistics = new CustomerOrderStatistics
                {

                    TotolRequestedQuantity = RequestedQTY.Result,
                    TotalDeliverdQuantity = DeliverdQTY.Result,
                    TotalDrivers = DriverQTY.Result,
                    TotalVehicles = VehicleQTY.Result,
                    RequestedBySevenDayed = reportsByDatesViewModelsRequested,
                    DeliverdBySevenDayed = reportsByDatesViewModelsDeliverd,
                    customerNotification = CustomerNotificationGetActive
                };

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerOrderStatistics));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }

        }

        [HttpPost]
        public HttpResponseMessage AdminStatistics(SearchViewModel searchViewModel)
        {
            DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();

            try
            {
                DateTime TodayDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

                var RequestedQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderRequestedQtyTodateAdmin @CurrentDate",
                       new SqlParameter("CurrentDate", System.Data.SqlDbType.DateTime) { Value = TodayDate }
                     ).FirstOrDefault();

                var DriverQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AdminDriverAll @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).FirstOrDefault();

                var VehicleQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("AdminVehicleAll @CompanyId",
                      new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = searchViewModel.CompanyId }
                    ).FirstOrDefault();

                var DeliverdQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderDeliverdQtyTodateAdmin @CurrentDate",
                       new SqlParameter("CurrentDate", System.Data.SqlDbType.DateTime) { Value = TodayDate }
                     ).FirstOrDefault();

                var VirtualQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("GETAdminTotalVirtual"
                    ).FirstOrDefault();

                var BookedQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("GETAdminTotalBooking"
                    ).FirstOrDefault();

                var CustomersBookedQTY = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomersTotalConfirmBooking"
                    ).FirstOrDefault();

                //Requested 7 Record
                List<CustomerOrderDateViewModel> reportsByDatesViewModelsRequested = new List<CustomerOrderDateViewModel>();
                var SevenDatesRequested = unitOfWork.GetRepositoryInstance<CustomerOrderDateViewModel>().ReadStoredProcedure("CustomerOrderSevenDaysAdmin"
                         ).ToList();

                foreach (var item in SevenDatesRequested)
                {
                    CustomerOrderDateViewModel customerOrderDateViewModel = new CustomerOrderDateViewModel();
                    var CountByDate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderCountByDateAdmin @CreatedDare",
                       new SqlParameter("CreatedDare", System.Data.SqlDbType.DateTime) { Value = Convert.ToDateTime(item.CreatedDate) }
                       ).FirstOrDefault();

                    customerOrderDateViewModel.CreatedDate = item.CreatedDate;
                    customerOrderDateViewModel.Total = CountByDate.Result;
                    if (customerOrderDateViewModel.Total > 0)
                    {
                        reportsByDatesViewModelsRequested.Add(customerOrderDateViewModel);
                    }
                }

                //Deliverd 7 Record
                List<CustomerOrderDateViewModel> reportsByDatesViewModelsDeliverd = new List<CustomerOrderDateViewModel>();
                var SevenDatesDeliverd = unitOfWork.GetRepositoryInstance<CustomerOrderDateViewModel>().ReadStoredProcedure("CustomerOrderSevenDaysDeliverdAdmin"
                        ).ToList();

                foreach (var item in SevenDatesDeliverd)
                {
                    CustomerOrderDateViewModel customerOrderDateViewModel = new CustomerOrderDateViewModel();
                    var CountByDate = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CustomerOrderCountByDateDeliverdAdmin @CreatedDare",
                         new SqlParameter("CreatedDare", System.Data.SqlDbType.DateTime) { Value = Convert.ToDateTime(item.CreatedDate) }
                       ).FirstOrDefault();

                    customerOrderDateViewModel.CreatedDate = item.CreatedDate;
                    customerOrderDateViewModel.Total = CountByDate.Result;

                    if (customerOrderDateViewModel.Total > 0)
                    {
                        reportsByDatesViewModelsDeliverd.Add(customerOrderDateViewModel);
                    }
                }
                var CustomerNotificationGetActive = unitOfWork.GetRepositoryInstance<CustomerNotification>().ReadStoredProcedure("CustomerNotificationGetActive"
                    ).ToList();

                CustomerOrderStatistics customerOrderStatistics = new CustomerOrderStatistics
                {
                    TotolRequestedQuantity = RequestedQTY.Result,
                    TotalDeliverdQuantity = DeliverdQTY.Result,
                    TotalDrivers = DriverQTY.Result,
                    TotalVehicles = VehicleQTY.Result,
                    RequestedBySevenDayed = reportsByDatesViewModelsRequested,
                    DeliverdBySevenDayed = reportsByDatesViewModelsDeliverd,
                    customerNotification = CustomerNotificationGetActive,
                    VirtualTotalQuantity = VirtualQTY.Result,
                    BookedTotalQuantity = BookedQTY.Result,
                    CustomerConfirmBooking = CustomersBookedQTY.Result
                };
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(customerOrderStatistics));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        //Custoer Notification
        [NonAction]
        public int AdminNotificaton(CustomerOrderListViewModel customerOrderListViewModel)
        {
            NotificationController notificationController = new NotificationController();

            List<NotificationInformation> notificationInformation = notificationController.GetAllAdminTokens("Admin");

            var tekenNot = notificationInformation.Where(x => x.DeviceToken.ToLower() == "token not availibe").ToList();

            foreach (var item in tekenNot)
            {
                notificationInformation.Remove(item);
            }

            var Tokenss = notificationInformation.Where(x => x.Device.ToLower().Trim() == "ios")
                                                    .Select(x => new NotificationInformation()
                                                    {
                                                        Device = x.Device,
                                                        DeviceId = x.DeviceId,
                                                        DeviceToken = x.DeviceToken
                                                    }).ToList();

            var tokens = new string[Tokenss.Count];

            for (int i = 0; i < Tokenss.Count; i++)
            {
                tokens[i] = Tokenss[i].DeviceToken;
            }
            var pushSent = PushNotificationLogic.SendPushNotification(tokens, customerOrderListViewModel.Title, customerOrderListViewModel.NotificationCode, customerOrderListViewModel.Message);

            var TokenWebAndroid = notificationInformation.Where(x => x.Device.ToLower().Trim() != "ios")
                                                    .Select(x => new NotificationInformation()
                                                    {
                                                        Device = x.Device,
                                                        DeviceId = x.DeviceId,
                                                        DeviceToken = x.DeviceToken
                                                    }).ToList();
            tokens = new string[TokenWebAndroid.Count];

            int Result = 0;

            for (int i = 0; i < TokenWebAndroid.Count; i++)
            {
                tokens[i] = TokenWebAndroid[i].DeviceToken;
                var WebNoti = notificationController.SendPushNotificationWebAndroid(TokenWebAndroid[i].DeviceToken, customerOrderListViewModel.Title, customerOrderListViewModel.NotificationCode, customerOrderListViewModel.Message);
                Result = WebNoti;
            }

            return Result;
        }

        [NonAction]
        public int CustomerNotification(CustomerOrderListViewModel customerOrderListViewModel)
        {
            NotificationController notificationController = new NotificationController();

            List<NotificationInformation> notificationInformation = notificationController.GetCompaniesTokens(customerOrderListViewModel.CustomerId);

            var tekenNot = notificationInformation.Where(x => x.DeviceToken.ToLower() == "token not availibe").ToList();

            foreach (var item in tekenNot)
            {
                notificationInformation.Remove(item);
            }
            var Tokenss = notificationInformation.Where(x => x.Device.ToLower().Trim() == "ios").ToList();

            var tokens = new string[Tokenss.Count];

            for (int i = 0; i < Tokenss.Count; i++)
            {
                tokens[i] = Tokenss[i].DeviceToken;
            }
            var pushSent = PushNotificationLogic.SendPushNotification(tokens, customerOrderListViewModel.Title, customerOrderListViewModel.NotificationCode, customerOrderListViewModel.Message);

            List<SearchViewModel> searchViewModels = new List<SearchViewModel>();

            int Result = 0;
            foreach (var item in notificationInformation)
            {
                if (item.Device.ToLower().Trim() != "ios")
                {
                    SearchViewModel searchViewModel = new SearchViewModel { 

                        DeviceTiken = item.DeviceToken,
                        CompanyName = "Test Company Name",
                        Quantity = customerOrderListViewModel.RequestedQuantity,
                        //searchViewModel.NotificationCode = "ADM-001";
                        NotificationCode = customerOrderListViewModel.NotificationCode,
                        // searchViewModel.Title = "New Order Received";
                        Title = customerOrderListViewModel.Title,
                        // searchViewModel.Message = searchViewModel.CompanyName + "Created a new Order of " + customerOrderListViewModel.RequestedQuantity + " Gallon";
                        Message = customerOrderListViewModel.Message
                    };
                searchViewModels.Add(searchViewModel);
                }

                Result = notificationController.SendMessage(searchViewModels);
            }

            return 1;

        }

        [NonAction]
        public int DriverNotification(CustomerOrderListViewModel customerOrderListViewModel)
        {
            NotificationController notificationController = new NotificationController();

            List<NotificationInformation> notificationInformation = notificationController.GetDriverTokens(customerOrderListViewModel.email);

            var tekenNot = notificationInformation.Where(x => x.DeviceToken == "token not availibe").ToList();

            foreach (var item in tekenNot)
            {
                notificationInformation.Remove(item);
            }

            var Tokenss = notificationInformation.Where(x => x.Device == "ios").ToList();

            var tokens = new string[Tokenss.Count];

            for (int i = 0; i < Tokenss.Count; i++)
            {
                tokens[i] = Tokenss[i].DeviceToken;
            }
            var pushSent = PushNotificationLogic.SendPushNotification(tokens, customerOrderListViewModel.Title, customerOrderListViewModel.NotificationCode, customerOrderListViewModel.Message);

            List<SearchViewModel> searchViewModels = new List<SearchViewModel>();

            foreach (var item in notificationInformation)
            {
                if (item.Device != "IOS")
                {
                    SearchViewModel searchViewModel = new SearchViewModel {

                        DeviceTiken = item.DeviceToken,
                        CompanyName = "Test Company Name",
                        Quantity = customerOrderListViewModel.RequestedQuantity,
                        //searchViewModel.NotificationCode = "ADM-001",
                        NotificationCode = customerOrderListViewModel.NotificationCode,
                        // searchViewModel.Title = "New Order Received",
                        Title = customerOrderListViewModel.Title,
                        // searchViewModel.Message = searchViewModel.CompanyName + "Created a new Order of " + customerOrderListViewModel.RequestedQuantity + " Gallon",
                        Message = customerOrderListViewModel.Message,
                    };
                    searchViewModels.Add(searchViewModel);
                }
            }

            int Result = notificationController.SendMessage(searchViewModels);

            return 1;

        }

    }
}
