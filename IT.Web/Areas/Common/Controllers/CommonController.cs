using IT.Core.ViewModels;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IT.Web.Areas.Common.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common/Common
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UpdateImage(CompanyImages companyImages)
        {


            if (Request.Files.Count > 0)
            {
                try
                {
                    //return Json("s");

                    companyImages.ImageName = companyImages.ImagesUrl.Split('/').Last();
                    companyImages.Flage = companyImages.ImageName.Split('-').Skip(0).FirstOrDefault();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string DirectoryUrl = companyImages.EntityName + "-" + companyImages.UID;

                        fname = companyImages.ImageName;

                        if (CommonFile.DeleteFile(companyImages.ImagesUrl))
                        {
                            fname = Path.Combine(Server.MapPath("~/Uploads/Uploads/" + DirectoryUrl), fname);

                            file.SaveAs(fname);
                            // Get the complete folder path and store the file inside it.  
                            companyImages.ImageName = "";
                        }
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            return View();
        }


        [HttpGet]
        public PartialViewResult ImagePreview()
        {
            return PartialView("~/Views/Shared/_PartialView/_EditImagePartialView.cshtml");
        }


        public async Task UploadAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://hello.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("C:/Images/testing.png"));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "testing.png" };
                //content.Add()
                content.Add(fileContent);

                HttpResponseMessage response = await client.PostAsync("api/SaveFile?title=hello&recordType=audio", content);
        
            }
        }

    }
}