using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking {
    class Client {
        private readonly NetworkStream _stream;
        private readonly byte[] _buffer = new byte[256];

        public Client(int port, string destination) {
            var client = new TcpClient(destination, port);
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
            Array.Copy(lengthBytes, 0, _buffer, 1, 2);
            Array.Copy(data, 0, _buffer, 3, data.Length);
            // Remove these lines
            SimulateDouble(data.Length + 3);
            await _stream.WriteAsync(_buffer, 0, (data.Length + 3) * 2);
            // Remove avove lines
            // await _stream.WriteAsync(_buffer, 0, data.Length + 3); // UNCOMMENT THIS
        }

        public void SimulateDouble(int length) { // Delete this method
            Array.Copy(_buffer, 0, _buffer, length, length);
        }
    }
}
