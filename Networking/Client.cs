using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.Packets;

namespace Networking
{
    public class Client
    {
        private readonly NetworkStream _stream;
        private readonly byte[] _buffer = new byte[1024];
        private readonly PacketBuilder _builder;

        public Client(string destinationIp, int port)
        {
            var client = new TcpClient(destinationIp, port);
            _stream = client.GetStream();
            _builder = new PacketBuilder();
        }

        public async void GetAndSendMessage()
        {
            var msg = Console.ReadLine();
            await SendAsync(new PacketMessage(msg));
        }

        public async void SendMessage(string msg)
        {
            await SendAsync(new PacketMessage(msg));
        }

        public async Task SendAsync(IPacket packet)
        {
            (bool Complete, int Size) status;
            do
            {
                status = _builder.BuildPacket(_buffer, packet);
                await _stream.WriteAsync(_buffer, 0, status.Size);
            } while (!status.Complete);
        }

        public async void SendTestFile()
        {
            var data = new byte[1800];
            for (int i = 0; i < 1800; i++)
                data[i] = (byte) (i % 256);
            Console.WriteLine($"Sending File: {data.Length} bytes");
            await SendAsync(new PacketFile(data));
        }
    }
}
