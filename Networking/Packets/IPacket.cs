namespace Networking.Packets {
    public interface IPacket {
        public PacketTypes ID { get; }
        public byte[] GetBytes();
        public void HandlePacket();
    }
}
