using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IT.Web.Areas.Email.Controllers
{
    [Autintication]
    public class EmailController : Controller
    {
        WebServices webServices = new WebServices();

        List<LPOInvoiceModel> lPOInvoiceModels = new List<LPOInvoiceModel>();
       

        // GET: Email/Email
        public ActionResult Index()
        {
            EmailFormModel model = new EmailFormModel();

            if (TempData["Id"] != null)
            {
                model.LPOINvoiceId = TempData["Id"].ToString();
                model.FileName = TempData["FileName"].ToString();


                model.FlagToRedirect = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();


                TempData.Keep();

                return View(model);
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index2(EmailFormModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    int Id = Convert.ToInt32(model.LPOINvoiceId);

                    var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p></p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.ToEmail));  // replace with valid value 

                    if (model.CC != null)
                    {
                        message.To.Add(new MailAddress(model.CC));  
                    }
                    message.From = new MailAddress("itmolen1@gmail.com");  // replace with valid value
                                                                           //message.Subject = "Please find attached the file for the requested order confirmation.";
                    message.Subject = "Please find the attached.";

                    string FileName = "~/PDF/" + model.FileName;

                    using (var smtp = new SmtpClient())
                    {
                        using (var fstemp = new FileStream(Server.MapPath(FileName), FileMode.Open, FileAccess.Read))
                        {
                            message.Attachments.Add(new Attachment(fstemp, model.FileName));

                            message.Body = string.Format(body, model.FromName, model.Message);

                            message.IsBodyHtml = true;

                            var credential = new NetworkCredential
                            {
                                UserName = "itmolen1@gmail.com",  // replace with valid value
                                Password = "Daman0011234a"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            await smtp.SendMailAsync(message);

                            if (model.FlagToRedirect != null)
                            {
                                TempData["Success"] = "Your message has been sent successfully";
                                return Redirect(model.FlagToRedirect);
                            }

                            fstemp.Close();
                        }
                        return RedirectToRoute("Sent");
                    }
                }
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(EmailFormModel model)
        {
            try
            { 
                MailMessage mail = new MailMessage();

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p></p>";
                //set the addresses 
                mail.From = new MailAddress("itmolen1@gmail.com"); //IMPORTANT: This must be same as your smtp authentication address.
                mail.To.Add(model.ToEmail);

                //set the content 
                mail.Subject = "Please find the attachment";
                mail.Body = string.Format(body, model.FromName, "this is test match");
                //send the message 
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                NetworkCredential Credentials = new NetworkCredential("itmolen1@gmail.com", "Daman0011234a");
                smtp.Credentials = Credentials;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                
                await smtp.SendMailAsync(mail);

                if (model.FlagToRedirect != null)
                {
                    TempData["Success"] = "Your message has been sent successfully";
                    return Redirect(model.FlagToRedirect);
                }
                return RedirectToRoute("Sent");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public ActionResult Sent()
        {
            return View();
        }



        public ActionResult EmailQuotation()
        {

            return View();
        }
    }
}