using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace NLogSmtp
{
    class Program
    {

        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);
                logger.Warn("Test from test app");
            }
            
            Console.ReadKey();
        }
    }
}
