
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for ReceivePort.
    /// </summary>
    public class ReceivePort : BizTalkBaseObject
    {
        private bool twoWay;
        private bool routeFailedMessages;
        private BizTalkBaseObjectCollectionEx receiveLocations;
        private AuthenticationType authenticationType;
        private TrackingType trackingType;
        private NameIdPairCollection outboundMaps;
        private NameIdPairCollection inboundMaps;
        private NameIdPairCollection boundOrchestrations;

        /// <summary>
        /// Creates a new <see cref="ReceivePort"/>
        /// </summary>
        public ReceivePort()
        {
            this.receiveLocations = new BizTalkBaseObjectCollectionEx();
            this.outboundMaps = new NameIdPairCollection();
            this.inboundMaps = new NameIdPairCollection();
            this.boundOrchestrations = new NameIdPairCollection();
        }

        /// <summary>
        /// Creates a new <see cref="ReceivePort"/>
        /// </summary>
        /// <param name="name"></param>
        public ReceivePort(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Transform", typeof(NameIdPair))]
        public NameIdPairCollection OutboundMaps
        {
            get { return this.outboundMaps; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Transform", typeof(NameIdPair))]
        public NameIdPairCollection InboundMaps
        {
            get { return this.inboundMaps; }
            set { this.inboundMaps = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Orchestration", typeof(NameIdPair))]
        public NameIdPairCollection BoundOrchestrations
        {
            get { return this.boundOrchestrations; }
            set { this.boundOrchestrations = value; }
        }

        public AuthenticationType AuthenticationType
        {
            get { return this.authenticationType; }
            set { this.authenticationType = value; }
        }

        public TrackingType TrackingType
        {
            get { return this.trackingType; }
            set { this.trackingType = value; }
        }

        public bool TwoWay
        {
            get { return this.twoWay; }
            set { this.twoWay = value; }
        }

        public bool RouteFailedMessages
        {
            get { return this.routeFailedMessages; }
            set { this.routeFailedMessages = value; }
        }

        [XmlArrayItem("ReceiveLocation", typeof(ReceiveLocation))]
        public BizTalkBaseObjectCollectionEx ReceiveLocations
        {
            get { return this.receiveLocations; }
        }

        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.ReceivePort port)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            if (port != null)
            {
                this.QualifiedName = String.Empty;
                this.authenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), ((int)port.Authentication).ToString());
                this.trackingType = (TrackingType)Enum.Parse(typeof(TrackingType), ((int)port.Tracking).ToString());
                this.ApplicationName = port.Application.Name;
                this.twoWay = port.IsTwoWay;
                this.routeFailedMessages = port.RouteFailedMessage;
                this.CustomDescription = port.Description;

                foreach (BizTalkCore.ReceiveLocation location in port.ReceiveLocations)
                {
                    ReceiveLocation rl = new ReceiveLocation(location.Name);
                    rl.Load(explorer, location);
                    rl.ParentPort = this.NameIdPair;
                    this.receiveLocations.Add(rl);
                }
            }

            TraceManager.SmartTrace.TraceOut();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        internal override void FixReferences(Microsoft.BizTalk.ExplorerOM.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            BizTalkCore.ReceivePort port = explorer.ReceivePorts[this.Name];

            if (port != null)
            {
                // Outbound Transforms
                if (port.OutboundTransforms != null)
                {
                    foreach (BizTalkCore.Transform transform in port.OutboundTransforms)
                    {
                        Transform t = this.Application.Maps[transform.FullName] as Transform;

                        if (t != null)
                        {
                            t.ReceivePorts.Add(this.NameIdPair);
                            this.outboundMaps.Add(t.NameIdPair);
                        }
                    }
                }

                // Inbound Transforms
                if (port.InboundTransforms != null)
                {
                    foreach (BizTalkCore.Transform transform in port.InboundTransforms)
                    {
                        Transform t = this.Application.Maps[transform.FullName] as Transform;

                        if (t != null)
                        {
                            t.ReceivePorts.Add(this.NameIdPair);
                            this.inboundMaps.Add(t.NameIdPair);
                        }
                    }
                }

                // Locations
                try
                {
                    foreach (ReceiveLocation location in this.receiveLocations)
                    {
                        NameIdPair nameIdPair = new NameIdPair(location.Name, this.Id);

                        Pipeline pl = this.Application.ParentInstallation.Pipelines[location.ReceivePipeline.Name] as Pipeline;

                        if (pl != null)
                        {
                            //Colin Dijkgraaf 20141010
                            //location.ReceivePipeline = pl.NameIdPair;
                            //pl.ReceiveLocations.Add(nameIdPair);
                            if (pl.Name == location.ReceivePipeline.Name)  // Actually check that a match was found.
                            {
                                location.ReceivePipeline = pl.NameIdPair;
                                pl.ReceiveLocations.Add(nameIdPair);
                            }
                        }

                        if (this.twoWay)
                        {
                            pl = this.Application.ParentInstallation.Pipelines[location.SendPipeline.Name] as Pipeline;

                            if (pl != null)
                            {
                                //Colin Dijkgraaf 20141010
                                //location.SendPipeline = pl.NameIdPair;
                                if (pl.Name == location.SendPipeline.Name)  // Actually check that a match was found.
                                {
                                    location.SendPipeline = pl.NameIdPair;
                                }
                            }
                        }

                        Protocol p = this.Application.ParentInstallation.ProtocolTypes[location.TransportProtocol] as Protocol;

                        if (p != null)
                        {
                            p.ReceiveLocations.Add(nameIdPair);

                            //foreach (NameIdPair handler in p.ReceiveHandlers)CD 20140402
                            //{
                                //Host h = this.Application.ParentInstallation.Hosts[handler.Name] as Host;
                            Host h = this.Application.ParentInstallation.Hosts[port.PrimaryReceiveLocation.ReceiveHandler.Name] as Host;

                                if (h != null)
                                {
                                    h.HostedReceiveLocations.Add(nameIdPair);
                                    location.ReceiveHandler = h.NameIdPair;
                                }
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }

                TraceManager.SmartTrace.TraceOut();
                return;
            }
        }
    }
}
