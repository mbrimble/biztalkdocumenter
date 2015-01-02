
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for Party.
    /// </summary>
    public class Party : BizTalkBaseObject
    {
        private EncryptionCert signatureCert;
        private string data;
        private bool dataSpecified = false;
        private BizTalkBaseObjectCollectionEx aliases;
        private BizTalkInstallation parentInstallation;
        private NameIdPairCollection roleLinks;
        private NameIdPairCollection sendPorts;

        /// <summary>
        /// 
        /// </summary>
        public Party()
        {
            this.aliases = new BizTalkBaseObjectCollectionEx();
            this.roleLinks = new NameIdPairCollection();
            this.sendPorts = new NameIdPairCollection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Party(string name)
            : this()
        {
            this.Name = name;
        }

        #region Public Properties

        [XmlIgnore]
        public BizTalkInstallation ParentInstallation
        {
            get { return this.parentInstallation; }
            set { this.parentInstallation = value; }
        }

        [XmlElement("CustomData")]
        public string Data
        {
            get { return this.data; }
            set { this.data = value; this.dataSpecified = true; }
        }

        [XmlIgnore]
        public bool DataSpecified
        {
            get { return this.dataSpecified; }
            set { this.dataSpecified = value; }
        }

        public EncryptionCert SignatureCert
        {
            get { return this.signatureCert; }
            set { this.signatureCert = value; }
        }

        [XmlArrayItem("Alias", typeof(Alias))]
        public BizTalkBaseObjectCollectionEx Aliases
        {
            get { return this.aliases; }
            set { this.aliases = value; }
        }

        [XmlArrayItem("LinkedRole", typeof(NameIdPair))]
        public NameIdPairCollection LinkedRoles
        {
            get { return this.roleLinks; }
            set { this.roleLinks = value; }
        }

        [XmlArrayItem("SendPort", typeof(NameIdPair))]
        public NameIdPairCollection SendPorts
        {
            get { return this.sendPorts; }
            set { this.sendPorts = value; }
        }

        #endregion

        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.Party party)
        {
            if (party != null)
            {
                this.QualifiedName = String.Empty;
                this.aliases.Clear();
                this.sendPorts.Clear();

                this.Data = party.CustomData;
                this.signatureCert = new EncryptionCert(party.SignatureCert);

                if (party.Aliases != null)
                {
                    foreach (BizTalkCore.PartyAlias alias in party.Aliases)
                    {
                        Alias a = new Alias();
                        // 2015/1/1 MTB Added try catch to handle exceptions bubbled up from  BizTalk object model
                        try
                        {
                            a.Name = alias.Name;
                        }
                        catch (InvalidCastException e)
                        {
                            a.Name = "Not Valued";
                        }

                        a.IsAutoCreated = alias.IsAutoCreated;
                        a.Qualifier = alias.Qualifier;
                        a.Value = alias.Value;
                        a.QualifiedName = a.Name + "_" + a.Qualifier + "_" + a.Value;
                        this.aliases.Add(a);
                    }
                }

                if (party.SendPorts != null)
                {
                    foreach (BizTalkCore.SendPort sp in party.SendPorts)
                    {
                        this.sendPorts.Add(new NameIdPair(sp.Name, ""));
                    }
                }
            }

            return;
        }

        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            foreach (NameIdPair sendPort in this.sendPorts)
            {
                SendPort sp = this.parentInstallation.SendPorts[sendPort.Name] as SendPort;

                if (sp != null)
                {
                    sendPort.Id = sp.NameIdPair.Id;
                }
            }

            return;
        }
    }
}
