using System;

namespace Stump.ORM.SubSonic.DataProviders.Log
{
    public class DelegatingLogAdapter : ILogAdapter
    {
        private Action<string> _logger;

        public DelegatingLogAdapter(Action<string> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger(message);
        }
    }
}