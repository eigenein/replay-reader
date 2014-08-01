using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace Core
{
    public class Decoder : IDisposable
    {
        private readonly BinaryReader reader;

        public Decoder(Stream stream, bool leaveOpen)
        {
            reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen);
        }

        public IEnumerable<object> ReadUnencryptedPart()
        {
            ReplayHeader header = ReadHeader();
            for (int i = 0; i < header.JsonBlockCount; i++)
            {
                yield return ReadJson();
            }
        }

        private object ReadJson()
        {
            int length = reader.ReadInt32();
            string value = Encoding.ASCII.GetString(reader.ReadBytes(length));
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Reads replay header.
        /// </summary>
        private ReplayHeader ReadHeader()
        {
            byte[] header = reader.ReadBytes(8);
            return new ReplayHeader(header[4]);
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
