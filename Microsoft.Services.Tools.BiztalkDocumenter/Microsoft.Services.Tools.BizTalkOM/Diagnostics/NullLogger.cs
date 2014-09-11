using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    public class NullLogger : ILogger
    {

        #region ILogger Members

        public void Log(string message)
        {
            return;
        }

        public void LogInfo(string message)
        {
            return;
        }

        public void LogWarning(string message)
        {
            return;
        }

        public void LogError(string message, Exception ex)
        {
            return;
        }

        public void LogFatal(string message, Exception ex)
        {
            return;
        }

        #endregion
    }
}
