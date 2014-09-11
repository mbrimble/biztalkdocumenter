
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;


namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.BizTalk.SSOClient.Interop;
    using Microsoft.Win32;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;
    using System.Collections.Generic;

    /// <summary>
    /// BizTalkInstallation.
    /// </summary>
    public class BizTalkInstallation : BizTalkBaseObject
    {
        public const string ConnectionStringFormat = "Server={0};Database={1};Integrated Security=SSPI";

        private BizTalkCore.BtsCatalogExplorer explorer;

        private string server;
        private string mgmtDatabaseName;
        private string rulesServer = string.Empty;
        private string rulesDatabase = string.Empty;

        private BizTalkBaseObjectCollectionEx applications;

        private BizTalkBaseObjectCollectionEx hosts;
        private BizTalkBaseObjectCollectionEx parties;
        private BizTalkBaseObjectCollectionEx protocolTypes;

        private BizTalkBaseObjectCollectionEx schemas;
        private BizTalkBaseObjectCollectionEx assemblies;
        private BizTalkBaseObjectCollectionEx pipelines;
        private BizTalkBaseObjectCollectionEx sendPorts;
        private BizTalkBaseObjectCollectionEx sendPortGroups;
        private BizTalkBaseObjectCollectionEx receivePorts;
        private BizTalkBaseObjectCollectionEx maps;
        private BizTalkBaseObjectCollectionEx orchestrations;
        private BizTalkBaseObjectCollectionEx roles;

        private ArrayList requestedApplications;

        #region Constructors

        public BizTalkInstallation()
        {
            this.applications = new BizTalkBaseObjectCollectionEx();
            this.protocolTypes = new BizTalkBaseObjectCollectionEx();
            this.hosts = new BizTalkBaseObjectCollectionEx();
            this.parties = new BizTalkBaseObjectCollectionEx();

            this.schemas = new BizTalkBaseObjectCollectionEx();
            this.assemblies = new BizTalkBaseObjectCollectionEx();
            this.pipelines = new BizTalkBaseObjectCollectionEx();
            this.sendPorts = new BizTalkBaseObjectCollectionEx();
            this.sendPortGroups = new BizTalkBaseObjectCollectionEx();
            this.receivePorts = new BizTalkBaseObjectCollectionEx();
            this.maps = new BizTalkBaseObjectCollectionEx();
            this.orchestrations = new BizTalkBaseObjectCollectionEx();
            this.roles = new BizTalkBaseObjectCollectionEx();
        }

        #endregion

        #region Public Properties

        public string Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        public string MgmtDatabaseName
        {
            get { return this.mgmtDatabaseName; }
            set { this.mgmtDatabaseName = value; }
        }

        public string RulesServer
        {
            get { return this.rulesServer; }
            set { this.rulesServer = value; }
        }

        public string RulesDatabase
        {
            get { return this.rulesDatabase; }
            set { this.rulesDatabase = value; }
        }

        [XmlArrayItem("Protocol", typeof(Protocol))]
        public BizTalkBaseObjectCollectionEx ProtocolTypes
        {
            get { return this.protocolTypes; }
        }

        [XmlArrayItem("Application", typeof(BizTalkApplication))]
        public BizTalkBaseObjectCollectionEx Applications
        {
            get { return this.applications; }
        }

        [XmlArrayItem("Host", typeof(Host))]
        public BizTalkBaseObjectCollectionEx Hosts
        {
            get { return this.hosts; }
        }

        [XmlArrayItem("Party", typeof(Party))]
        public BizTalkBaseObjectCollectionEx Parties
        {
            get { return this.parties; }
        }

        public BizTalkBaseObjectCollectionEx Schemas
        {
            get { return this.schemas; }
        }

        public BizTalkBaseObjectCollectionEx Assemblies
        {
            get { return this.assemblies; }
        }

        public BizTalkBaseObjectCollectionEx Pipelines
        {
            get { return this.pipelines; }
        }

        public BizTalkBaseObjectCollectionEx SendPorts
        {
            get { return this.sendPorts; }
        }

        public BizTalkBaseObjectCollectionEx SendPortGroups
        {
            get { return this.sendPortGroups; }
        }

        public BizTalkBaseObjectCollectionEx ReceivePorts
        {
            get { return this.receivePorts; }
        }

        public BizTalkBaseObjectCollectionEx Maps
        {
            get { return this.maps; }
        }

        public BizTalkBaseObjectCollectionEx Orchestrations
        {
            get { return this.orchestrations; }
        }

        public BizTalkBaseObjectCollectionEx Roles
        {
            get { return this.roles; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveToFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BizTalkInstallation));

            StreamWriter sw = new StreamWriter(fileName);
            xmlSerializer.Serialize(sw, this);
            sw.Close();
        }

        public ArrayList ListDeployedAssemblies()
        {
            ArrayList assemblyNames = new ArrayList();

            this.InitExplorer(this.server, this.mgmtDatabaseName);

            foreach (BizTalkCore.BtsAssembly btsAssembly in this.explorer.Assemblies)
            {
                if (!btsAssembly.IsSystem)
                {
                    BizTalkAssembly bta = new BizTalkAssembly(btsAssembly);
                    assemblyNames.Add(btsAssembly.DisplayName);
                }
            }
            return assemblyNames;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList GetApplicationNames()
        {
            ArrayList names = new ArrayList();

            this.InitExplorer(this.server, this.mgmtDatabaseName);
            foreach (BizTalkCore.Application application in this.explorer.Applications)
            {
                names.Add(application.Name);
            }

            return names;
        }

        public ArrayList GetOrchestrationNames()
        {
            TraceManager.SmartTrace.TraceIn();
            ArrayList names = new ArrayList();

            this.InitExplorer(this.server, this.mgmtDatabaseName);

            foreach (BizTalkCore.BtsAssembly asm in this.explorer.Assemblies)
            {
                foreach (BizTalk.ExplorerOM.BtsOrchestration orch in asm.Orchestrations)
                {
                    names.Add(string.Format("{0}|{1}", orch.BtsAssembly.DisplayName, orch.FullName));
                }
            }

            TraceManager.SmartTrace.TraceOut();
            return names;
        }

        public Orchestration GetOrchestration(string assemblyName, string orchestrationName)
        {
            Orchestration orch = null;

            try
            {
                this.InitExplorer(this.server, this.mgmtDatabaseName);

                BizTalkCore.BtsAssembly asm = null;

                foreach (BizTalk.ExplorerOM.BtsAssembly a in this.explorer.Assemblies)
                {
                    if (a.DisplayName == assemblyName)
                    {
                        asm = a;
                        break;
                    }
                }

                BizTalk.ExplorerOM.BtsOrchestration o = asm.Orchestrations[orchestrationName];

                orch = new Orchestration(o.FullName);
                orch.Load(this.explorer, o);
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            return orch;
        }

        #region LoadConfig

        public void LoadConfig(ArrayList requestedApplications, bool includeReferencedApplications)
        {
            //Updated by Colin Dijkgraaf 4/2/2014
            if (includeReferencedApplications)
            {

                Dictionary<string, string> requestedAppsHash = new Dictionary<string, string>();

                ArrayList referencedApplications = new ArrayList();



                this.InitExplorer(this.server, this.mgmtDatabaseName);



                // Create a Hash Table

                foreach (string requestedApplication in requestedApplications)
                {

                    requestedAppsHash.Add(requestedApplication, requestedApplication);

                    referencedApplications.Add(requestedApplication);

                }



                // Check to see if referenced applications included

                foreach (string requestedApplication in requestedApplications)
                {

                    foreach (BizTalkCore.Application application in this.explorer.Applications[requestedApplication].References)
                    {

                        if (!requestedAppsHash.ContainsKey(application.Name))
                        {

                            requestedAppsHash.Add(application.Name, application.Name);

                            referencedApplications.Add(application.Name);

                        }

                    }

                }



                // If we have new references

                if (referencedApplications.Count > requestedApplications.Count)
                {

                    requestedApplications = referencedApplications;

                    // Check for recursive references

                    LoadConfig(requestedApplications, includeReferencedApplications);

                }

                else
                {

                    this.requestedApplications = requestedApplications;

                    this.LoadConfig();

                }

            }

            else
            {

                this.requestedApplications = requestedApplications;

                this.LoadConfig();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadConfig()
        {
            this.LoadConfigInternal(LoadOption.Complete);
        }

        #endregion

        #region OM Event Hanlders

        private void RoleLinks_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.roles.Add(obj);
        }

        private void SendPorts_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.sendPorts.Add(obj);
        }

        private void SendPortGroups_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.sendPortGroups.Add(obj);
        }

        private void Schemas_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.schemas.Add(obj);
        }

        private void ReceivePorts_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.receivePorts.Add(obj);
        }

        private void Pipelines_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.pipelines.Add(obj);
        }

        private void Orchestrations_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.orchestrations.Add(obj);
        }

        private void Maps_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.maps.Add(obj);
        }

        private void Assemblies_OnObjectAdded(BizTalkBaseObject obj)
        {
            this.assemblies.Add(obj);
        }

        #endregion

        #region CreateFilterGroups

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterXml"></param>
        /// <returns></returns>
        internal static BizTalkBaseObjectCollectionEx CreateFilterGroups(string filterXml)
        {
            BizTalkBaseObjectCollectionEx groups = new BizTalkBaseObjectCollectionEx();

            XmlDocument filterDoc = new XmlDocument();
            //CD 20140407
            try 
            {
                filterDoc.LoadXml(filterXml);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format(
                    @"The Filter XML is malformed (quite often caused by the binding being reformatted and putting whitespace in front of the filter XML): 
                      XML='{0}' ",
                    filterXml));
            }

            foreach (XmlNode groupNode in filterDoc.SelectNodes("./Filter/Group"))
            {
                FilterGroup fg = new FilterGroup();
                // Added so that it actually adds it to the list - CD 20140408
                fg.Name = "Filter Group";

                foreach (XmlNode statementNode in groupNode.SelectNodes("./Statement"))
                {
                    Filter f = new Filter();
                    f.Property = statementNode.Attributes.GetNamedItem("Property").Value.ToString();
                    f.FilterOperator = (FilterOperator)Enum.Parse(typeof(FilterOperator), statementNode.Attributes.GetNamedItem("Operator").Value.ToString());
                    // Added so that it actually adds it to the list - CD 20140408
                    f.Name = "Filter";

                    XmlNode valueNode = statementNode.Attributes.GetNamedItem("Value");

                    if (valueNode != null)
                    {
                        f.Value = valueNode.Value.ToString();
                    }
                    // Changed to ignore duplicate name - CD 20140408
                    fg.Filter.Add(f, true);
                }
                // Changed to ignore duplicate name - CD 20140408
                groups.Add(fg, true);
            }

            return groups;
        }

        #endregion

        #region LoadConfigInternal

        /// <summary>
        /// LoadConfigInternal
        /// </summary>
        private void LoadConfigInternal(LoadOption loadOption)
        {
            this.InitExplorer(this.server, this.mgmtDatabaseName);

            this.DocumentHosts();
            this.DocumentParties();
            this.DocumentProtocolTypes();
            this.DocumentApplications(loadOption);

            this.FixReferences(this.explorer);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void DocumentApplications(LoadOption loadOption)
        {
            this.applications.Clear();

            foreach (BizTalkCore.Application application in this.explorer.Applications)
            {
                if (this.requestedApplications == null ||
                    this.requestedApplications.Count == 0 ||
                    this.Contains(this.requestedApplications, application.Name, true))
                //this.requestedApplications.Contains(application.Name))
                {
                    BizTalkApplication app = new BizTalkApplication(application.Name);
                    app.Assemblies.OnObjectAdded += new ObjectAddedEvent(Assemblies_OnObjectAdded);
                    app.Maps.OnObjectAdded += new ObjectAddedEvent(Maps_OnObjectAdded);
                    app.Orchestrations.OnObjectAdded += new ObjectAddedEvent(Orchestrations_OnObjectAdded);
                    app.Pipelines.OnObjectAdded += new ObjectAddedEvent(Pipelines_OnObjectAdded);
                    app.ReceivePorts.OnObjectAdded += new ObjectAddedEvent(ReceivePorts_OnObjectAdded);
                    app.Schemas.OnObjectAdded += new ObjectAddedEvent(Schemas_OnObjectAdded);
                    app.SendPortGroups.OnObjectAdded += new ObjectAddedEvent(SendPortGroups_OnObjectAdded);
                    app.SendPorts.OnObjectAdded += new ObjectAddedEvent(SendPorts_OnObjectAdded);
                    app.RoleLinks.OnObjectAdded += new ObjectAddedEvent(RoleLinks_OnObjectAdded);

                    app.ParentInstallation = this;
                    app.Load(this.explorer, application);
                    this.applications.Add(app);
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private void DocumentHosts()
        {
            foreach (BizTalkCore.Host host in this.explorer.Hosts)
            {
                Host h = new Host(host.Name);
                h.ParentInstallation = this;
                h.Load(this.explorer, host);
                this.hosts.Add(h);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DocumentProtocolTypes()
        {
            foreach (BizTalkCore.ProtocolType protocolType in this.explorer.ProtocolTypes)
            {
                Protocol protocol = new Protocol(protocolType.Name);
                protocol.ParentInstallation = this;
                protocol.Load(this.explorer, protocolType);
                this.protocolTypes.Add(protocol);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DocumentParties()
        {
            foreach (BizTalkCore.Party party in this.explorer.Parties)
            {
                Party p = new Party(party.Name);
                p.ParentInstallation = this;
                p.Load(this.explorer, party);
                this.parties.Add(p);
            }
        }

        #region InitExplorer

        private void InitExplorer()
        {
            RegistryKey bizTalkAdminKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration");

            string database = (string)bizTalkAdminKey.GetValue("MgmtDBName", "BizTalkMgmtDb");
            string server = (string)bizTalkAdminKey.GetValue("MgmtDBServer", Environment.MachineName);

            bizTalkAdminKey.Close();

            this.InitExplorer(server, database);
        }

        private void InitExplorer(string serverName, string databaseName)
        {
            TraceManager.SmartTrace.TraceIn();
            explorer = new BizTalkCore.BtsCatalogExplorer();
            explorer.ConnectionString = string.Format(ConnectionStringFormat, serverName, databaseName);
            CheckSSOPermissions();
            TraceManager.SmartTrace.TraceOut();
        }

        #endregion

        #region CheckSSOPermissions

        /// <summary>
        /// Check to see if the current user has SSO Administrator permissions.
        /// </summary>
        /// <returns>
        /// True if the current user has SSO Administrator permissions, or
        /// false otherwise.
        /// </returns>
        private static void CheckSSOPermissions()
        {
            // Try and get the global info from SSO
            try
            {
                int flags;
                int auditAppDeleteMax;
                int auditMappingDeleteMax;
                int auditNtpLookupMax;
                int auditXpLookupMax;
                int ticketTimeout;
                int credCacheTimeout;
                string secretServer;
                string SSOAdminGroup;
                string affiliateAppMgrGroup;
                ISSOAdmin admin = new ISSOAdmin();
               // Microsoft.BizTalk.SSOClient.Interop.ISSOAdmin admin = new Microsoft.BizTalk.SSOClient.Interop.ISSOAdmin();
                admin.GetGlobalInfo(
                    out flags,
                    out auditAppDeleteMax,
                    out auditMappingDeleteMax,
                    out auditNtpLookupMax,
                    out auditXpLookupMax,
                    out ticketTimeout,
                    out credCacheTimeout,
                    out secretServer,
                    out SSOAdminGroup,
                    out affiliateAppMgrGroup);
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceManager.SmartTrace.TraceError(ex);

                // Failed so we don't have the correct permissions
                throw new ApplicationException(string.Format(
                    @"The current user '\\{0}\{1}' failed to connect to the BizTalk management database.",
                    Environment.UserDomainName,
                    Environment.UserName));
            }
        }

        #endregion

        #region FixReferences

        /// <summary>
        /// 
        /// </summary>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            foreach (Host host in this.hosts)
            {
                host.FixReferences(explorer);
            }
            foreach (Protocol protocol in this.protocolTypes)
            {
                protocol.FixReferences(explorer);
            }
            foreach (Party party in this.parties)
            {
                party.FixReferences(explorer);
            }
            foreach (BizTalkApplication application in this.applications)
            {
                application.FixReferences(explorer);
            }
        }

        #endregion

        /// <summary>
        /// Util method to do a case insensitive comparison
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool Contains(ArrayList source, string target, bool caseInsensitive)
        {
            bool result = false;

            foreach (string token in source)
            {
                if (caseInsensitive)
                {
                    if (String.Compare(token, target, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                else
                {
                    if (String.Compare(token, target) == 0)
                    {
                        result = true;
                        break;
                    }

                }

            }

            return result;
        }
    }
}
