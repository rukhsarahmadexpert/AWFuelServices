using IT.Core.ViewModels;
using IT.Repository;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class NotificationController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        readonly ServiceResponseModel userRepsonse = new ServiceResponseModel();

       // readonly string contentType = "application/json";

       public int SendPushNotificationWebAndroid(string deviceTokens, string Title, string NotificationCode, string Message)
       {
            var data = new
            {
                //  to = "ekTZ2VAAibk:APA91bEDdF5TgwQY4BRibImhvquxjQkGFSmqYI9Ez-8uwfvVu4vXBbK8sjMPNXl8PwhBGYe0c9oShcjkIlITsY2cTRwa05_SBZZu0P22JbJ1K0P-U-F5fi9KZeJAJ9L_T8I6cbp76hys",
                to = deviceTokens,
                // IOS,
                data = new
                {
                    Code = NotificationCode,
                    //Name = searchViewModel[0].CompanyName,
                    //Quantity = searchViewModel[0].Quantity.ToString(),
                    Titles = Title,
                    Messages = Message,

                }
            };

            SendNotification(data);

            return 1;
        }


        [HttpPost]
        public int SendMessage(List<SearchViewModel> searchViewModel)
        {
            foreach (var item in searchViewModel)
            {
                try
                {
                    var Android2 = "cgLBkmfLjpU:APA91bFcUPZyQ0OR9H6bGuKrmBqoJo_KxzodK9jAVk8ysPPZUhj73UaoYuYqLPwhRF5ip73AZiLhsP1CfK-NGAA5b-ldTMlCwkXujwoUD3KtInKibFielDKYM07TqJN_RmpQQ1brD95Q";
                    // var Android = "fZW6FXJes1Y:APA91bEfhblC_EbTPHAqSApMEaBs9xkm0HKJFSTGNdjd-NdYzhvcs1pbDheCoYcgbHA578v1EFNgx7gbZlISr2EInFbdajV1D5YW8ByK9UZTi-x4NjpDWNoEzURpY3o6lwxT5YwHtezT";
                  //  var IOS = "dkhQf8Z_jzQ:APA91bEmQnMFrKpu0iL7SL3hOP-wpEdxo6zcL_8ogF_JxIUO8oSUR14cd4cIeUrGy_GObcMgecIkrrZjhiTWp5Olz05xmGyDgwnC8K43Mylp54e63WJKsP_dC9lY9CEN45SymegH6igN";
                                      
                    
                 if (item.DeviceTiken != null && item.DeviceTiken != "")
                    {
                        Android2 = item.DeviceTiken;
                    }      

                    var data = new
                    {
                        //  to = "ekTZ2VAAibk:APA91bEDdF5TgwQY4BRibImhvquxjQkGFSmqYI9Ez-8uwfvVu4vXBbK8sjMPNXl8PwhBGYe0c9oShcjkIlITsY2cTRwa05_SBZZu0P22JbJ1K0P-U-F5fi9KZeJAJ9L_T8I6cbp76hys",
                        to = Android2,
                       // IOS,
                        data = new
                        {
                            Code = searchViewModel[0].NotificationCode,
                            //Name = searchViewModel[0].CompanyName,
                            //Quantity = searchViewModel[0].Quantity.ToString(),
                            Titles = searchViewModel[0].Title,
                            Messages = searchViewModel[0].Message,

                        }
                    };

                    SendNotification(data);

                    //userRepsonse.Success((new JavaScriptSerializer()).Serialize("Android notification sended successfully"));
                    // return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                catch (Exception ex)
                {
                    throw ex;
                   // userRepsonse.Success((new JavaScriptSerializer()).Serialize("Exception =>:" + ex.ToString()));
                   // return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
                }
                // return Ok;            
            }

            return 1;
        }


        public void SendNotification(object data)
        {
            var Serlializer = new JavaScriptSerializer();
            var json = Serlializer.Serialize(data);
            Byte[] ByteArry = Encoding.UTF8.GetBytes(json);


            SendNotification(ByteArry);
        }

        public void SendNotification(Byte[] byteArray)
        {
            try
            {
                            
            string SERVER_API_KEY = ConfigurationManager.AppSettings["SERVER_API_KEY"];
            string SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"];

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format($"Authorization: key={SERVER_API_KEY}"));
            tRequest.Headers.Add(string.Format($"Sender: id={SENDER_ID}"));

            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray,0,byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //New Code for IOS
        private void SendPushNotification(string deviceToken, string message)
        {
            try
            {
                //Get Certificate
                var applCert = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Files/Certificate/IOS/certificate_developer.pfx"));


                var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,applCert,"",true);

                var apnsBroker = new ApnsServiceBroker(config);


                apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {
                    aggregateEx.Handle(ex =>
                    {
                        //See what kind of exception it to further diafnose
                        if (ex is ApnsNotificationException)
                        {

                            //----------Before Change-------
                            /*var notificationException = (ApnsNotificationException)ex;

                            var apnsNotification = notificationException.Notification;
                            var StatusCode = notificationException.ErrorStatusCode;
                            string desc = $"Apple Notification Failed: ID={apnsNotification.Identifier},Code={StatusCode}";
                            Console.WriteLine(desc);                           */
                           
                            //----------


                            //Deal with the failed notification
                            var apnsNotification = ((ApnsNotificationException)ex).Notification;
                            var StatusCode = ((ApnsNotificationException)ex).ErrorStatusCode;
                            string desc = $"Apple Notification Failed: ID={apnsNotification.Identifier},Code={StatusCode}";
                            Console.WriteLine(desc);
                            //lblStatus.Text = desc;
                        }
                        else
                        {
                            string desc = $"Apple Notification Failed for some unknown reason: {ex.InnerException}";
                        }

                        return true;
                    });
                };


                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    //lblStatus.text = "Apple notification send"; 
                }; 
                
                var fbs = new FeedbackService(config);
                fbs.FeedbackReceived += (string devicToken, DateTime timeStamp) =>
                {
                    //Remove the device token from your database
                    //timeStamp is the time the token was reported as expired

                };

                //Start process
                apnsBroker.Start();

                var data = new
                             {
                                Key = "Test Massage Received IOS",
                                title = "WelCome",
                                message = "Hello Ravi",
                                name = "Ravi",
                                userId = "1",
                                status = true
                    
                              };


                var Serlializer = new JavaScriptSerializer();
                var json = Serlializer.Serialize(data);
                Byte[] ByteArry = Encoding.UTF8.GetBytes(json);

                //message = ByteArry
                
                if (deviceToken != "")
                {
                    apnsBroker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = deviceToken,
                        // Payload = JObject.Parse(("{\"aps\":{\"badge\":1,\"sound\":\"oven.caf\",\"alert\":\"" + (ByteArry + "\"}}")))
                        Payload = JObject.Parse(("{\"aps\":{\"badge\":1,\"sound\":\"oven.caf\",\"alert\":\"" + (message + "\"}}")))
                       
                    });
                      
                }

                apnsBroker.Stop();

            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<NotificationInformation> GetAllAdminTokens(string Authority)
        {
            try
            {
                List<NotificationInformation> notificationInformation = new List<NotificationInformation>();

                notificationInformation = unitOfWork.GetRepositoryInstance<NotificationInformation>().ReadStoredProcedure("NotificationInformationGetByAuthority @Authority"
                      , new SqlParameter("Authority", System.Data.SqlDbType.NVarChar) { Value = Authority }
                      ).ToList();

                return notificationInformation;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public List<NotificationInformation> GetCompaniesTokens(int CompanyId)
        {
            try
            {
                List<NotificationInformation> notificationInformation = new List<NotificationInformation>();

                notificationInformation = unitOfWork.GetRepositoryInstance<NotificationInformation>().ReadStoredProcedure("GetCompaniesTokens @CompanyId"
                      , new SqlParameter("CompanyId", System.Data.SqlDbType.Int) { Value = CompanyId }
                      ).ToList();

                return notificationInformation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<NotificationInformation> GetDriverTokens(string email)
        {
            try
            {
                List<NotificationInformation> notificationInformation = new List<NotificationInformation>();

                notificationInformation = unitOfWork.GetRepositoryInstance<NotificationInformation>().ReadStoredProcedure("GetDriverTokens @email"
                      , new SqlParameter("email", System.Data.SqlDbType.NVarChar) { Value = email }
                      ).ToList();

                return notificationInformation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
