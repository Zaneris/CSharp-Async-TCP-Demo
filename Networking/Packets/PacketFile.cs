using System;

namespace Networking.Packets
{
    public class PacketFile : IPacket
    {
        private readonly byte[] _data;

        public PacketFile(byte[] data)
        {
            _data = data;
        }

        public PacketFile(byte[] data, int start, int length)
        {
            _data = new byte[length];
            Array.Copy(data, start, _data, 0, length);
        }

        public PacketTypes Id => PacketTypes.File;

        public byte[] GetBytes()
        {
            return _data;
        }

        public void HandlePacket()
        {
            Console.WriteLine($"Received File: {_data.Length} bytes");
        }
    }
}
