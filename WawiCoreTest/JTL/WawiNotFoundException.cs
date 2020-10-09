using System;
using System.Runtime.Serialization;

namespace WawiCoreTest.JTL
{
    [Serializable]
    public sealed class WawiNotFoundException : Exception
    {
        public WawiNotFoundException() : base("JTL-Wawi Installation not found")
        {
        }

        public WawiNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}