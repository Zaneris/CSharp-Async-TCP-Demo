using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.Packets;

namespace Networking {
    class Server {
        private readonly TcpListener _server;
        private readonly List<Task> _clients;
        private readonly Task _task;

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
            var packetList = new List<IPacket>();
            var builder = new PacketBuilder();
            var bytes = new byte[1024];
            int i;
            while((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0) {
                foreach(var packet in builder.ReceivePacket(bytes, i, packetList))
                    packet.HandlePacket();
            }
        }
    }
}
