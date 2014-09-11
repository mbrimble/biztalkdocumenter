namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    using System;
    using System.Configuration;

    /// <summary>
    /// The central manager class for application tracing, through which all application tracing should be done.
    /// </summary>
    /// <remarks>
    /// This class instantiates any required trace listeners, and provides a means of getting access to a singleton
    /// instance of the <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/> class through which all tracing
    /// is performed.
    /// </remarks>
    public sealed class TraceManager
    {
        private static SmartTrace smartTrace;
        private static string logPath = null;

        /// <summary>
        /// Has only static members so cannot be instantiated.
        /// </summary>
        private TraceManager()
        {
        }

        /// <summary>
        /// Initialises static members of the <see cref="Microsoft.Sdc.BizTalkOM.TraceManager"/>.
        /// </summary>
        static TraceManager()
        {
            //int level = 0;

            //try
            //{
            //    level = Convert.ToInt32(ConfigurationSettings.AppSettings["TraceLevel"]);
            //    logPath = ConfigurationSettings.AppSettings["TraceLogDirectory"];
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("Trace Manager threw an exception :", e);
            //}

            //System.Diagnostics.DefaultTraceListener dtl = (System.Diagnostics.DefaultTraceListener)System.Diagnostics.Trace.Listeners["Default"];
            //if ((dtl != null && level > 0) && logPath.Length > 0) 
            //{
            //    Directory.CreateDirectory(logPath);
            //    string fullPath = Path.Combine(logPath, "Microsoft.Sdc.BizTalkOM Trace" + System.Diagnostics.Process.GetCurrentProcess().Id + ".log");
            //    dtl.LogFileName = fullPath;
            //}
            smartTrace = new SmartTrace("SDC", "Provides Tracing for Microsoft.Sdc.BizTalkOM Application", 8);
            string enableTracingFlag = ConfigurationManager.AppSettings["enable_tracing"];
            if (!String.IsNullOrEmpty(enableTracingFlag))
            {
                System.Diagnostics.Debug.WriteLine("Enable Tracing Flag is set to : " + enableTracingFlag);
                smartTrace.isTracingOn = Boolean.Parse(enableTracingFlag);
            }
            else
            {
                smartTrace.isTracingOn = false;
            }




        }

        /// <summary>
        /// Return a singleton instance of the <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/> object.
        /// </summary>
        public static SmartTrace SmartTrace
        {
            get
            {
                return smartTrace;
            }
        }
    }
}
