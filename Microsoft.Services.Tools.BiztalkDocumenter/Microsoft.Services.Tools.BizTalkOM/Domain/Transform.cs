
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for Transform.
    /// </summary>
    public sealed class Transform : BizTalkBaseObject
    {
        private NameIdPair targetSchema;
        private NameIdPair sourceSchema;
        private string xsltSource;
        private NameIdPairCollection sendPorts;
        private NameIdPairCollection receivePorts;
        private NameIdPairCollection orchestrations;
        private NameIdPair parentAssembly;

        public Transform()
        {
            this.sendPorts = new NameIdPairCollection();
            this.receivePorts = new NameIdPairCollection();
            this.orchestrations = new NameIdPairCollection();
        }

        public Transform(string name)
            : this()
        {
            this.Name = name;
        }

        public Transform(BizTalkCore.Transform tfm)
            : this()
        {
            this.Name = tfm.FullName;
            this.QualifiedName = tfm.AssemblyQualifiedName;
            this.AssemblyName = tfm.AssemblyQualifiedName;
            this.SourceSchema = new NameIdPair(tfm.SourceSchema.FullName, "");
            this.TargetSchema = new NameIdPair(tfm.TargetSchema.FullName, "");
            this.CustomDescription = tfm.Description;
            this.ApplicationName = tfm.Application.Name;
            this.XsltSource = tfm.XmlContent;
        }

        public NameIdPair ParentAssembly
        {
            get { return this.parentAssembly; }
            set { this.parentAssembly = value; }
        }

        public NameIdPair TargetSchema
        {
            get { return this.targetSchema; }
            set { this.targetSchema = value; }
        }

        public NameIdPair SourceSchema
        {
            get { return this.sourceSchema; }
            set { this.sourceSchema = value; }
        }

        [XmlArrayItem("SendPort", typeof(NameIdPair))]
        public NameIdPairCollection SendPorts
        {
            get { return this.sendPorts; }
            set { this.sendPorts = value; }
        }

        [XmlArrayItem("ReceivePort", typeof(NameIdPair))]
        public NameIdPairCollection ReceivePorts
        {
            get { return this.receivePorts; }
            set { this.receivePorts = value; }
        }

        [XmlArrayItem("Orchestration", typeof(NameIdPair))]
        public NameIdPairCollection Orchestrations
        {
            get { return this.orchestrations; }
            set { this.orchestrations = value; }
        }

        [XmlIgnore()]
        public string XsltSource
        {
            get { return this.xsltSource; }
            set
            {
                this.xsltSource = value;
                this.xsltSource = this.xsltSource.Replace("utf-16", "utf-8");
                this.xsltSource = this.xsltSource.Replace("UTF-16", "UTF-8");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            Schema s = this.Application.ParentInstallation.Schemas[this.sourceSchema.Name] as Schema;

            if (s != null)
            {
                this.sourceSchema = s.NameIdPair;
            }

            s = this.Application.ParentInstallation.Schemas[this.targetSchema.Name] as Schema;

            if (s != null)
            {
                this.targetSchema = s.NameIdPair;
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
