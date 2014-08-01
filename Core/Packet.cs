using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    /// <summary>
    /// Replay packet.
    /// </summary>
    public class Packet
    {
        private readonly PacketType packetType;
        private readonly PacketSubtype subtype;
        private readonly float clock;

        private readonly IDictionary<PropertyType, Property> properties = 
            new Dictionary<PropertyType, Property>();

        public Packet(PacketType packetType, PacketSubtype subtype, float clock)
        {
            this.packetType = packetType;
            this.subtype = subtype;
            this.clock = clock;
        }

        /// <summary>
        /// Gets or sets packet property.
        /// </summary>
        public Property this[PropertyType propertyType]
        {
            get
            {
                return properties[propertyType];
            }
            set
            {
                properties[propertyType] = value;
            }
        }

        /// <summary>
        /// Gets packet type.
        /// </summary>
        public PacketType PacketType
        {
            get
            {
                return packetType;
            }
        }

        /// <summary>
        /// Gets packet subtype.
        /// </summary>
        public PacketSubtype Subtype
        {
            get
            {
                return subtype;
            }
        }

        /// <summary>
        /// Gets packet clock.
        /// </summary>
        public float Clock
        {
            get
            {
                return clock;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<PropertyType, Property> pair in properties)
            {
                builder.AppendFormat(", {0}={1}", pair.Key, pair.Value);
            }
            return String.Format("Packet[Type={0}, Clock={1}{2}]", packetType, clock, builder);
        }
    }
}
