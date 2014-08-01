using System;
using System.Text;

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

    /// <summary>
    /// Represents position and orientation.
    /// </summary>
    public class VectorProperty : Property
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        public static VectorProperty FromPayload(byte[] payload, int startIndex)
        {
            return new VectorProperty(
                BitConverter.ToSingle(payload, startIndex),
                BitConverter.ToSingle(payload, startIndex + sizeof(float)),
                BitConverter.ToSingle(payload, startIndex + 2 * sizeof(float)));
        }

        private VectorProperty(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return String.Format("({0}; {1}; {2})", x, y, z);
        }
    }

    public class StringProperty : Property<string>
    {
        public static StringProperty FromPayload(byte[] payload, int startIndex)
        {
            return new StringProperty(Encoding.UTF8.GetString(
                payload, startIndex + 4, BitConverter.ToInt32(payload, startIndex)));
        }

        private StringProperty(string value) : base(value)
        {
            // Do nothing.
        }
    }
}
