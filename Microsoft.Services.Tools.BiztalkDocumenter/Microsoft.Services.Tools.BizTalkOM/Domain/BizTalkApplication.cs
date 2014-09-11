
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml.Serialization;
    using Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class BizTalkApplication : BizTalkBaseObject
    {
        private string name;
        private BizTalkInstallation parentInstallation;

        private BizTalkBaseObjectCollectionEx schemas;
        private BizTalkBaseObjectCollectionEx assemblies;
        private BizTalkBaseObjectCollectionEx pipelines;
        private BizTalkBaseObjectCollectionEx sendPorts;
        private BizTalkBaseObjectCollectionEx sendPortGroups;
        private BizTalkBaseObjectCollectionEx receivePorts;
        private BizTalkBaseObjectCollectionEx maps;
        private BizTalkBaseObjectCollectionEx orchestrations;
        private BizTalkBaseObjectCollectionEx roleLinks;

        private NameIdPairCollection referencedApplications;
        private NameIdPairCollection backReferencedApplications;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BizTalkApplication()
        {
            this.schemas = new BizTalkBaseObjectCollectionEx();
            this.assemblies = new BizTalkBaseObjectCollectionEx();
            this.pipelines = new BizTalkBaseObjectCollectionEx();
            this.sendPorts = new BizTalkBaseObjectCollectionEx();
            this.sendPortGroups = new BizTalkBaseObjectCollectionEx();
            this.receivePorts = new BizTalkBaseObjectCollectionEx();
            this.maps = new BizTalkBaseObjectCollectionEx();
            this.orchestrations = new BizTalkBaseObjectCollectionEx();
            this.referencedApplications = new NameIdPairCollection();
            this.backReferencedApplications = new NameIdPairCollection();
            this.roleLinks = new BizTalkBaseObjectCollectionEx();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationName"></param>
        public BizTalkApplication(string applicationName)
            : this()
        {
            this.Name = applicationName;
            this.name = applicationName;
        }

        #endregion

        #region Public Properties

        [XmlIgnore]
        public BizTalkInstallation ParentInstallation
        {
            get { return this.parentInstallation; }
            set { this.parentInstallation = value; }
        }

        [XmlArrayItem("Application", typeof(NameIdPair))]
        public NameIdPairCollection ReferencedApplications
        {
            get { return this.referencedApplications; }
            set { this.referencedApplications = value; }
        }

        [XmlArrayItem("Application", typeof(NameIdPair))]
        public NameIdPairCollection BackReferencedApplications
        {
            get { return this.backReferencedApplications; }
            set { this.backReferencedApplications = value; }
        }

        [XmlArrayItem("Schema", typeof(Schema))]
        public BizTalkBaseObjectCollectionEx Schemas
        {
            get { return this.schemas; }
            set { this.schemas = value; }
        }

        [XmlArrayItem("Assembly", typeof(BizTalkAssembly))]
        public BizTalkBaseObjectCollectionEx Assemblies
        {
            get { return this.assemblies; }
            set { this.assemblies = value; }
        }

        [XmlArrayItem("Pipeline", typeof(Pipeline))]
        public BizTalkBaseObjectCollectionEx Pipelines
        {
            get { return this.pipelines; }
            set { this.pipelines = value; }
        }

        [XmlArrayItem("SendPort", typeof(SendPort))]
        public BizTalkBaseObjectCollectionEx SendPorts
        {
            get { return this.sendPorts; }
            set { this.sendPorts = value; }
        }

        [XmlArrayItem("SendPortGroup", typeof(SendPortGroup))]
        public BizTalkBaseObjectCollectionEx SendPortGroups
        {
            get { return this.sendPortGroups; }
            set { this.sendPortGroups = value; }
        }

        [XmlArrayItem("ReceivePort", typeof(ReceivePort))]
        public BizTalkBaseObjectCollectionEx ReceivePorts
        {
            get { return this.receivePorts; }
            set { this.receivePorts = value; }
        }

        [XmlArrayItem("Map", typeof(Transform))]
        public BizTalkBaseObjectCollectionEx Maps
        {
            get { return this.maps; }
            set { this.maps = value; }
        }

        [XmlArrayItem("Orchestration", typeof(Orchestration))]
        public BizTalkBaseObjectCollectionEx Orchestrations
        {
            get { return this.orchestrations; }
            set { this.orchestrations = value; }
        }

        [XmlArrayItem("Role", typeof(Role))]
        public BizTalkBaseObjectCollectionEx RoleLinks
        {
            get { return this.roleLinks; }
            set { this.roleLinks = value; }
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears all artifacts from this instance
        /// </summary>
        public void Clear()
        {
            this.schemas.Clear();
            this.assemblies.Clear();
            this.pipelines.Clear();
            this.sendPorts.Clear();
            this.sendPortGroups.Clear();
            this.receivePorts.Clear();
            this.maps.Clear();
            this.orchestrations.Clear();
            this.roleLinks.Clear();
            this.referencedApplications.Clear();
            this.backReferencedApplications.Clear();
        }

        #endregion

        #region Load

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        public void Load(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();


            this.name = application.Name;

            TraceManager.SmartTrace.TraceInfo("Processing Application  : " + this.Name);

            // Referenced Applications
            LoadReferences(application);

            LoadBackReferences(application);

            LoadPipelines(explorer, application);

            LoadSchemas(explorer, application);

            LoadMaps(application);

            LoadOrchestrations(explorer, application);

            LoadReceivePorts(explorer, application);

            LoadSendPorts(explorer, application);

            LoadSendPortGroups(explorer, application);

            LoadAssemblies(explorer, application);

            LoadRoleLinks(explorer, application);

            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadReferences(Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            foreach (BizTalk.ExplorerOM.Application app in application.References)
            {
                this.referencedApplications.Add(new NameIdPair(app.Name, ""));
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadBackReferences(Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Back Referenced Applications
            foreach (BizTalk.ExplorerOM.Application app in application.BackReferences)
            {
                this.backReferencedApplications.Add(new NameIdPair(app.Name, ""));
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadPipelines(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Pipelines
            foreach (BizTalk.ExplorerOM.Pipeline pipeline in application.Pipelines)
            {
                Pipeline p = new Pipeline(pipeline.FullName);
                p.Application = this;
                p.Load(explorer, pipeline);
                this.Pipelines.Add(p);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadSchemas(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Schema
            foreach (BizTalk.ExplorerOM.Schema schema in application.Schemas)
            {
                Schema s = new Schema(schema.FullName);
                s.Application = this;
                s.Load(explorer, schema);
                this.Schemas.Add(s);
                //if (this.Schemas[schema.FullName] != null)
                //{
                //    Schema s = new Schema(schema.FullName);
                //    s.Application = this;
                //    s.Load(explorer, schema);
                //    this.Schemas.Add(s);
                //}
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadMaps(Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Maps
            foreach (BizTalk.ExplorerOM.Transform transform in application.Transforms)
            {
                Transform t = new Transform(transform);
                t.Application = this;
                this.Maps.Add(t);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadOrchestrations(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Orchestrations
            foreach (BizTalk.ExplorerOM.BtsOrchestration orchestration in application.Orchestrations)
            {
                TraceManager.SmartTrace.TraceInfo(orchestration.FullName);
                Orchestration o = new Orchestration(orchestration.FullName);
                o.Application = this;
                o.Load(explorer, orchestration);
                this.orchestrations.Add(o);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadReceivePorts(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Receive Ports
            foreach (BizTalk.ExplorerOM.ReceivePort receivePort in application.ReceivePorts)
            {
                ReceivePort rp = new ReceivePort(receivePort.Name);
                rp.Application = this;
                rp.Load(explorer, receivePort);
                this.ReceivePorts.Add(rp);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadSendPorts(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Send Ports
            foreach (BizTalk.ExplorerOM.SendPort sendPort in application.SendPorts)
            {
                SendPort sp = new SendPort(sendPort.Name);
                sp.Application = this;
                sp.Load(explorer, sendPort);
                this.sendPorts.Add(sp);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadSendPortGroups(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Send Port Groups
            foreach (BizTalk.ExplorerOM.SendPortGroup sendPortGroup in application.SendPortGroups)
            {
                SendPortGroup spg = new SendPortGroup(sendPortGroup.Name);
                spg.Application = this;
                spg.Load(explorer, sendPortGroup);
                this.SendPortGroups.Add(spg);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadRoleLinks(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Role Links
            foreach (BizTalk.ExplorerOM.Role role in application.Roles)
            {
                Role r = new Role(role.Name);
                r.Application = this;
                r.Load(explorer, role);
                this.roleLinks.Add(r);
            }
            TraceManager.SmartTrace.TraceOut();
        }

        private void LoadAssemblies(BtsCatalogExplorer explorer, Application application)
        {
            TraceManager.SmartTrace.TraceIn();
            // Assemblies - need to check for maps, schema, pipelines, orchestrations
            foreach (BizTalk.ExplorerOM.BtsAssembly assembly in application.Assemblies)
            {
                try
                {
                    BizTalkAssembly a = new BizTalkAssembly(assembly);
                    a.Application = this;
                    a.Load(explorer, assembly);
                    this.Assemblies.Add(a);
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
            TraceManager.SmartTrace.TraceOut();
        }

        #endregion

        #region FixReferences

        /// <summary>
        /// 
        /// </summary>
        internal override void FixReferences(BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            // Referenced Applications
            foreach (NameIdPair app in this.referencedApplications)
            {
                BizTalkApplication a = this.parentInstallation.Applications[app.Name] as BizTalkApplication;

                if (a != null)
                {
                    app.Id = a.NameIdPair.Id;
                }
            }

            // Back Referenced Applications
            foreach (NameIdPair app in this.backReferencedApplications)
            {
                BizTalkApplication a = this.parentInstallation.Applications[app.Name] as BizTalkApplication;

                if (a != null)
                {
                    app.Id = a.NameIdPair.Id;
                }
            }

            foreach (Transform transform in this.maps)
            {
                transform.FixReferences(explorer);
            }
            foreach (SendPort sendPort in this.sendPorts)
            {
                sendPort.FixReferences(explorer);
            }
            foreach (SendPortGroup sendPortGroup in this.sendPortGroups)
            {
                sendPortGroup.FixReferences(explorer);
            }
            foreach (ReceivePort receivePort in this.receivePorts)
            {
                receivePort.FixReferences(explorer);
            }
            foreach (Schema schema in this.schemas)
            {
                schema.FixReferences(explorer);
            }
            foreach (Orchestration orchestration in this.orchestrations)
            {
                orchestration.FixReferences(explorer);
            }
            foreach (BizTalkAssembly assembly in this.assemblies)
            {
                assembly.FixReferences(explorer);
            }
            foreach (Role roleLink in this.roleLinks)
            {
                roleLink.FixReferences(explorer);
            }

            TraceManager.SmartTrace.TraceOut();
        }

        #endregion
    }
}
