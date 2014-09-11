using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger()
        {
            string loggerName = ConfigurationManager.AppSettings["loggerName"];
            if (!String.IsNullOrEmpty(loggerName))
            {
                if (loggerName.ToLower().Equals("entlib"))
                    return new EntLibLogger();
                else
                {
                    return new NullLogger();
                }
            }

            return new NullLogger();

        }

    }
}
