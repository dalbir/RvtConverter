using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace RvtConverter
{
   public class Log
    {
        private static ILog _logger { get; set; }
        public static ILog logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(typeof(Log));
                }
                return _logger;
            }
            private set => _logger = value;
        }
        public static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
