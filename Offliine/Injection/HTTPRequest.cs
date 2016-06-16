using System.Collections.Generic;

namespace Offliine.Injection
{
    public class HTTPRequest
    {
        public string Method, Protocol, Path;
        public List<HTTPPropriety> Proprieties;

        public HTTPRequest(string method, string protocol, string path, List<HTTPPropriety> proprieties)
        {
            Method = method;
            Protocol = protocol;
            Path = path;
            Proprieties = proprieties;
        }

        public HTTPPropriety GetPropriety(string key)
        {
            foreach (var prop in Proprieties)
                if (prop.Key == key)
                    return prop;

            return null;
        }
    }
}