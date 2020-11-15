using System;
using System.Net.Sockets;
using System.Threading;

namespace Listener
{
    public class ConnectedClient
    {
        TcpClient _tcpClient;
        Thread _thr;
        Bus _bus;

        public ConnectedClient(TcpClient tcpClient, Bus bus)
        {
            _tcpClient = tcpClient;
        }

        public void StartClient()
        {
            _thr = new Thread(() => 
            {
                Byte[] bytes = new Byte[256];
                String data = null;

                NetworkStream stream = _tcpClient.GetStream();
                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // need to use bus here
                    _bus.SendMessage(data);

                    // Send back a response.
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                // Shutdown and end connection
                _tcpClient.Close(); 
            });

            _thr.Start();
        }
    }
}

