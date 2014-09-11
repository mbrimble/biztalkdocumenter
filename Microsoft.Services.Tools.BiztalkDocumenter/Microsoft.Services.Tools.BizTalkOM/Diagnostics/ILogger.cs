using System;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    public interface ILogger
    {
        void Log(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex);
        void LogFatal(string message, Exception ex);
    }
}
