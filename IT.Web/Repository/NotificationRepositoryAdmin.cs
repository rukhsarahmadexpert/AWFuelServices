using IT.Web.Hubs;
using IT.Web.Models;
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
    public class NotificationRepositoryAdmin
    {
        public void Initialization()
        {
            // Create a dependency connection.
            SqlDependency.Start(GetConnectionString());
            CanRequestNotifications();
        }

        public int SomeMethod()
        {
            DataSet ds = new DataSet();
            int Res;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();

                using (SqlCommand command =
               new SqlCommand("select Id,OrderProgress from [dbo].CustomerOrderGroup where IsRead = 0 AND IsSend=1", con))
                {
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                    Initialization();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    Res = ds.Tables[0].Rows.Count;
                }
                return Res;
            }
        }
        
        // Handler method
        public void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Update)
            {
                AdminHub.SendNewOrder();
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