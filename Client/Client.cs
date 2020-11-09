using System.Threading;
using System;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        TcpClient _client;
        NetworkStream _stream;
        Thread _readingThread;
        CancellationTokenSource _cts = new CancellationTokenSource();
        public event EventHandler<string> MessageReceived; 

        public void Connect(string serverAddress, int port)
        {
            _client = new TcpClient(serverAddress, port);
            _stream = _client.GetStream();
            _readingThread = new Thread(() => 
            {
                while (!_cts.IsCancellationRequested)
                {
                    String responseData = String.Empty;
                    Byte[] data = new Byte[256];
                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    MessageReceived?.Invoke(this, responseData);
                }
            });
            _readingThread.Start();
        }
        public void SendCommand(string command)
        {
            Byte[] bytes = new Byte[256];
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            _stream.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {
            _cts.Cancel();
            _stream.Close();
            _client.Close();
        }
    }
}