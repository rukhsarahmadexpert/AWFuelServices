using System.Web.Mvc;

namespace IT.Web.Areas.AWFEmployee
{
    public class AWFEmployeeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AWFEmployee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AWFEmployee_default",
                "AWFEmployee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}