using System;
using System.Collections.Generic;
using System.Threading;
using log4net.Appender;
using log4net.Core;

namespace Log4Net_SmtpThrottlingAppender
{
    // <summary>

      ///         Caching version of the standard log4net SmtpAppender. This appender will
      ///         cache log events that are to be sent out via Smtp and send them in block.
      ///         Configuration options:
      ///         <FlushInterval value="hh:mm:ss" />
      ///               Indicates the periodic interval for log events flushing (e.g. sending and e-mail), specified
      ///               as a time span.  If the value of FlushInterval is 0:0:0 (zero), periodic interval flushing
      ///               is surpressed.  Default value:  0:5:0 (5minutes).
      ///          <FlushCount value="x" />
      ///               Indicates the number of log events received by the appender which will trigger flushing
      ///               (e.g. sending and e-mail).  If the value FlushCount is 0, buffer flush triggering based
      ///               the number of log events received is surpressed. Default value:  20 events.
      /// 
      ///         <IncludeContextLogEvents value="[true/false]"/>"
      ///               True if context log events are included in the buffered log event cache.  Cache events are
      ///               those that are of a lower level than that specified for triggering the SmtpCachingAppender.
      ///               False if no context log events are included. Default value:  true.
      ///           <MaxBufferSize value="x"/>
      ///               Maximum number of log events, both SmtpCachingAppender events and context events to save
      ///               in the cache buffer.  If more than MaxBufferSize events are received before the flushing
      ///               criteria is met, only the newest MaxBufferSize events are saved in the buffer.  A value
      ///               of 0 indicates no limit in the size of the cache.  Default value:  0 (no limit)
      ///          Sample SmtpCachingAppender configuration (in additionto all standard SmtpAppender options).
      ///               Note the namespace and assembly name for the appender.
      /// 
      ///         <appender name="SmtpCachingAppender" type="log4net.Extensions.SmtpCachingAppender, Utilities">
      ///               . . .
      ///               <FlushInterval value="00:05:00" />
      ///               <FlushCount value="20" />
      ///               <IncludeContextLogEvents value="false"/>"
      ///               <MaxBufferSize value="3"/>
      ///         </appender>
      ///
      /// </summary>
      
      public class SmtpThrottlingAppender : SmtpAppender
      {

            // Appender state data
            public Timer Timer { get; private set; }
            bool _timedFlush;
            private int _numCachedMessages;
            private readonly List<LoggingEvent> _loggingEvents = new List<LoggingEvent>();

 

            /// <summary>
            ///         TimeSpan indicating the maximum period of time elapsed before sending
            ///         any cached SMTP log events.
            /// </summary>
            public TimeSpan FlushInterval { get; set; } = new TimeSpan(0, 5, 0);
        
            /// <summary>
            ///         Maximium number of SMTP events to cache before sending via SMTP.
            /// </summary>
            public int FlushCount { get; set; } = 20;

            public int MaxBufferSize { get; set; }

            public bool IncludeContextLogEvents { get; set; } = true;
 

            /// <summary>
            ///         Create a timer that fires to force flushing cached log events
            ///         via SMTP at a specified interval.
            /// </summary>
            public override void ActivateOptions()
            {
                  if (FlushInterval > TimeSpan.Zero)
                  {
                        Timer = new Timer(OnTimer, null, FlushInterval, FlushInterval);
                  }

                  base.ActivateOptions();
            }

            private void OnTimer(object stateInfo)
            {
                  _timedFlush = true;
                  Flush(true);
            }

            protected override void SendBuffer(LoggingEvent[] events)
            {
                  if (IncludeContextLogEvents)
                  {
                        _loggingEvents.AddRange(events);
                  }
                  else
                  {
                        _loggingEvents.Add(events[events.Length -1]);
                  }

                  if (MaxBufferSize != 0)
                  {
                        var numRemoved = _loggingEvents.Count - MaxBufferSize;

                        if ((numRemoved > 0) && (numRemoved <= _loggingEvents.Count))

                        {
                              _loggingEvents.RemoveRange(0, numRemoved);
                        }
                  }

 

                  _numCachedMessages++;

                  if ((FlushCount == 0 || _numCachedMessages < FlushCount) && !_timedFlush) return;

                  if (_loggingEvents.Count > 0)
                  {
                      var bufferedEvents = _loggingEvents.ToArray();
                      base.SendBuffer(bufferedEvents);
                      _loggingEvents.Clear();
                  }

                  // Reset cache buffer conditions.
                  _numCachedMessages = 0;
                  _timedFlush = false;
            }
      }
}