using System.Collections.Generic;

namespace Offliine.Injection
{
    public class SystemVersions
    {
        private static readonly List<SystemVersion> Versions = new List<SystemVersion>();

        public static readonly SystemVersion Us551 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.12 NintendoBrowser/4.3.1.11264.US", Constants.C550, "stagefright.bin", new [] { "550" });
        public static readonly SystemVersion Us550 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.11 NintendoBrowser/4.3.0.11224.US", Constants.C550, "stagefright.bin", new [] { "550" });
        public static readonly SystemVersion Us540 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.9 NintendoBrowser/4.2.0.11146.US", Constants.C532, "stagefright.bin", new [] { "532", "540" });
        public static readonly SystemVersion Us532 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.28 (KHTML, like Gecko) NX/3.0.3.12.15 NintendoBrowser/4.1.1.9601.US", Constants.C532, "stagefright.bin", new [] { "532" });

        public static readonly SystemVersion Eu551 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.12 NintendoBrowser/4.3.1.11264.EU", Constants.C550, "stagefright.bin", new[] { "550" });
        public static readonly SystemVersion Eu550 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.11 NintendoBrowser/4.3.0.11224.EU", Constants.C550, "stagefright.bin", new[] { "550" });
        public static readonly SystemVersion Eu540 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.9 NintendoBrowser/4.2.0.11146.EU", Constants.C532, "stagefright.bin", new[] { "532", "540" });
        public static readonly SystemVersion Eu532 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.28 (KHTML, like Gecko) NX/3.0.3.12.15 NintendoBrowser/4.1.1.9601.EU", Constants.C532, "stagefright.bin", new[] { "532" });

        public static readonly SystemVersion Jp540 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.30 (KHTML, like Gecko) NX/3.0.4.2.9 NintendoBrowser/4.2.0.11146.JP", Constants.C532, "stagefright.bin", new[] { "532", "540" });
        public static readonly SystemVersion Jp532 = _registerVersion("Mozilla/5.0 (Nintendo WiiU) AppleWebKit/536.28 (KHTML, like Gecko) NX/3.0.3.12.15 NintendoBrowser/4.1.1.9601.JP", Constants.C532, "stagefright.bin", new[] { "532" });

        private static SystemVersion _registerVersion(string browserVersion, IConstants constants, string loaderName, string[] payloadVersions)
        {
            var ret = new SystemVersion(browserVersion, constants, loaderName, payloadVersions);
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
        public readonly string BrowserVersion, LoaderName;
        public readonly IConstants Constants;
        public readonly string[] PayloadVersions;

        public SystemVersion(string browserVersion, IConstants constants, string loaderName, string[] payloadVersions)
        {
            BrowserVersion = browserVersion;
            Constants = constants;
            LoaderName = loaderName;
            PayloadVersions = payloadVersions;
        }
    }
}