
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// OrchestrationRoleLink
    /// </summary>
    public class OrchestrationRoleLink : BizTalkBaseObject
    {
        private string typeName = string.Empty;
        private NameIdPair consumerRole;
        private NameIdPair providerRole;
        private NameIdPair initiatingRole;

        public OrchestrationRoleLink()
        {
        }

        public OrchestrationRoleLink(XmlNode linkNode, XmlNamespaceManager mgr)
        {
            // Basic Info
            this.Name = linkNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
            this.typeName = linkNode.SelectSingleNode("om:Property[@Name='Type']", mgr).Attributes.GetNamedItem("Value").Value;

            XmlNode initRoleNode = linkNode.SelectSingleNode("om:Property[@Name='RoleName']", mgr);

            if (initRoleNode != null)
            {
                this.initiatingRole = new NameIdPair(initRoleNode.Attributes.GetNamedItem("Value").Value, "");
            }

            // Service Link Type Info
            string shortTypeName = this.typeName.Substring(this.typeName.LastIndexOf(".") + 1);
            string serviceTypeXpath = ".//om:Element[@Type='ServiceLinkType' and om:Property[@Value='" + shortTypeName + "']]";

            XmlNode svcLinkTypeNode = linkNode.OwnerDocument.SelectSingleNode(serviceTypeXpath, mgr);

            if (svcLinkTypeNode != null)
            {
                XmlNode providerRoleNode = svcLinkTypeNode.SelectSingleNode("om:Element[@Type='RoleDeclaration' and position()=1]", mgr);
                XmlNode consumerRoleNode = svcLinkTypeNode.SelectSingleNode("om:Element[@Type='RoleDeclaration' and position()=2]", mgr);

                if (providerRoleNode != null)
                {
                    string name = providerRoleNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                    this.providerRole = new NameIdPair(name, "");
                }

                if (consumerRoleNode != null)
                {
                    string name = consumerRoleNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                    this.consumerRole = new NameIdPair(name, "");
                }
            }

            return;
        }

        #region Public properties

        public string TypeName
        {
            get { return this.typeName; }
            set { this.typeName = value; }
        }

        [XmlElement("ConsumerRole", typeof(NameIdPair))]
        public NameIdPair ConsumerRole
        {
            get { return this.consumerRole; }
            set { this.consumerRole = value; }
        }

        [XmlElement("ProviderRole", typeof(NameIdPair))]
        public NameIdPair ProviderRole
        {
            get { return this.providerRole; }
            set { this.providerRole = value; }
        }

        [XmlElement("InitiatingRole", typeof(NameIdPair))]
        public NameIdPair InitiatingRole
        {
            get { return this.initiatingRole; }
            set { this.initiatingRole = value; }
        }

        #endregion
    }
}
