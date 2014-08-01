using System;

namespace Core
{
    public abstract class Property
    {
        // Nothing.
    }

    public abstract class Property<TValue> : Property
    {
        private readonly TValue value;

        protected Property(TValue value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class FloatProperty : Property<float>
    {
        public FloatProperty(float value) : base(value)
        {
            // Do nothing.
        }
    }

    public class IntProperty : Property<int>
    {
        public IntProperty(int value) : base(value)
        {
            // Do nothing.
        }
    }

    public class ShortProperty : Property<short>
    {
        public ShortProperty(short value) : base(value)
        {
            // Do nothing.
        }
    }

    public class SubtypeProperty : Property<PacketSubtype>
    {
        public SubtypeProperty(PacketSubtype value) : base(value)
        {
            // Do nothing.
        }
    }
}
