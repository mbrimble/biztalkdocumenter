
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for ReceiveLocation.
    /// </summary>
    public class ReceiveLocation : BizTalkBaseObject
    {
        private string address;
        private string data;
        private bool dataSpecified = false;
        private string transportProtocol;
        private NameIdPair receivePipeline;
        private NameIdPair sendPipeline;
        private ServiceWindow serviceWindow;
        private NameIdPair receiveHandler;
        private NameIdPair parentPort;

        public ReceiveLocation()
        {
            this.data = string.Empty;
        }

        public ReceiveLocation(string name)
            : this()
        {
            this.Name = name;
        }

        #region Public properties

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
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

        public string TransportProtocol
        {
            get { return this.transportProtocol; }
            set { this.transportProtocol = value; }
        }

        public NameIdPair ReceivePipeline
        {
            get { return this.receivePipeline; }
            set { this.receivePipeline = value; }
        }

        public NameIdPair SendPipeline
        {
            get { return this.sendPipeline; }
            set { this.sendPipeline = value; }
        }

        public ServiceWindow ServiceWindow
        {
            get { return this.serviceWindow; }
            set { this.serviceWindow = value; }
        }

        public NameIdPair ReceiveHandler
        {
            get { return this.receiveHandler; }
            set { this.receiveHandler = value; }
        }

        public NameIdPair ParentPort
        {
            get { return this.parentPort; }
            set { this.parentPort = value; }
        }

        #endregion

        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.ReceiveLocation location)
        {
            this.Address = location.Address;

            this.Name = location.Name;
            this.receivePipeline = new NameIdPair(location.ReceivePipeline.FullName, "");
            this.transportProtocol = location.TransportType.Name;
            this.data = location.TransportTypeData;
            this.CustomDescription = location.Description;

            if (location.SendPipeline != null)
            {
                this.sendPipeline = new NameIdPair(location.SendPipeline.FullName, "");
            }
            else
            {
                this.sendPipeline = new NameIdPair("<Not configured>", "");
            }

            this.serviceWindow = new ServiceWindow();
            this.serviceWindow.Enabled = location.ServiceWindowEnabled;
            this.serviceWindow.StartDateEnabled = location.StartDateEnabled;
            this.serviceWindow.EndDateEnabled = location.EndDateEnabled;
            this.serviceWindow.EndTime = location.ToTime;
            this.serviceWindow.StartTime = location.FromTime;
        }
    }
}
