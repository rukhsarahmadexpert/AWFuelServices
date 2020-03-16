using IT.WebServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT.WebServices.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //unitOfWork.GetRepositoryInstance<object>().GetAll().ToList();
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            if (userModel.UserName == "smartaccess" && userModel.Password == "smartaccesspwd")
            {
                Session["UserId"] = "001";

                return RedirectToAction("Index", "Help");
            }
            else
            {
                ModelState.AddModelError("UserName", "Username or Password incorrect");
                return View(userModel);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session["UserId"] = null;

            return RedirectToAction(nameof(Login));
        }
    }
}