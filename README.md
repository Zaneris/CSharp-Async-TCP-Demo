## C# Async TCP Demo

This project is intended for demonstration purposes only.

The code within is not production ready and lacks error handling.

### Usage

Running the project will create a single server instance, and a single client instance. It will then send a test file of 1800 bytes, followed by a simple ASCII message, from the client instance, to the server instance via TCP.

### IPacket

There are 2 examples of an implementation of **IPacket** within the project:

- **PacketMessage** - A simple string to ASCII to bytes packet.
- **PacketFile** - Used for transmitting an array of bytes.

There also exists **PacketBuilder** which is used for converting **IPacket** objects into bytes, and from bytes to **IPacket** for use in sending and receiving via the **TCPClient** and **TCPListener**.
