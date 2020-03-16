using System.Web.Mvc;

namespace IT.Web.Areas.AWFVehicle
{
    public class AWFVehicleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AWFVehicle";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AWFVehicle_default",
                "AWFVehicle/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}