
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Management;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Protocol.
    /// </summary>
    public sealed class Protocol : BizTalkBaseObject
    {
        private Capabilities capabilities;
        private string configurationGuid;
        private BizTalkInstallation parentInstallation;

        private bool applicationProtocol;
        private bool deleteProtected;
        private bool initInboundProtocolContext;
        private bool initOutboundProtocolContext;
        private bool initReceiveLocationContext;
        private bool initTransmitLocationContext;
        private bool initTransmitterOnServiceStart;
        private bool receiveIsCreatable;
        private bool requireSingleInstance;
        private bool staticHandlers;
        private bool supportsOrderedDelivery;
        private bool supportsReceive;
        private bool supportsRequestResponse;
        private bool supportsSend;
        private bool supportsSoap;
        private bool supportsSolicitResponse;

        private NameIdPairCollection sendHandlers = new NameIdPairCollection();
        private NameIdPairCollection receiveHandlers = new NameIdPairCollection();

        private NameIdPairCollection sendPorts;
        private NameIdPairCollection receiveLocations;

        #region Constructors

        public Protocol()
        {
            this.sendPorts = new NameIdPairCollection();
            this.receiveLocations = new NameIdPairCollection();
        }

        public Protocol(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        [XmlIgnore]
        public BizTalkInstallation ParentInstallation
        {
            get { return this.parentInstallation; }
            set { this.parentInstallation = value; }
        }

        public string ConfigurationGuid
        {
            get { return this.configurationGuid; }
            set { this.configurationGuid = value; }
        }

        [XmlArrayItem("SendHandler", typeof(NameIdPair))]
        public NameIdPairCollection SendHandlers
        {
            get { return this.sendHandlers; }
            set { this.sendHandlers = value; }
        }

        [XmlArrayItem("ReceiveHandler", typeof(NameIdPair))]
        public NameIdPairCollection ReceiveHandlers
        {
            get { return this.receiveHandlers; }
            set { this.receiveHandlers = value; }
        }

        public bool ApplicationProtocol
        {
            get { return this.applicationProtocol; }
            set { this.applicationProtocol = value; }
        }

        public bool DeleteProtected
        {
            get { return this.deleteProtected; }
            set { this.deleteProtected = value; }
        }

        public bool InitInboundProtocolContext
        {
            get { return this.initInboundProtocolContext; }
            set { this.initInboundProtocolContext = value; }
        }

        public bool InitOutboundProtocolContext
        {
            get { return this.initOutboundProtocolContext; }
            set { this.initOutboundProtocolContext = value; }
        }

        public bool InitReceiveLocationContext
        {
            get { return this.initReceiveLocationContext; }
            set { this.initReceiveLocationContext = value; }
        }

        public bool InitTransmitLocationContext
        {
            get { return this.initTransmitLocationContext; }
            set { this.initTransmitLocationContext = value; }
        }

        public bool InitTransmitterOnServiceStart
        {
            get { return this.initTransmitterOnServiceStart; }
            set { this.initTransmitterOnServiceStart = value; }
        }

        public bool ReceiveIsCreatable
        {
            get { return this.receiveIsCreatable; }
            set { this.receiveIsCreatable = value; }
        }

        public bool RequireSingleInstance
        {
            get { return this.requireSingleInstance; }
            set { this.requireSingleInstance = value; }
        }

        public bool StaticHandlers
        {
            get { return this.staticHandlers; }
            set { this.staticHandlers = value; }
        }

        public bool SupportsOrderedDelivery
        {
            get { return this.supportsOrderedDelivery; }
            set { this.supportsOrderedDelivery = value; }
        }

        public bool SupportsReceive
        {
            get { return this.supportsReceive; }
            set { this.supportsReceive = value; }
        }

        public bool SupportsRequestResponse
        {
            get { return this.supportsRequestResponse; }
            set { this.supportsRequestResponse = value; }
        }

        public bool SupportsSend
        {
            get { return this.supportsSend; }
            set { this.supportsSend = value; }
        }

        public bool SupportsSoap
        {
            get { return this.supportsSoap; }
            set { this.supportsSoap = value; }
        }

        public bool SupportsSolicitResponse
        {
            get { return this.supportsSolicitResponse; }
            set { this.supportsSolicitResponse = value; }
        }

        public Capabilities Capabilities
        {
            get { return this.capabilities; }
            set { try { this.capabilities = value; } catch (Exception) { } }
        }

        [XmlArrayItem("SendPort", typeof(NameIdPair))]
        public NameIdPairCollection SendPorts
        {
            get { return this.sendPorts; }
            set { this.sendPorts = value; }
        }

        [XmlArrayItem("ReceiveLocation", typeof(NameIdPair))]
        public NameIdPairCollection ReceiveLocations
        {
            get { return this.receiveLocations; }
            set { this.receiveLocations = value; }
        }

        #endregion

        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.ProtocolType protocolType)
        {
            this.configurationGuid = protocolType.ConfigurationGuid.ToString();
            this.DecryptCapabilities(protocolType.Capabilities);
            this.GetHandlerInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capabilities"></param>
        private void DecryptCapabilities(BizTalkCore.Capabilities capabilities)
        {
            try
            {
                this.capabilities = (Capabilities)Enum.Parse(typeof(Capabilities), ((int)capabilities).ToString());

                this.applicationProtocol = (capabilities & BizTalkCore.Capabilities.ApplicationProtocol) != 0;
                this.deleteProtected = (capabilities & BizTalkCore.Capabilities.DeleteProtected) != 0;
                this.initInboundProtocolContext = (capabilities & BizTalkCore.Capabilities.InitInboundProtocolContext) != 0;
                this.initReceiveLocationContext = (capabilities & BizTalkCore.Capabilities.InitReceiveLocationContext) != 0;
                this.initTransmitLocationContext = (capabilities & BizTalkCore.Capabilities.InitTransmitLocationContext) != 0;
                this.initTransmitterOnServiceStart = (capabilities & BizTalkCore.Capabilities.InitTransmitterOnServiceStart) != 0;
                this.receiveIsCreatable = (capabilities & BizTalkCore.Capabilities.ReceiveIsCreatable) != 0;
                this.requireSingleInstance = (capabilities & BizTalkCore.Capabilities.RequireSingleInstance) != 0;
                this.staticHandlers = (capabilities & BizTalkCore.Capabilities.StaticHandlers) != 0;
                this.supportsOrderedDelivery = (capabilities & BizTalkCore.Capabilities.SupportsOrderedDelivery) != 0;
                this.supportsReceive = (capabilities & BizTalkCore.Capabilities.SupportsReceive) != 0;
                this.supportsRequestResponse = (capabilities & BizTalkCore.Capabilities.SupportsRequestResponse) != 0;
                this.supportsSend = (capabilities & BizTalkCore.Capabilities.SupportsSend) != 0;
                this.supportsSoap = (capabilities & BizTalkCore.Capabilities.SupportsSoap) != 0;
                this.supportsSolicitResponse = (capabilities & BizTalkCore.Capabilities.SupportsSolicitResponse) != 0;
            }
            catch (Exception)
            {
                this.capabilities = Capabilities.ApplicationProtocol;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetHandlerInfo()
        {
            TraceManager.SmartTrace.TraceIn();

            try
            {
                string q = "SELECT * FROM MSBTS_SendHandler WHERE AdapterName = \"" + this.Name + "\"";
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", q);

                foreach (ManagementObject oReturn in mos.Get())
                {
                    string handlerName = oReturn["HostName"].ToString();

                    Host h = this.parentInstallation.Hosts[handlerName] as Host;

                    if (h != null)
                    {
                        this.sendHandlers.Add(h.NameIdPair);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            try
            {
                string q = "SELECT * FROM MSBTS_ReceiveHandler WHERE AdapterName = \"" + this.Name + "\"";
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", q);

                foreach (ManagementObject oReturn in mos.Get())
                {
                    string handlerName = oReturn["HostName"].ToString();

                    Host h = this.parentInstallation.Hosts[handlerName] as Host;

                    if (h != null)
                    {
                        this.receiveHandlers.Add(h.NameIdPair);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
