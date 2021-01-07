using System;
using System.Collections.Generic;
using System.Text;

namespace Networking {
    public interface IPacket {
        public byte ID { get; }
        public byte[] GetBytes();
        public void HandlePacket();
    }
}
