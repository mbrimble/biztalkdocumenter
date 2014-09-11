
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// BizTalkBaseObject.
    /// </summary>
    [XmlInclude(typeof(Alias))]
    [XmlInclude(typeof(BizTalkApplication))]
    [XmlInclude(typeof(BizTalkAssembly))]
    [XmlInclude(typeof(BizTalkInstallation))]
    [XmlInclude(typeof(Filter))]
    [XmlInclude(typeof(FilterGroup))]
    [XmlInclude(typeof(Host))]
    [XmlInclude(typeof(HostInstance))]
    [XmlInclude(typeof(MessagePart))]
    [XmlInclude(typeof(Orchestration))]
    [XmlInclude(typeof(OrchestrationPort))]
    [XmlInclude(typeof(OrchestrationMessage))]
    [XmlInclude(typeof(OrchestrationMultiPartMessage))]
    [XmlInclude(typeof(OrchestrationVariable))]
    [XmlInclude(typeof(OrchestrationRoleLink))]
    [XmlInclude(typeof(Party))]
    [XmlInclude(typeof(Pipeline))]
    [XmlInclude(typeof(Protocol))]
    [XmlInclude(typeof(ReceiveLocation))]
    [XmlInclude(typeof(ReceivePort))]
    [XmlInclude(typeof(Schema))]
    [XmlInclude(typeof(SendPort))]
    [XmlInclude(typeof(SendPortGroup))]
    [XmlInclude(typeof(Transform))]
    public abstract class BizTalkBaseObject
    {
        private static XmlSerializerCache xmlSerializerCache = new XmlSerializerCache();

        private NameIdPair nameIdPair;

        /// <summary>
        /// Creates a new <see cref="BizTalkBaseObject"/>
        /// </summary>
        public BizTalkBaseObject()
        {
            this.Id = Guid.NewGuid().ToString();
            // Not all artifacts will have one but nevertheless an important property.
            this.AssemblyName = String.Empty;
        }

        #region Public properties

        [XmlIgnore()]
        public NameIdPair NameIdPair
        {
            get
            {
                if (this.nameIdPair == null)
                {
                    this.nameIdPair = new NameIdPair(this.Name, this.Id);
                }

                return this.nameIdPair;
            }
        }

        [XmlIgnore()]
        public BizTalkApplication Application { get; set; }

        public string Name { get; set; }

        public string ApplicationName { get; set; }

        public string CustomDescription { get; set; }

        public string Id { get; set; }

        public string QualifiedName { get; set; }

        [XmlIgnore()]
        public string AssemblyName { get; set; }

        #endregion

        internal virtual void FixReferences(BtsCatalogExplorer explorer)
        {
            return;
        }

        public string GetXml()
        {
            MemoryStream ms = new MemoryStream();

            //XmlSerializer xmlSerializer = xmlSerializerCache.GetCachedSerializer(this.GetType());
            XmlSerializer xmlSerializer = xmlSerializerCache.GetCachedSerializer(typeof(BizTalkBaseObject));
            xmlSerializer.Serialize(ms, this);
            ms.Position = 0;

            string data = string.Empty;

            using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
            {
                data = sr.ReadToEnd();
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Cleanup()
        {
            return;
        }
    }
}
