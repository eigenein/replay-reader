using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
    public class PacketReader : IDisposable
    {
        private static readonly IDictionary<PropertyType, PropertyGetter> PropertyGetters =
            new Dictionary<PropertyType, PropertyGetter>()
            {
                {PropertyType.Health, payload => new ShortProperty(BitConverter.ToInt16(payload, 12))},
            };

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
            HashSet<PropertyType> propertyTypes = GetPropertyTypes(packet, payload);
            FillProperties(packet, propertyTypes, payload);
        }

        /// <summary>
        /// Gets property types for the specified packet.
        /// </summary>
        private HashSet<PropertyType> GetPropertyTypes(Packet packet, byte[] payload)
        {
            PacketSubtype subtype = PacketSubtype.None;
            HashSet<PropertyType> propertyTypes = new HashSet<PropertyType>();
            // Read subtype if possible.
            if ((packet.PacketType == PacketType.EntityProperty) ||
                (packet.PacketType == PacketType.EntityMethod))
            {
                subtype = (PacketSubtype)BitConverter.ToInt32(payload, 4);
                packet[PropertyType.Subtype] = new SubtypeProperty(subtype);
            }
            // Fill property type set.
            switch (packet.PacketType)
            {
                case PacketType.EntityEnter:
                case PacketType.EntityCreate:
                    propertyTypes.Add(PropertyType.PlayerId);
                    break;
                case PacketType.EntityMoveWithError:
                    propertyTypes.Add(PropertyType.Position);
                    propertyTypes.Add(PropertyType.HullOrientation);
                    propertyTypes.Add(PropertyType.PlayerId);
                    break;
                case PacketType.EntityProperty:
                    propertyTypes.Add(PropertyType.PlayerId);
                    if (subtype == PacketSubtype.Subtype03)
                    {
                        propertyTypes.Add(PropertyType.Health);
                    }
                    if (subtype == PacketSubtype.Subtype07)
                    {
                        propertyTypes.Add(PropertyType.DestroyedTrackId);
                    }
                    break;
                case PacketType.EntityMethod:
                    propertyTypes.Add(PropertyType.PlayerId);
                    // TODO: property_t::tank_destroyed
                    switch (subtype)
                    {
                        case PacketSubtype.Subtype01:
                            propertyTypes.Add(PropertyType.Source);
                            propertyTypes.Add(PropertyType.Health);
                            break;
                        case PacketSubtype.Subtype05:
                            propertyTypes.Add(PropertyType.Source);
                            break;
                        case PacketSubtype.Subtype0B:
                            propertyTypes.Add(PropertyType.Source);
                            propertyTypes.Add(PropertyType.Target);
                            break;
                        case PacketSubtype.Subtype17:
                            propertyTypes.Add(PropertyType.Target);
                            break;
                    }
                    break;
                case PacketType.BattleChatMessage:
                    propertyTypes.Add(PropertyType.Message);
                    break;
                case PacketType.NestedEntityProperty:
                    propertyTypes.Add(PropertyType.PlayerId);
                    // TODO: property_t::destroyed_track_id
                    // TODO: property_t::alt_track_state
                    break;
            }
            return propertyTypes;
        }

        /// <summary>
        /// Fills property values.
        /// </summary>
        private void FillProperties(
            Packet packet,
            IEnumerable<PropertyType> propertyTypes,
            byte[] payload)
        {
            foreach (PropertyType propertyType in propertyTypes)
            {
                PropertyGetter getter;
                if (PropertyGetters.TryGetValue(propertyType, out getter))
                {
                    packet[propertyType] = PropertyGetters[propertyType](payload);
                }
            }
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
