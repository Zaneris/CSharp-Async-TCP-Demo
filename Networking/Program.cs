﻿using System;

namespace Networking {
    class Program {
        static void Main() {
            Console.WriteLine("Hello World!");
            var server = new Server("127.0.0.1", 13000);
            var client = new Client("127.0.0.1", 13000);
            client.SendTestFile();
            client.SendMessage("Hello from client!");
            while(true) client.GetAndSendMessage(); // Send text entered in console.
        }
    }
}
