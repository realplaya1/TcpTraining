using System.Net.Http;
using System.IO.Enumeration;
using System.IO;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Security.AccessControl;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ClientClassNamespace
{
    public class ClientClass 
    {
        private string _serverAddres;
        private int _port;

        private Thread _listeningThread;

        private bool _isListening;

        private TcpClient _client;

        public void Connect()
        {
            Int32 port = 13000;
            _client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();
        }

        public void SendMessage()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, DataAdapter.Length);
        }

        private void StartListenning()
        {
            _isListening = true;

            _listeningThread = new Thread(() =>
            {
                while (_isListening)
                {
                    byte[] data = new byte[256];
                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    OnMessageReceived?.Invoke(message); 
                }
                
            });

            _listeningThread.Start();
        }

        private void StopListen()
        {
            _isListening = false;
        }

        public void Disconnect()
        {
            StopListen();
            _listeningThread.Abort();
            _stream.Dispose();
            _client.Close();
        }
    }
}