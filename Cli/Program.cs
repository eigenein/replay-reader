using System;
using System.IO;

using Core;

namespace Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Replay file is expected.");
                return 1;
            }
            using (Decoder decoder = new Decoder(File.OpenRead(args[0]), false))
            {
                foreach (object o in decoder.ReadUnencryptedPart())
                {
                    Console.WriteLine(o);
                }
                foreach (Packet packet in decoder.ReadEncryptedPart())
                {
                    Console.WriteLine(packet);
                }
            }
            return 0;
        }
    }
}
