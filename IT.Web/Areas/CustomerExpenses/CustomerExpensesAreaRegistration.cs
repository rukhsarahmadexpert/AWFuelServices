using System.Web.Mvc;

namespace IT.Web.Areas.CustomerExpenses
{
    public class CustomerExpensesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CustomerExpenses";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CustomerExpenses_default",
                "CustomerExpenses/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}