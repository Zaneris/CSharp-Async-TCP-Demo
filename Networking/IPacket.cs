namespace Networking {
    public interface IPacket {
        public byte ID { get; }
        public byte[] GetBytes();
        public void HandlePacket();
    }
}
