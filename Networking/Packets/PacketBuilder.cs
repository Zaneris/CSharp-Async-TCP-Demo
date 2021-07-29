using System;
using System.Collections.Generic;

namespace Networking.Packets
{
    public class PacketBuilder
    {
        private const int HEADER_SIZE = 3;
        private PacketTypes _packetType;
        private byte[] _buffer;
        private int _remainingBytes;
        private int _receivedPosition;
        private int _lastSendByte;

        public (bool Complete, int Size) BuildPacket(byte[] buffer, IPacket packet)
        {
            var bytes = packet.GetBytes();
            if (_lastSendByte == 0)
            {
                // Starting with new data.
                // Begin building header.
                buffer[0] = (byte) packet.Id;
                var lengthOfPacket = BitConverter.GetBytes((ushort) bytes.Length);
                buffer[1] = lengthOfPacket[0];
                buffer[2] = lengthOfPacket[1];
                // Header Size ^ of 3 bytes.

                if (buffer.Length >= bytes.Length + HEADER_SIZE)
                {
                    Array.Copy(bytes, 0, buffer, HEADER_SIZE, bytes.Length);
                    _lastSendByte = 0; // All bytes sent.
                    return (true, bytes.Length + HEADER_SIZE);
                }
                else
                {
                    // All bytes will not be sent.
                    Array.Copy(bytes, 0, buffer, HEADER_SIZE, buffer.Length - HEADER_SIZE);
                    _lastSendByte += buffer.Length - HEADER_SIZE; // Store our position.
                    return (false, buffer.Length);
                }
            }
            else
            {
                // Last packet isn't finished yet.
                var remainingBytes = bytes.Length - _lastSendByte;
                if (buffer.Length >= bytes.Length - _lastSendByte)
                {
                    Array.Copy(bytes, _lastSendByte, buffer, 0, remainingBytes);
                    _lastSendByte = 0; // All bytes sent.
                    return (true, remainingBytes);
                }
                else
                {
                    // All bytes will not be sent.
                    Array.Copy(bytes, _lastSendByte, buffer, 0, buffer.Length);
                    _lastSendByte += buffer.Length; // Store our position.
                    return (false, buffer.Length);
                }
            }
        }

        public IEnumerable<IPacket> ReceivePacket(byte[] bytes, int length, List<IPacket> list)
        {
            int i = 0;
            list.Clear();
            while (i < length)
            {
                var packetLength = _buffer is null ? BitConverter.ToUInt16(bytes, i + 1) : _buffer.Length;
                if (_buffer is null && packetLength <= bytes.Length - HEADER_SIZE)
                {
                    // bytes contains 1 or more full packets.
                    switch ((PacketTypes) bytes[i])
                    {
                        case PacketTypes.Message:
                            list.Add(new PacketMessage(bytes, i + HEADER_SIZE, packetLength));
                            break;
                        case PacketTypes.File:
                            list.Add(new PacketFile(bytes, i + HEADER_SIZE, packetLength));
                            break;
                    }

                    i += packetLength + HEADER_SIZE;
                }
                else
                {
                    // An incomplete packet has been received.
                    if (_buffer is null)
                    {
                        _packetType = (PacketTypes) bytes[i];
                        _buffer = new byte[packetLength];
                        _receivedPosition = bytes.Length - i - HEADER_SIZE;
                        _remainingBytes = packetLength - _receivedPosition;
                        Array.Copy(bytes, i + HEADER_SIZE, _buffer, 0, _receivedPosition);
                        return list;
                    }

                    if (length >= _remainingBytes)
                    {
                        // Full packet can be built with received bytes.
                        Array.Copy(bytes, i, _buffer, _receivedPosition, _remainingBytes);
                        switch (_packetType)
                        {
                            case PacketTypes.Message:
                                list.Add(new PacketMessage(_buffer, 0, _buffer.Length));
                                break;
                            case PacketTypes.File:
                                list.Add(new PacketFile(_buffer));
                                break;
                        }

                        _buffer = null;
                        i += _remainingBytes;
                    }
                    else
                    {
                        // Still not enough bytes to build the packet.
                        Array.Copy(bytes, 0, _buffer, _receivedPosition, length);
                        _receivedPosition += bytes.Length;
                        _remainingBytes -= bytes.Length;
                        return list;
                    }
                }
            }

            return list;
        }
    }
}
