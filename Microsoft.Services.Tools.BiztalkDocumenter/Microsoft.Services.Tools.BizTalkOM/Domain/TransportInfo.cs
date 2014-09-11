
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for TransportInfo.
    /// </summary>
    public class TransportInfo
    {
        private string address;
        private object deliveryNotification;
        private bool orderedDelivery;
        private bool primary;
        private int retryCount;
        private int retryInterval;
        private string type;
        private string data;
        private bool dataSpecified = false;
        private ServiceWindow serviceWindow;
        private NameIdPair sendHandler;

        /// <summary>
        /// 
        /// </summary>
        public TransportInfo()
        {
            this.retryCount = 3;
            this.retryInterval = 5;
            this.primary = false;
            this.orderedDelivery = false;
            this.data = string.Empty;
        }

        public TransportInfo(BizTalkCore.TransportInfo transportInfo, bool primary)
            : this()
        {
            if (transportInfo != null)
            {
                this.Address = transportInfo.Address;
                this.OrderedDelivery = transportInfo.OrderedDelivery;
                this.Primary = primary;
                this.RetryCount = transportInfo.RetryCount;
                this.RetryInterval = transportInfo.RetryInterval;
                this.deliveryNotification = transportInfo.DeliveryNotification;

                if (transportInfo.TransportType != null)
                {
                    this.Type = transportInfo.TransportType.Name;
                }

                this.Data = transportInfo.TransportTypeData;

                this.ServiceWindow = new ServiceWindow();
                this.ServiceWindow.Enabled = transportInfo.ServiceWindowEnabled;
                this.ServiceWindow.StartDateEnabled = transportInfo.ServiceWindowEnabled;
                this.ServiceWindow.EndDateEnabled = transportInfo.ServiceWindowEnabled;

                this.ServiceWindow.EndTime = transportInfo.ToTime;
                this.ServiceWindow.StartTime = transportInfo.FromTime;
            }
        }

        #region Public Properties

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

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public bool Primary
        {
            get { return this.primary; }
            set { this.primary = value; }
        }

        public bool OrderedDelivery
        {
            get { return this.orderedDelivery; }
            set { this.orderedDelivery = value; }
        }

        public int RetryCount
        {
            get { return this.retryCount; }
            set { this.retryCount = value; }
        }

        public int RetryInterval
        {
            get { return this.retryInterval; }
            set { this.retryInterval = value; }
        }

        public ServiceWindow ServiceWindow
        {
            get { return this.serviceWindow; }
            set { this.serviceWindow = value; }
        }

        // CD 20140513 Added for Secondary Transport
        public NameIdPair SendHandler
        {
            get { return this.sendHandler; }
            set { this.sendHandler = value; }
        }


        #endregion
    }
}
