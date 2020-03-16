using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Hubs
{
    public class DriverHub : Hub
    {
        [HubMethodName("getAsignedOrderNotification")]
        public static void GetAsignedOrderNotification()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<DriverHub>();
            context.Clients.All.SendNotification();
        }
    }
}