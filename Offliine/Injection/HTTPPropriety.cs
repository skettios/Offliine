using Android.Hardware.Camera2;

namespace Offliine.Injection
{
    public class HTTPPropriety
    {
        public string Key, Value;

        public HTTPPropriety(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}