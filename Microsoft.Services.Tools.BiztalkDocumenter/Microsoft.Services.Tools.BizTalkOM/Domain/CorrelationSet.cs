
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Collections;
    using System.Xml.Serialization;

    /// <summary>
    /// Summary description for OrchestrationMessage.
    /// </summary>
    public class CorrelationSet : BizTalkBaseObject
    {
        private string typeName = string.Empty;
        private string scope = "Global";
        private string description = "-";
        private string definingTypeId = string.Empty;
        private ArrayList correlatedProperties = new ArrayList();

        public CorrelationSet()
        {
        }

        public string DefiningTypeId
        {
            get { return this.definingTypeId; }
            set { this.definingTypeId = value; }
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

        [XmlArrayItem("Property", typeof(string))]
        public ArrayList CorrelatedProperties
        {
            get { return this.correlatedProperties; }
            set { this.correlatedProperties = value; }
        }
    }
}
