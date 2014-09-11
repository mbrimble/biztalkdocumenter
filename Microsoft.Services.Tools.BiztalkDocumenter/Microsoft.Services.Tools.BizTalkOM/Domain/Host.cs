
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Management;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Host.
    /// </summary>
    public sealed class Host : BizTalkBaseObject
    {
        /// <summary>
        /// The WMI class name.
        /// </summary>
        private const string ClassName = "MSBTS_Host";

        /// <summary>
        /// The WMI class name.
        /// </summary>
        private const string ConfigClassName = "MSBTS_HostSetting";

        /// <summary>
        /// The WMI class name.
        /// </summary>
        private const string InstanceClassName = "MSBTS_HostInstanceSetting";

        private string groupName;
        private bool defaultHost;
        private bool inprocess;
        private bool isolated;
        private bool authTrusted;
        private bool hostTrackingEnabled;
        private BizTalkBaseObjectCollectionEx hostInstances;
        private NameIdPairCollection hostedReceiveLocations;
        private NameIdPairCollection hostedSendPorts;
        private NameIdPairCollection hostedOrchestrations;
        private BizTalkInstallation parentInstallation;

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Host"/>
        /// </summary>
        public Host()
        {
            this.hostInstances = new BizTalkBaseObjectCollectionEx();
            this.hostedReceiveLocations = new NameIdPairCollection();
            this.hostedOrchestrations = new NameIdPairCollection();
            this.hostedSendPorts = new NameIdPairCollection();
        }

        /// <summary>
        /// Creates a new <see cref="Host"/>
        /// </summary>
        /// <param name="name"></param>
        public Host(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        [XmlIgnore]
        public BizTalkInstallation ParentInstallation
        {
            get { return this.parentInstallation; }
            set { this.parentInstallation = value; }
        }

        [XmlArrayItem("HostInstance", typeof(HostInstance))]
        public BizTalkBaseObjectCollectionEx HostInstances
        {
            get { return this.hostInstances; }
            set { this.hostInstances = value; }
        }

        public string GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }

        public bool HostTrackingEnabled
        {
            get { return this.hostTrackingEnabled; }
            set { this.hostTrackingEnabled = value; }
        }

        public bool AuthTrusted
        {
            get { return this.authTrusted; }
            set { this.authTrusted = value; }
        }

        public bool DefaultHost
        {
            get { return this.defaultHost; }
            set { this.defaultHost = value; }
        }

        public bool Inprocess
        {
            get { return this.inprocess; }
            set { this.inprocess = value; }
        }

        public bool Isolated
        {
            get { return this.isolated; }
            set { this.isolated = value; }
        }

        [XmlArrayItem("ReceiveLocation", typeof(NameIdPair))]
        public NameIdPairCollection HostedReceiveLocations
        {
            get { return this.hostedReceiveLocations; }
            set { this.hostedReceiveLocations = value; }
        }

        [XmlArrayItem("SendPort", typeof(NameIdPair))]
        public NameIdPairCollection HostedSendPorts
        {
            get { return this.hostedSendPorts; }
            set { this.hostedSendPorts = value; }
        }

        [XmlArrayItem("Orchestration", typeof(NameIdPair))]
        public NameIdPairCollection HostedOrchestrations
        {
            get { return this.hostedOrchestrations; }
            set { this.hostedOrchestrations = value; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="host"></param>
        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.Host host)
        {
            TraceManager.SmartTrace.TraceIn(explorer, host);

            this.defaultHost = host.IsDefault;
            this.groupName = host.NTGroupName;

            ManagementObject wmiHost = this.GetInstance(ConfigClassName);

            int hostType = Convert.ToInt32(wmiHost.Properties["HostType"].Value.ToString());
            this.inprocess = hostType == 1 ? true : false;
            this.isolated = hostType == 2 ? true : false;

            this.authTrusted = (bool)wmiHost.Properties["AuthTrusted"].Value;
            this.hostTrackingEnabled = (bool)wmiHost.Properties["HostTracking"].Value;

            try
            {
                string q = "SELECT * FROM MSBTS_HostInstanceSetting WHERE HostName = \"" + this.Name + "\"";
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", q);

                foreach (ManagementObject oReturn in mos.Get())
                {
                    HostInstance hi = new HostInstance();
                    hi.Name = oReturn["RunningServer"].ToString();
                    hi.HostName = oReturn["HostName"].ToString();
                    hi.Logon = oReturn["Logon"].ToString();
                    hi.Disabled = Convert.ToBoolean(oReturn["IsDisabled"].ToString());
                    hi.QualifiedName = hi.HostName + "_" + hi.Name + "_" + hi.Logon;


                    this.hostInstances.Add(hi, true);
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
        }

        /// <summary>
        /// Obtains an instance of an Host by its name.
        /// </summary>
        /// <param name="name">The name of the Host to get.</param>
        /// <returns>The requested Host.</returns>
        private ManagementObject GetInstance(string className)
        {
            // Get the host setting class
            ManagementClass hostClass = this.GetManagementClass(className);

            // Get all the host instances
            ManagementObjectCollection hosts = hostClass.GetInstances();
            foreach (ManagementObject host in hosts)
            {
                // Return the host if the name matches
                string checkName = (string)host.Properties["Name"].Value;
                if (checkName == this.Name)
                {
                    return host;
                }
            }

            // Host not found so return null
            return null;
        }

        private ManagementClass GetManagementClass(string className)
        {
            // Get the management path
            ManagementPath path = new ManagementPath();
            path.NamespacePath = Constants.NamespacePath;
            path.ClassName = className;

            // Get the host setting class
            return new ManagementClass(path);
        }
    }
}
