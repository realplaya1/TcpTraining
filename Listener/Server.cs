using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Listener
{
    public class Server
    {
        private TcpListener _server;

        public event EventHandler<TcpClient> OnClientConnected;

        public Server(string localAddress, int port)
        {
            IPAddress address = IPAddress.Parse(localAddress);
            _server = new TcpListener(address, port);
        }

        public void StartAcceptingConnections()
        {
            _server.Start();

            Thread thread = new Thread(() => {
                // Enter the listening loop.
                while (true)
                {
                    ListenForConnections();
                }
            });
        }

        private void ListenForConnections()
        {
            Console.Write("Waiting for a connection... ");

            TcpClient client = _server.AcceptTcpClient();
            OnClientConnected?.Invoke(this, client);
        }
    }
}