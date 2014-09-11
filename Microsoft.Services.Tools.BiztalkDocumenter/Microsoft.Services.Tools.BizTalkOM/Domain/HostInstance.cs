
namespace Microsoft.Services.Tools.BizTalkOM
{

    /// <summary>
    /// Summary description for HostInstance.
    /// </summary>
    public class HostInstance : BizTalkBaseObject
    {
        public HostInstance()
        {
        }

        public HostInstance(string name, string hostName, string logon, bool disabled)
        {
            this.Name = name;
            this.HostName = hostName;
            this.Disabled = disabled;
            this.Logon = logon;
        }

        /// <summary>
        /// Parent Host Name
        /// </summary>
        public string HostName { get; set; }

        public string Logon { get; set; }

        public bool Disabled { get; set; }
    }
}
