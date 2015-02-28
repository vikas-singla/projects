using System;
using log4net;
using log4net.Config;

namespace Oceanus.Logging
{
    public class Log4NetLogger : ILogger
    {
        private static readonly ILog log = LogManager.GetLogger("SixLoonies");

        public Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        #region ILogger

        public void Debug(string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        public void Debug(string message, Exception ex)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message, ex);
            }
        }

        public void Error(string message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        public void Error(string message, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }

        public void Warn(string message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        public void Warn(string message, Exception ex)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message, ex);
            }
        }

        public void Info(string message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        public void Info(string message, Exception ex)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message, ex);
            }
        }

        #endregion

    }
}