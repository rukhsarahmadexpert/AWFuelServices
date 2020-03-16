using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IT.Web.Hubs
{
    public class AdminHub : Hub
    {
        [HubMethodName("sendNewOrder")]
        public static void SendNewOrder()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AdminHub>();
            context.Clients.All.updateMessages();
        }


        [HubMethodName("sendMessageOnDelivery")]
        public static void SendMessageOnDelivery()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AdminHub>();
            context.Clients.All.updateDeliveryMessages();
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }



    }
}