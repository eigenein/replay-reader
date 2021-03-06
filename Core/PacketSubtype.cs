﻿using System;

namespace Core
{
    /// <summary>
    /// Replay packet subtype.
    /// </summary>
    public enum PacketSubtype
    {
        None = 0x00,
        Subtype01 = 0x01,
        Subtype03 = 0x03,
        Subtype05 = 0x05,
        Subtype07 = 0x07,
        Subtype0B = 0x0B,
        Subtype11 = 0x11,
        Subtype17 = 0x17,
    }
}
