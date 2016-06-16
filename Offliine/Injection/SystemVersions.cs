using System.Collections.Generic;

namespace Offliine.Injection
{
    public class SystemVersions
    {
        private static readonly List<SystemVersion> Versions = new List<SystemVersion>();

        public static readonly SystemVersion Us551 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.12 NintendoBrowser/4.3.1.11264.US", Constants.C550, "stagefright.bin", "550");
        public static readonly SystemVersion Us550 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.11 NintendoBrowser/4.3.0.11224.US", Constants.C550, "stagefright.bin", "550");

        private static SystemVersion _registerVersion(string browserVersion, IConstants constants, string loaderName, string payloadVersion)
        {
            var ret = new SystemVersion(browserVersion, constants, loaderName, payloadVersion);
            Versions.Add(ret);
            return ret;
        }

        public static SystemVersion GetSystemVersion(HTTPPropriety propriety)
        {
            if (propriety == null)
                return null;

            foreach (var version in Versions)
            {
                if (version.BrowserVersion.Equals(propriety.Value))
                    return version;
            }

            return null;
        }
    }

    public class SystemVersion
    {
        public readonly string BrowserVersion, LoaderName, PayloadVersion;
        public readonly IConstants Constants;

        public SystemVersion(string browserVersion, IConstants constants, string loaderName, string payloadVersion)
        {
            BrowserVersion = browserVersion;
            Constants = constants;
            LoaderName = loaderName;
            PayloadVersion = payloadVersion;
        }
    }
}