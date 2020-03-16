using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace IT.Web.Hubs
{
    public class MyHub : Hub
    {
              
        [HubMethodName("sendMessages")]
        public static void SendMessages()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.updateMessages();           
        }


        [HubMethodName("sendAcceptMessagesCustomer")]
        public static void SendAcceptMessagesCustomer()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.sendNotifyMasseges();

            // context.Clients.User(userId).sendMessage(Message);
        }

        [HubMethodName("sendDeliverMassageCustomer")]
        public static void SendDeliverMassageCustomer()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.sendDeliveryNotifyMasseges();

            // context.Clients.User(userId).sendMessage(Message);
        }
    }
}