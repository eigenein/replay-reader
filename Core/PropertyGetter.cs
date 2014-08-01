using System;

namespace Core
{
    public delegate Property PropertyGetter(PacketSubtype subtype, byte[] payload);
}
