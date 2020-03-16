using System.Web.Mvc;

namespace IT.Web.Areas.AWFEmployeeSalary
{
    public class AWFEmployeeSalaryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AWFEmployeeSalary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AWFEmployeeSalary_default",
                "AWFEmployeeSalary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}