using SendingPushNotifications.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationIOS1
{
    class Program
    {
        static void Main(string[] args)
        {
            string Title = "Order Created";
            string NotificationCode = "ADM-001";
            string Message = "New Order Created";

            Console.WriteLine("Hello Everyone!");
            Console.WriteLine("Let's send push notifications!!!");

            Console.WriteLine();

            Console.Write("How many devices are going to receive this notification? ");
            int.TryParse(Console.ReadLine(), out int devicesCount);
            var tokens = new string[devicesCount];

            Console.WriteLine();

            for (int i = 0; i < devicesCount; i++)
            {
                Console.Write($"Token for device number {i + 1}: ");
                tokens[i] = Console.ReadLine();
                Console.WriteLine();
            }

            Console.WriteLine("Do you want to send notifications?");
            Console.WriteLine("1 - Yes!!!!");
            Console.WriteLine("0 - No, I'm wasting my time!!!");

            int.TryParse(Console.ReadLine(), out int sendNotification);
            if (sendNotification == 1)
            {
                var pushSent = PushNotificationLogic.SendPushNotification(tokens, Title, NotificationCode, Message);
                Console.WriteLine($"Notification sent");
            }
            else
            {
                Console.WriteLine("thank you! you are not sending messages");
            }

            Console.ReadKey();
        }
    }
}
