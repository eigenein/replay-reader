using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
    public class PacketReader : IDisposable
    {
        private readonly BinaryReader reader;

        public PacketReader(BinaryReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads packet from the current position.
        /// </summary>
        public Packet ReadPacket()
        {
            int payloadLength;
            try
            {
                payloadLength = reader.ReadInt32();
            }
            catch (EndOfStreamException)
            {
                return null;
            }
            // Read common properties.
            PacketType packetType = (PacketType)reader.ReadInt32();
            float clock = reader.ReadSingle();
            Packet packet = new Packet(packetType, clock);
            byte[] payload = reader.ReadBytes(payloadLength); // read payload
            ReadProperties(packet, payload); // read packet properties
            return packet;
        }

        /// <summary>
        /// Reads properties to the packet.
        /// </summary>
        private void ReadProperties(Packet packet, byte[] payload)
        {
            //
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                reader.Dispose();
            }
        }
    }
}
