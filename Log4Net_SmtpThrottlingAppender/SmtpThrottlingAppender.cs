using System;
using log4net.Appender;
using log4net.Core;

namespace Log4Net_SmtpThrottlingAppender
{
    public class SmtpThrottlingAppender : SmtpAppender
    {
        private DateTime _lastFlush = DateTime.MinValue;

        public TimeSpan FlushInterval { get; set; } = new TimeSpan(0, 5, 0);

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (DateTime.Now - _lastFlush > FlushInterval)
            {
                base.SendBuffer(events);
                _lastFlush = DateTime.Now;
            } 
        }
    }
}
