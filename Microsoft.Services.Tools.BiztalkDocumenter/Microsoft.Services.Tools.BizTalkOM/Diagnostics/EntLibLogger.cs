namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class EntLibLogger : ILogger
    {
        public void Log(string message)
        {
            this.Log(message, LoggingCategory.General, LoggingPriority.Normal);
        }

        public void Log(string message, LoggingCategory loggingCategory, LoggingPriority loggingPriority)
        {

            LogEntry entry = new LogEntry();
            entry.Message = message;
            entry.Categories.Add(loggingCategory.ToString());
            entry.Priority = (int)Enum.Parse(typeof(LoggingPriority), loggingPriority.ToString());
            Logger.Write(entry);

        }

        public void LogInfo(string message)
        {
            this.Log(message, LoggingCategory.Info, LoggingPriority.Normal);
        }

        public void LogWarning(string message)
        {
            this.Log(message, LoggingCategory.Warning, LoggingPriority.High);
        }

        public void LogError(string message, Exception ex)
        {
            this.Log(message, LoggingCategory.Error, LoggingPriority.High);
        }

        public void LogFatal(string message, Exception ex)
        {
            this.Log(message, LoggingCategory.Error, LoggingPriority.Highest);
        }


    }
}
