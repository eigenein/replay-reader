using System;

namespace Core
{
    public class ReplayHeader
    {
        private readonly int jsonBlockCount;

        public ReplayHeader(int jsonBlockCount)
        {
            this.jsonBlockCount = jsonBlockCount;
        }

        public int JsonBlockCount
        {
            get
            {
                return jsonBlockCount;
            }
        }
    }
}
