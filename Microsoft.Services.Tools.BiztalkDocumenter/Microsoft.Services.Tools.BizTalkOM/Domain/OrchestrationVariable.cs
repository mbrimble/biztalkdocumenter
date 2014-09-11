
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml;

    /// <summary>
    /// Summary description for OrchestrationVariable.
    /// </summary>
    public class OrchestrationVariable : BizTalkBaseObject
    {
        private string typeName = string.Empty;
        private string scope = "Global";
        private string scopeId = String.Empty;
        private string initialValue = string.Empty;
        private string description = string.Empty;

        public OrchestrationVariable()
        {
        }

        public OrchestrationVariable(XmlNode varNode, XmlNamespaceManager mgr)
        {
            XmlAttribute scopeAtt = varNode.Attributes.GetNamedItem("ParentLink") as XmlAttribute;
            if (scopeAtt != null &&
                string.Compare(scopeAtt.Value, "Scope_VariableDeclaration", true) == 0)
            {
                this.Scope = varNode.ParentNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                this.scopeId = varNode.ParentNode.Attributes["OID"].Value;
            }

            this.Name = varNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
            this.TypeName = varNode.SelectSingleNode("om:Property[@Name='Type']", mgr).Attributes.GetNamedItem("Value").Value;

            XmlNode descNode = varNode.SelectSingleNode("om:Property[@Name='AnalystComments']", mgr);
            if (descNode != null)
            {
                this.Description = descNode.Attributes.GetNamedItem("Value").Value;
            }

            XmlNode ivalNode = varNode.SelectSingleNode("om:Property[@Name='InitialValue']", mgr);
            if (ivalNode != null)
            {
                this.InitialValue = ivalNode.Attributes.GetNamedItem("Value").Value;
            }

            // Build the QName - sometimes there are duplicates in variable name as well as scope name 
            // so we need the Id's to be added to the QName
            //this.QualifiedName = this.Name + "," + this.Scope + "(" + this.ScopeId "," + this.Id;
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

        public string InitialValue
        {
            get { return this.initialValue; }
            set { this.initialValue = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

    }
}
