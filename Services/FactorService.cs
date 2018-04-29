using System.Collections.Generic;

namespace BlankDroid.Services
{
    public static class FactorService
    {
        private static Dictionary<string, bool> _factorsContext = new Dictionary<string, bool>();

        public static Dictionary<string, bool> GetContext()
        {
            return _factorsContext;
        }

        public static void UpdateContext(string name, bool status)
        {
            if (_factorsContext.ContainsKey(name))
            {
                _factorsContext[name] = status;
            }
            else
            {
                _factorsContext.Add(name, status);
            }
        }
    }
}