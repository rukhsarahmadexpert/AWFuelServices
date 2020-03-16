using System.Web.Mvc;

namespace IT.Web.Areas.AWDriver
{
    public class AWDriverAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AWDriver";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AWDriver_default",
                "AWDriver/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}