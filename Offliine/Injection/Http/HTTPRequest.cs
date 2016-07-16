﻿using System.Collections.Generic;

namespace Offliine.Injection.Http
{
    public class HttpRequest
    {
        public string Method, Protocol, Path;
        public List<HttpPropriety> Proprieties;

        public HttpRequest(string method, string protocol, string path, List<HttpPropriety> proprieties)
        {
            Method = method;
            Protocol = protocol;
            Path = path;
            Proprieties = proprieties;
        }

        public HttpPropriety GetPropriety(string key)
        {
            foreach (var prop in Proprieties)
            {
                if (prop.Key == key)
                    return prop;
            }

            return null;
        }
    }
}
