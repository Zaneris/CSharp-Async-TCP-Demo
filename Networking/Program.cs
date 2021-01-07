using System;

namespace Networking {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var server = new Server(13000, "127.0.0.1");
            var client = new Client(13000, "127.0.0.1");
            while(true) client.GetAndSendMessage();
        }
    }
}
