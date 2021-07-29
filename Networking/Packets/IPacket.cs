namespace Networking.Packets
{
    public interface IPacket
    {
        public PacketTypes Id { get; }
        public byte[] GetBytes();
        public void HandlePacket();
    }
}
