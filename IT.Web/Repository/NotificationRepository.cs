using IT.Web.Hubs;
using IT.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace IT.Web.Repository
{
    public class NotificationRepository
    {

        public void Initialization()
        {
            // Create a dependency connection.
            SqlDependency.Start(GetConnectionString());
            CanRequestNotifications();
        }

        DataSet ds = new DataSet();
        SqlConnection co = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefualConnection"].ConnectionString);
        public int GetAllMessages()
        {
            int Res;
            var messages = new List<CustomerOrder>();
            using (var cmd = new SqlCommand(@"select OrderId,OrderProgress from [dbo].CustomerOrder where IsRead = 0", co))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                SqlDependency dependency = new SqlDependency(cmd);
                dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                Initialization();

                da.Fill(ds);
                Res = ds.Tables[0].Rows.Count;
            }
            return Res;
        }
              

        private void OnDependencyChange(object sender,
         SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
            {
                MyHub.SendMessages();
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