
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml;

    /// <summary>
    /// Summary description for OrchestrationMessage.
    /// </summary>
    public class MessagePart : BizTalkBaseObject
    {
        private string className = string.Empty;
        private bool isBodyPart = true;
        private string description = string.Empty;

        public MessagePart()
        {
        }

        public MessagePart(XmlNode msgNode, XmlNamespaceManager mgr)
        {
            this.Name = msgNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
            this.ClassName = msgNode.SelectSingleNode("om:Property[@Name='ClassName']", mgr).Attributes.GetNamedItem("Value").Value;

            string isBP = msgNode.SelectSingleNode("om:Property[@Name='IsBodyPart']", mgr).Attributes.GetNamedItem("Value").Value;
            this.IsBodyPart = isBP == "True" ? true : false;

            XmlNode descNode = msgNode.SelectSingleNode("om:Property[@Name='AnalystComments']", mgr);
            if (descNode != null)
            {
                this.Description = descNode.Attributes.GetNamedItem("Value").Value;
            }
        }

        public string ClassName
        {
            get { return this.className; }
            set { this.className = value; }
        }

        public bool IsBodyPart
        {
            get { return this.isBodyPart; }
            set { this.isBodyPart = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
