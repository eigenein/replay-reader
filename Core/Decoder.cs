using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using Newtonsoft.Json;

namespace Core
{
    public class Decoder : IDisposable
    {
        private readonly BinaryReader reader;

        private readonly byte[] key =
        {
            0xDE, 0x72, 0xBE, 0xA0, 0xDE, 0x04, 0xBE, 0xB1,
            0xDE, 0xFE, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF
        };

        public Decoder(Stream stream, bool leaveOpen)
        {
            reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen);
        }

        /// <summary>
        /// Reads unencrypted part of the replay.
        /// </summary>
        public IEnumerable<object> ReadUnencryptedPart()
        {
            ReplayHeader header = ReadHeader();
            for (int i = 0; i < header.JsonBlockCount; i++)
            {
                yield return ReadJson();
            }
        }

        /// <summary>
        /// Reads encrypted part of replay.
        /// </summary>
        public IEnumerable<Packet> ReadEncryptedPart()
        {
            reader.ReadInt32(); // skip
            int compressedLength = reader.ReadInt32();
            Stream decryptedStream = ReadDecryptedStream(compressedLength); // decrypt
            decryptedStream.Seek(0, SeekOrigin.Begin); // rewind
            InflaterInputStream inflatedStream = new InflaterInputStream(decryptedStream);
            return ReadPackets(inflatedStream); // read packets from inflated stream
        }

        /// <summary>
        /// Reads replay header.
        /// </summary>
        private ReplayHeader ReadHeader()
        {
            byte[] header = reader.ReadBytes(8);
            return new ReplayHeader(header[4]);
        }

        /// <summary>
        /// Reads JSON from the current position.
        /// </summary>
        private object ReadJson()
        {
            int length = reader.ReadInt32();
            string value = Encoding.ASCII.GetString(reader.ReadBytes(length));
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Reads decrypted stream from the current position.
        /// </summary>
        private Stream ReadDecryptedStream(int compressedLength)
        {
            Blowfish blowfish = new Blowfish(key);
            Stream outputStream = new MemoryStream();
            byte[] previousBlock = new byte[8]; // previous decrypted block
            while (compressedLength != 0)
            {
                byte[] buffer = reader.ReadBytes(8);
                blowfish.Decipher(buffer, buffer.Length);
                // XOR with previous block.
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] ^= previousBlock[i];
                }
                buffer.CopyTo(previousBlock, 0); // remember current block
                int blockLength = Math.Min(buffer.Length, compressedLength); // the last block is shorter than 8 bytes
                outputStream.Write(buffer, 0, blockLength); // write decrypted block
                compressedLength -= blockLength;
            }
            return outputStream;
        }

        /// <summary>
        /// Reads packets from inflated stream.
        /// </summary>
        private IEnumerable<Packet> ReadPackets(Stream stream)
        {
            PacketReader packetReader = new PacketReader(
                new BinaryReader(stream, Encoding.ASCII, false));
            while (true)
            {
                Packet packet = packetReader.ReadPacket();
                if (packet == null)
                {
                    break;
                }
                yield return packet;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
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
