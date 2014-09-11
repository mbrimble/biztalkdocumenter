
namespace Microsoft.Services.Tools.BizTalkOM
{

    /// <summary>
    /// OrchestrationError.
    /// </summary>
    public class OrchestrationError
    {
        private string message = string.Empty;
        private string stackTrace = string.Empty;

        public OrchestrationError()
            : this(string.Empty)
        {
        }

        public OrchestrationError(string message)
            : this(message, string.Empty)
        {
        }

        public OrchestrationError(string message, string stackTrace)
        {
            this.message = message;
            this.stackTrace = stackTrace;
        }

        public string Messsage
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public string StackTrace
        {
            get { return this.stackTrace; }
            set { this.stackTrace = value; }
        }
    }
}
