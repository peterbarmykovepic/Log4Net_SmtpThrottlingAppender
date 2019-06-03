using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Log4NetSmtp
{
    class Program
    {

        static void Main(string[] args)
        {
            var config = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.Configure(config);
            var logger = LogManager.GetLogger("log");
            
            logger.Debug("This is a Debug message");
            logger.Info("This is a Info message");
            logger.Warn("This is a Warning message");
            logger.Error("This is an Error message");
            logger.Fatal("This is a Fatal message");
            

            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);
                logger.Warn("Test from test app");
            }
            
            Console.ReadKey();
        }
    }
}
