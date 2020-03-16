    using IT.Core.ViewModels;
using IT.Web.MISC;
using IT.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT.Web.Areas.Home.Controllers
{
   // [Autintication]
    public class HomeController : Controller
    {
        UserCompanyViewModel userCompanyViewModel = new UserCompanyViewModel();
        NotificationRepositoryAdmin repo = new NotificationRepositoryAdmin();

        public ActionResult Index()
        {
            userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;

            if (Session["UserId"] != null && userCompanyViewModel.Authority == "CustomerAdmin")
            {

                return View();
            }
            else
            {
                return Redirect("/Login");
            }
           
        }

        public ActionResult NewTheme()
        {
            return View();
        }

        public ActionResult TT()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;
                        
             return View();
        }
        
        [HttpGet]        
        public ActionResult DriverDashBoard()
        {           

            userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;

            if (Session["UserId"] != null && userCompanyViewModel.Authority == "Driver")
            {
                return View();
            }
            else
            {
                return Redirect("/Login");
            }
        }
     
        public ActionResult Test()
        {
            return View();
        }
             
        public JsonResult GetMessageses()
        {
            NotificationRepositoryAdmin repos = new NotificationRepositoryAdmin();

            messages = repos.SomeMethod();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetMessagesesDriver(int Id)
        {
            DriverRepository DriverRepo = new  DriverRepository();
            int messages = DriverRepo.GetAllMessages(Id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult MsgFromDriverOnAccept(int Id)
        {
            CustomerNotificationRepository customerRepo = new CustomerNotificationRepository();
            var model = customerRepo.GetAllMessages(Id);
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        DeliveryNotificationRepository del = new DeliveryNotificationRepository();
        int messages = 0;
        //inform driver and Admin or Order Delivery
        public JsonResult CustomerDeliveyInfo(int Id)
        {
            if (messages == 0)
            {
                messages = del.GetAllMessages(Id);
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(messages,JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MessageToAdmonOnDelivery()
        {
            if (messages == 0)
            {
                messages = del.AdminInfoOnDelivery();
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CustomerHome()
        {
            //userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;

            if (Session["UserId"] != null && userCompanyViewModel.Authority == "CustomerAdmin")
            {
                Session["UserId"] = 1;

                return View();
            }
            else
            {
                return Redirect("/Login");
            }
           
        }


    }
}