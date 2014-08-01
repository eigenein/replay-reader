using System;
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

        public Packet ReadPacket()
        {
            throw new NotImplementedException();
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
