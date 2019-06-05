Nuget

https://www.nuget.org/packages/Log4Net_SmtpThrottlingAppender/


```text
Caching version of the standard log4net SmtpAppender. This appender will
cache log events that are to be sent out via Smtp and send them in block.
Configuration options:
<FlushInterval value="hh:mm:ss" />
   Indicates the periodic interval for log events flushing (e.g. sending and e-mail), specified
   as a time span.  If the value of FlushInterval is 0:0:0 (zero), periodic interval flushing
   is surpressed.  Default value:  0:5:0 (5minutes).
 <FlushCount value="x" />
   Indicates the number of log events received by the appender which will trigger flushing
   (e.g. sending and e-mail).  If the value FlushCount is 0, buffer flush triggering based
   the number of log events received is surpressed. Default value:  20 events.
 
<IncludeContextLogEvents value="[true/false]"/>"
   True if context log events are included in the buffered log event cache.  Cache events are
   those that are of a lower level than that specified for triggering the SmtpCachingAppender.
   False if no context log events are included. Default value:  true.
  <MaxBufferSize value="x"/>
   Maximum number of log events, both SmtpCachingAppender events and context events to save
   in the cache buffer.  If more than MaxBufferSize events are received before the flushing
   criteria is met, only the newest MaxBufferSize events are saved in the buffer.  A value
   of 0 indicates no limit in the size of the cache.  Default value:  0 (no limit)
 Sample SmtpCachingAppender configuration (in additionto all standard SmtpAppender options).
   Note the namespace and assembly name for the appender.
 
<appender name="SmtpCachingAppender" type="log4net.Extensions.SmtpCachingAppender, Utilities">
   . . .
   <FlushInterval value="00:05:00" />
   <FlushCount value="20" />
   <IncludeContextLogEvents value="false"/>"
   <MaxBufferSize value="3"/>
</appender>
```
