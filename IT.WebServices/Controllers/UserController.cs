using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using IT.WebServices.MISC;
using IT.WebServices.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class UserController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        //UserViewModel userViewModel = new UserViewModel();
        string contentType = "application/json"; //ConfigurationManager.AppSettings["ContentType"].ToString();
        
        [HttpPost]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var userViewModel = new UserViewModel();

                /*
                SqlParameter[] sqlParameters;
                string parameter = SQLParameters.GetParameters<DoctorViewModel>(doctorViewModel);
                sqlParameters = SQLParameters.GetSQLParameters<DoctorViewModel>(doctorViewModel, "GetAll").ToArray();
                var doctorsList = unitOfWork.GetRepositoryInstance<DoctorViewModel>().ReadStoredProcedure("Doctor_Detail " + parameter, sqlParameters).ToList();
                */
                var userList = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("UserAll"
                    //var userList = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("SPGetUser @Id,@Name"
                    //,new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = 1 }
                    //,new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value  =1}
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Register([FromBody] UserViewModel userViewModel)
        {
            try
            {

                userViewModel.Password = HashPassword.GetHashCode(userViewModel.Password);

                var userIsAlreadyAvailible = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("CheckUser @UserName"
                     , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = userViewModel.UserName }
                    ).FirstOrDefault();
                if (userIsAlreadyAvailible.Result > 0)
                {
                    userRepsonse.AlradyUserAvailible((new JavaScriptSerializer()).Serialize("User Already Availible"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {
                    var userList = unitOfWork.GetRepositoryInstance<UserCompanyViewModel>().ReadStoredProcedure("AddUser @FullName,@UserName,@Password,@CompanyId,@RoleId"
                        , new SqlParameter("FullName", System.Data.SqlDbType.VarChar) { Value = userViewModel.FullName }                      
                        , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = userViewModel.UserName }
                        , new SqlParameter("Password", System.Data.SqlDbType.NVarChar) { Value = userViewModel.Password }
                        , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = 0 }
                        , new SqlParameter("RoleId", System.Data.SqlDbType.Int) { Value = 3 }
                         ).FirstOrDefault();                   
                    
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetRoles(int Id)
        {
            try
            {
                var RoleList = unitOfWork.GetRepositoryInstance<RolesViewModel>().ReadStoredProcedure("RoleAll @CompanyId"
                  , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = Id });

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(RoleList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage Login(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.Password = HashPassword.GetHashCode(loginViewModel.Password);
                var responselist = unitOfWork.GetRepositoryInstance<UserCompanyViewModel>().ReadStoredProcedure("UserLogin @UserName,@Password"
                    , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.UserName }
                    , new SqlParameter("Password", System.Data.SqlDbType.VarChar) { Value = loginViewModel.Password }
                    ).FirstOrDefault();

                if (responselist.FullName == null)
                {
                    userRepsonse.NotFound((new JavaScriptSerializer()).Serialize("Data Not Found"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {
                    if (responselist.CompanyId > 0)
                    {

                        if (loginViewModel.Token != null && loginViewModel.Token != "")
                        {
                            var notificationAddResponse = unitOfWork.GetRepositoryInstance<UserCompanyViewModel>().ReadStoredProcedure("NotificationInformationAdd @DeviceId,@DeviceToken,@Email,@Authority,@CompanyId,@Device"
                                                       , new SqlParameter("DeviceId", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.DeviceId == null ? (object)DBNull.Value : loginViewModel.DeviceId }
                                                       , new SqlParameter("DeviceToken", System.Data.SqlDbType.VarChar) { Value = loginViewModel.Token == null ? (object)DBNull.Value : loginViewModel.Token }
                                                       , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.UserName }
                                                       , new SqlParameter("Authority", System.Data.SqlDbType.VarChar) { Value = responselist.Authority }
                                                       , new SqlParameter("CompanyId", System.Data.SqlDbType.VarChar) { Value = responselist.CompanyId }
                                                       , new SqlParameter("Device", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.Device == null ? (object)DBNull.Value : loginViewModel.Device }
                                                       ).FirstOrDefault();
                        }
                    }

                    if(responselist.Authority == "AdminDriver")
                    {
                        CustomerOrderController customerOrderController = new CustomerOrderController();
                        CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();

                        customerOrderListViewModel.NotificationCode = "ADM-004";
                        customerOrderListViewModel.Title = "Driver Login";
                        customerOrderListViewModel.Message =  responselist.FullName +" Is availible";
                        customerOrderListViewModel.RequestedQuantity = 0;

                        int Res = customerOrderController.AdminNotificaton(customerOrderListViewModel);
                    }                    
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(responselist));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
                try
                {

                    changePasswordViewModel.Password = HashPassword.GetHashCode(changePasswordViewModel.Password);
                    changePasswordViewModel.NewPassword = HashPassword.GetHashCode(changePasswordViewModel.NewPassword);

                    var responselist = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("UserPasswordChange @Id,@Password,@NewPassword,@Email"
                        , new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = changePasswordViewModel.Id }
                        , new SqlParameter("Password", System.Data.SqlDbType.NVarChar) { Value = changePasswordViewModel.Password }
                        , new SqlParameter("NewPassword", System.Data.SqlDbType.VarChar) { Value = changePasswordViewModel.NewPassword }
                        , new SqlParameter("Email", System.Data.SqlDbType.VarChar) { Value = changePasswordViewModel.Email }
                        ).FirstOrDefault();

                    if (responselist.Result == 0)
                    {
                        userRepsonse.NotFound((new JavaScriptSerializer()).Serialize("Change password Failed"));
                        return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    }

                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("Password chnage successfully"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception exception)
                {
                    userRepsonse.Exception(exception.Message);
                    return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
                }
        }
        
        [HttpPost]
        public HttpResponseMessage ForgotPassword(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.Password = HashPassword.GetHashCode(loginViewModel.Password);             

                var responselist = unitOfWork.GetRepositoryInstance<SingleStringValueResult>().ReadStoredProcedure("UserEmailIsExist @UserName"
                    , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.UserName }
                    ).FirstOrDefault();

                if (responselist.Result == "No Email Found")
                {
                    userRepsonse.NotExist((new JavaScriptSerializer()).Serialize("Email No Found"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }

                string Number = GenerateNewRandom();

                if (sendMailRessetPassword(loginViewModel.UserName, responselist.Result, Number))
                {

                    SearchViewModel searchViewModel = new SearchViewModel();
                    searchViewModel.searchkey = Number;
                   
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize(searchViewModel));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                else
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("Failed to send email"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }       
        }
        
        [HttpPost]
        public HttpResponseMessage UpdatePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {

                 changePasswordViewModel.NewPassword = HashPassword.GetHashCode(changePasswordViewModel.NewPassword);

                var responselist = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("UserPasswordUpassword @NewPassword,@Email"                   
                    , new SqlParameter("NewPassword", System.Data.SqlDbType.VarChar) { Value = changePasswordViewModel.NewPassword }
                    , new SqlParameter("Email", System.Data.SqlDbType.VarChar) { Value = changePasswordViewModel.Email }
                    ).FirstOrDefault();

                if (responselist.Result == 0)
                {
                    userRepsonse.NotExist((new JavaScriptSerializer()).Serialize("Change password Failed"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize("Password chnage successfully"));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
            
        [HttpPost]
        public HttpResponseMessage LogOut(SearchViewModel searchViewModel)
        {
            try
            {                
                var responselist = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().ReadStoredProcedure("LogOut @DeviceId"
                    , new SqlParameter("DeviceId", System.Data.SqlDbType.NVarChar) { Value = searchViewModel.DeviceId }
                    ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(1));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
               
        [HttpPost]
        public HttpResponseMessage UpdateToken(LoginViewModel loginViewModel)
        {
            try
            {
                var notificationAddResponse = unitOfWork.GetRepositoryInstance<UserCompanyViewModel>().ReadStoredProcedure("NotificationInformationUpdate @DeviceId,@DeviceToken,@Email,@Authority,@CompanyId,@Device"
                                                       , new SqlParameter("DeviceId", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.DeviceId == null ? (object)DBNull.Value : loginViewModel.DeviceId }
                                                       , new SqlParameter("DeviceToken", System.Data.SqlDbType.VarChar) { Value = loginViewModel.Token == null ? (object)DBNull.Value : loginViewModel.Token }
                                                       , new SqlParameter("Email", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.UserName ?? (object)DBNull.Value }
                                                       , new SqlParameter("Authority", System.Data.SqlDbType.VarChar) { Value = loginViewModel.Authority ?? (object)DBNull.Value }
                                                       , new SqlParameter("CompanyId", System.Data.SqlDbType.VarChar) { Value = loginViewModel.CompanyId }
                                                       , new SqlParameter("Device", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.Device == null ? (object)DBNull.Value : loginViewModel.Device }
                                                       ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(1));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage UserInformationByUserName(LoginViewModel loginViewModel)
        {
            try
            {
                var notificationAddResponse = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("UserInformationByUserName @userName"
                                                        ,new SqlParameter("userName", System.Data.SqlDbType.NVarChar) { Value = loginViewModel.UserName ?? (object)DBNull.Value }
                                                        ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(notificationAddResponse));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UserInformationUpdate()
        {
            UserViewModel userViewModel = new UserViewModel();

            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    userRepsonse.Success((new JavaScriptSerializer()).Serialize("Media type Not Supported"));
                    return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                    //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;

                //access files  
                IList<HttpContent> files = provider.Files;

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string DDTT = unixTimestamp.ToString();

                for (int i = 0; i < files.Count; i++)
                {

                    HttpContent file1 = files[i];

                    var thisFileName = DDTT + file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = String.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = String.Empty;
                    string URL = String.Empty;
                    string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                    if (formData["ClientDocs"] == "ClientDocs")
                    {
                        var path = HttpRuntime.AppDomainAppPath;
                        directoryName = System.IO.Path.Combine(path, "ClientDocument");
                        filename = System.IO.Path.Combine(directoryName, thisFileName);

                        //Deletion exists file  
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }

                        if (file1.Headers.ContentDisposition.Name == "\"ImageUrl\"" || file1.Headers.ContentDisposition.DispositionType == "ImageUrl")
                        {
                            userViewModel.ImageUrl = thisFileName;
                        }

                        string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                        URL = DocsPath + thisFileName;

                    }

                    //Directory.CreateDirectory(@directoryName);  
                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        //close file  
                        file.Close();
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("DocsUrl", URL);
                }

                userViewModel.UserId = Convert.ToInt32(HttpContext.Current.Request["UserId"]);
                userViewModel.UserName = HttpContext.Current.Request["UserName"];
                userViewModel.FullName = HttpContext.Current.Request["FullName"];
                userViewModel.Gender = HttpContext.Current.Request["Gender"];
                userViewModel.DOB = Convert.ToDateTime(HttpContext.Current.Request["DOB"]); 
                
                if(userViewModel.ImageUrl == null || userViewModel.ImageUrl == "")
                {
                    userViewModel.ImageUrl = HttpContext.Current.Request["ImageUrl"];
                }

                var notificationUpdateResponse = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("UserInformationUpdate @UserID,@UserName,@FullName,@ImageUrl,@Gender,@DOB"
                                                        , new SqlParameter("UserID", System.Data.SqlDbType.Int) { Value = userViewModel.UserId }
                                                        , new SqlParameter("UserName", System.Data.SqlDbType.NVarChar) { Value = userViewModel.UserName ?? (object)DBNull.Value }
                                                        , new SqlParameter("FullName", System.Data.SqlDbType.NVarChar) { Value = userViewModel.FullName ?? (object)DBNull.Value }
                                                        , new SqlParameter("ImageUrl", System.Data.SqlDbType.NVarChar) { Value = userViewModel.ImageUrl ?? (object)DBNull.Value }
                                                        , new SqlParameter("Gender", System.Data.SqlDbType.NVarChar) { Value = userViewModel.Gender ?? (object)DBNull.Value }
                                                        , new SqlParameter("DOB", System.Data.SqlDbType.DateTime) { Value = userViewModel.DOB ?? (object)DBNull.Value }
                                                        ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(notificationUpdateResponse));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }

        #region Email
        [NonAction]
        public bool sendMailRessetPassword(string ToEmail, string Code, string Number)
        {
            try
            {

                #region formatter
                string text = string.Format("Please click on this link to {0}: {1}", "", "Please verify your email to loogin");
                string html = "Reset Password Code:<br/>";

                html += Number;

                //html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + link);
                #endregion

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("info@awfuel.com"); //IMPORTANT: This must be same as your smtp authentication address.
                mail.To.Add(ToEmail);

                //set the content 
                mail.Subject = "email verification";
                mail.IsBodyHtml = true;
                mail.Body = html;
                //send the message 
                SmtpClient smtp = new SmtpClient("mail.awfuel.com");

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                smtp.Port = 8889;
                NetworkCredential Credentials = new NetworkCredential("info@awfuel.com", "Admin@123");
                smtp.Credentials = Credentials;
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        [NonAction]
        private static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
    }
}
