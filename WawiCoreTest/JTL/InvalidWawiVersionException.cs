using System;
using System.Runtime.Serialization;

namespace WawiCoreTest.JTL
{
    [Serializable]
    public sealed class InvalidWawiVersionException : Exception
    {
        public InvalidWawiVersionException(Version requiredVersion, Version currentVersion) : base(
            $"JTL-Wawi Installation version mismatch. Installed: {currentVersion}, required: {requiredVersion}")
        {
        }

        public InvalidWawiVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}