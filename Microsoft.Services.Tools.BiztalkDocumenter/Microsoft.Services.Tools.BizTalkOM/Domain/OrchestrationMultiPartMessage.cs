
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Summary description for OrchestrationMessage.
    /// </summary>
    public class OrchestrationMultiPartMessage : BizTalkBaseObject
    {
        private string typeName = string.Empty;
        private string modifier = "Internal";
        private string description = string.Empty;
        private BizTalkBaseObjectCollectionEx parts;

        public OrchestrationMultiPartMessage()
        {
            this.parts = new BizTalkBaseObjectCollectionEx();
        }

        public OrchestrationMultiPartMessage(XmlNode msgNode, XmlNamespaceManager mgr)
        {
            this.parts = new BizTalkBaseObjectCollectionEx();

            this.Name = msgNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
            this.Modifier = msgNode.SelectSingleNode("om:Property[@Name='TypeModifier']", mgr).Attributes.GetNamedItem("Value").Value;

            XmlNode descNode = msgNode.SelectSingleNode("om:Property[@Name='AnalystComments']", mgr);
            if (descNode != null)
            {
                this.Description = descNode.Attributes.GetNamedItem("Value").Value;
            }

            XmlNodeList partNodes = msgNode.SelectNodes("om:Element[@Type='PartDeclaration']", mgr);

            foreach (XmlNode partNode in partNodes)
            {
                MessagePart mp = new MessagePart(partNode, mgr);
                this.parts.Add(mp);
            }
        }

        [XmlArrayItem("MultiPartMessage", typeof(MessagePart))]
        public BizTalkBaseObjectCollectionEx Parts
        {
            get { return this.parts; }
            set { this.parts = value; }
        }

        public string Modifier
        {
            get { return this.modifier; }
            set { this.modifier = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
