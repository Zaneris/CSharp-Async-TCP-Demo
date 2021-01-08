using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking {
    class Client {
        private readonly NetworkStream _stream;
        private readonly byte[] _buffer = new byte[256];
        private const int HEADER_SIZE = 3;

        public Client(string destinationIp, int port) {
            var client = new TcpClient(destinationIp, port);
            _stream = client.GetStream();
        }

        public async void GetAndSendMessage() {
            var msg = Console.ReadLine();
            await SendAsync(new PacketMessage(msg));
        }

        public async Task SendAsync(IPacket packet) {
            var data = packet.GetBytes();
            _buffer[0] = packet.ID;
            var lengthBytes = BitConverter.GetBytes((ushort)data.Length);
            Array.Copy(lengthBytes, 0, _buffer, 1, lengthBytes.Length);
            Array.Copy(data, 0, _buffer, HEADER_SIZE, data.Length);
            await _stream.WriteAsync(_buffer, 0, data.Length + HEADER_SIZE);
        }
    }
}
