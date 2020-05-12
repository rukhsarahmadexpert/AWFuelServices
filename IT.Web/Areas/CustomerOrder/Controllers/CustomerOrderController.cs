using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.CustomerOrder.Controllers
{

    [Autintication]
    public class CustomerOrderController : Controller
    {
        WebServices webServices = new WebServices();

        readonly CustomerOrderViewModel CustomerOrder = new CustomerOrderViewModel();
        readonly List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<CustomerOrderViewModel> customerOrderViewModels = new List<CustomerOrderViewModel>();
        List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
        CustomerNoteOrderViewModel CustomerNoteOrderViewModel = new CustomerNoteOrderViewModel();
        List<CustomerNoteOrderViewModel> customerNoteOrderViewModels = new List<CustomerNoteOrderViewModel>();
        OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel();
        CustomerOrderGroupViewModel CustomerOrderGroupViewModel = new CustomerOrderGroupViewModel();
        List<CustomerGroupOrderDetailsViewModel> customerGroupOrderDetailsViewModel = new List<CustomerGroupOrderDetailsViewModel>();

        public ActionResult Index()
        {

            var results = webServices.Post(new CountryViewModel(), "Country/All");
            if (results.Data != "[]")
            {
                List<CountryViewModel> CountryViewModel = (new JavaScriptSerializer()).Deserialize<List<CountryViewModel>>(results.Data.ToString());
            }

            int CompanyId = Convert.ToInt32(Session["CompanyId"]);
            var result = webServices.Post(new DriverViewModel(), "Driver/All/" + CompanyId);

            List<DriverViewModel> driverViewModelss = new List<DriverViewModel>();

            if (result.Data != "[]")
            {
                driverViewModelss = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());
            }
            var resultVehicle = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
            if (resultVehicle.Data != "[]")
            {
                vehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(resultVehicle.Data.ToString());
            }
            ViewBag.Vehicle = vehicleViewModels;
            ViewBag.Driver = driverViewModelss;
            ViewBag.CountryList = CountryViewModel;

            return View();
        } 

        public JsonResult GetAll(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/All/" + CompanyId);
                if (result.Data != "[]")
                {
                    customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.CreateDates.Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CreatedTime = x.CreatedTime,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName

                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CreatedTime = x.CreatedTime,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Create(CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    customerOrderViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    customerOrderViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(customerOrderViewModel, "CustomerOrder/Add");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    else
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult ManageOrders()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUnreadOrderAll(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new VehicleViewModel(), "CustomerOrder/GetAllUnreadOrder");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.TraficPlateNumber.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Date.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       Company = x.Company,
                       Time = x.Time,
                       Date = x.Date,
                       DriverName = x.DriverName,
                       TraficPlateNumber = x.TraficPlateNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderId = x.OrderId,


                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              Company = x.Company,
                              Time = x.Time,
                              Date = x.Date,
                              DriverName = x.DriverName,
                              TraficPlateNumber = x.TraficPlateNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderId = x.OrderId,

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public ActionResult OrderViewed(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(customerNoteOrderViewModel, "CustomerOrder/OrderViewed");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    else
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult OrderById(CustomerNoteOrderViewModel customerNoteOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(customerNoteOrderViewModel, "CustomerOrder/OrderById");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            CustomerNoteOrderViewModel = (new JavaScriptSerializer()).Deserialize<CustomerNoteOrderViewModel>(result.Data.ToString());
                        }
                        return Json(CustomerNoteOrderViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult OrderAcceptedAwfuel()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAsignedOrderAWFuel(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/GetAsignedOrderAWFuel");
                if (result.Data != "[]")
                {
                    customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());
                }

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.CompanyName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.CompanyName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {
                       CompanyName = x.CompanyName,
                       CreatedTime = x.CreatedTime,
                       CreateDates = x.CreateDates,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       CreatedBy = x.CreatedBy
                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              CreateDates = x.CreateDates,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              Id = x.Id,
                              Name = x.Name,
                              OrderProgress = x.OrderProgress
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult OrderDeliverd()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDeliverdOrderAWFuel(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/GetDeliverdOrder");
                if (result.Data != "[]")
                {
                    customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.CompanyName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.CompanyName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {
                       CompanyName = x.CompanyName,
                       CreatedTime = x.CreatedTime,
                       AcceptDate = x.AcceptDate,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       OrderDate = x.OrderDate

                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              AcceptDate = x.AcceptDate,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              Id = x.Id,
                              Name = x.Name,
                              OrderDate = x.OrderDate
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult OrderDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrderDetails(int Id)
        {
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel();
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new OrderDetailsViewModel(), "CustomerOrder/GetDeliverdOrderDetails/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            orderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<OrderDetailsViewModel>(result.Data.ToString());
                        }
                        return Json(orderDetailsViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CustomerOrderNote()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new OrderDetailsViewModel(), "CustomerOrder/GetTop");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                        }
                        return Json(customerNoteOrderViewModels, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ViewDeliveryInfo(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = webServices.Post(new OrderDetailsViewModel(), "CustomerOrder/ViewDeliveryInfo/" + Id);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            orderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<OrderDetailsViewModel>(result.Data.ToString());
                        }
                        return Json(orderDetailsViewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult OrderAccepted()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAsignedOrder(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/GetAsignedOrder/" + CompanyId);
                customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {

                       CreatedTime = x.CreatedTime,
                       CreateDates = x.CreateDates,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       CreatedBy = x.CreatedBy
                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              CreateDates = x.CreateDates,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              Id = x.Id,
                              Name = x.Name,
                              OrderProgress = x.OrderProgress
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult OrderReceived()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CustomerOrderReceived(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/OrderReceived/" + CompanyId);
                customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {

                       CreatedTime = x.CreatedTime,
                       CreateDates = x.CreateDates,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       CreatedBy = x.CreatedBy
                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels.OrderBy(x => x.Id)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              CreateDates = x.CreateDates,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              Id = x.Id,
                              Name = x.Name,
                              OrderProgress = x.OrderProgress
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult CustomerReceivedOrderDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ViewedNotifyCustomer(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                var result = webServices.Post("", "CustomerOrder/ViewedNotifyCustomer/" + customerOrderDeliverVewModel.Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        if (Res > 0)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpGet]
        public ActionResult AcceptedOrderAdmin()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult AcceptedOrdersAdmin(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new CustomerNoteOrderViewModel(), "CustomerOrder/AcceptedOrdersAdmin");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult DeliverdOrderAdmin()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DeliverdOrdersAdmin(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new CustomerNoteOrderViewModel(), "CustomerOrder/DeliverdOrderAdmin");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        //Driver Order
        [HttpGet]
        public ActionResult DriverAsignedOrder()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAsignedOrderByDriver(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int UserId = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/GetAsignedOrderByDriver/" + UserId);
                customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {

                       CreatedTime = x.CreatedTime,
                       CreateDates = x.CreateDates,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       CreatedBy = x.CreatedBy
                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              CreateDates = x.CreateDates,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              Id = x.Id,
                              Name = x.Name,
                              OrderProgress = x.OrderProgress
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult DriverDeliverdOrder()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDeliverOrderByDriver(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int UserId = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(new CustomerOrderViewModel(), "CustomerOrder/GetDeliverOrderByDriver/" + UserId);
                customerOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderViewModels.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.DriverName.Contains(parm.sSearch) ||
                               x.VehicleNumber.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderViewModels = customerOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.VehicleNumber.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderViewModel
                   {

                       CreatedTime = x.CreatedTime,
                       CreateDates = x.CreateDates,
                       DriverName = x.DriverName,
                       VehicleNumber = x.VehicleNumber,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       Id = x.Id,
                       Name = x.Name,
                       CreatedBy = x.CreatedBy
                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderViewModels.Count();

                    customerOrderViewModels = customerOrderViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderViewModel
                          {
                              CompanyName = x.CompanyName,
                              CreatedTime = x.CreatedTime,
                              CreateDates = x.CreateDates,
                              DriverName = x.DriverName,
                              VehicleNumber = x.VehicleNumber,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              Id = x.Id,
                              Name = x.Name,
                              OrderProgress = x.OrderProgress
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public ActionResult CustomerOrderDeliverdUpdate(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                int Res;
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(customerOrderDeliverVewModel, "CustomerOrder/CustomerOrderDeliverdUpdate");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        return Json("Falied", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CusOrderDelUpdateCusConfirmed(int Id)
        {
            try
            {

                var result = webServices.Post(new OrderDetailsViewModel(), "CustomerOrder/CusOrderDelUpdateCusConfirmed/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data.ToString());
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Customer Group Order
        #region Customer Group Order Admin

        public ActionResult CustomerGroupOrder()
        {

            int CompanyId = Convert.ToInt32(Session["CompanyId"]);
            var resultVehicle = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
            if (resultVehicle.Data != "[]")
            {
                vehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(resultVehicle.Data.ToString());
            }

            var result = webServices.Post(new DriverViewModel(), "Driver/All/" + CompanyId);

            List<DriverViewModel> driverViewModelss = new List<DriverViewModel>();

            if (result.Data != "[]")
            {
                driverViewModelss = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());

                if (driverViewModelss[0].Name != "Select Driver")
                {
                    driverViewModelss.Insert(0, new DriverViewModel() { Id = 0, Name = "Select Driver" });
                }

            }

            ViewBag.Driver = driverViewModelss;
            ViewBag.Vehicle = vehicleViewModels;

            return View();
        }

        [HttpPost]
        public ActionResult CustomerGroupOrder(CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                // customerOrderListViewModel.RequestedQuantity = 10;
                if (ModelState.IsValid)
                {
                    OrderNumber orderNumber = new OrderNumber();

                    customerOrderListViewModel.CustomerOrderId = orderNumber.OrderNewNumber();
                    customerOrderListViewModel.CustomerId = Convert.ToInt32(Session["CompanyId"]);
                    customerOrderListViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerGroupOrderAdd");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Id = (new JavaScriptSerializer().Deserialize<int>(result.Data));

                            return Json(Id, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        public ActionResult CustomerGroupOrderDetails(int Id)
        {

            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderDetailsByOrderId/" + Id);
                if (resultCustomerOrderDetails.Data != "[]")
                {
                    customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                }

                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult CustomerGroupOrderEdit(int Id)
        {
            try
            {

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderDetailsByOrderId/" + Id);
                if (resultCustomerOrderDetails.Data != "[]")
                {
                    customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                }

                var result = webServices.Post(new DriverViewModel(), "Driver/All/" + CompanyId);

                List<DriverViewModel> driverViewModelss = new List<DriverViewModel>();

                if (result.Data != "[]")
                {
                    driverViewModelss = (new JavaScriptSerializer()).Deserialize<List<DriverViewModel>>(result.Data.ToString());

                    if (driverViewModelss[0].Name != "Select Driver")
                    {
                        driverViewModelss.Insert(0, new DriverViewModel() { Id = 0, Name = "Select Driver" });
                    }

                }


                ViewBag.Driver = driverViewModelss;

                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult CustomerGroupOrderUpdate(CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                // customerOrderListViewModel.RequestedQuantity = 10;
                if (ModelState.IsValid)
                {
                    customerOrderListViewModel.CustomerId = Convert.ToInt32(Session["CompanyId"]);
                    customerOrderListViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerGroupOrderUpdate");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Id = (new JavaScriptSerializer().Deserialize<int>(result.Data));

                            return Json(Id, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        public ActionResult CustomerOrderViewByAdmin(int Id)
        {
            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                if(CustomerOrderGroupViewModel.OrderProgress == "Order Created")
                {
                    var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderDetailsByOrderId/" + Id);
                    if (resultCustomerOrderDetails.Data != "[]")
                    {
                        customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                    }
                }
                else
                {
                    var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/GroupOrderAcDetDetailsByOrderId/" + Id);
                    if (resultCustomerOrderDetails.Data != "[]")
                    {
                        customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                    }
                }
               
                List<RejectedOrderDetails> rejectedOrderDetails = new List<RejectedOrderDetails>();

                if (CustomerOrderGroupViewModel.OrderProgress == "Reject By Admin")
                {
                    var resultRejectDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderRejectDetailsById/" + Id);

                    if(resultRejectDetails.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if(resultRejectDetails.Data != "[]")
                        {
                            rejectedOrderDetails = (new JavaScriptSerializer().Deserialize<List<RejectedOrderDetails>>(resultRejectDetails.Data.ToString()));
                        }
                    }
                }

                ViewBag.rejectedOrderDetails = rejectedOrderDetails;
                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ActionResult CustomerOrderAsignedViewByAdmin(int Id)
        {
            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderDetailsGroupAsignedByOrderId/" + Id);
                if (resultCustomerOrderDetails.Data != "[]")
                {
                    customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                }

                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        public ActionResult AllUnreadOrderGroup()
        {
            return View();
        }

        public ActionResult GetAllUnreadOrderGroup(DataTablesParm parm)
        {
            CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel {
                IsSend = true,
                OrderProgress = "Order Created",
            };
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(customerOrderGroupViewModel, "CustomerOrder/GetAllUnreadOrderGroup");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AllRejectedOrderGroupAdmin(DataTablesParm parm)
        {
            CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel {
                IsSend = true,
                OrderProgress = "Reject By Admin"
            };
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(customerOrderGroupViewModel, "CustomerOrder/GetAllUnreadOrderGroup");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost]
        public ActionResult RejectDescriptionAdd(RejectedOrderDetails rejectedOrderDetails)
        {
            try
            {
                rejectedOrderDetails.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var result = webServices.Post(rejectedOrderDetails, "CustomerOrder/rejectedOrderDetails");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerOrderGroupAsignedDriverAdd(CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customerOrderListViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerOrderGroupAsignedDriverAdd");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            int Id = (new JavaScriptSerializer().Deserialize<int>(result.Data));

                            return Json(Id, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerOrderRejectByAdmin(int Id)
        {
            try
            {
                var Result = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderRejectByAdmin/" + Id);
                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                } 
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
               
        public ActionResult CustomerOrderRejectedAllAdmin()
        {
            return View();
        }        

        [HttpPost]
        public ActionResult CustomerOrderAccepted(CustomerOrderGroupViewModel customerOrderGroupViewModel)
        {
            try
            {

                //return Json("Failed", JsonRequestBehavior.AllowGet);

                var Result = webServices.Post(customerOrderGroupViewModel, "CustomerOrder/CustomerOrderAccepted");
                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Driver View Asigned Order details

        public ActionResult DriverViewGroupAsignedOrder(int Id)
        {
            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderGroupAsignedByOrderId/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                var resultCustomerOrderDetails = webServices.Post(new CustomerGroupOrderDetailsViewModel(), "CustomerOrder/CustomerOrderGroupAsignedDetailsByOrderId/" + Id);
                if (resultCustomerOrderDetails.Data != "[]")
                {
                    customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                }

                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public ActionResult AsignedOrderDriver()
        {
            return View();
        }
        
        public ActionResult AcceptedOrderDriver()
        {
            return View();
        }

        public ActionResult DeliverdOrderDriver()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CustomerOrderGroupAsignedDriverByDriverId(DataTablesParm parm)
        {
            List<CustomerOrderGroupViewModel> customerOrderGroupViewModel = new List<CustomerOrderGroupViewModel>();

            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                CustomerOrderGroupViewModel.Id = UserId;
                CustomerOrderGroupViewModel.OrderProgress = "Asigned to Driver";

                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderGroupBYDriverAsignedId");
                customerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderGroupViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderGroupViewModel.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.CreatedDates.Contains(parm.sSearch.ToLower()) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CreatedDates.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderGroupViewModel
                   {

                       Id = x.Id,
                       CreatedDates = x.CreatedDates,
                       Name = x.Name,
                       RequestedQuantity = x.RequestedQuantity,
                       DeliverdQuantity = x.DeliverdQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,

                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderGroupViewModel.Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderGroupViewModel
                          {
                              Id = x.Id,
                              CreatedDates = x.CreatedDates,
                              Name = x.Name,
                              RequestedQuantity = x.RequestedQuantity,
                              DeliverdQuantity = x.DeliverdQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderGroupViewModel,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderGroupViewModel,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public JsonResult CustomerOrderGroupAcceptedDriverByDriverId(DataTablesParm parm)
        {
            List<CustomerOrderGroupViewModel> customerOrderGroupViewModel = new List<CustomerOrderGroupViewModel>();

            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                CustomerOrderGroupViewModel.Id = UserId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Accepted";

                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderGroupBYDriverAsignedId");
                customerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderGroupViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderGroupViewModel.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.CreatedDates.Contains(parm.sSearch.ToLower()) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CreatedDates.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderGroupViewModel
                   {

                       Id = x.Id,
                       CreatedDates = x.CreatedDates,
                       Name = x.Name,
                       RequestedQuantity = x.RequestedQuantity,
                       DeliverdQuantity = x.DeliverdQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,

                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderGroupViewModel.Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderGroupViewModel
                          {
                              Id = x.Id,
                              CreatedDates = x.CreatedDates,
                              Name = x.Name,
                              RequestedQuantity = x.RequestedQuantity,
                              DeliverdQuantity = x.DeliverdQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderGroupViewModel,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderGroupViewModel,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public JsonResult CustomerOrderGroupDeliverdDriverByDriverId(DataTablesParm parm)
        {
            List<CustomerOrderGroupViewModel> customerOrderGroupViewModel = new List<CustomerOrderGroupViewModel>();

            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                CustomerOrderGroupViewModel.Id = UserId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Deliverd";

                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderGroupBYDriverAsignedId");
                customerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderGroupViewModel>>(result.Data.ToString());

                if (parm.sSearch != null)
                {
                    totalCount = customerOrderGroupViewModel.Where(x => x.Id.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.CreatedDates.Contains(parm.sSearch.ToLower()) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CreatedDates.Contains(parm.sSearch) ||
                               x.Id.ToString().Contains(parm.sSearch) ||
                               x.RequestedQuantity.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerOrderGroupViewModel
                   {

                       Id = x.Id,
                       CreatedDates = x.CreatedDates,
                       Name = x.Name,
                       RequestedQuantity = x.RequestedQuantity,
                       DeliverdQuantity = x.DeliverdQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,

                   }).ToList();
                }
                else
                {
                    totalCount = customerOrderGroupViewModel.Count();

                    customerOrderGroupViewModel = customerOrderGroupViewModel
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerOrderGroupViewModel
                          {
                              Id = x.Id,
                              CreatedDates = x.CreatedDates,
                              Name = x.Name,
                              RequestedQuantity = x.RequestedQuantity,
                              DeliverdQuantity = x.DeliverdQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerOrderGroupViewModel,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerOrderGroupViewModel,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CustomerOrderDetailsGroupUpDelQTY(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                int Res;
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(customerOrderDeliverVewModel, "CustomerOrder/CustomerOrderDetailsGroupUpDelQTY");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        return Json("Falied", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Customer Order Group All Asigned To Driver ADMIN
        [HttpGet]
        public ActionResult CustomerOrderGroupAllAsigned()
        {
            return View();
        }

        public ActionResult GetAllAsignedOrderGroup(DataTablesParm parm)
        {
            try
            {
                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(new CustomerNoteOrderViewModel(), "CustomerOrder/CustomerOrderGroupAllAsignedToDriver");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion
        
        #region  Customer Order Group Customer Area

        public ActionResult CustomerRequestedOrderGroup()
        {
            return View();
        }
         
        public ActionResult CustomerOrderAllByCompanyId(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                CustomerOrderGroupViewModel.Id = CompanyId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Created";
                CustomerOrderGroupViewModel.IsSend = false;

                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderAllByCompanyId");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
      
        public ActionResult CustomerOrderGroupByOrderId(int Id)
        {
            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    CustomerOrderGroupViewModel = (new JavaScriptSerializer()).Deserialize<CustomerOrderGroupViewModel>(resultCustomerOrder.Data.ToString());
                }

                var resultCustomerOrderDetails = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CusOrderDetGrpByOdrIdForCustomer/" + Id);
                if (resultCustomerOrderDetails.Data != "[]")
                {
                    customerGroupOrderDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerGroupOrderDetailsViewModel>>(resultCustomerOrderDetails.Data.ToString());
                }

                ViewBag.CustomerOrderGroupViewModel = CustomerOrderGroupViewModel;
                ViewBag.customerGroupOrderDetailsViewModel = customerGroupOrderDetailsViewModel;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult MyAcceptedOrder()
        {
            return View();

        }
        
        public ActionResult CustomerOrderAcceptedByCompanyId(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                CustomerOrderGroupViewModel.Id = CompanyId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Accepted";
                CustomerOrderGroupViewModel.IsSend = true;


                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1; 
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderAllByCompanyId");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public ActionResult MyReceivedOrder()
        {
            return View();
        }
        
        public ActionResult CustomerOrderReceivedByCompanyId(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                CustomerOrderGroupViewModel.Id = CompanyId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Deliverd";
                CustomerOrderGroupViewModel.IsSend = true;


                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderAllByCompanyId");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
       
        public ActionResult CustomerOrderSend(int Id)
        {
            try
            {
                var resultCustomerOrder = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderSend/" + Id);
                if (resultCustomerOrder.Data != "[]")
                {
                    int Res = (new JavaScriptSerializer()).Deserialize<int>(resultCustomerOrder.Data);

                    if (Res > 0)
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }

                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public ActionResult CustomerSendedOrders()
        {
            return View();
        }

        public ActionResult CustomerOrderAllSendedByCompanyId(DataTablesParm parm)
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                CustomerOrderGroupViewModel.Id = CompanyId;
                CustomerOrderGroupViewModel.OrderProgress = "Order Created";
                CustomerOrderGroupViewModel.IsSend = true;

                int pageNo = 1;
                int totalCount = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                var result = webServices.Post(CustomerOrderGroupViewModel, "CustomerOrder/CustomerOrderAllByCompanyId");
                if (result.Data != "[]")
                {
                    customerNoteOrderViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerNoteOrderViewModel>>(result.Data.ToString());
                }
                if (parm.sSearch != null)
                {
                    totalCount = customerNoteOrderViewModels.Where(x => x.OrderId.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.Name.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch)).Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.ToList()
                        .Where(x => x.DriverName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.CustomerOrderId.Contains(parm.sSearch) ||
                               x.Company.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.OrderId.ToString().Contains(parm.sSearch) ||
                               x.OrderQuantity.ToString().Contains(parm.sSearch.ToLower()) ||
                               x.CreateDates.Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new CustomerNoteOrderViewModel
                   {
                       CreateDates = x.CreateDates,
                       CustomerOrderId = x.CustomerOrderId,
                       Company = x.Company,
                       TRN = x.TRN,
                       OrderQuantity = x.OrderQuantity,
                       UserName = x.UserName,
                       OrderProgress = x.OrderProgress,
                       OrderId = x.OrderId

                   }).ToList();
                }
                else
                {
                    totalCount = customerNoteOrderViewModels.Count();

                    customerNoteOrderViewModels = customerNoteOrderViewModels.OrderBy(x => x.OrderId)
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                          .Select(x => new CustomerNoteOrderViewModel
                          {
                              CreateDates = x.CreateDates,
                              CustomerOrderId = x.CustomerOrderId,
                              Company = x.Company,
                              TRN = x.TRN,
                              OrderQuantity = x.OrderQuantity,
                              UserName = x.UserName,
                              OrderProgress = x.OrderProgress,
                              OrderId = x.OrderId

                          }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = customerNoteOrderViewModels,
                        parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = customerNoteOrderViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost]
        public ActionResult CustomerOrderGroupDetailsDriverView(int Id)
        {
           

            try
            {
                List<CustomerOrderGroupViewModel> customerOrderGroupViewModels = new List<CustomerOrderGroupViewModel>();
               var result = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerOrderGroupDetailsDriverView/" + Id);
                if (result.Data != "[]")
                {
                    customerOrderGroupViewModels = (new JavaScriptSerializer()).Deserialize<List<CustomerOrderGroupViewModel>>(result.Data.ToString());

                    return Json(customerOrderGroupViewModels, JsonRequestBehavior.AllowGet);                                       
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult CustomerDeliverdOrderUpdate(int Id)
        {
            try
            {
                var result = webServices.Post("", "CustomerOrder/CustomerDeliverdOrderUpdate/"+Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data.ToString());
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
                
        public ActionResult CutomerOrderCompleteDetails()
        {
            return View();
        }

        
        #endregion
    }
}