
namespace Microsoft.Services.Tools.BiztalkDocumenter.Publishers.Word
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;
    using Microsoft.Services.Tools.BizTalkOM;

    /// <summary>
    /// Summary description for WordXmlPublisher.
    /// </summary>
    public class WordXmlPublisher : IPublisher
    {
        private Color TableTitleBackground = Color.FromArgb(169, 199, 240);
        private Color TableTitleColumnBackground = Color.FromArgb(220, 235, 254);

        string fileName = @"c:\BizTalkDocumentation.xml";
        BizTalkInstallation bi = null;

        private string picAssembly = string.Empty;
        private string picFilterSmall = string.Empty;
        private string picHost = string.Empty;
        private string picLogo = string.Empty;
        private string picMapSmall = string.Empty;
        private string picOrchestration = string.Empty;
        private string picOrchestrationSmall = string.Empty;
        private string picPipeline = string.Empty;
        private string picProtocol = string.Empty;
        private string picReceivePort = string.Empty;
        private string picReceivePortSmall = string.Empty;
        private string picSchema = string.Empty;
        private string picSchemaSmall = string.Empty;
        private string picSendPort1 = string.Empty;
        private string picSendPort2 = string.Empty;
        private string picTransportSmall = string.Empty;

        public WordXmlPublisher()
        {
        }

        public event UpdatePercentageComplete PercentageDocumentationComplete;

        #region IPublisher Members

        public bool Prepare()
        {
            #region Build the Base64 encoded image strings

            string resourcePrefix = "Microsoft.Services.Tools.BiztalkDocumenter.Publishers.Word";
            picAssembly = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Assembly.jpg");
            picFilterSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.FilterSmall.jpg");
            picHost = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Host.jpg");
            picLogo = GetEncodedImageStringFromResource(resourcePrefix + ".Res.logo.jpg");
            picMapSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.MapSmall.jpg");
            picOrchestration = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Orchestration.jpg");
            picOrchestrationSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.OrchestrationSmall.jpg");
            picPipeline = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Pipeline.jpg");
            picProtocol = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Protocol.jpg");
            picReceivePort = GetEncodedImageStringFromResource(resourcePrefix + ".Res.ReceivePort.jpg");
            picReceivePortSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.ReceivePortSmall.jpg");
            picSchema = GetEncodedImageStringFromResource(resourcePrefix + ".Res.Schema.jpg");
            picSchemaSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.SchemaSmall.jpg");
            picSendPort1 = GetEncodedImageStringFromResource(resourcePrefix + ".Res.SendPort1.jpg");
            picSendPort2 = GetEncodedImageStringFromResource(resourcePrefix + ".Res.SendPort2.jpg");
            picTransportSmall = GetEncodedImageStringFromResource(resourcePrefix + ".Res.TransportSmall.jpg");

            #endregion

            return true;
        }

        #region Publish

        public void Publish(BizTalkInstallation bi, Microsoft.Services.Tools.BiztalkDocumenter.PublishType publishType, string resourceFolder, string publishFolder, string reportTitle, bool publishRules)
        {
            this.bi = bi;

            if (!Directory.Exists(publishFolder))
            {
                Directory.CreateDirectory(publishFolder);
            }

            this.fileName = Path.Combine(publishFolder, string.Format("{0}.xml", reportTitle));
            //this.fileName = this.fileName.Replace(" ", "");

            if (File.Exists(this.fileName))
            {
                File.Delete(this.fileName);
            }

#if (DEBUG)
            StreamWriter debugSw = new StreamWriter(Path.Combine(Path.GetTempPath(), "DebugBizTalkInstallation.xml"));
            string debugData = bi.GetXml();
            debugSw.Write(debugData);
            debugSw.Flush();
            debugSw.Close();
#endif

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                WordXmlWriter wr = new WordXmlWriter(fs);

                wr.WriteStartDocument();

                //============================================
                // Styles
                //============================================
                this.WriteStyles(wr);

                wr.WriteStartBody();
                wr.WriteStartSection();

                //============================================
                // Footers
                //============================================
                string footerText = "Generated on: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                wr.WriteFooterString(footerText, "footer");

                //============================================
                // Title Page
                //============================================
                this.WriteTitlePage(wr);

                //============================================
                // Hosts
                //============================================
                this.WriteHosts(wr);
                this.UpdatePercentageComplete(20);

                //============================================
                // Send Port Groups
                //============================================
                this.WriteSendPortGroups(wr);
                this.UpdatePercentageComplete(30);

                //============================================
                // Send Ports
                //============================================
                this.WriteSendPorts(wr);
                this.UpdatePercentageComplete(40);

                //============================================
                // Receive ports
                //============================================
                this.WriteReceivePorts(wr);
                this.UpdatePercentageComplete(50);

                //============================================
                // Protocols
                //============================================
                this.WriteProtocols(wr);
                this.UpdatePercentageComplete(60);

                //============================================
                // Assemblies
                //============================================
                if (publishType != PublishType.SchemaOnly)
                {
                    this.WriteAssemblies(wr);
                }
                this.UpdatePercentageComplete(70);

                //============================================
                // Orchestrations
                //============================================
                if (publishType != PublishType.SchemaOnly)
                {
                    this.WriteOrchestrations(wr);
                }
                this.UpdatePercentageComplete(80);

                //============================================
                // Pipelines
                //============================================
                this.WritePipelines(wr);
                this.UpdatePercentageComplete(90);

                wr.WriteEndSection();
                wr.WriteEndBody();
                wr.WriteEndDocument();

                wr.BaseStream.Flush();
                fs.Close();
            }
            this.UpdatePercentageComplete(100);
        }
        /// <summary>
        /// Added MTB 08/03/2014
        /// </summary>
        /// <param name="bi"></param>
        /// <param name="publishType"></param>
        /// <param name="resourceFolder"></param>
        /// <param name="publishFolder"></param>
        /// <param name="reportTitle"></param>
        /// <param name="publishRules"></param>
        /// <param name="ssoLocations"></param>
        /// <param name="ssoApplications"></param>
        /// <param name="bizTalkConfigurationPath"></param>
        public void Publish(BizTalkInstallation bi, PublishType publishType, string resourceFolder, string publishFolder,
                          string reportTitle, bool publishRules, string[] ssoLocations, string[] ssoApplications, string bizTalkConfigurationPath)
        {
            throw new NotImplementedException();
        }



        #endregion

        public void ShowOutput()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("winword");
                psi.Arguments = "\"" + this.fileName + "\"";
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Error occurred in launching WORD to view the file. Check if Word is Installed. You can find the output WordML at : " +
                    this.fileName);

                //throw;
            }
        }

        public bool Cleanup()
        {
            return true;
        }

        #endregion

        private string GetEncodedImageStringFromResource(string resourceName)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream s = a.GetManifestResourceStream(resourceName);
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, (int)s.Length);
            return Convert.ToBase64String(buffer);
        }

        #region WriteTitlePage

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        private void WriteTitlePage(WordXmlWriter wr)
        {
            string footerText = "Generated on: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            wr.WriteStartSubSection();
            wr.WriteBodyTextWithPreceedingImage("", "", false, picLogo, 78, 500);
            wr.WriteBodyText("BizTalk Configuration", "mainTitle");
            wr.WriteBodyText("Documentation", "mainSubTitle");
            wr.WriteNewLine();
            wr.WriteNewLine();
            wr.WriteNewLine();
            wr.WriteBodyText("Installation server: " + bi.Server, "artifactHeading");
            wr.WriteBodyText("Installation database: " + bi.MgmtDatabaseName, "artifactHeading");
            wr.WriteNewLine();
            wr.WriteBodyText(footerText, "bodyBold");
            wr.WriteEndSubSection();
        }

        #endregion

        #region WriteStyles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        private void WriteStyles(WordXmlWriter wr)
        {
            wr.WriteStartStyles();
            wr.WriteStyle("body", "paragraph", "Arial", 8, false, false, Color.Black, Alignment.Left);
            wr.WriteStyle("bodyBold", "paragraph", "Arial", 8, false, true, Color.Black, Alignment.Left);
            wr.WriteStyle("mainTitle", "paragraph", "Arial Black", 35, false, false, Color.Black, Alignment.Left);
            wr.WriteStyle("mainSubTitle", "paragraph", "Arial Black", 28, false, false, Color.Black, Alignment.Left);
            wr.WriteStyle("footer", "paragraph", "Arial", 7, false, false, Color.Gray, Alignment.Right);
            wr.WriteStyle("hyperlink", "character", "Arial", 8, true, false, Color.LightBlue, Alignment.Left);
            wr.WriteStyle("artifactHeading", "paragraph", "Arial", 11, false, true, Color.Black, Color.White, Alignment.Left, 2);
            wr.WriteStyle("configSectionHeading", "paragraph", "Arial", 18, false, true, Color.White, Color.LightSteelBlue, Alignment.Left, 1);
            wr.WriteStyle("tab1", "table", "Arial", 8);
            wr.WriteStyle("artifactSubHeading", "paragraph", "Arial", 9, true, true, Color.DarkSlateGray, Alignment.Left);

            wr.WriteStyle("test", "paragraph", "Arial", 10, true, true, Color.Teal, Alignment.Left);
            wr.WriteStyle("test3", "paragraph", "Arial", 10, true, true, Color.Tomato, Alignment.Left);
            wr.WriteEndStyles();
        }

        #endregion

        #region WriteHosts

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        private void WriteHosts(WordXmlWriter wr)
        {
            if (this.bi.Hosts.Count > 0)
            {
                int counter = 0;
                wr.WritePageBreak();
                wr.WriteBodyText("Host Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (Host host in bi.Hosts)
                {
                    counter++;
                    wr.WriteStartSubSection();
                    wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, host.Name), "artifactHeading", false, picHost, 14, 14);
                    wr.WriteNewLine();

                    wr.WriteBodyText("Group Name:  " + host.GroupName, "body");
                    wr.WriteBodyText("Host Tracking Enabled:  " + host.HostTrackingEnabled.ToString(), "body");
                    wr.WriteBodyText("Authentication Trusted:  " + host.AuthTrusted.ToString(), "body");
                    wr.WriteBodyText("Default Host:  " + host.DefaultHost.ToString(), "body");

                    string hostType = host.Inprocess ? "In-Process" : "Isolated";
                    wr.WriteBodyText("Host Type:  " + hostType, "body");

                    //============================================
                    // What orchestrations are running in this host
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Orchestrations running in this host", "artifactSubHeading");
                    wr.WriteNewLine();

                    int orchCounter = 0;
                    foreach (BizTalkAssembly bta in this.bi.Assemblies)
                    {
                        foreach (Orchestration orch in bi.Orchestrations)
                        {
                            if (string.Compare(orch.Host.Name, host.Name, true) == 0)
                            {
                                orchCounter++;
                                wr.WriteBodyText(orch.Name, "body");
                                break;
                            }
                        }
                    }

                    if (orchCounter == 0)
                    {
                        wr.WriteBodyText("There are no orchestrations running in this host", "body");
                    }

                    //Receive locations running in this host

                    wr.WriteNewLine();
                    wr.WriteBodyText("Receive locations running in this host", "artifactSubHeading");
                    wr.WriteNewLine();

                    int locCounter = 0;
                    foreach (ReceivePort rp in this.bi.ReceivePorts)
                    {
                        foreach (ReceiveLocation rl in rp.ReceiveLocations)
                        {
                            if (string.Compare(rl.ReceiveHandler.Name, host.Name, true) == 0)
                            {
                                locCounter++;
                                wr.WriteBodyText(rl.Name, "body");
                            }
                        }
                    }

                    if (locCounter == 0)
                    {
                        wr.WriteBodyText("There are no receive locations running in this host", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }
            }
        }

        #endregion

        #region WriteAssemblies

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        private void WriteAssemblies(WordXmlWriter wr)
        {
            int counter = 0;
            wr.WritePageBreak();
            wr.WriteBodyText("Assembly Configuration", "configSectionHeading");
            wr.WriteNewLine();

            foreach (BizTalkAssembly assembly in bi.Assemblies)
            {
                string name = assembly.DisplayName.Split(new char[] { ',' })[0];
                counter++;

                wr.WriteStartSubSection();
                wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, name), "artifactHeading", false, picAssembly, 14, 14);
                wr.WriteNewLine();

                wr.WriteBodyText("Name:  " + name, "body");
                wr.WriteBodyText("Version:  " + assembly.Version, "body");
                wr.WriteBodyText("Culture:  " + assembly.Culture, "body");
                wr.WriteBodyText("Public Key Token:  " + assembly.PublicKeyToken, "body");

                //============================================
                // Orchetrations in this assembly
                //============================================
                if (assembly.Orchestrations.Count > 0)
                {
                    wr.WriteNewLine();
                    wr.WriteBodyText("Orchestrations contained in this assembly", "artifactSubHeading");
                    wr.WriteNewLine();

                    foreach (NameIdPair orchestration in assembly.Orchestrations)
                    {
                        wr.WriteBodyText(orchestration.Name, "body");
                    }
                }

                //============================================
                // Maps in this assembly
                //============================================
                if (assembly.Maps.Count > 0)
                {
                    wr.WriteNewLine();
                    wr.WriteBodyText("Maps contained in this assembly", "artifactSubHeading");
                    wr.WriteNewLine();

                    foreach (NameIdPair map in assembly.Maps)
                    {
                        wr.WriteBodyText(map.Name, "body");
                    }
                }

                //============================================
                // Schema in this assembly
                //============================================
                if (assembly.Schemas.Count > 0)
                {
                    wr.WriteNewLine();
                    wr.WriteBodyText("Schema contained in this assembly", "artifactSubHeading");
                    wr.WriteNewLine();

                    foreach (NameIdPair schema in assembly.Schemas)
                    {
                        wr.WriteBodyText(schema.Name, "body");
                    }
                }

                wr.WriteNewLine();
                wr.WriteNewLine();
                wr.WriteEndSubSection();
            }
        }

        #endregion

        #region WriteSendPorts

        private void WriteSendPorts(WordXmlWriter wr)
        {
            if (this.bi.SendPorts.Count > 0)
            {
                int counter = 0;
                wr.WritePageBreak();
                wr.WriteBodyText("Send Port Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (SendPort port in this.bi.SendPorts)
                {
                    counter++;
                    wr.WriteStartSubSection();

                    string picData = port.TwoWay ? picSendPort2 : picSendPort1;

                    wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, port.Name), "artifactHeading", false, picData, 14, 14);
                    wr.WriteNewLine();

                    string trackingType = (int)port.TrackingType == 0 ? "Not specified" : port.TrackingType.ToString();

                    wr.WriteBodyText("Priority:  " + port.Priority.ToString(), "body");
                    wr.WriteBodyText("Tracking Type:  " + trackingType, "body");
                    wr.WriteBodyText("Send Pipeline:  " + port.SendPipeline, "body");
                    wr.WriteBodyText("Dynamic:  " + port.Dynamic.ToString(), "body");
                    wr.WriteBodyText("Two-Way:  " + port.TwoWay.ToString(), "body");

                    // Primary Transport
                    this.WriteTransportInfo(wr, port.PrimaryTransport, "Primary Transport");

                    // Secondary Transport
                    this.WriteTransportInfo(wr, port.SecondaryTransport, "Secondary Transport");

                    //============================================
                    // Orchestrations bound to this send port
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Orchestrations bound to this send port", "artifactSubHeading");
                    wr.WriteNewLine();

                    int orchCounter = 0;
                    foreach (NameIdPair boundOrch in port.BoundOrchestrations)
                    {
                        orchCounter++;
                        wr.WriteBodyText(boundOrch.Name, "body");
                    }


                    if (orchCounter == 0)
                    {
                        wr.WriteBodyText("There are no orchestrations bound to this send port", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteNewLine();

                    wr.WriteEndSubSection();
                }
            }
            return;
        }

        #endregion

        #region WriteProtocols

        private void WriteProtocols(WordXmlWriter wr)
        {
            if (this.bi.ProtocolTypes.Count > 0)
            {
                wr.WritePageBreak();
                wr.WriteBodyText("Protocol Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (Protocol protocol in this.bi.ProtocolTypes)
                {
                    wr.WriteStartSubSection();

                    wr.WriteBodyTextWithPreceedingImage("  " + protocol.Name, "artifactHeading", false, picProtocol, 16, 16);

                    wr.WriteNewLine();

                    wr.WriteBodyText("Delete protected: " + protocol.DeleteProtected.ToString(), "body");
                    wr.WriteBodyText("Uses adapter framework UI for receive handler configuration: " + protocol.InitInboundProtocolContext.ToString(), "body");
                    wr.WriteBodyText("Uses adapter framework UI for send handler configuration: " + protocol.InitOutboundProtocolContext.ToString(), "body");
                    wr.WriteBodyText("Uses adapter framework UI for receive location configuration: " + protocol.InitReceiveLocationContext.ToString(), "body");
                    wr.WriteBodyText("Uses adapter framework UI for send port configuration: " + protocol.InitTransmitLocationContext.ToString(), "body");
                    wr.WriteBodyText("Starts when the service starts: " + protocol.InitTransmitterOnServiceStart.ToString(), "body");
                    wr.WriteBodyText("Receive handler of adapter is hosted in-process: " + protocol.ReceiveIsCreatable.ToString(), "body");
                    wr.WriteBodyText("Requires a single instance per server: " + protocol.RequireSingleInstance.ToString(), "body");
                    wr.WriteBodyText("Supports static handlers: " + protocol.StaticHandlers.ToString(), "body");
                    wr.WriteBodyText("Supports ordered delivery: " + protocol.SupportsOrderedDelivery.ToString(), "body");
                    wr.WriteBodyText("Supports receive operations: " + protocol.SupportsReceive.ToString(), "body");
                    wr.WriteBodyText("Supports request-response operations: " + protocol.SupportsRequestResponse.ToString(), "body");
                    wr.WriteBodyText("Supports send operations: " + protocol.SupportsSend.ToString(), "body");
                    wr.WriteBodyText("Supports the SOAP protocol: " + protocol.SupportsSoap.ToString(), "body");
                    wr.WriteBodyText("Supports solicit-response operations: " + protocol.SupportsSolicitResponse.ToString(), "body");

                    //============================================
                    // What send ports are using this protocol
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Send ports using this protocol", "artifactSubHeading");
                    wr.WriteNewLine();

                    int portCounter = 0;
                    foreach (SendPort sp in this.bi.SendPorts)
                    {
                        bool added = false;
                        if (sp.PrimaryTransport != null)
                        {
                            if (string.Compare(sp.PrimaryTransport.Type, protocol.Name, true) == 0)
                            {
                                added = true;
                                portCounter++;
                                wr.WriteBodyText(sp.Name, "body");
                            }

                            if (!added && sp.SecondaryTransport != null)
                            {
                                if (string.Compare(sp.SecondaryTransport.Type, protocol.Name, true) == 0)
                                {
                                    added = true;
                                    portCounter++;
                                    wr.WriteBodyText(sp.Name, "body");
                                }
                            }
                        }
                    }

                    if (portCounter == 0)
                    {
                        wr.WriteBodyText("There are no send ports using this protocol", "body");
                    }

                    //============================================
                    // What receive ports are using this protocol
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Receive ports using this protocol", "artifactSubHeading");
                    wr.WriteNewLine();

                    portCounter = 0;
                    foreach (ReceivePort rp in this.bi.ReceivePorts)
                    {
                        foreach (ReceiveLocation rl in rp.ReceiveLocations)
                        {
                            if (string.Compare(rl.TransportProtocol, protocol.Name, true) == 0)
                            {
                                portCounter++;
                                wr.WriteBodyText(rp.Name, "body");
                            }
                        }
                    }

                    if (portCounter == 0)
                    {
                        wr.WriteBodyText("There are no receive ports using this protocol", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }
            }
            return;
        }

        #endregion

        #region WriteReceivePorts

        private void WriteReceivePorts(WordXmlWriter wr)
        {
            if (bi.ReceivePorts.Count > 0)
            {
                int counter = 0;
                wr.WritePageBreak();
                wr.WriteBodyText("Receive Port Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (ReceivePort port in bi.ReceivePorts)
                {
                    counter++;
                    wr.WriteStartSubSection();

                    wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, port.Name), "artifactHeading", false, picReceivePortSmall, 14, 14);
                    wr.WriteNewLine();

                    string trackingType = (int)port.TrackingType == 0 ? "Not specified" : port.TrackingType.ToString();
                    //string pipeline = (port.SendPipeline == null || port.SendPipeline == string.Empty) ? "Not specified" : port.SendPipeline;

                    wr.WriteBodyText("Authentication Type:  " + port.AuthenticationType.ToString(), "body");
                    wr.WriteBodyText("Tracking Type:  " + trackingType, "body");
                    //wr.WriteBodyText("Send Pipeline:  " + pipeline, "body");
                    wr.WriteBodyText("Two-Way:  " + port.TwoWay.ToString(), "body");

                    // Receive Ports
                    if (port.ReceiveLocations.Count > 0)
                    {
                        int counter2 = 0;
                        wr.WriteNewLine();
                        wr.WriteBodyText("Receive Locations", "artifactSubHeading");
                        wr.WriteNewLine();

                        foreach (ReceiveLocation rl in port.ReceiveLocations)
                        {
                            counter2++;
                            wr.WriteBodyText(string.Format("{0}. {1}", counter2, rl.Name), "bodyBold");
                            wr.WriteNewLine();

                            wr.WriteStartTable("tab1");

                            TableCellData[] tcd = new TableCellData[2];
                            tcd[0] = new TableCellData("Property", true, TableTitleBackground);
                            tcd[1] = new TableCellData("Value", true, TableTitleBackground);
                            wr.WriteTableRow(tcd);

                            tcd[0] = new TableCellData("Address", true, TableTitleColumnBackground);
                            tcd[1] = new TableCellData(rl.Address);
                            wr.WriteTableRow(tcd);

                            tcd[0] = new TableCellData("Transport Protocol", true, TableTitleColumnBackground);
                            tcd[1] = new TableCellData(rl.TransportProtocol);
                            wr.WriteTableRow(tcd);

                            tcd[0] = new TableCellData("Receive Pipeline", true, TableTitleColumnBackground);
                            tcd[1] = new TableCellData(rl.ReceivePipeline.Name);
                            wr.WriteTableRow(tcd);

                            tcd[0] = new TableCellData("Receive Handler", true, TableTitleColumnBackground);
                            tcd[1] = new TableCellData(rl.ReceiveHandler.Name);
                            wr.WriteTableRow(tcd);

                            wr.WriteEndTable();
                        }
                    }

                    //============================================
                    // What orchestrations are bound to this port
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Orchestrations bound to this receive port", "artifactSubHeading");
                    wr.WriteNewLine();

                    int orchCounter = 0;
                    foreach (NameIdPair boundOrch in port.BoundOrchestrations)
                    {
                        orchCounter++;
                        wr.WriteBodyText(boundOrch.Name, "body");
                    }

                    if (orchCounter == 0)
                    {
                        wr.WriteBodyText("There are no orchestrations bound to this receive port", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }
            }
            return;
        }

        #endregion

        #region WriteOrchestrations

        private void WriteOrchestrations(WordXmlWriter wr)
        {
            if (bi.Orchestrations.Count > 0)
            {
                wr.WritePageBreak();
                wr.WriteBodyText("Orchestration Configuration", "configSectionHeading");
                wr.WriteNewLine();
                int counter = 0;
                foreach (Orchestration o in bi.Orchestrations)
                {
                    counter++;
                    wr.WriteStartSubSection();

                    wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, o.Name), "artifactHeading", false, picOrchestration, 14, 14);
                    wr.WriteNewLine();
                    wr.WriteBodyText("Parent Assembly:  " + o.ParentAssemblyFormattedName, "body");
                    wr.WriteBodyText("Host:  " + o.Host.Name, "body");

                    this.WriteOrchestrationPorts(wr, o);
                    this.WriteInvokedOrchestrations(wr, o);

                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }


            }

            return;
        }

        private void WriteInvokedOrchestrations(WordXmlWriter wr, Orchestration o)
        {
            if (o.InvokedOrchestrations.Count > 0)
            {
                wr.WriteNewLine();
                wr.WriteBodyText("Invoked orchestrations", "artifactSubHeading");
                wr.WriteNewLine();

                foreach (Orchestration io in o.InvokedOrchestrations)
                {
                    wr.WriteBodyText(io.Name, "body");
                }
                wr.WriteNewLine();
            }
        }

        private void WriteOrchestrationPorts(WordXmlWriter wr, Orchestration o)
        {
            if (o.Ports.Count > 0)
            {
                wr.WriteNewLine();
                wr.WriteBodyText("Ports contained within this orchestration", "artifactSubHeading");
                wr.WriteNewLine();

                foreach (OrchestrationPort p in o.Ports)
                {
                    string boundPortName = string.Empty;

                    if (p.SendPortName != null && p.SendPortName.Name != string.Empty)
                    {
                        boundPortName = p.SendPortName.Name;
                    }
                    else if (p.SendPortGroupName != null && p.SendPortGroupName.Name != string.Empty)
                    {
                        boundPortName = p.SendPortGroupName.Name;
                    }
                    else if (p.ReceivePortName != null && p.ReceivePortName.Name != string.Empty)
                    {
                        boundPortName = p.ReceivePortName.Name;
                    }

                    if (boundPortName != string.Empty)
                    {
                        boundPortName = p.Name + " - Bound to: " + boundPortName;
                    }
                    else
                    {
                        boundPortName = p.Name + " - UnBound";
                    }

                    wr.WriteBodyText(boundPortName, "body");
                }
                wr.WriteNewLine();
            }
        }

        #endregion

        #region WriteSendPortGroups

        private void WriteSendPortGroups(WordXmlWriter wr)
        {
            if (this.bi.SendPortGroups.Count > 0)
            {
                wr.WritePageBreak();
                wr.WriteBodyText("Send Port Group Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (SendPortGroup group in this.bi.SendPortGroups)
                {
                    wr.WriteStartSubSection();

                    wr.WriteBodyTextWithPreceedingImage("  " + group.Name, "artifactHeading", false, picSendPort1, 14, 14);
                    wr.WriteNewLine();

                    wr.WriteBodyText("Send ports contained in this group", "artifactSubHeading");
                    wr.WriteNewLine();

                    foreach (NameIdPair sendPort in group.SendPorts)
                    {
                        wr.WriteBodyText(sendPort.Name, "body");
                    }

                    //============================================
                    // Orchestrations bound to this send port group
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Orchestrations bound to this send port group", "artifactSubHeading");
                    wr.WriteNewLine();

                    int orchCounter = 0;
                    foreach (NameIdPair boundOrch in group.BoundOrchestrations)
                    {
                        orchCounter++;
                        wr.WriteBodyText(boundOrch.Name, "body");
                    }

                    if (orchCounter == 0)
                    {
                        wr.WriteBodyText("There are no orchestrations bound to this send port group", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }
            }
            return;
        }

        #endregion

        #region WritePipelines

        private void WritePipelines(WordXmlWriter wr)
        {
            if (this.bi.Pipelines.Count > 0)
            {
                int counter = 0;
                wr.WritePageBreak();
                wr.WriteBodyText("Pipeline Configuration", "configSectionHeading");
                wr.WriteNewLine();

                foreach (Pipeline pipeline in this.bi.Pipelines)
                {
                    counter++;
                    wr.WriteStartSubSection();

                    wr.WriteBodyTextWithPreceedingImage(string.Format("  {0}. {1}", counter, pipeline.Name), "artifactHeading", false, picPipeline, 14, 14);

                    wr.WriteNewLine();

                    wr.WriteBodyText("Pipeline Type: " + pipeline.PipelineType.ToString(), "body");
                    wr.WriteBodyText("Assembly Name: " + pipeline.ParentAssembly.Name, "body");

                    //============================================
                    // Process Flow
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Pipeline Process Flow", "artifactSubHeading");

                    XmlDocument pipelineData = new XmlDocument();
                    pipelineData.LoadXml(pipeline.ViewData);

                    XmlNodeList stageNodes = pipelineData.SelectNodes("./Document/Stages/Stage/PolicyFileStage");

                    int j = 0;
                    foreach (XmlNode stageNode in stageNodes)
                    {
                        j++;
                        wr.WriteNewLine();
                        wr.WriteBodyText(j.ToString() + ".  " + stageNode.Attributes.GetNamedItem("Name").Value, "bodyBold");

                        XmlNodeList componentNodes = stageNode.SelectNodes("../Components/Component");

                        int i = 0;
                        bool found = false;
                        foreach (XmlNode componentNode in componentNodes)
                        {
                            found = true;
                            i++;
                            wr.WriteNewLine();
                            wr.WriteBodyText("     " + componentNode.SelectSingleNode("./ComponentName").InnerText, "bodyBold");
                            wr.WriteBodyText("     " + componentNode.SelectSingleNode("./Name").InnerText, "body");
                        }

                        if (found)
                        {
                            wr.WriteNewLine();
                        }
                    }

                    //============================================
                    // Send ports using this pipeline
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Send ports using this pipeline", "artifactSubHeading");
                    wr.WriteNewLine();

                    int portCount = 0;
                    foreach (SendPort sp in this.bi.SendPorts)
                    {
                        bool added = false;
                        //if (string.Compare(sp.SendPipeline, pipeline.Name, true) == 0)
                        if (string.Compare(sp.SendPipeline.Name, pipeline.Name, true) == 0)
                        {
                            added = true;
                            portCount++;
                            wr.WriteBodyText(sp.Name, "body");
                        }

                        if (!added)
                        {
                            //if (string.Compare(sp.ReceivePipeline, pipeline.Name, true) == 0)
                            if (string.Compare(sp.ReceivePipeline.Name, pipeline.Name, true) == 0)
                            {
                                added = true;
                                portCount++;
                                wr.WriteBodyText(sp.Name, "body");
                            }
                        }
                    }

                    if (portCount == 0)
                    {
                        wr.WriteBodyText("There are no send ports using this pipeline", "body");
                    }

                    //============================================
                    // Receive locations using this pipeline
                    //============================================
                    wr.WriteNewLine();
                    wr.WriteBodyText("Receive locations using this pipeline", "artifactSubHeading");
                    wr.WriteNewLine();

                    portCount = 0;
                    foreach (ReceivePort rp in this.bi.ReceivePorts)
                    {
                        foreach (ReceiveLocation rl in rp.ReceiveLocations)
                        {
                            if (string.Compare(rl.ReceivePipeline.Name, pipeline.Name, true) == 0)
                            {
                                portCount++;
                                wr.WriteBodyText(rl.Name, "body");
                            }
                        }
                    }

                    if (portCount == 0)
                    {
                        wr.WriteBodyText("There are no receive locations using this pipeline", "body");
                    }

                    wr.WriteNewLine();
                    wr.WriteNewLine();
                    wr.WriteEndSubSection();
                }
            }
            return;
        }

        #endregion

        #region WriteTransportInfo

        private void WriteTransportInfo(WordXmlWriter wr, TransportInfo ti, string title)
        {
            wr.WriteNewLine();
            wr.WriteBodyText(title, "artifactSubHeading", false);
            wr.WriteNewLine();
            if (ti != null && ti.Type != string.Empty)
            {
                wr.WriteStartTable("tab1");

                TableCellData[] tcd = new TableCellData[2];
                tcd[0] = new TableCellData("Property", true, TableTitleBackground);
                tcd[1] = new TableCellData("Value", true, TableTitleBackground);
                wr.WriteTableRow(tcd);

                tcd[0] = new TableCellData("Address", true, TableTitleColumnBackground);
                tcd[1] = new TableCellData(ti.Address);
                wr.WriteTableRow(tcd);

                tcd[0] = new TableCellData("Type", true, TableTitleColumnBackground);
                tcd[1] = new TableCellData(ti.Type);
                wr.WriteTableRow(tcd);

                tcd[0] = new TableCellData("Ordered Delivery", true, TableTitleColumnBackground);
                tcd[1] = new TableCellData(ti.OrderedDelivery.ToString());
                wr.WriteTableRow(tcd);

                tcd[0] = new TableCellData("Retry Count", true, TableTitleColumnBackground);
                tcd[1] = new TableCellData(ti.RetryCount.ToString());
                wr.WriteTableRow(tcd);

                tcd[0] = new TableCellData("Retry Interval", true, TableTitleColumnBackground);
                tcd[1] = new TableCellData(ti.RetryInterval.ToString());
                wr.WriteTableRow(tcd);

                wr.WriteEndTable();

                if (ti.ServiceWindow != null)
                {
                    wr.WriteNewLine();
                    wr.WriteBodyText("Service Window", "bodyBold");
                    wr.WriteNewLine();
                    wr.WriteBodyText("Enabled: " + ti.ServiceWindow.Enabled.ToString(), "body");
                    wr.WriteBodyText("Start Date: " + ti.ServiceWindow.StartTime.ToString(), "body");
                    wr.WriteBodyText("End Date: " + ti.ServiceWindow.EndTime.ToString(), "body");
                }
            }
            else
            {
                wr.WriteBodyText("Not yet specified", "body", false);
            }
        }

        #endregion

        private void UpdatePercentageComplete(int percentage)
        {
            if (this.PercentageDocumentationComplete != null)
            {
                this.PercentageDocumentationComplete(percentage);
            }
        }

        #region IPublisher Members


        //public void Publish(BizTalkInstallation bi, PublishType publishType, string resourceFolder, string publishFolder, string reportTitle, bool publishRules)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
