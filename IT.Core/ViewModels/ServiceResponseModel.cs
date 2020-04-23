using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ServiceResponseModel
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public void Success(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.Accepted;
            this.IsSuccess = true;
            this.Message = "Success";
            this.Data = entity;
        }
        public void Failed(string entity, string message)
        {
            this.StatusCode = System.Net.HttpStatusCode.ExpectationFailed;
            this.IsSuccess = false;
            this.Message = message;
            this.Data = entity;
        }
        public void BadRequest(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.BadRequest;
            this.IsSuccess = false;
            this.Message = "Bad Request";
            this.Data = entity;
        }
        public void Exception(string errorMessage)
        {
            this.StatusCode = System.Net.HttpStatusCode.Conflict;
            this.IsSuccess = true;
            this.Message = errorMessage;
            this.Data = null;
        }
        public void UnAuthorized()
        {
            this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            this.IsSuccess = false;
            this.Message = "UnAuthorized user";
            this.Data = null;
        }

        public void NotFound(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.NotFound;
            this.IsSuccess = true;
            this.Message = "Data Not Found";
            this.Data = "";
        }

        public void AlradyUserAvailible(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.OK;
            this.IsSuccess = false;
            this.Message = entity;
            this.Data = "";
        }

        public void NotExist(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.NotFound;
            this.IsSuccess = false;
            this.Message = "Email Not Found";
            this.Data = "";
        }

    }
}
