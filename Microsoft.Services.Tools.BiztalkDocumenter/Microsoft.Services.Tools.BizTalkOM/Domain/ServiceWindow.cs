
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;

	/// <summary>
	/// Summary description for ServiceWindow.
	/// </summary>
	public class ServiceWindow
    {
        private bool enabled;
        private bool startDateEnabled;
        private bool endDateEnabled;
        private DateTime endTime;
        private DateTime startTime;

        public ServiceWindow()
        {
        }

        #region Public Properties

        /// <summary>
        /// Specify the time at which the service window ends
        /// </summary>
        public DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        /// <summary>
        /// Specify the time at which the service window begins
        /// </summary>
        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        /// <summary>
        /// Specify if the service window is enabled
        /// </summary>
        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        /// <summary>
        /// Specify if the service window is enabled
        /// </summary>
        public bool StartDateEnabled
        {
            get { return this.startDateEnabled; }
            set { this.startDateEnabled = value; }
        }

        /// <summary>
        /// Specify if the service window is enabled
        /// </summary>
        public bool EndDateEnabled
        {
            get { return this.endDateEnabled; }
            set { this.endDateEnabled = value; }
        }

        #endregion
    }
}
