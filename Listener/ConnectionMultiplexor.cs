using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Listener
{
    public class ConnectionMultiplexor
    {
        private TcpListener _server;
        Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();

        public ConnectionMultiplexor(string localAddress, int port)
        {
            IPAddress address = IPAddress.Parse(localAddress);
            _server = new TcpListener(address, port);
        }

        public void StartAcceptingConnections()
        {
            _server.Start();

            // Enter the listening loop.
            while (true)
            {
                ListenForConnections();
            }
        }

        private void ListenForConnections()
        {
            Console.Write("Waiting for a connection... ");

            TcpClient client = _server.AcceptTcpClient();
            Console.WriteLine("Connected!");

            _clients.Add(client.GetHashCode().ToString(), client);

            Thread clientThread = new Thread(() =>
            {
                Byte[] bytes = new Byte[256];
                String data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();
                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    Console.WriteLine("Cursor Move Upper Left");
                    

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                // Shutdown and end connection
                client.Close();
            });

            clientThread.Start();
        }
    }
}