using System.Web.Mvc;

namespace IT.Web.Areas.FuelTransfer
{
    public class FuelTransferAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FuelTransfer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FuelTransfer_default",
                "FuelTransfer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}