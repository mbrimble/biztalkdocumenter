
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for SendPortGroup.
    /// </summary>
    public class SendPortGroup : BizTalkBaseObject
    {
        private string status;
        private string filter;
        private BizTalkBaseObjectCollectionEx filterGroups;
        private NameIdPairCollection sendPorts;
        private NameIdPairCollection boundOrchestrations;

        public SendPortGroup()
        {
            sendPorts = new NameIdPairCollection();
            this.filterGroups = new BizTalkBaseObjectCollectionEx();
            this.boundOrchestrations = new NameIdPairCollection();
        }

        public SendPortGroup(string name)
            : this()
        {
            this.Name = name;
        }

        [XmlArrayItem("SendPort", typeof(NameIdPair))]
        public NameIdPairCollection SendPorts
        {
            get { return this.sendPorts; }
        }

        [XmlArrayItem("FilterGroup", typeof(FilterGroup))]
        public BizTalkBaseObjectCollectionEx FilterGroups
        {
            get { return this.filterGroups; }
            set { this.filterGroups = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Orchestration", typeof(NameIdPair))]
        public NameIdPairCollection BoundOrchestrations
        {
            get { return this.boundOrchestrations; }
            set { this.boundOrchestrations = value; }
        }

        public string Filter
        {
            get { return this.filter; }
            set { this.filter = value; }
        }

        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.SendPortGroup group)
        {
            if (group != null)
            {
                this.QualifiedName = String.Empty;
                this.status = group.Status.ToString();
                this.CustomDescription = group.Description;
                this.ApplicationName = group.Application.Name;

                // Filters
                if (group.Filter != string.Empty)
                {
                    this.FilterGroups = BizTalkInstallation.CreateFilterGroups(group.Filter);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            BizTalkCore.SendPortGroup group = explorer.SendPortGroups[this.Name];

            if (group != null)
            {
                // Send ports
                foreach (BizTalkCore.SendPort sendPort in group.SendPorts)
                {
                    SendPort sp = this.Application.SendPorts[sendPort.Name] as SendPort;

                    if (sp != null)
                    {
                        sp.SendPortGroups.Add(this.NameIdPair);
                        this.sendPorts.Add(sp.NameIdPair);
                    }
                }
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
