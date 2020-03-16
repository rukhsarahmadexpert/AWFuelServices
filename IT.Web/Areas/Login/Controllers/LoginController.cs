using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        WebServices webServices = new WebServices();
        // GET: Login/Login
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                loginViewModel.Password = HashPassword.GetHashCode(loginViewModel.Password);

                var result = webServices.Post(loginViewModel, "User/Login");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    UserCompanyViewModel userCompanyViewModel = new UserCompanyViewModel();
                    userCompanyViewModel = (new JavaScriptSerializer()).Deserialize<UserCompanyViewModel>(result.Data);

                    if (userCompanyViewModel.UserId > 0)
                    {
                        Session["userCompanyViewModel"] = userCompanyViewModel;
                        Session["UserId"] = userCompanyViewModel.UserId;
                        if (userCompanyViewModel.CompanyId > 0)
                        {
                            Session["userCompanyViewModel"] = userCompanyViewModel;
                            Session["CompanyId"] = userCompanyViewModel.CompanyId;

                            if (userCompanyViewModel.Authority == "Driver")
                            {
                                return Redirect("/Driver-Home");
                            }
                            else if (userCompanyViewModel.Authority == "CustomerAdmin")
                            {
                                return Redirect("/Home");
                            }
                            else
                            {
                                return Redirect("/Dashboard");
                            }
                        }
                        else
                        {
                            return Redirect("/Company");
                        }
                    }
                    else
                    {
                        return View(loginViewModel);
                    }
                }
                else
                {
                    return View(loginViewModel);
                }
            }
            else
            {
                return View(loginViewModel);
            }
        }


        [HttpPost]
        public ActionResult Register(UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userViewModel.Password = HashPassword.GetHashCode(userViewModel.Password);

                    var result = webServices.Post(userViewModel, "User/Register");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        
                        int results = (new JavaScriptSerializer()).Deserialize<int>(result.Data);                       

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View(userViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        public ActionResult LogOut()
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();

                //Clear cookies
                string[] cookies = Request.Cookies.AllKeys;
                foreach (string cookie in cookies)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
                Session.Clear();
                Session.Abandon();

                return Redirect("Login");
            }
            catch (Exception)
            {
                return Redirect("Login");
            }
        }


        [HttpGet]
        public ActionResult LogOutDriver()
        {
            try
            {

                SearchViewModel searchViewModel = new SearchViewModel();
                searchViewModel.Id = Convert.ToInt32(Session["DriverId"]);
                var result = webServices.Post(searchViewModel, "AWFDriver/ReleaseVehicle");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                    Response.Cache.SetNoStore();

                    //Clear cookies
                    string[] cookies = Request.Cookies.AllKeys;
                    foreach (string cookie in cookies)
                    {
                        Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                    }
                    Session.Clear();
                    Session.Abandon();


                    return Redirect("Login");
                }
                else
                {
                    return Redirect("Login");
                }
            }
            catch (Exception)
            {
                return Redirect("Login");
            }
        }
    }
}