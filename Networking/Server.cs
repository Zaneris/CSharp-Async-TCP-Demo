using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking {
    class Server {
        private readonly TcpListener _server;
        private readonly List<Task> _clients;
        private readonly Task _task;
        private const int HEADER_SIZE = 3;

        public Server(string listeningIp, int port) {
            var ip = IPAddress.Parse(listeningIp);
            _clients = new List<Task>();
            _server = new TcpListener(ip, port);
            _server.Start();
            _task = ListenForClients();
        }

        private async Task ListenForClients() {
            while(true) {
                Console.WriteLine("Listening For Clients!");
                var client = await _server.AcceptTcpClientAsync();
                Console.WriteLine("Client Connected To Server!");
                var stream = client.GetStream();
                var clientTask = ListenToClient(stream);
                _clients.Add(clientTask);
            }
        }

        private async Task ListenToClient(NetworkStream stream) {
            var bytes = new byte[256];
            int i;
            while((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0) {
                IPacket packet = null;
                int index = 0;
                while(index < i) {
                    var packetLength = BitConverter.ToUInt16(bytes, index + 1);
                    switch(bytes[index]) {
                        case 1: // PacketMessage
                            packet = new PacketMessage(bytes, index + HEADER_SIZE, packetLength);
                            break;
                    }
                    index += packetLength + HEADER_SIZE;
                    packet.HandlePacket();
                }
            }
        }
    }
}
