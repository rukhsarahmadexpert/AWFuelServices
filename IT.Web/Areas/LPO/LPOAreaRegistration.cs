using System.Web.Mvc;

namespace IT.Web.Areas.LPO
{
    public class LPOAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LPO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LPO_default",
                "LPO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}