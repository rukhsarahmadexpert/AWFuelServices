using IT.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace IT.Web.Repository
{
    public class DeliveryNotificationRepository
    {
       
        public void Initialization()
        {
            // Create a dependency connection.
            SqlDependency.Start(GetConnectionString());
            CanRequestNotifications();
        }

        public int GetAllMessages(int Id)
        {
            DataSet ds = new DataSet();
            int Res;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();

                using (SqlCommand command =
               new SqlCommand("select OrderId from [dbo].CustomerOrderDeliver where IsCustomerConfirmed =0 AND CustomerCompanyId =" + Id, con))
                {
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                    Initialization();

                    var ReturnValue = command.ExecuteScalar();

                    if (ReturnValue != null)
                    {
                        Res = Convert.ToInt32(ReturnValue);
                    }
                    else
                    {
                        Res = 0;
                    }
                                       
                }
                return Res;
            }
        }


        public int AdminInfoOnDelivery()
        {
            DataSet ds = new DataSet();
            int Res;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();

                using (SqlCommand command =
               new SqlCommand("select Id from CustomerOrderDeliver where IsActive = 1", con))
                {                    
                    SqlDataAdapter da = new SqlDataAdapter(command);

                    var str = command.ExecuteScalar();

                    if (str != null)
                    {
                        Res = Convert.ToInt32(str);
                    }
                    else
                    {
                        Res = 0;
                    }
                }
                return Res;
            }
        }

        // Handler method
        public void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
            {
                MyHub.SendDeliverMassageCustomer();
            } 
            else if(e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
            {
                AdminHub.SendMessageOnDelivery();
            }
        }


        public void Termination()
        {
            // Release the dependency.
            SqlDependency.Stop(GetConnectionString());
        }

        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DefualConnection"].ConnectionString;
        }

        private bool CanRequestNotifications()
        {
            SqlClientPermission permission =
            new SqlClientPermission(
            PermissionState.Unrestricted);
            try
            {
                permission.Demand();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}