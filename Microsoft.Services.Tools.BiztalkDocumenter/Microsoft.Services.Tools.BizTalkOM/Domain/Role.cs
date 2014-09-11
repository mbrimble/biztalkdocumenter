
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

	/// <summary>
	/// Summary description for Alias.
	/// </summary>
	public class Role : BizTalkBaseObject
	{
        private string serviceLinkType;
        private NameIdPair parentAssembly;
        private NameIdPairCollection enlistedParties;

        //private string qualifier;
        //private string value;

        /// <summary>
        /// 
        /// </summary>
        public Role()
		{
            this.enlistedParties = new NameIdPairCollection();
        }

        /// <summary>
        /// Creates a new <see cref="Role"/>
        /// </summary>
        public Role(string name) : this()
        {
            this.Name = name;
        }

        #region Public Properties

        public NameIdPair ParentAssembly
        {
            get { return this.parentAssembly; }
            set { this.parentAssembly = value; }
        }

        public string ServiceLinkType
        {
            get { return this.serviceLinkType; }
            set { this.serviceLinkType = value; }
        }

        [XmlArrayItem("Party", typeof(NameIdPair))]
        public NameIdPairCollection EnlistedParties
        {
            get { return this.enlistedParties; }
            set { this.enlistedParties = value; }
        }

        #endregion
        
        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.Role role)
        {
            if (role != null)
            {
                this.QualifiedName = String.Empty;
                this.serviceLinkType = role.ServiceLinkType;
                this.parentAssembly = new NameIdPair(role.BtsAssembly.Name, "");

                foreach (BizTalkCore.EnlistedParty party in role.EnlistedParties)
                {
                    this.enlistedParties.Add(new NameIdPair(party.Party.Name, ""));
                }
            }

            return;
        }

        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            foreach (NameIdPair enlistedParty in this.enlistedParties)
            {
                Party party = this.Application.ParentInstallation.Parties[enlistedParty.Name] as Party;
                enlistedParty.Id = party.Id;
                party.LinkedRoles.Add(this.NameIdPair);                
            }

            BizTalkAssembly asm = this.Application.ParentInstallation.Assemblies[parentAssembly.Name] as BizTalkAssembly;

            if (asm != null)
            {
                this.parentAssembly = asm.NameIdPair;
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
