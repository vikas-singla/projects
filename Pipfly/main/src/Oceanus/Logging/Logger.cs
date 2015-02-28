namespace Oceanus.Logging
{
    public class Logger
    {
        private static ILogger _logger;

        public static ILogger Instance
        {
            get { return _logger ?? (_logger = new Log4NetLogger()); }
        }
    }
}