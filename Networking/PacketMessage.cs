﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Networking {
    class PacketMessage : IPacket {
        private readonly string _msg;

        public PacketMessage(string message) {
            _msg = message;
        }

        public PacketMessage(byte[] bytes, int start, int length) {
            _msg = Encoding.ASCII.GetString(bytes, start, length);
        }

        public byte ID => 1;

        public byte[] GetBytes() {
            return Encoding.ASCII.GetBytes(_msg);
        }

        public void HandlePacket() {
            Console.WriteLine($"Server Received: {_msg}");
        }
    }
}
