using IT.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace IT.Web.Repository
{
    public class CustomerNotificationRepository
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
                using (SqlCommand cmd = new SqlCommand(@"Select Id from [dbo].CustomerOrderGroup where OrderProgress = 'Order Accepted' AND IsCustomerViewNotification = 0 AND  CustomerId =" + Id, con))
                {

                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    Initialization();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    Res = ds.Tables[0].Rows.Count;
                }
                return Res;
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e) //this will be called when any changes occur in db table. 
        {
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Update)
            {
                MyHub.SendAcceptMessagesCustomer();
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