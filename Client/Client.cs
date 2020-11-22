using System.IO;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Security.AccessControl;
using System;
using System.Net.Sockets;

namespace ClientClassNamespace
{
    public class ClientClass 
    {
        private string _serverAddres;
        private int _port;

        private Thread _listeningThread;

        public void Connect()
        {
            Int32 port = 13000;
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();
        }

        public void SendMessage()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, DataAdapter.Length);
        }

        private void StartListenning()
        {
            _listeningThread = new Thread() =>
            {
                byte[] data = new byte[256];
                Int32 bytes = _stream.Read(data, 0, data.Length);
                string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                OnMessageReceived?.Invoke(message);
            };
        }
    }
}