using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IT.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Default      

            routes.MapRoute("Default", "Default",
                new
                {
                    controller = "Home",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
                }).DataTokens["area"] = "Home";

            routes.MapRoute(null, "",
                  new
                  {
                      controller = "Home",
                      action = "Index",
                      namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
                  }).DataTokens["area"] = "Home";

            //routes.MapRoute("Home", "Home",
            //      new
            //      {
            //          controller = "Home",
            //          action = "Index",
            //          namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
            //      }).DataTokens["area"] = "Home";

            //routes.MapRoute("Dashboard", "Dashboard",

            routes.MapRoute("Home", "Home",
                  new
                  {
                      controller = "Home",
                      action = "CustomerHome",
                      namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
                  }).DataTokens["area"] = "Home";

            routes.MapRoute("Dashboard", "Dashboard",
                 new
                 {
                     controller = "Home",
                     action = "Dashboard",
                     namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
                 }).DataTokens["area"] = "Home";


            routes.MapRoute("User", "User",
              new
              {
                  controller = "User",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.User.Controllers" }
              }).DataTokens["area"] = "User";

            #endregion

            #region Login
            routes.MapRoute("Login", "Login",
              new
              {
                  controller = "Login",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.Login.Controllers" }
              }).DataTokens["area"] = "Login";

            routes.MapRoute("LogOut", "LogOut",
              new
              {
                  controller = "Login",
                  action = "LogOut",
                  namespaces = new[] { "IT.Web.Areas.Login.Controllers" }
              }).DataTokens["area"] = "Login";

            #endregion

            #region AWF Employee

            routes.MapRoute("AWFEmployee", "AWFEmployee",
              new
              {
                  controller = "AWFEmployee",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
              }).DataTokens["area"] = "AWFEmployee";


            routes.MapRoute("AWFEmployeeAll", "AWFEmployeeAll",
             new
             {
                 controller = "AWFEmployee",
                 action = "GetAll",
                 namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
             }).DataTokens["area"] = "AWFEmployee";


            routes.MapRoute("AWFEmployee-Create", "AWFEmployee-Create",
             new
             {
                 controller = "AWFEmployee",
                 action = "Create",
                 namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
             }).DataTokens["area"] = "AWFEmployee";


            routes.MapRoute("AWFEmployee-Edit/{id}", "AWFEmployee-Edit/{id}",
            new
            {
                controller = "AWFEmployee",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
            }).DataTokens["area"] = "AWFEmployee";

            routes.MapRoute("AWFEmployee-Update", "AWFEmployee-Update",
            new
            {
                controller = "AWFEmployee",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
            }).DataTokens["area"] = "AWFEmployee";
            


            routes.MapRoute("AWFEmployee-Details/{id}", "AWFEmployee-Details/{id}",
              new
              {
                  controller = "AWFEmployee",
                  action = "Details",
                  namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
              }).DataTokens["area"] = "AWFEmployee";


            routes.MapRoute("AWFEmployee-Delete", "AWFEmployee-Delete",
               new
               {
                   controller = "AWFEmployee",
                   action = "Delete",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployee.Controllers" }
               }).DataTokens["area"] = "AWFEmployee";

            #endregion

            #region Driver AWFuel

            routes.MapRoute("AWFDriver", "AWFDriver",
              new
              {
                  controller = "AWFDriver",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
              }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("AWFDriver-Create", "AWFDriver-Create",
              new
              {
                  controller = "AWFDriver",
                  action = "Create",
                  namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
              }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("AWFDriverAll", "AWFDriverAll",
              new
              {
                  controller = "AWFDriver",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
              }).DataTokens["area"] = "AWFDriver";

            routes.MapRoute("Driver-Edit/{id}", "Driver-Edit/{id}",
             new
             {
                 controller = "AWFDriver",
                 action = "Edit",
                 namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
             }).DataTokens["area"] = "AWFDriver";

            routes.MapRoute("AWFDriver-Update", "AWFDriver-Update",
            new
            {
                controller = "AWFDriver",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
            }).DataTokens["area"] = "AWFDriver";
            

            routes.MapRoute("AWFDriver-Details/{id}", "AWFDriver-Details/{id}",
            new
            {
                controller = "AWFDriver",
                action = "Details",
                namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
            }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("AWFDriver-Delete", "AWFDriver-Delete",
            new
            {
                controller = "AWFDriver",
                action = "Delete",
                namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
            }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("DriverAWFInfoByEmail", "DriverAWFInfoByEmail",
            new
            {
                controller = "AWFDriver",
                action = "DriverAWFInfoByEmail",
                namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
            }).DataTokens["area"] = "AWFDriver";

            #endregion

            #region Vehicle AWFuel

            routes.MapRoute("AWFVehicle", "AWFVehicle",
             new
             {
                 controller = "AWFVehicle",
                 action = "Index",
                 namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
             }).DataTokens["area"] = "AWFVehicle";

            routes.MapRoute("AWFVehicleAll", "AWFVehicleAll",
              new
              {
                  controller = "AWFVehicle",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
              }).DataTokens["area"] = "AWFVehicle";


            routes.MapRoute("AWFVehicle-Delete", "AWFVehicle-Delete",
              new
              {
                  controller = "AWFVehicle",
                  action = "Delete",
                  namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
              }).DataTokens["area"] = "AWFVehicle";


            routes.MapRoute("AWFVehicle-Create", "AWFVehicle-Create",
              new
              {
                  controller = "AWFVehicle",
                  action = "Create",
                  namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
              }).DataTokens["area"] = "AWFVehicle";


            routes.MapRoute("AWFVehicle-Details/{id}", "AWFVehicle-Details/{id}",
                new
                {
                    controller = "AWFVehicle",
                    action = "Details",
                    namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
                }).DataTokens["area"] = "AWFVehicle";


            routes.MapRoute("AWFVehicle-Edit/{id}", "AWFVehicle-Edit/{id}",
                new
                {
                    controller = "AWFVehicle",
                    action = "Edit",
                    namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
                }).DataTokens["area"] = "AWFVehicle";


            routes.MapRoute("AWFVehicle-Update", "AWFVehicle-Update",
               new
               {
                   controller = "AWFVehicle",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
               }).DataTokens["area"] = "AWFVehicle";

            
            routes.MapRoute("VehicleGatAllUnAsigned", "VehicleGatAllUnAsigned",
                new
                {
                    controller = "AWFVehicle",
                    action = "VehicleGatAllUnAsigned",
                    namespaces = new[] { "IT.Web.Areas.AWFVehicle.Controllers" }
                }).DataTokens["area"] = "AWFVehicle";


            #endregion

            #region Bill

            routes.MapRoute("Bill", "Bill",
            new
            {
                controller = "Bill",
                action = "Index",
                namespaces = new[] { "IT.Web.Areas.Bill.Controllers" }
            }).DataTokens["area"] = "Bill";

            routes.MapRoute("Bill-Create/{Id}", "Bill-Create/{Id}",
            new
            {
                controller = "Bill",
                action = "Create",
                namespaces = new[] { "IT.Web.Areas.Bill.Controllers" }
            }).DataTokens["area"] = "Bill";

            routes.MapRoute("Bill-Details/{Id}", "Bill-Details/{Id}",
            new
            {
                controller = "Bill",
                action = "Details",
                namespaces = new[] { "IT.Web.Areas.Bill.Controllers" }
            }).DataTokens["area"] = "Bill";

            routes.MapRoute("Bill-Delete/{Id}", "Bill-Delete/{Id}",
            new
            {
                controller = "Bill",
                action = "Delete",
                namespaces = new[] { "IT.Web.Areas.Bill.Controllers" }
            }).DataTokens["area"] = "Bill";

            routes.MapRoute("BillAll", "BillAll",
            new
            {
                controller = "Bill",
                action = "GetAll",
                namespaces = new[] { "IT.Web.Areas.Bill.Controllers" }
            }).DataTokens["area"] = "Bill";




            #endregion
            
            #region LPO

            routes.MapRoute("LPO", "LPO",
            new
            {
                controller = "LPO",
                action = "Index",
                namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
            }).DataTokens["area"] = "LPO";


            routes.MapRoute("LPO-Create", "LPO-Create",
            new
            {
                controller = "LPO",
                action = "Create",
                namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
            }).DataTokens["area"] = "LPO";

           
            routes.MapRoute("LPOConverted", "LPOConverted",
            new
            {
                controller = "LPO",
                action = "LPOConverted",
                namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
            }).DataTokens["area"] = "LPO";


            routes.MapRoute("LPO-Details/{Id}", "LPO-Details/{Id}",
            new
            {
                controller = "LPO",
                action = "Details",
                namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
            }).DataTokens["area"] = "LPO";


            routes.MapRoute("LPO-Edit/{Id}", "LPO-Edit/{Id}",
                new
                {
                    controller = "LPO",
                    action = "Edit",
                    namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
                }).DataTokens["area"] = "LPO";

            routes.MapRoute("DeleteLPoDetailsRow", "DeleteLPoDetailsRow",
                new
                {
                    controller = "LPO",
                    action = "DeleteLPoDetailsRow",
                    namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
                }).DataTokens["area"] = "LPO";


            routes.MapRoute("LPO-Update", "LPO-Update",
               new
               {
                   controller = "LPO",
                   action = "Update",
                   namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
               }).DataTokens["area"] = "LPO";


            routes.MapRoute("PrintLPO/{Id}", "PrintLPO/{Id}",
               new
               {
                   controller = "LPO",
                   action = "PrintLPO",
                   namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
               }).DataTokens["area"] = "LPO";


            routes.MapRoute("SaveDwnload", "SaveDwnload",
              new
              {
                  controller = "LPO",
                  action = "SaveDwnload",
                  namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
              }).DataTokens["area"] = "LPO";

            routes.MapRoute("CheckISFileExist", "CheckISFileExist",
              new
              {
                  controller = "LPO",
                  action = "CheckISFileExist",
                  namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
              }).DataTokens["area"] = "LPO";

            routes.MapRoute("LPOAll", "LPOAll",
             new
             {
                 controller = "LPO",
                 action = "GetAll",
                 namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
             }).DataTokens["area"] = "LPO";


            routes.MapRoute("GetAllConverted", "GetAllConverted",
               new
               {
                   controller = "LPO",
                   action = "GetAllConverted",
                   namespaces = new[] { "IT.Web.Areas.LPO.Controllers" }
               }).DataTokens["area"] = "LPO";


            #endregion
            
            #region Product

            routes.MapRoute("Product", "Product",
              new
              {
                  controller = "Product",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
              }).DataTokens["area"] = "Product";


            routes.MapRoute("ProductAll", "ProductAll",
              new
              {
                  controller = "Product",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
              }).DataTokens["area"] = "Product";

            routes.MapRoute("Product-Create", "Product-Create",
             new
             {
                 controller = "Product",
                 action = "Add",
                 namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
             }).DataTokens["area"] = "Product";


            routes.MapRoute("Product-Edit/{Id}", "Product-Edit/{Id}",
            new
            {
                controller = "Product",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
            }).DataTokens["area"] = "Product";

            routes.MapRoute("Product-Update", "Product-Update",
            new
            {
                controller = "Product",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
            }).DataTokens["area"] = "Product";


            routes.MapRoute("Product-Delete/{Id}", "Product-Delete/{Id}",
            new
            {
                controller = "Product",
                action = "Delete",
                namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
            }).DataTokens["area"] = "Product";

            #endregion

            #region ProductUnit

            routes.MapRoute("ProductUnit", "ProductUnit",
              new
              {
                  controller = "ProductUnit",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
              }).DataTokens["area"] = "ProductUnit";


            routes.MapRoute("ProductUnit-Create", "ProductUnit-Create",
              new
              {
                  controller = "ProductUnit",
                  action = "Add",
                  namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
              }).DataTokens["area"] = "ProductUnit";


            routes.MapRoute("UnitAll", "UnitAll",
              new
              {
                  controller = "ProductUnit",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
              }).DataTokens["area"] = "ProductUnit";

            routes.MapRoute("Unit-Edit/{Id}", "Unit-Edit/{Id}",
              new
              {
                  controller = "ProductUnit",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
              }).DataTokens["area"] = "ProductUnit";

            routes.MapRoute("UnitUpdate", "UnitUpdate",
               new
               {
                   controller = "ProductUnit",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
               }).DataTokens["area"] = "ProductUnit";


            routes.MapRoute("Unit-Delete/{Id}", "Unit-Delete/{Id}",
               new
               {
                   controller = "ProductUnit",
                   action = "Delete",
                   namespaces = new[] { "IT.Web.Areas.ProductUnit.Controllers" }
               }).DataTokens["area"] = "ProductUnit";

           
            #endregion
            
            #region Expense

            routes.MapRoute("Expense", "Expense",
             new
             {
                 controller = "Expense",
                 action = "Index",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("Expense-Create", "Expense-Create",
             new
             {
                 controller = "Expense",
                 action = "Create",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";

            routes.MapRoute("ExpenseFor", "ExpenseFor",
             new
             {
                 controller = "Expense",
                 action = "ExpenseFor",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("LoadVehicle", "LoadVehicle",
             new
             {
                 controller = "Expense",
                 action = "LoadVehicle",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("LoadEmployee", "LoadEmployee",
             new
             {
                 controller = "Expense",
                 action = "LoadEmployee",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("LoadGeneralExpense", "LoadGeneralExpense",
            new
            {
                controller = "Expense",
                action = "LoadGeneralExpense",
                namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
            }).DataTokens["area"] = "Expense";

            routes.MapRoute("ExpenseEdit/{id}", "ExpenseEdit/{id}",
           new
           {
               controller = "Expense",
               action = "Edit",
               namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
           }).DataTokens["area"] = "Expense";

          routes.MapRoute("ExpenseUpdate", "ExpenseUpdate",
          new
          {
              controller = "Expense",
              action = "Update",
              namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
          }).DataTokens["area"] = "Expense";

            routes.MapRoute("Expense-Details/{Id}", "Expense-Details/{Id}",
             new
             {
                 controller = "Expense",
                 action = "Details",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("DeleteExpDetailsRow", "DeleteExpDetailsRow",
             new
             {
                 controller = "Expense",
                 action = "DeleteExpDetailsRow",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";
            

            routes.MapRoute("ExpenseAll", "ExpenseAll",
             new
             {
                 controller = "Expense",
                 action = "GetAll",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("PrintExpense/{id}", "PrintExpense/{id}",
             new
             {
                 controller = "Expense",
                 action = "PrintExpense",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            routes.MapRoute("CheckIExpenseExist/{id}", "CheckIExpenseExist/{id}",
             new
             {
                 controller = "Expense",
                 action = "CheckIExpenseExist",
                 namespaces = new[] { "IT.Web.Areas.Expense.Controllers" }
             }).DataTokens["area"] = "Expense";


            #endregion

            #region Vender
            
            routes.MapRoute("Vender", "Vender",
             new
             {
                 controller = "Vender",
                 action = "Index",
                 namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
             }).DataTokens["area"] = "Vender";

            routes.MapRoute("Vender-Create", "Vender-Create",
             new
             {
                 controller = "Vender",
                 action = "Create",
                 namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
             }).DataTokens["area"] = "Vender";

            routes.MapRoute("VenderAll", "VenderAll",
              new
              {
                  controller = "Vender",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
              }).DataTokens["area"] = "Vender";

            routes.MapRoute("Vender-Edit/{id}", "Vender-Edit/{id}",
              new
              {
                  controller = "Vender",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
              }).DataTokens["area"] = "Vender";


            routes.MapRoute("Vender-Update", "Vender-Update",
               new
               {
                   controller = "Vender",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
               }).DataTokens["area"] = "Vender";


            routes.MapRoute("Vender-Delete", "Vender-Delete",
              new
              {
                  controller = "Vender",
                  action = "Delete",
                  namespaces = new[] { "IT.Web.Areas.Vender.Controllers" }
              }).DataTokens["area"] = "Vender";



            #endregion
            
            #region Purchase

            routes.MapRoute("Purchase", "Purchase",
              new
              {
                  controller = "Purchase",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
              }).DataTokens["area"] = "Purchase";


            routes.MapRoute("Purchase-Create", "Purchase-Create",
               new
               {
                   controller = "Purchase",
                   action = "Create",
                   namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
               }).DataTokens["area"] = "Purchase";


            routes.MapRoute("Purchase-Details/{Id}", "Purchase-Details/{Id}",
               new
               {
                   controller = "Purchase",
                   action = "Details",
                   namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
               }).DataTokens["area"] = "Purchase";


            routes.MapRoute("Purchase-Edit/{Id}", "Purchase-Edit/{Id}",
              new
              {
                  controller = "Purchase",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
              }).DataTokens["area"] = "Purchase";


            routes.MapRoute("DeleteLPurchaseDetailsRow/{Id}", "DeleteLPurchaseDetailsRow/{Id}",
                new
                {
                    controller = "Purchase",
                    action = "DeleteLPurchaseDetailsRow",
                    namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                }).DataTokens["area"] = "Purchase";


            routes.MapRoute("Purchase-Update", "Purchase-Update",
                new
                {
                    controller = "Purchase",
                    action = "Update",
                    namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                }).DataTokens["area"] = "Purchase";


            routes.MapRoute("printPurchase/{Id}", "printPurchase/{Id}",
                 new
                 {
                     controller = "Purchase",
                     action = "PrintLPO",
                     namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                 }).DataTokens["area"] = "Purchase";


            routes.MapRoute("CheckISPurchaseFileExist", "CheckISPurchaseFileExist",
                 new
                 {
                     controller = "Purchase",
                     action = "CheckISFileExist",
                     namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                 }).DataTokens["area"] = "Purchase";


            routes.MapRoute("CreateFromLPO/{Id}", "CreateFromLPO/{Id}",
                 new
                 {
                     controller = "Purchase",
                     action = "CreateFromLPO",
                     namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                 }).DataTokens["area"] = "Purchase";


            routes.MapRoute("PurchaseAll", "PurchaseAll",
                 new
                 {
                     controller = "Purchase",
                     action = "GetAll",
                     namespaces = new[] { "IT.Web.Areas.Purchase.Controllers" }
                 }).DataTokens["area"] = "Purchase";

            #endregion

            #region Site

            routes.MapRoute("Site", "Site",
                 new
                 {
                     controller = "Site",
                     action = "Index",
                     namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
                 }).DataTokens["area"] = "Site";

            routes.MapRoute("Site-Create", "Site-Create",
                 new
                 {
                     controller = "Site",
                     action = "Create",
                     namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
                 }).DataTokens["area"] = "Site";


            routes.MapRoute("SiteAll", "SiteAll",
                 new
                 {
                     controller = "Site",
                     action = "GetAll",
                     namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
                 }).DataTokens["area"] = "Site";


            routes.MapRoute("SiteGetAll", "SiteGetAll",
                 new
                 {
                     controller = "Site",
                     action = "SiteGetAll",
                     namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
                 }).DataTokens["area"] = "Site";


            routes.MapRoute("Site-Edit/{id}", "Site-Edit/{id}",
                new
                {
                    controller = "Site",
                    action = "Edit",
                    namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
                }).DataTokens["area"] = "Site";


            routes.MapRoute("Site-Update", "Site-Update",
               new
               {
                   controller = "Site",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
               }).DataTokens["area"] = "Site";


            routes.MapRoute("Site-Details/{Id}", "Site-Details/{Id}",
              new
              {
                  controller = "Site",
                  action = "Details",
                  namespaces = new[] { "IT.Web.Areas.Site.Controllers" }
              }).DataTokens["area"] = "Site";


            #endregion

            #region FuelTransfer

            routes.MapRoute("FuelTransfer", "FuelTransfer",
                 new
                 {
                     controller = "FuelTransfer",
                     action = "Index",
                     namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                 }).DataTokens["area"] = "FuelTransfer";


            routes.MapRoute("FuelTransferAdd", "FuelTransferAdd",
                new
                {
                    controller = "FuelTransfer",
                    action = "Create",
                    namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                }).DataTokens["area"] = "FuelTransfer";

            routes.MapRoute("FuelTransferAll", "FuelTransferAll",
                 new
                 {
                     controller = "FuelTransfer",
                     action = "GetAll",
                     namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                 }).DataTokens["area"] = "FuelTransfer";


            routes.MapRoute("FuelTransferEdit/{id}", "FuelTransferEdit/{id}",
                  new
                  {
                      controller = "FuelTransfer",
                      action = "Edit",
                      namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                  }).DataTokens["area"] = "FuelTransfer";


            routes.MapRoute("FuelTransferUpdate", "FuelTransferUpdate",
                 new
                 {
                     controller = "FuelTransfer",
                     action = "Edit",
                     namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                 }).DataTokens["area"] = "FuelTransfer";



            routes.MapRoute("FuelTransferDetails/{Id}", "FuelTransferDetails/{Id}",
                new
                {
                    controller = "FuelTransfer",
                    action = "Details",
                    namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                }).DataTokens["area"] = "FuelTransfer";


            routes.MapRoute("LoadVehicleFuelTransfer", "LoadVehicleFuelTransfer",
                new
                {
                    controller = "FuelTransfer",
                    action = "LoadVehicleFuelTransfer",
                    namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                }).DataTokens["area"] = "FuelTransfer";

            routes.MapRoute("LoadSiteFuelTransfer", "LoadSiteFuelTransfer",
                new
                {
                    controller = "FuelTransfer",
                    action = "LoadSiteFuelTransfer",
                    namespaces = new[] { "IT.Web.Areas.FuelTransfer.Controllers" }
                }).DataTokens["area"] = "FuelTransfer";
            
            #endregion

            #region Stock

            routes.MapRoute("Stock", "Stock",
                new
                {
                    controller = "Stock",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
                }).DataTokens["area"] = "Stock";


            routes.MapRoute("Stock-Create", "Stock-Create",
                new
                {
                    controller = "Stock",
                    action = "Create",
                    namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
                }).DataTokens["area"] = "Stock";

            routes.MapRoute("StockAll", "StockAll",
                new
                {
                    controller = "Stock",
                    action = "GetAll",
                    namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
                }).DataTokens["area"] = "Stock";

            routes.MapRoute("Stock-Edit/{id}", "Stock-Edit/{id}",
               new
               {
                   controller = "Stock",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
               }).DataTokens["area"] = "Stock";


            routes.MapRoute("Stock-Update", "Stock-Update",
               new
               {
                   controller = "Stock",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
               }).DataTokens["area"] = "Stock";


            routes.MapRoute("GetAvailibleQuantity", "GetAvailibleQuantity",
              new
              {
                  controller = "Stock",
                  action = "GetAvailibleQuantity",
                  namespaces = new[] { "IT.Web.Areas.Stock.Controllers" }
              }).DataTokens["area"] = "Stock";


            routes.MapRoute("GetProdctFromLPODetailsByLPOID/{Id}", "GetProdctFromLPODetailsByLPOID/{Id}",
              new
              {
                  controller = "Product",
                  action = "GetProdctFromLPODetailsByLPOID",
                  namespaces = new[] { "IT.Web.Areas.Product.Controllers" }
              }).DataTokens["area"] = "Product";
            
            #endregion

            #region Quotation

            routes.MapRoute("Quotation", "Quotation",
                new
                {
                    controller = "Quotation",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
                }).DataTokens["area"] = "Quotation";

            routes.MapRoute("Quotation-Create", "Quotation-Create",
               new
               {
                   controller = "Quotation",
                   action = "Create",
                   namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
               }).DataTokens["area"] = "Quotation";

            
            routes.MapRoute("Quotation-Details/{Id}", "Quotation-Details/{Id}",
               new
               {
                   controller = "Quotation",
                   action = "Details",
                   namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
               }).DataTokens["area"] = "Quotation";


            routes.MapRoute("QuotationAll", "QuotationAll",
              new
              {
                  controller = "Quotation",
                  action = "GetAll",
                  namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
              }).DataTokens["area"] = "Quotation";


            routes.MapRoute("Quotation-Edit/{Id}", "Quotation-Edit/{Id}",
              new
              {
                  controller = "Quotation",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
              }).DataTokens["area"] = "Quotation";


            routes.MapRoute("Quotation-Update", "Quotation-Update",
              new
              {
                  controller = "Quotation",
                  action = "Update",
                  namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
              }).DataTokens["area"] = "Quotation";


            routes.MapRoute("DeleteQuotDetailsRow", "DeleteQuotDetailsRow",
              new
              {
                  controller = "Quotation",
                  action = "DeleteQuotationDetailsRow",
                  namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
              }).DataTokens["area"] = "Quotation";


            routes.MapRoute("QuotationSaveDwnload", "QuotationSaveDwnload",
             new
             {
                 controller = "Quotation",
                 action = "SaveDwnload",
                 namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
             }).DataTokens["area"] = "Quotation";


            routes.MapRoute("PrintQuotation/{Id}", "PrintQuotation/{Id}",
             new
             {
                 controller = "Quotation",
                 action = "PrintQuotation",
                 namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
             }).DataTokens["area"] = "Quotation";


            routes.MapRoute("CheckISFileExistQuotation", "CheckISFileExistQuotation",
              new
              {
                  controller = "Quotation",
                  action = "CheckISFileExist",
                  namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
              }).DataTokens["area"] = "Quotation";


            routes.MapRoute("MakeInvoice", "MakeInvoice",
               new
               {
                   controller = "Quotation",
                   action = "MakeInvoice",
                   namespaces = new[] { "IT.Web.Areas.Quotation.Controllers" }
               }).DataTokens["area"] = "Quotation";

            #endregion
            
            #region Invoice
            routes.MapRoute("Invoice", "Invoice",
                new
                {
                    controller = "Invoice",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
                }).DataTokens["area"] = "Invoice";


            routes.MapRoute("InvoiceAll", "InvoiceAll",
             new
             {
                 controller = "Invoice",
                 action = "GetAll",
                 namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
             }).DataTokens["area"] = "Invoice";


            routes.MapRoute("Invoice-Create", "Invoice-Create",
            new
            {
                controller = "Invoice",
                action = "Create",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";


            routes.MapRoute("CheckISFileExistInvoice", "CheckISFileExistInvoice",
            new
            {
                controller = "Invoice",
                action = "CheckISFileExist",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";


            routes.MapRoute("Invoice-Edit/{Id}", "Invoice-Edit/{Id}",
            new
            {
                controller = "Invoice",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";


            routes.MapRoute("Invoice-Details/{Id}", "Invoice-Details/{Id}",
            new
            {
                controller = "Invoice",
                action = "Details",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";


           routes.MapRoute("SaveDwnloadInvoice", "SaveDwnloadInvoice",
           new
           {
               controller = "Invoice",
               action = "SaveDwnload",
               namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
           }).DataTokens["area"] = "Invoice";


            routes.MapRoute("PrintInvoice/{Id}", "PrintInvoice/{Id}",
               new
               {
                   controller = "Invoice",
                   action = "PrintInvoice",
                   namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
               }).DataTokens["area"] = "Invoice";


            routes.MapRoute("Invoice-Update", "Invoice-Update",
            new
            {
                controller = "Invoice",
                action = "Update",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";


            routes.MapRoute("CreateFromQuotation", "CreateFromQuotation",
            new
            {
                controller = "Invoice",
                action = "CreateFromQuotation",
                namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
            }).DataTokens["area"] = "Invoice";

            routes.MapRoute("DeleteInvoiceDetailsRow", "DeleteInvoiceDetailsRow",
           new
           {
               controller = "Invoice",
               action = "DeleteInvoiceDetailsRow",
               namespaces = new[] { "IT.Web.Areas.Invoice.Controllers" }
           }).DataTokens["area"] = "Invoice";

            
            #endregion

            #region Accounts

            routes.MapRoute("Accounts", "Accounts",
               new
               {
                   controller = "Accounts",
                   action = "Index",
                   namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
               }).DataTokens["area"] = "Accounts";

            routes.MapRoute("AccountAll", "AccountAll",
               new
               {
                   controller = "Accounts",
                   action = "GetAll",
                   namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
               }).DataTokens["area"] = "Accounts";

            routes.MapRoute("UnpadInvoice/{Id}", "UnpadInvoice/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "UnpadInvoice",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";

            routes.MapRoute("UnpadBill/{Id}", "UnpadBill/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "UnpadBill",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("AmountReceived", "AmountReceived",
                new
                {
                    controller = "Accounts",
                    action = "AmountReceived",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("AmountIssued", "AmountIssued",
                new
                {
                    controller = "Accounts",
                    action = "AmountIssued",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("AccountCustomerStatistics/{Id}", "AccountCustomerStatistics/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "AccountCustomerStatistics",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";

            routes.MapRoute("AccountVenderStatistics/{Id}", "AccountVenderStatistics/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "AccountVenderStatistics",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("DeleteTransiction/{Id}", "DeleteTransiction/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "DeleteTransiction",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("ChequeReceived/{Id}", "ChequeReceived/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "ChequeReceived",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";

            routes.MapRoute("PendingCheque", "PendingCheque",
                 new
                 {
                     controller = "Accounts",
                     action = "PendingCheque",
                     namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                 }).DataTokens["area"] = "Accounts";


            routes.MapRoute("ChequePendingAll", "ChequePendingAll",
                 new
                 {
                     controller = "Accounts",
                     action = "ChequePendingAll",
                     namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                 }).DataTokens["area"] = "Accounts";


            routes.MapRoute("PendingChequeDetails/{Id}", "PendingChequeDetails/{Id}",
                new
                {
                    controller = "Accounts",
                    action = "PendingChequeDetails",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("AccountPaymentReceiveFromCheque", "AccountPaymentReceiveFromCheque",
                new
                {
                    controller = "Accounts",
                    action = "AccountPaymentReceiveFromCheque",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("ReceivedCheques", "ReceivedCheques",
                 new
                 {
                     controller = "Accounts",
                     action = "ReceivedCheques",
                     namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                 }).DataTokens["area"] = "Accounts";


            routes.MapRoute("AccountChequeCashedAll", "AccountChequeCashedAll",
                new
                {
                    controller = "Accounts",
                    action = "AccountChequeCashedAll",
                    namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
                }).DataTokens["area"] = "Accounts";


            routes.MapRoute("ChequeOverDue", "ChequeOverDue",
               new
               {
                   controller = "Accounts",
                   action = "ChequeOverDue",
                   namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
               }).DataTokens["area"] = "Accounts";


            routes.MapRoute("ChequeOverDueList", "ChequeOverDueList",
               new
               {
                   controller = "Accounts",
                   action = "ChequeOverDueList",
                   namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
               }).DataTokens["area"] = "Accounts";


            routes.MapRoute("OverDueChequeDetails/{Id}", "OverDueChequeDetails/{Id}",
               new
               {
                   controller = "Accounts",
                   action = "OverDueChequeDetails",
                   namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
               }).DataTokens["area"] = "Accounts";

            routes.MapRoute("ChequeIssued", "ChequeIssued",
              new
              {
                  controller = "Accounts",
                  action = "ChequeIssued",
                  namespaces = new[] { "IT.Web.Areas.Accounts.Controllers" }
              }).DataTokens["area"] = "Accounts";

            #endregion

            #region Report

            routes.MapRoute("Report", "Report",
              new
              {
                  controller = "Report",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("PurchaseAllReport", "PurchaseAllReport",
             new
             {
                 controller = "Report",
                 action = "PurchaseAll",
                 namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
             }).DataTokens["area"] = "Report";


            routes.MapRoute("PurchaseFromDateToDate", "PurchaseFromDateToDate",
            new
            {
                controller = "Report",
                action = "PurchaseFromDateToDate",
                namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
            }).DataTokens["area"] = "Report";

            routes.MapRoute("PurchaseAllByDate", "PurchaseAllByDate",
                new
                {
                    controller = "Report",
                    action = "PurchaseAllByDate",
                    namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                }).DataTokens["area"] = "Report";

            routes.MapRoute("PurchaseByVenderANDDateRang", "PurchaseByVenderANDDateRang",
               new
               {
                   controller = "Report",
                   action = "PurchaseByVenderANDDateRang",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";


          routes.MapRoute("PurchaseByVenderReport", "PurchaseByVenderReport",
              new
              {
                  controller = "Report",
                  action = "PurchaseByVenderReport",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("BillAllReport", "BillAllReport",
              new
              {
                  controller = "Report",
                  action = "BillAll",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("BillFromDateToDate", "BillFromDateToDate",
              new
              {
                  controller = "Report",
                  action = "BillFromDateToDate",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("BillAllByDate", "BillAllByDate",
                 new
                 {
                     controller = "Report",
                     action = "BillAllByDate",
                     namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                 }).DataTokens["area"] = "Report";


            routes.MapRoute("BillAllByVender", "BillAllByVender",
                 new
                 {
                     controller = "Report",
                     action = "BillAllByVender",
                     namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                 }).DataTokens["area"] = "Report";


            routes.MapRoute("SaleReportByDateRang", "SaleReportByDateRang",
                 new
                 {
                     controller = "Report",
                     action = "SaleReportByDateRang",
                     namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                 }).DataTokens["area"] = "Report";


            routes.MapRoute("SaleAllReport", "SaleAllReport",
               new
               {
                   controller = "Report",
                   action = "SaleAllReport",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";


            routes.MapRoute("SaleAllReportByDate", "SaleAllReportByDate",
              new
              {
                  controller = "Report",
                  action = "SaleAllReportByDate",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("SaleAllReportByCustomer", "SaleAllReportByCustomer",
              new
              {
                  controller = "Report",
                  action = "SaleAllReportByCustomer",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";


            routes.MapRoute("SaleAllReportByCustomerAndDate", "SaleAllReportByCustomerAndDate",
              new
              {
                  controller = "Report",
                  action = "SaleAllReportByCustomerAndDate",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";



            routes.MapRoute("ExpenseAllReport", "ExpenseAllReport",
              new
              {
                  controller = "Report",
                  action = "ExpenseAllReport",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";



            routes.MapRoute("ExpenseAllReportDateRange", "ExpenseAllReportDateRange",
              new
              {
                  controller = "Report",
                  action = "ExpenseAllReportDateRange",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";



            routes.MapRoute("ExpenseAllReportByEmployee", "ExpenseAllReportByEmployee",
             new
             {
                 controller = "Report",
                 action = "ExpenseAllReportByEmployee",
                 namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
             }).DataTokens["area"] = "Report";


            routes.MapRoute("ExpenseAllReportByDate", "ExpenseAllReportByDate",
            new
            {
                controller = "Report",
                action = "ExpenseAllReportByDate",
                namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
            }).DataTokens["area"] = "Report";



            routes.MapRoute("LPOAllReport", "LPOAllReport",
            new
            {
                controller = "Report",
                action = "LPOAllReport",
                namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
            }).DataTokens["area"] = "Report";


            routes.MapRoute("LPOReportNotConverted", "LPOReportNotConverted",
                new
                {
                    controller = "Report",
                    action = "LPOReportNotConverted",
                    namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                }).DataTokens["area"] = "Report";


            routes.MapRoute("LPOAllNotConverted", "LPOAllNotConverted",
                new
                {
                    controller = "Report",
                    action = "LPOAllNotConverted",
                    namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                }).DataTokens["area"] = "Report";


            routes.MapRoute("LPOAllConverted", "LPOAllConverted",
            new
            {
                controller = "Report",
                action = "LPOAllConverted",
                namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
            }).DataTokens["area"] = "Report";


            routes.MapRoute("LPOAllByDate", "LPOAllConverted",
                new
                {
                    controller = "Report",
                    action = "LPOAllByDate",
                    namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
                }).DataTokens["area"] = "Report";


            routes.MapRoute("LPOFromDateToDate", "LPOFromDateToDate",
               new
               {
                   controller = "Report",
                   action = "LPOFromDateToDate",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";


            routes.MapRoute("UnpadBillByVender", "UnpadBillByVender",
               new
               {
                   controller = "Report",
                   action = "UnpadBillByVender",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";


            routes.MapRoute("UnpadInvoiceReport", "UnpadInvoiceReport",
               new
               {
                   controller = "Report",
                   action = "UnpadInvoiceReport",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";

            
            routes.MapRoute("UnpadInvoiceReportByDate", "UnpadInvoiceReportByDate",
               new
               {
                   controller = "Report",
                   action = "UnpadInvoiceReportByDate",
                   namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
               }).DataTokens["area"] = "Report";


            routes.MapRoute("UnpadInvoiceReportFromDateToDate", "UnpadInvoiceReportFromDateToDate",
              new
              {
                  controller = "Report",
                  action = "UnpadInvoiceReportFromDateToDate",
                  namespaces = new[] { "IT.Web.Areas.Report.Controllers" }
              }).DataTokens["area"] = "Report";
            
            //Customer Reports

            routes.MapRoute("CustomerReports", "CustomerReports",
                 new
                 {
                     controller = "CustomerReports",
                     action = "Index",
                     namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
                 }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("UnpadInvoiceReportCustomer", "UnpadInvoiceReportCustomer",
                 new
                 {
                     controller = "CustomerReports",
                     action = "UnpadInvoiceReportCustomer",
                     namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
                 }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("UnpadInvoiceReportCustomerByDate", "UnpadInvoiceReportCustomerByDate",
                  new
                  {
                      controller = "CustomerReports",
                      action = "UnpadInvoiceReportCustomerByDate",
                      namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
                  }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("UnpadInvoiceReportFromDateToDateCustomer", "UnpadInvoiceReportFromDateToDateCustomer",
                new
                {
                    controller = "CustomerReports",
                    action = "UnpadInvoiceReportFromDateToDateCustomer",
                    namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
                }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("OrderDeliverdReportByVehicle", "OrderDeliverdReportByVehicle",
               new
               {
                   controller = "CustomerReports",
                   action = "OrderDeliverdReportByVehicle",
                   namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
               }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("OrderDeliverdReportFromDateToDate", "OrderDeliverdReportFromDateToDate",
                 new
                 {
                     controller = "CustomerReports",
                     action = "OrderDeliverdReportFromDateToDate",
                     namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
                 }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("PaidInvoiceReportByCompanyId", "PaidInvoiceReportByCompanyId",
               new
               {
                   controller = "CustomerReports",
                   action = "PaidInvoiceReportByCompanyId",
                   namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
               }).DataTokens["area"] = "CustomerReports";



            routes.MapRoute("PartailPaidInvoiceReportByCompanyId", "PartailPaidInvoiceReportByCompanyId",
               new
               {
                   controller = "CustomerReports",
                   action = "PartailPaidInvoiceReportByCompanyId",
                   namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
               }).DataTokens["area"] = "CustomerReports";


            routes.MapRoute("OverDueInvoiceReportByCompanyId", "OverDueInvoiceReportByCompanyId",
              new
              {
                  controller = "CustomerReports",
                  action = "OverDueInvoiceReportByCompanyId",
                  namespaces = new[] { "IT.Web.Areas.CustomerReports.Controllers" }
              }).DataTokens["area"] = "CustomerReports";



            #endregion

            #region Designation

            routes.MapRoute("Designation", "Designation",
             new
             {
                 controller = "Designation",
                 action = "Index",
                 namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
             }).DataTokens["area"] = "Designation";


            routes.MapRoute("DesignationAll", "DesignationAll",
            new
            {
                controller = "Designation",
                action = "GetAll",
                namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
            }).DataTokens["area"] = "Designation";


            routes.MapRoute("Designation-Create", "Designation-Create",
               new
               {
                   controller = "Designation",
                   action = "Create",
                   namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
               }).DataTokens["area"] = "Designation";


            routes.MapRoute("Designation-Edit/{id}", "Designation-Edit/{id}",
               new
               {
                   controller = "Designation",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
               }).DataTokens["area"] = "Designation";

            routes.MapRoute("DesignationUpdate", "DesignationUpdate",
               new
               {
                   controller = "Designation",
                   action = "Update",
                   namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
               }).DataTokens["area"] = "Designation";


            routes.MapRoute("DesignationDelete", "DesignationDelete",
               new
               {
                   controller = "Designation",
                   action = "Delete",
                   namespaces = new[] { "IT.Web.Areas.Designation.Controllers" }
               }).DataTokens["area"] = "Designation";

            #endregion

            #region Employee Customer

            routes.MapRoute("Employee", "Employee",
             new
             {
                 controller = "Employee",
                 action = "Index",
                 namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
             }).DataTokens["area"] = "Employee";


            routes.MapRoute("EmployeeAll", "EmployeeAll",
            new
            {
                controller = "Employee",
                action = "GetAll",
                namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
            }).DataTokens["area"] = "Employee";


            routes.MapRoute("Employee-Create", "Employee-Create",
             new
             {
                 controller = "Employee",
                 action = "Create",
                 namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
             }).DataTokens["area"] = "Employee";


            routes.MapRoute("Employee-Edit/{id}", "Employee-Edit/{id}",
            new
            {
                controller = "Employee",
                action = "Edit",
                namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
            }).DataTokens["area"] = "Employee";


            routes.MapRoute("Employee-Update", "Employee-Update",
           new
           {
               controller = "Employee",
               action = "Edit",
               namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
           }).DataTokens["area"] = "Employee";


            routes.MapRoute("Employee-Details/{id}", "Employee-Details/{id}",
            new
            {
                controller = "Employee",
                action = "Details",
                namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
            }).DataTokens["area"] = "Employee";


            routes.MapRoute("Employee-Delete", "Employee-Delete",
            new
            {
                controller = "Employee",
                action = "Delete",
                namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
            }).DataTokens["area"] = "Employee";


            routes.MapRoute("LoadEmployeeAll/{id}", "LoadEmployeeAll/{id}",
            new
            {
                controller = "Employee",
                action = "LoadEmployeeAll",
                namespaces = new[] { "IT.Web.Areas.Employee.Controllers" }
            }).DataTokens["area"] = "Employee";


            #endregion
            
            #region Customer Order AND Accepted etc

            routes.MapRoute("Order", "Order",
               new
               {
                   controller = "CustomerOrder",
                   action = "Index",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerDeliverdOrderUpdate/{Id}", "CustomerDeliverdOrderUpdate/{Id}",
               new
               {
                   controller = "CustomerOrder",
                   action = "CustomerDeliverdOrderUpdate",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";
            

            routes.MapRoute("CustomerOrderAll", "CustomerOrderAll",
               new
               {
                   controller = "CustomerOrder",
                   action = "GetAll",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("Order-Create", "Order-Create",
               new
               {
                   controller = "CustomerOrder",
                   action = "Create",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";



            //Managing customer order

            routes.MapRoute("AcceptedOrderAdmin", "AcceptedOrderAdmin",
                new
                {
                    controller = "CustomerOrder",
                    action = "AcceptedOrderAdmin",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";

            //List of accepted order
            routes.MapRoute("AcceptedOrdersAdmin", "AcceptedOrdersAdmin", 
                new
                {
                    controller = "CustomerOrder",
                    action = "AcceptedOrdersAdmin",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("DeliverdOrderAdmin", "DeliverdOrderAdmin",
                new
                {
                    controller = "CustomerOrder",
                    action = "DeliverdOrderAdmin",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("DeliverdOrdersAdmin", "DeliverdOrdersAdmin",
               new
               {
                   controller = "CustomerOrder",
                   action = "DeliverdOrdersAdmin",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("NewOrder", "NewOrder",
               new
               {
                   controller = "CustomerOrder",
                   action = "NewOrder",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerGroupOrder", "CustomerGroupOrder",
               new
               {
                   controller = "CustomerOrder",
                   action = "CustomerGroupOrder",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerOrderDetails/{Id}", "CustomerOrderDetails/{Id}",
              new
              {
                  controller = "CustomerOrder",
                  action = "CustomerGroupOrderDetails",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("OrderGroupEdit/{Id}", "OrderGroupEdit/{Id}",
              new
              {
                  controller = "CustomerOrder",
                  action = "CustomerGroupOrderEdit",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerOrderRejectByAdmin/{Id}", "CustomerOrderRejectByAdmin/{Id}",
             new
             {
                 controller = "CustomerOrder",
                 action = "CustomerOrderRejectByAdmin",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderRejectedAllAdmin", "CustomerOrderRejectedAllAdmin",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderRejectedAllAdmin",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("RejectDescriptionAdd", "RejectDescriptionAdd",
            new
            {
                controller = "CustomerOrder",
                action = "RejectDescriptionAdd",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";
            

            routes.MapRoute("AllRejectedOrderGroupAdmin", "AllRejectedOrderGroupAdmin",
            new
            {
                controller = "CustomerOrder",
                action = "AllRejectedOrderGroupAdmin",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";
            
            //top 5 order desc 

            routes.MapRoute("CustomerOrderNote", "CustomerOrderNote",
              new
              {
                  controller = "CustomerOrder",
                  action = "CustomerOrderNote",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";

            //managing admin orders

            routes.MapRoute("ManageOrders", "ManageOrders",
             new
             {
                 controller = "CustomerOrder",
                 action = "ManageOrders",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            //GetUnreadOrderAll for admin
            routes.MapRoute("GetUnreadOrderAll", "GetUnreadOrderAll",
            new
            {
                controller = "CustomerOrder",
                action = "GetUnreadOrderAll",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";


            //OrderViewed make view customer order

            routes.MapRoute("OrderViewed", "OrderViewed",
               new
               {
                   controller = "CustomerOrder",
                   action = "OrderViewed",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("OrderById", "OrderById",
               new
               {
                   controller = "CustomerOrder",
                   action = "OrderById",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";

            //Customer accepted order

            routes.MapRoute("OrderAccepted", "OrderAccepted",
               new
               {
                   controller = "CustomerOrder",
                   action = "OrderAccepted",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("GetAsignedOrder", "GetAsignedOrder",
              new
              {
                  controller = "CustomerOrder",
                  action = "GetAsignedOrder",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";


            //AWFuel accepted order

            routes.MapRoute("OrderAcceptedAwfuel", "OrderAcceptedAwfuel",
             new
             {
                 controller = "CustomerOrder",
                 action = "OrderAcceptedAwfuel",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("GetAsignedOrderAWFuel", "GetAsignedOrderAWFuel",
              new
              {
                  controller = "CustomerOrder",
                  action = "GetAsignedOrderAWFuel",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";


            //AWFuel Order Deliverd To Customers
            routes.MapRoute("OrderDeliverd", "OrderDeliverd",
                 new
                 {
                     controller = "CustomerOrder",
                     action = "OrderDeliverd",
                     namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                 }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("GetDeliverdOrderAWFuel", "GetDeliverdOrderAWFuel",
                 new
                 {
                     controller = "CustomerOrder",
                     action = "GetDeliverdOrderAWFuel",
                     namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                 }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("OrderDetails", "OrderDetails",
                new
                {
                    controller = "CustomerOrder",
                    action = "OrderDetails",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("ViewDeliveryInfo", "ViewDeliveryInfo",
               new
               {
                   controller = "CustomerOrder",
                   action = "ViewDeliveryInfo",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("OrderReceived", "OrderReceived",
              new
              {
                  controller = "CustomerOrder",
                  action = "OrderReceived",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderReceived", "CustomerOrderReceived",
             new
             {
                 controller = "CustomerOrder",
                 action = "CustomerOrderReceived",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerReceivedOrderDetails", "CustomerReceivedOrderDetails",
             new
             {
                 controller = "CustomerOrder",
                 action = "CustomerReceivedOrderDetails",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("ViewedNotifyCustomer", "ViewedNotifyCustomer",
            new
            {
                controller = "CustomerOrder",
                action = "ViewedNotifyCustomer",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";


            //Driver Area            

            routes.MapRoute("DriverAsignedOrder", "DriverAsignedOrder",
               new
               {
                   controller = "CustomerOrder",
                   action = "DriverAsignedOrder",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("GetAsignedOrderByDriver", "GetAsignedOrderByDriver",
              new
              {
                  controller = "CustomerOrder",
                  action = "GetAsignedOrderByDriver",
                  namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
              }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("DriverDeliverdOrder", "DriverDeliverdOrder",
             new
             {
                 controller = "CustomerOrder",
                 action = "DriverDeliverdOrder",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("GetDeliverOrderByDriver", "GetDeliverOrderByDriver",
             new
             {
                 controller = "CustomerOrder",
                 action = "GetDeliverOrderByDriver",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderDeliverdUpdate", "CustomerOrderDeliverdUpdate",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderDeliverdUpdate",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";
            

            routes.MapRoute("CusOrderDelUpdateCusConfirmed/{Id}", "CusOrderDelUpdateCusConfirmed/{Id}",
             new
             {
                 controller = "CustomerOrder",
                 action = "CusOrderDelUpdateCusConfirmed",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerGroupOrderUpdate", "CustomerGroupOrderUpdate",
             new
             {
                 controller = "CustomerOrder",
                 action = "CustomerGroupOrderUpdate",
                 namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
             }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderViewByAdmin/{Id}", "CustomerOrderViewByAdmin/{Id}",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderViewByAdmin",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";

            //for data
            routes.MapRoute("GetAllUnreadOrderGroup", "GetAllUnreadOrderGroup",
            new
            {
                controller = "CustomerOrder",
                action = "GetAllUnreadOrderGroup",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";

            //for View
            routes.MapRoute("AllUnreadOrderGroup", "AllUnreadOrderGroup",
            new
            {
                controller = "CustomerOrder",
                action = "AllUnreadOrderGroup",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";

            //Asign groupOrder to Driver
            routes.MapRoute("GroupOrderAsignedToDriver", "GroupOrderAsignedToDriver",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderGroupAsignedDriverAdd",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";


            //Get Asigned Group order by Driver
            routes.MapRoute("DriverViewGroupAsignedOrder/{Id}", "DriverViewGroupAsignedOrder/{Id}",
               new
               {
                   controller = "CustomerOrder",
                   action = "DriverViewGroupAsignedOrder",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
               }).DataTokens["area"] = "CustomerOrder";

            
            routes.MapRoute("CustomerOrderAccepted", "CustomerOrderAccepted",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderAccepted",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("AsignedOrderDriver", "AsignedOrderDriver",
            new
            {
                controller = "CustomerOrder",
                action = "AsignedOrderDriver",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("AcceptedOrderDriver", "AcceptedOrderDriver",
            new
            {
                controller = "CustomerOrder",
                action = "AcceptedOrderDriver",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("DeliverdOrderDriver", "DeliverdOrderDriver",
            new
            {
                controller = "CustomerOrder",
                action = "DeliverdOrderDriver",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
            }).DataTokens["area"] = "CustomerOrder";
            

            //Driver Asigned Orders
            routes.MapRoute("CustomerOrderGroupAsignedDriver", "CustomerOrderGroupAsignedDriver",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderGroupAsignedDriverByDriverId",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";

            //Driver Accepted Order
            routes.MapRoute("CustomerOrderGroupAcceptDriver", "CustomerOrderGroupAcceptDriver",
               new
               {
                   controller = "CustomerOrder",
                   action = "CustomerOrderGroupAcceptedDriverByDriverId",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

               }).DataTokens["area"] = "CustomerOrder";
            
            routes.MapRoute("CustomerOrderDetailsGroupUpDelQTY", "CustomerOrderDetailsGroupUpDelQTY",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderDetailsGroupUpDelQTY",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderGroupAllAsigned", "CustomerOrderGroupAllAsigned",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderGroupAllAsigned",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("GetAllAsignedOrderGroup", "GetAllAsignedOrderGroup",
            new
            {
                controller = "CustomerOrder",
                action = "GetAllAsignedOrderGroup",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerOrderAsignedViewByAdmin/{Id}", "CustomerOrderAsignedViewByAdmin/{Id}",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderAsignedViewByAdmin",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";


            #region Customer Order Group Customer Area
            //Customer Order Group Requested Order
            //View
            routes.MapRoute("MyRequestedOrder", "MyRequestedOrder",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerRequestedOrderGroup",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";

            //List
            routes.MapRoute("CustomerOrderAllByCompanyId", "CustomerOrderAllByCompanyId",
            new
            {
                controller = "CustomerOrder",
                action = "CustomerOrderAllByCompanyId",
                namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

            }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerOrderSend/{Id}", "CustomerOrderSend/{Id}",
               new
               {
                   controller = "CustomerOrder",
                   action = "CustomerOrderSend",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

               }).DataTokens["area"] = "CustomerOrder";
            
            //Customer Order Details
            routes.MapRoute("CustomerOrderGroupByOrderId/{Id}", "CustomerOrderGroupByOrderId/{Id}",
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerOrderGroupByOrderId",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

                }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("MyAcceptedOrder", "MyAcceptedOrder",
                new
                {
                    controller = "CustomerOrder",
                    action = "MyAcceptedOrder",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

                }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerOrderAcceptedByCompanyId", "CustomerOrderAcceptedByCompanyId", 
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerOrderAcceptedByCompanyId",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";

            

            routes.MapRoute("CustomerOrderGroupDeliverdDriverByDriverId", "CustomerOrderGroupDeliverdDriverByDriverId",
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerOrderGroupDeliverdDriverByDriverId",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("MyReceivedOrder", "MyReceivedOrder",
                new
                {
                    controller = "CustomerOrder",
                    action = "MyReceivedOrder",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }
                }).DataTokens["area"] = "CustomerOrder";
            

            routes.MapRoute("CustomerOrderReceivedByCompanyId", "CustomerOrderReceivedByCompanyId", 
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerOrderReceivedByCompanyId",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

                }).DataTokens["area"] = "CustomerOrder";


            routes.MapRoute("CustomerSendedOrders", "CustomerSendedOrders",
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerSendedOrders",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

                }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderAllSendedByCompanyId", "CustomerOrderAllSendedByCompanyId",
                new
                {
                    controller = "CustomerOrder",
                    action = "CustomerOrderAllSendedByCompanyId",
                    namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

                }).DataTokens["area"] = "CustomerOrder";

            routes.MapRoute("CustomerOrderGroupDetailsDriverView/{Id}", "CustomerOrderGroupDetailsDriverView/{Id}",
               new
               {
                   controller = "CustomerOrder",
                   action = "CustomerOrderGroupDetailsDriverView",
                   namespaces = new[] { "IT.Web.Areas.CustomerOrder.Controllers" }

               }).DataTokens["area"] = "CustomerOrder";

            

            #endregion


            #endregion

            #region Vehicle Customer

            routes.MapRoute("Vehicle", "Vehicle",
                new
                {
                    controller = "Vehicle",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
                }).DataTokens["area"] = "Vehicle";


            routes.MapRoute("VehicleAll", "VehicleAll",
               new
               {
                   controller = "Vehicle",
                   action = "GetAll",
                   namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
               }).DataTokens["area"] = "Vehicle";


            routes.MapRoute("Vehicle-Delete", "Vehicle-Delete",
              new
              {
                  controller = "Vehicle",
                  action = "Delete",
                  namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
              }).DataTokens["area"] = "Vehicle";

            routes.MapRoute("Vehicle-Create", "Vehicle-Create",
                 new
                 {
                     controller = "Vehicle",
                     action = "Create",
                     namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
                 }).DataTokens["area"] = "Vehicle";

            routes.MapRoute("Vehicle-Details/{id}", "Vehicle-Details/{id}",
                new
                {
                    controller = "Vehicle",
                    action = "Details",
                    namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
                }).DataTokens["area"] = "Vehicle";


            routes.MapRoute("Vehicle-Edit/{id}", "Vehicle-Edit/{id}",
               new
               {
                   controller = "Vehicle",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
               }).DataTokens["area"] = "Vehicle";


            routes.MapRoute("VehicleUpdate", "VehicleUpdate",
               new
               {
                   controller = "Vehicle",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Vehicle.Controllers" }
               }).DataTokens["area"] = "Vehicle";

            #endregion

            #region Driver Customer

            routes.MapRoute("Driver", "Driver",
                new
                {
                    controller = "Driver",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                }).DataTokens["area"] = "Driver";


            routes.MapRoute("Driver-Create", "Driver-Create",
                new
                {
                    controller = "Driver",
                    action = "Create",
                    namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                }).DataTokens["area"] = "Driver";


            routes.MapRoute("DriverAll", "DriverAll",
                 new
                 {
                     controller = "Driver",
                     action = "GetAll",
                     namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                 }).DataTokens["area"] = "Driver";

            routes.MapRoute("Driver-EditCustomer/{id}", "Driver-EditCustomer/{id}",
                 new
                 {
                     controller = "Driver",
                     action = "Edit",
                     namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                 }).DataTokens["area"] = "Driver";


            routes.MapRoute("DriverUpdateCustomer", "DriverUpdateCustomer",
                 new
                 {
                     controller = "Driver",
                     action = "Edit",
                     namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                 }).DataTokens["area"] = "Driver";
            

            routes.MapRoute("Driver-Details/{id}", "Driver-Details/{id}",
                new
                {
                    controller = "Driver",
                    action = "Details",
                    namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                }).DataTokens["area"] = "Driver";
            

            routes.MapRoute("Driver-Delete", "Driver-Delete",
                new
                {
                    controller = "Driver",
                    action = "Delete",
                    namespaces = new[] { "IT.Web.Areas.Driver.Controllers" }
                }).DataTokens["area"] = "Driver";
            
            #endregion

            #region Customer Expenses

            routes.MapRoute("CustomerExpenses", "CustomerExpenses",
                new
                {
                    controller = "CustomerExpenses",
                    action = "Index",
                    namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
                }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("CustomerExpensesCreate", "CustomerExpensesCreate",
                new
                {
                    controller = "CustomerExpenses",
                    action = "Create",
                    namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
                }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("LoadCustomerVehicle", "LoadCustomerVehicle",
               new
               {
                   controller = "CustomerExpenses",
                   action = "LoadCustomerVehicle",
                   namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
               }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("LoadGeneralExpenseCustomer", "LoadGeneralExpenseCustomer",
              new
              {
                  controller = "CustomerExpenses",
                  action = "LoadGeneralExpenseCustomer",
                  namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
              }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("ExpenseCustomer-Details/{Id}", "ExpenseCustomer-Details/{Id}",
              new
              {
                  controller = "CustomerExpenses",
                  action = "Details",
                  namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
              }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("CustomerExpenseEdit/{id}", "CustomerExpenseEdit/{id}",
              new
              {
                  controller = "CustomerExpenses",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
              }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("CustomerExpenseUpdate", "CustomerExpenseUpdate",
             new
             {
                 controller = "CustomerExpenses",
                 action = "Update",
                 namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
             }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("PrintExpenseCustomer/{id}", "PrintExpenseCustomer/{id}",
             new
             {
                 controller = "CustomerExpenses",
                 action = "PrintExpense",
                 namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
             }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("CheckIExpenseExistCustomerExpense/{id}", "CheckIExpenseExistCustomerExpense/{id}",
             new
             {
                 controller = "CustomerExpenses",
                 action = "CheckIExpenseExist",
                 namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
             }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("CustomerExpenseAll", "CustomerExpenseAll",
            new
            {
                controller = "CustomerExpenses",
                action = "GetAll",
                namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
            }).DataTokens["area"] = "CustomerExpenses";


            routes.MapRoute("DeleteCustomerExpDetailsRow", "DeleteCustomerExpDetailsRow",
            new
            {
                controller = "CustomerExpenses",
                action = "DeleteExpDetailsRow",
                namespaces = new[] { "IT.Web.Areas.CustomerExpenses.Controllers" }
            }).DataTokens["area"] = "CustomerExpenses";


            #endregion
            
            #region Email

            routes.MapRoute("Email", "Email",
               new
               {
                   controller = "Email",
                   action = "Index",
                   namespaces = new[] { "IT.Web.Areas.Email.Controllers" }
               }).DataTokens["area"] = "Email";


            routes.MapRoute("Sent", "Sent",
                new
                {
                    controller = "Email",
                    action = "Sent",
                    namespaces = new[] { "IT.Web.Areas.Email.Controllers" }
                }).DataTokens["area"] = "Email";


            routes.MapRoute("EmailQuotation", "EmailQuotation",
                new
                {
                    controller = "Email",
                    action = "EmailQuotation",
                    namespaces = new[] { "IT.Web.Areas.Email.Controllers" }
                }).DataTokens["area"] = "Email";

            #endregion

            #region Project

            routes.MapRoute("Project", "Project",
               new
               {
                   controller = "Project",
                   action = "Index",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";


            routes.MapRoute("ProjectAll", "ProjectAll",
               new
               {
                   controller = "Project",
                   action = "GetAll",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";


            routes.MapRoute("Project-Create", "Project-Create",
               new
               {
                   controller = "Project",
                   action = "Create",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";


            routes.MapRoute("Project-Edit/{Id}", "Project-Edit/{Id}",
               new
               {
                   controller = "Project",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";


            routes.MapRoute("Project-Update", "Project-Update",
               new
               {
                   controller = "Project",
                   action = "Edit",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";


            routes.MapRoute("Project-Details/{Id}", "Project-Details/{Id}",
               new
               {
                   controller = "Project",
                   action = "Details",
                   namespaces = new[] { "IT.Web.Areas.Project.Controllers" }
               }).DataTokens["area"] = "Project";

            #endregion

            #region AWFEmployeeSalary

            routes.MapRoute("EmployeeSalary", "EmployeeSalary",
              new
              {
                  controller = "AWFEmployeeSalary",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
              }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("EmployeeByProjectId/{Id}", "EmployeeByProjectId/{Id}",
              new
              {
                  controller = "AWFEmployeeSalary",
                  action = "AllEmployeeByProjectId",
                  namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
              }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("GeneratSalary", "GeneratSalary",
              new
              {
                  controller = "AWFEmployeeSalary",
                  action = "GeneratSalary",
                  namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
              }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("IsSalaryGenerated", "IsSalaryGenerated",
             new
             {
                 controller = "AWFEmployeeSalary",
                 action = "IsSalaryGenerated",
                 namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
             }).DataTokens["area"] = "AWFEmployeeSalary";

            routes.MapRoute("EmployeeLoadIssued", "EmployeeLoadIssued",
            new
            {
                controller = "AWFEmployeeSalary",
                action = "EmployeeLoadIssued",
                namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
            }).DataTokens["area"] = "AWFEmployeeSalary";

            routes.MapRoute("SaveEmployeeLoan", "SaveEmployeeLoan",
               new
               {
                   controller = "AWFEmployeeSalary",
                   action = "SaveEmployeeLoan",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
               }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("EmployeeStatistics/{Id}", "EmployeeStatistics/{Id}",
               new
               {
                   controller = "AWFEmployeeSalary",
                   action = "EmployeeStatistics",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
               }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("EmployeeLoanReturn", "EmployeeLoanReturn",
                new
                {
                    controller = "AWFEmployeeSalary",
                    action = "EmployeeLoanReturn",
                    namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
                }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("EmployeeDediction", "EmployeeDediction",
               new
               {
                   controller = "AWFEmployeeSalary",
                   action = "EmployeeDediction",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
               }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("SaveDeduction", "SaveDeduction",
               new
               {
                   controller = "AWFEmployeeSalary",
                   action = "SaveDeduction",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
               }).DataTokens["area"] = "AWFEmployeeSalary";
            

            routes.MapRoute("EmployeeAllowansis", "EmployeeAllowansis",
                new {
                    controller = "AWFEmployeeSalary",
                    action = "EmployeeAllowansis",
                    namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
                }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("EmployeeAllowanceSaved", "EmployeeAllowanceSaved",
                new
                {
                    controller = "AWFEmployeeSalary",
                    action = "EmployeeAllowanceSaved",
                    namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
                }).DataTokens["area"] = "AWFEmployeeSalary";


            routes.MapRoute("IssueEmployeeSalary", "IssueEmployeeSalary",
                new
                {
                    controller = "AWFEmployeeSalary",
                    action = "IssueEmployeeSalary",
                    namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
                }).DataTokens["area"] = "AWFEmployeeSalary";

            routes.MapRoute("AllowanceTypeAdd", "AllowanceTypeAdd",
               new
               {
                   controller = "AWFEmployeeSalary",
                   action = "AllowanceTypeAdd",
                   namespaces = new[] { "IT.Web.Areas.AWFEmployeeSalary.Controllers" }
               }).DataTokens["area"] = "AWFEmployeeSalary";
            
            #endregion

            #region Company Customer

            routes.MapRoute("Company-Create", "Company-Create",
              new
              {
                  controller = "Company",
                  action = "Create",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";


            routes.MapRoute("Company", "Company",
              new
              {
                  controller = "Company",
                  action = "Index",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";


            routes.MapRoute("Company-Edit/{id}", "Company-Edit/{id}",
              new
              {
                  controller = "Company",
                  action = "Edit",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";


            routes.MapRoute("CopnayInfoById/{id}", "CopnayInfoById/{id}",
              new
              {
                  controller = "Company",
                  action = "CopnayInfoById",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";


            routes.MapRoute("Company-Details/{id}", "Company-Details/{id}",
              new
              {
                  controller = "Company",
                  action = "Details",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";


            routes.MapRoute("CustomerProfile/{id}", "CustomerProfile/{id}",
              new
              {
                  controller = "Company",
                  action = "CustomerProfile",
                  namespaces = new[] { "IT.Web.Areas.Company.Controllers" }
              }).DataTokens["area"] = "Company";
            
            #endregion

            #region Home           

            routes.MapRoute("Driver-Home", "Driver-Home",
              new
              {
                  controller = "Home",
                  action = "DriverDashBoard",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";


            routes.MapRoute("Test", "Test",
              new
              {
                  controller = "Home",
                  action = "Test",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";


            routes.MapRoute("GetMessageses", "GetMessageses",
              new
              {
                  controller = "Home",
                  action = "GetMessageses",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";


            routes.MapRoute("GetMessagesesDriver/{Id}", "GetMessagesesDriver/{Id}",
              new
              {
                  controller = "Home",
                  action = "GetMessagesesDriver",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";

            routes.MapRoute("MsgFromDriverOnAccept/{Id}", "MsgFromDriverOnAccept/{Id}",
              new
              {
                  controller = "Home",
                  action = "MsgFromDriverOnAccept",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";

            routes.MapRoute("MessagesesDriverAdmin/{Id}", "MessagesesDriverAdmin/{Id}",
              new
              {
                  controller = "Home",
                  action = "CustomerDeliveyInfo",
                  namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
              }).DataTokens["area"] = "Home";

            routes.MapRoute("MessageToAdmonOnDelivery", "MessageToAdmonOnDelivery",
             new
             {
                 controller = "Home",
                 action = "MessageToAdmonOnDelivery",
                 namespaces = new[] { "IT.Web.Areas.Home.Controllers" }
             }).DataTokens["area"] = "Home";
            
            #endregion

            #region AWFuel Driver Login History

            //check is Driver asign vehicle

            routes.MapRoute("IsDriverTakinVehicle", "IsDriverTakinVehicle",
               new
                {
                    controller = "AWFDriver",
                    action = "IsDriverTakinVehicle",
                    namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
                }).DataTokens["area"] = "AWFDriver";

            //asign vehicle to driver
            routes.MapRoute("DriverLoginHistory", "DriverLoginHistory",
              new
              {
                  controller = "AWFDriver",
                  action = "DriverLoginHistory",
                  namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
              }).DataTokens["area"] = "AWFDriver";

            //load driver online
            routes.MapRoute("DriverAllOnline", "DriverAllOnline",
             new
             {
                 controller = "AWFDriver",
                 action = "DriverAllOnline",
                 namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
             }).DataTokens["area"] = "AWFDriver";

            #endregion
            
            #region Login Logout

            //routes.MapRoute("LogOut", "LogOut",
            //  new
            //  {
            //      controller = "Home",
            //      action = "Login",
            //      namespaces = new[] { "IT.Web.Areas.Login.Controllers" }
            //  }).DataTokens["area"] = "Login";


            routes.MapRoute("LogOutDriver", "LogOutDriver",
             new
             {
                 controller = "Login",
                 action = "LogOutDriver",
                 namespaces = new[] { "IT.Web.Areas.Login.Controllers" }
             }).DataTokens["area"] = "Login";


            #endregion
            
            #region Asign Customer Order to Driver
            routes.MapRoute("CustomerOrderAsignToDriver", "CustomerOrderAsignToDriver",
             new
             {
                 controller = "AWFDriver",
                 action = "CustomerOrderAsignToDriver",
                 namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
             }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("DriverViewOrder", "DriverViewOrder",
             new
             {
                 controller = "AWFDriver",
                 action = "DriverViewOrder",
                 namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
             }).DataTokens["area"] = "AWFDriver";


            routes.MapRoute("CustomerOrderAcceptDriver", "CustomerOrderAcceptDriver",
             new
             {
                 controller = "AWFDriver",
                 action = "CustomerOrderAcceptDriver",
                 namespaces = new[] { "IT.Web.Areas.AWFDriver.Controllers" }
             }).DataTokens["area"] = "AWFDriver";


            #endregion

        }
    }
}
