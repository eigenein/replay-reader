using System;

namespace Core
{
    public abstract class Property
    {
        // Nothing.
    }

    public abstract class Property<TValue>
    {
        private readonly TValue value;

        protected Property(TValue value)
        {
            this.value = value;
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
}
