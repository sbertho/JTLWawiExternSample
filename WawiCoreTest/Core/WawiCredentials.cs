using JTLwawiExtern;

namespace WawiCoreTest.Core
{
    internal sealed class WawiCredentials : IWawiCredentails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordEncrypted { get; set; }
    }
}