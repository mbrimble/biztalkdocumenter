
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml;

    /// <summary>
    /// Summary description for OrchestrationMessage.
    /// </summary>
    public class OrchestrationMessage : BizTalkBaseObject
    {
        private string typeName = string.Empty;
        private string scope = "Global";
        private string scopeId = String.Empty;
        private string description = string.Empty;

        public OrchestrationMessage()
        {
        }

        public OrchestrationMessage(XmlNode msgNode, XmlNamespaceManager mgr)
        {
            XmlAttribute scopeAtt = msgNode.Attributes.GetNamedItem("ParentLink") as XmlAttribute;
            if (scopeAtt != null &&
                string.Compare(scopeAtt.Value, "Scope_MessageDeclaration", true) == 0)
            {
                this.Scope = msgNode.ParentNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                this.scopeId = msgNode.ParentNode.Attributes["OID"].Value;
            }

            this.Name = msgNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
            this.TypeName = msgNode.SelectSingleNode("om:Property[@Name='Type']", mgr).Attributes.GetNamedItem("Value").Value;

            XmlNode descNode = msgNode.SelectSingleNode("om:Property[@Name='AnalystComments']", mgr);
            if (descNode != null)
            {
                this.Description = descNode.Attributes.GetNamedItem("Value").Value;
            }

            // Build the QName - sometimes there are duplicates in variable name as well as scope name 
            // so we need the Id's to be added to the QName
            this.QualifiedName = this.Name + "," + this.Id + "," + this.Scope + " (" + this.scopeId + ")";

        }

        public string TypeName
        {
            get { return this.typeName; }
            set { this.typeName = value; }
        }

        public string Scope
        {
            get { return this.scope; }
            set { this.scope = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
