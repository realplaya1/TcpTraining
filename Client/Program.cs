using System.Reflection.Metadata;
using System;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.Connect("127.0.0.1", 13000);
            client.MessageReceived += (sender, message) => 
            {
                Console.WriteLine($"Message received: {message}");
            };

            string data = null;
            while(data != "Stop")
            {
                data = Console.ReadLine();
                if(data != "Stop")
                {
                    client.SendCommand(data);
                }
            }

            client.Disconnect();
        }
    }
}
