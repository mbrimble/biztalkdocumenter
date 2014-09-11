
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.Services.Tools.BizTalkOM;
using BizTalk.Utilities.SSO.Core;
using System.Collections;
using Microsoft.Services.Tools.BiztalkDocumenter;

namespace Microsoft.Services.Tools.BiztalkDocumenter.Publishers
{
    /// <summary>
    /// Summary description for CompiledHelpPublisher.
    /// </summary>
    public class CompiledHelpPublisher : IPublisher
    {
        private string targetDir;
        private string publishingFolder;
        private string resourceFolder = string.Empty;
        private HelpFileWriter hfw;
        private string btsOutputDir = Path.GetTempPath();
        private string FILE_NAME = "BTSDoc.chm";
        private BizTalkInstallation bi;
        private const string resourcePrefix = "Microsoft.Services.Tools.Publishers.CompiledHelp";
        private string[] sso;
        private PublishType publishType; //MTB 08/03/2014
        private string[] ssoApps;//MTB 08/03/2014
        private string bizTalkConfigPath;//MTB 08/03/2014

        #region StyleSheets
        
        private XslCompiledTransform appTransform = new XslCompiledTransform();
        private XslCompiledTransform asmTransform = new XslCompiledTransform();
        private XslCompiledTransform configTransform = new XslCompiledTransform();
        private XslCompiledTransform correlationTypeTransform = new XslCompiledTransform();
        private XslCompiledTransform hostTransform = new XslCompiledTransform();
        private XslCompiledTransform mapTransform = new XslCompiledTransform();
        private XslCompiledTransform map2Transform = new XslCompiledTransform(); //MTB 08/03/2014
        private XslCompiledTransform orchCodeTransform = new XslCompiledTransform();
        private XslCompiledTransform orchestrationImageTransform = new XslCompiledTransform();
        private XslCompiledTransform orchTransform = new XslCompiledTransform();
        private XslCompiledTransform partyTransform = new XslCompiledTransform();
        private XslCompiledTransform pipelineTransform = new XslCompiledTransform();
        private XslCompiledTransform pipelineDisplayTransform = new XslCompiledTransform();
        private XslCompiledTransform policyTransform = new XslCompiledTransform();
        private XslCompiledTransform protocolTransform = new XslCompiledTransform();
        private XslCompiledTransform roleTransform = new XslCompiledTransform();
        private XslCompiledTransform rpTransform = new XslCompiledTransform();
        private XslCompiledTransform schemaTransform = new XslCompiledTransform();
        private XslCompiledTransform sendPortGroupTransform = new XslCompiledTransform();
        private XslCompiledTransform sendPortTransform = new XslCompiledTransform();
        private XslCompiledTransform ssoTransform = new XslCompiledTransform(); //MTB 08/03/2014
        private XslCompiledTransform vocabularyTransform = new XslCompiledTransform();
 
        #endregion

        public CompiledHelpPublisher()
        {
        }

        public event UpdatePercentageComplete PercentageDocumentationComplete;

        private void WriteTransformedXmlDataToFile(string fileName, string xmlData, XslCompiledTransform transform, XsltArgumentList args)
        {
            StreamWriter sw = new StreamWriter(File.Create(fileName));
            this.WriteTransformedXmlDataToStream(sw.BaseStream, xmlData, transform, args);
            sw.Close();
        }

        private string WriteTransformedXmlDataToString(string xmlData, XslCompiledTransform transform, XsltArgumentList args)
        {
            MemoryStream ms = new MemoryStream();
            this.WriteTransformedXmlDataToStream(ms, xmlData, transform, args);

            ms.Position = 0;
            string s = Encoding.ASCII.GetString(ms.GetBuffer());
            ms.Close();
            ms = null;

            return s;
        }

        private void WriteTransformedXmlDataToStream(Stream s, string xmlData, XslCompiledTransform transform, XsltArgumentList args)
        {
            XPathDocument data = new XPathDocument(new MemoryStream(Encoding.UTF8.GetBytes(xmlData)));
            transform.Transform(data, args, s);
            s.Flush();
            data = null;
        }

        private void WriteDataToFile(string fileName, string data)
        {
            StreamWriter sw = new StreamWriter(File.Create(fileName));
            sw.WriteLine(data);
            sw.Flush();
            sw.Close();
        }

        #region IPublisher Members

        public bool Prepare()
        {
            string hhCompilerLocation = HelpFileWriter.GetCompilerLocation();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bi"></param>
        /// <param name="publishType"></param>
        /// <param name="resourceFolder"></param>
        /// <param name="publishFolder"></param>
        /// <param name="reportTitle"></param>
        /// <param name="publishRules"></param>
        /// <remarks>This now deprecated</remarks>
        public void Publish(BizTalkInstallation bi, PublishType publishType, string resourceFolder, string publishFolder, string reportTitle, bool publishRules)
        {
            this.bi = bi;
            this.resourceFolder = resourceFolder;
            FILE_NAME = Path.Combine(publishFolder, reportTitle + ".chm");

            this.publishingFolder = publishFolder;
            //force the target dir to be a subset of the working folder
            this.targetDir = Path.Combine(publishFolder, @"~working\BTSDOC");


            this.PrepareFilesAndDirectories(reportTitle);

            this.LoadStylesheets();

            XsltArgumentList xsltArgs = CreateTransformParameterSets();
            HelpFileNode hfnWorkingNode;

            this.WriteApplications(bi, xsltArgs);
            this.WriteParties(bi, xsltArgs);
            this.UpdatePercentageComplete(10);
            this.WritePlatformSettings(bi, xsltArgs);
            this.WriteRules(bi, publishRules);

            //============================================
            // Now we can compile the help file...
            //============================================
#if (DEBUG)
            //string debugPath = Path.Combine(Path.GetTempPath(), "DebugBizTalkInstallation.xml");
            string debugPath = Path.Combine(this.publishingFolder, "DebugBizTalkInstallation.xml");
            bi.SaveToFile(debugPath);
            hfw.RootNode.CreateChild("Debug", debugPath);
#endif

            hfw.Compile();
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
            this.bi = bi;
            this.resourceFolder = resourceFolder;
            FILE_NAME = Path.Combine(publishFolder, reportTitle + ".chm");
            sso = ssoLocations; //MTB 08/03/2014
            this.publishingFolder = publishFolder;
            //force the target dir to be a subset of the working folder
            this.targetDir = Path.Combine(publishFolder, @"~working\BTSDOC");
            this.ssoApps = ssoApplications; //MTB 08/03/2014
            this.bizTalkConfigPath = bizTalkConfigurationPath; //MTB 08/03/2014

            this.PrepareFilesAndDirectories(reportTitle);

            this.LoadStylesheets();

            XsltArgumentList xsltArgs = CreateTransformParameterSets();
            HelpFileNode hfnWorkingNode;

            this.WriteApplications(bi, xsltArgs);
            this.WriteSso(xsltArgs); //MTB 08/03/2014
            this.WriteParties(bi, xsltArgs);
            this.UpdatePercentageComplete(10);
            this.WritePlatformSettings(bi, xsltArgs);
            this.WriteRules(bi, publishRules);
            this.WriteConfiguration(xsltArgs);
            this.WriteAdditionalNotes();

            //============================================
            // Now we can compile the help file...
            //============================================
#if (DEBUG)
            //string debugPath = Path.Combine(Path.GetTempPath(), "DebugBizTalkInstallation.xml");
            string debugPath = Path.Combine(this.publishingFolder, "DebugBizTalkInstallation.xml");
            bi.SaveToFile(debugPath);
            hfw.RootNode.CreateChild("Debug", debugPath);
#endif

            hfw.Compile();
            this.UpdatePercentageComplete(100);
        }


        private void WriteRules(BizTalkInstallation bi, bool publishRules)
        {
            if (publishRules)
            {
                HelpFileNode rulesNode = null;

                RulesEngineHelper reh = new RulesEngineHelper(bi.RulesServer, bi.RulesDatabase);
                reh.PrepareVocabs();

                BizTalkBaseObjectCollectionEx rules = reh.GetRuleSets();

                if (rules.Count > 0)
                {
                    if (rulesNode == null)
                    {
                        rulesNode = this.hfw.RootNode.CreateChild("Business Rules Engine");
                    }

                    HelpFileNode hfn = rulesNode.CreateChild("Policies");

                    foreach (RuleArtifact ra in rules)
                    {
                        string fileName = reh.ExportRuleSetToFile(ra, Path.Combine(this.targetDir, "Policy"));

                        if (fileName != null)
                        {
                            hfn.CreateChild(ra.FullName, fileName);
                        }
                    }

                    hfn.SortChildren();
                }

                BizTalkBaseObjectCollectionEx vocs = reh.GetVocabularies();

                if (vocs.Count > 0)
                {
                    if (rulesNode == null)
                    {
                        rulesNode = this.hfw.RootNode.CreateChild("Business Rules Engine");
                    }

                    HelpFileNode hfn = rulesNode.CreateChild("Vocabularies");

                    foreach (RuleArtifact ra in vocs)
                    {
                        string fileName = reh.ExportVocabularyToFile(ra, Path.Combine(this.targetDir, "Vocabulary"));

                        if (fileName != null)
                        {
                            string htmlFileName = Path.Combine(this.targetDir, "Vocabulary/" + ra.HtmlFileName);

                            //this.vocabularyTransform.Transform(fileName, htmlFileName, new XmlUrlResolver()); MTB 30/06/2013

                            this.vocabularyTransform.Transform(fileName, htmlFileName);

                            hfn.CreateChild(ra.FullName, htmlFileName);
                        }
                    }

                    hfn.SortChildren();
                }
            }
        }

        private void WritePlatformSettings(BizTalkInstallation bi, XsltArgumentList xsltArgs)
        {
            HelpFileNode hfnPlatformNode = this.hfw.RootNode.CreateChild("Platform Settings");

            #region Hosts
            //============================================
            // Hosts
            //============================================

            if (bi.Hosts.Count > 0)
            {
                HelpFileNode hfn = null;

                foreach (Host host in bi.Hosts)
                {
                    if (hfn == null)
                    {
                        hfn = hfnPlatformNode.CreateChild("Hosts");
                    }

                    string fileName = "Host\\" + host.Id + ".htm";

                    // Write the index page entry
                    HelpFileNode childNode = hfn.CreateChild(host.Name, fileName);

                    this.WriteTransformedXmlDataToFile(
                        Path.Combine(this.targetDir, fileName),
                        host.GetXml(),
                        this.hostTransform,
                        xsltArgs);
                }

                hfn.SortChildren();
            }
            #endregion
            this.UpdatePercentageComplete(20);

            #region Protocols
            //============================================
            // Protocols
            //============================================
            if (bi.ProtocolTypes.Count > 0)
            {
                HelpFileNode hfn = null;

                foreach (Protocol protocol in bi.ProtocolTypes)
                {
                    if (hfn == null)
                    {
                        hfn = hfnPlatformNode.CreateChild("Adapters");
                    }

                    string fileName = "Protocol\\" + protocol.Id + ".htm";

                    // Write the index page entry
                    hfn.CreateChild(protocol.Name, fileName);

                    this.WriteTransformedXmlDataToFile(
                        Path.Combine(this.targetDir, fileName),
                        protocol.GetXml(),
                        this.protocolTransform,
                        xsltArgs);
                }

                if (hfn != null)
                {
                    hfn.SortChildren();
                }
            }
            #endregion
            this.UpdatePercentageComplete(30);
        }

        private void WriteParties(BizTalkInstallation bi, XsltArgumentList xsltArgs)
        {
            if (bi.Parties.Count > 0)
            {
                HelpFileNode hfnPartiesNode = this.hfw.RootNode.CreateChild("Parties");

                foreach (Party party in bi.Parties)
                {
                    string fileName = "Party\\" + party.Id + ".htm";

                    // Write the index page entry
                    HelpFileNode childNode = hfnPartiesNode.CreateChild(party.Name, fileName);

                    this.WriteTransformedXmlDataToFile(
                        Path.Combine(this.targetDir, fileName),
                        party.GetXml(),
                        this.partyTransform,
                        xsltArgs);
                }
            }
        }

        private void WriteApplications(BizTalkInstallation bi, XsltArgumentList xsltArgs)
        {
            HelpFileNode hfnWorkingNode;
            if (bi.Applications.Count > 0)
            {
                HelpFileNode hfn = this.hfw.RootNode.CreateChild("Applications");

                foreach (BizTalkApplication application in bi.Applications)
                {
                    string appHtmlFileName = Path.Combine(this.targetDir, "Application\\" + application.Id + ".html");

                    this.WriteTransformedXmlDataToFile(
                        Path.Combine(this.targetDir, appHtmlFileName),
                        application.GetXml(),
                        this.appTransform,
                        xsltArgs);

                    HelpFileNode applicationNode = hfn.CreateChild(application.Name, appHtmlFileName);

                    #region Orchestrations
                    //============================================
                    // Orchestrations
                    //============================================

                    if (application.Orchestrations.Count > 0)
                    {
                        HelpFileNode hfnOrchs = null;

                        foreach (Orchestration o in application.Orchestrations)
                        {
                            try
                            {
                                if (hfnOrchs == null)
                                {
                                    hfnOrchs = applicationNode.CreateChild("Orchestrations");
                                }

                                string fileName = "Orchestration/" + o.Id + ".htm";
                                string fileName2 = "Orchestration/Overview" + o.Id + ".htm";
                                string fileName3 = "Orchestration/CorrelationTypes" + o.Id + ".htm";
                                string fileName4 = "Orchestration/Code" + o.Id + ".htm";
                                string fileName5 = "Code" + o.Id + ".htm";

                                string imgName = string.Empty;

                                #region Save Image

                                if (o.ViewData != null && o.ViewData != string.Empty)
                                {
                                    imgName = Path.Combine(this.targetDir, "Orchestration/" + o.Id + ".jpg");
                                    try
                                    {
                                        o.SaveAsImage(imgName);
                                    }
                                    catch (Exception ex)
                                    {
                                        imgName = string.Empty;
                                    }
                                }

                                #endregion

                                this.WriteTransformedXmlDataToFile(
                                    Path.Combine(this.targetDir, fileName),
                                    o.GetXml(),
                                    this.orchTransform,
                                    xsltArgs);

                                HelpFileNode hfnOrch = null;
                                hfnOrch = hfnOrchs.CreateChild(o.Name, fileName);

                                if (imgName != string.Empty)
                                {
                                    // Write the index page entry
                                    hfnOrch.CreateChild("Process Overview", fileName2);

                                    // Process overview
                                    XsltArgumentList orchXsltArgs = new XsltArgumentList();
                                    orchXsltArgs.AddParam("ImgFile", "", imgName);
                                    orchXsltArgs.AddParam("CodeFile", "", fileName5);

                                    this.WriteTransformedXmlDataToFile(
                                        Path.Combine(this.targetDir, fileName2),
                                        o.GetXml(),
                                        this.orchestrationImageTransform,
                                        orchXsltArgs);
                                }

                                #region Correlation types

                                // Correlation types
                                if (hfnOrch != null && o.CorrelationSetTypes.Count > 0)
                                {
                                    hfnOrch.CreateChild("Correlation Set Types", fileName3);

                                    this.WriteTransformedXmlDataToFile(
                                        Path.Combine(this.targetDir, fileName3),
                                        o.GetXml(),
                                        this.correlationTypeTransform,
                                        xsltArgs);
                                }

                                #endregion

                                #region Code Elements

                                if (hfnOrch != null && o.ShapeMap.Count > 0)
                                {
                                    hfnOrch.CreateChild("Code Elements", fileName4);

                                    // Code Elements
                                    XsltArgumentList orchCodeXsltArgs = new XsltArgumentList();
                                    orchCodeXsltArgs.AddParam("OrchName", "", o.Name);

                                    this.WriteTransformedXmlDataToFile(
                                        Path.Combine(this.targetDir, fileName4),
                                        o.ArtifactData,
                                        this.orchCodeTransform,
                                        orchCodeXsltArgs);
                                }

                                #endregion
                            }
                            catch (Exception)
                            {
                            }
                        }

                        if (hfnOrchs != null)
                        {
                            hfnOrchs.SortChildren();
                        }
                    }

                    #endregion
                    this.UpdatePercentageComplete(10);

                    #region Role Links
                    //============================================
                    // Role Links
                    //============================================
                    if (application.RoleLinks.Count > 0)
                    {
                        HelpFileNode hfnRoleLinks = applicationNode.CreateChild("Role Links");

                        foreach (Role role in application.RoleLinks)
                        {
                            string fileName = "Role\\" + role.Id + ".htm";

                            // Write the index page entry
                            hfnRoleLinks.CreateChild(role.Name, fileName);

                            this.WriteTransformedXmlDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                role.GetXml(),
                                this.roleTransform,
                                xsltArgs);
                        }

                        hfnRoleLinks.SortChildren();
                    }
                    #endregion
                    this.UpdatePercentageComplete(20);

                    #region Assemblies
                    //============================================
                    // Assemblies
                    //============================================

                    if (application.Assemblies.Count > 0)
                    {
                        HelpFileNode hfnAssemblies = null;

                        foreach (BizTalkAssembly assembly in application.Assemblies)
                        {
                            if (hfnAssemblies == null)
                            {
                                hfnAssemblies = applicationNode.CreateChild("Assemblies");
                            }

                            string fileName = "Assembly\\" + assembly.Id + ".htm";

                            // Write the index page entry
                            HelpFileNode hfnChildNode = hfnAssemblies.CreateChild(assembly.DisplayName, fileName);

                            this.WriteTransformedXmlDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                assembly.GetXml(),
                                this.asmTransform,
                                xsltArgs);
                        }

                        if (hfnAssemblies != null)
                        {
                            hfnAssemblies.SortChildren();
                        }
                    }

                    #endregion
                    this.UpdatePercentageComplete(30);

                    #region SendPortGroups
                    //============================================
                    // Send port groups
                    //============================================
                    if (application.SendPortGroups.Count > 0)
                    {
                        HelpFileNode hfnSendPortGroups = applicationNode.CreateChild("Send Port Groups");

                        foreach (SendPortGroup group in application.SendPortGroups)
                        {
                            string fileName = "SendPortGroup\\" + group.Id + ".htm";

                            // Write the index page entry
                            hfnSendPortGroups.CreateChild(group.Name, fileName);

                            this.WriteTransformedXmlDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                group.GetXml(),
                                this.sendPortGroupTransform,
                                xsltArgs);
                        }

                        hfnSendPortGroups.SortChildren();
                    }
                    #endregion
                    this.UpdatePercentageComplete(40);

                    #region SendPorts
                    //============================================
                    // Send ports
                    //============================================
                    if (application.SendPorts.Count > 0)
                    {
                        HelpFileNode hfnSendPorts = null;

                        foreach (SendPort port in application.SendPorts)
                        {
                            if (hfnSendPorts == null)
                            {
                                hfnSendPorts = applicationNode.CreateChild("Send Ports");
                            }

                            XmlDocument sendPortDoc = new XmlDocument();
                            sendPortDoc.LoadXml(port.GetXml());

                            #region Fixes for links in html

                            //// Fix send pipeline id
                            //try
                            //{
                            //    // Fix send pipeline id
                            //    if (this.bi.Application == null||
                            //        (this.bi.Application != null && this.bi.Application.ContainsArtifact(port.SendPipeline, ArtifactType.Pipeline)))
                            //    {
                            //        XmlAttribute sendPipelineId = sendPortDoc.CreateNode(XmlNodeType.Attribute, "id", "") as XmlAttribute;
                            //        sendPipelineId.Value = bi.AllItemsIdMap[port.SendPipeline].ToString();
                            //        sendPortDoc.SelectSingleNode("./BizTalkBaseObject/SendPipeline").Attributes.Append(sendPipelineId);
                            //    }

                            //    // Fix receive pipeline id
                            //    if (port.TwoWay)
                            //    {
                            //        if (this.bi.Application == null||
                            //            (this.bi.Application != null && this.bi.Application.ContainsArtifact(port.ReceivePipeline, ArtifactType.Pipeline)))
                            //        {
                            //            XmlAttribute receivePipelineId = sendPortDoc.CreateNode(XmlNodeType.Attribute, "id", "") as XmlAttribute;
                            //            receivePipelineId.Value = bi.AllItemsIdMap[port.ReceivePipeline].ToString();
                            //            sendPortDoc.SelectSingleNode("./BizTalkBaseObject/ReceivePipeline").Attributes.Append(receivePipelineId);
                            //        }
                            //    }

                            //    // Fix primary transport id
                            //    if (port.PrimaryTransport != null)
                            //    {
                            //        if (this.bi.Application == null||
                            //            (this.bi.Application != null && this.bi.Application.ContainsArtifact(port.PrimaryTransport.Type, ArtifactType.Protocol)))
                            //        {
                            //            XmlAttribute primaryTransportId = sendPortDoc.CreateNode(XmlNodeType.Attribute, "id", "") as XmlAttribute;
                            //            primaryTransportId.Value = bi.AllItemsIdMap[port.PrimaryTransport.Type].ToString();
                            //            sendPortDoc.SelectSingleNode("./BizTalkBaseObject/PrimaryTransport/Type").Attributes.Append(primaryTransportId);
                            //        }
                            //    }

                            //    // Fix secondary transport id
                            //    if (port.SecondaryTransport != null)
                            //    {
                            //        if (this.bi.Application == null||
                            //            (this.bi.Application != null && this.bi.Application.ContainsArtifact(port.SecondaryTransport.Type, ArtifactType.Protocol)))
                            //        {
                            //            XmlAttribute secondaryTransportId = sendPortDoc.CreateNode(XmlNodeType.Attribute, "id", "") as XmlAttribute;
                            //            secondaryTransportId.Value = bi.AllItemsIdMap[port.SecondaryTransport.Type].ToString();
                            //            sendPortDoc.SelectSingleNode("./BizTalkBaseObject/SecondaryTransport/Type").Attributes.Append(secondaryTransportId);
                            //        }
                            //    }      
                            //}
                            //catch(Exception ex)
                            //{
                            //    // swallow exception - just means that links will not be created
                            //} 

                            #endregion

                            //XmlNode BoundOrchNode = sendPortDoc.CreateNode(XmlNodeType.Element, "BoundOrchestrations", "");
                            //sendPortDoc.DocumentElement.AppendChild(BoundOrchNode);

                            //foreach (XmlNode orchNode in orchestrationNodes)
                            //{
                            //    XmlNodeList portNodes = orchNode.SelectNodes("Ports/OrchestrationPort/SendPortName");

                            //    foreach (XmlNode portNode in portNodes)
                            //    {
                            //        if (string.Compare(portNode.InnerText, port.Name) == 0)
                            //        {
                            //            XmlNode boundOrchNode = sendPortDoc.CreateNode(XmlNodeType.Element, "Orchestration", "");
                            //            boundOrchNode.InnerText = orchNode.SelectSingleNode("Name").InnerText;

                            //            if (publishType != PublishType.SchemaOnly)
                            //            {
                            //                XmlAttribute id = sendPortDoc.CreateNode(XmlNodeType.Attribute, "id", "") as XmlAttribute;
                            //                id.Value = orchNode.SelectSingleNode("Id").InnerText;
                            //                boundOrchNode.Attributes.Append(id);
                            //            }

                            //            BoundOrchNode.AppendChild(boundOrchNode);
                            //        }
                            //    }
                            //}

                            string fileName = "SendPort\\" + port.Id + ".htm";

                            // Write the index page entry
                            hfnSendPorts.CreateChild(port.Name, fileName);

                            this.WriteTransformedXmlDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                port.GetXml(),
                                this.sendPortTransform,
                                xsltArgs);

                            try
                            {
                                if (port.PrimaryTransport != null && port.PrimaryTransport.Data != string.Empty)
                                {
                                    this.WriteDataToFile(
                                        Path.Combine(this.targetDir, "SendPort\\" + port.Id + "PTData.xml"),
                                        port.PrimaryTransport.Data);
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                if (port.SecondaryTransport != null && port.SecondaryTransport.Data != string.Empty)
                                {
                                    this.WriteDataToFile(
                                        Path.Combine(this.targetDir, "SendPort\\" + port.Id + "STData.xml"),
                                        port.SecondaryTransport.Data);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        if (hfnSendPorts != null)
                        {
                            hfnSendPorts.SortChildren();
                        }
                    }
                    #endregion
                    this.UpdatePercentageComplete(50);

                    #region ReceivePorts
                    //============================================
                    // Receive ports
                    //============================================
                    if (application.ReceivePorts.Count > 0)
                    {
                        HelpFileNode hfnReceivePorts = null;

                        foreach (ReceivePort port in application.ReceivePorts)
                        {
                            if (hfnReceivePorts == null)
                            {
                                hfnReceivePorts = applicationNode.CreateChild("Receive Ports");
                            }

                            string fileName = string.Format("ReceivePort/{0}.htm", port.Id);

                            // Write the index page entry
                            hfnReceivePorts.CreateChild(port.Name, fileName);

                            this.WriteTransformedXmlDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                port.GetXml(),
                                this.rpTransform,
                                xsltArgs);

                            foreach (ReceiveLocation location in port.ReceiveLocations)
                            {
                                try
                                {
                                    if (location.Data != string.Empty)
                                    {
                                        this.WriteDataToFile(
                                            Path.Combine(this.targetDir, string.Format(@"ReceivePort\{0}Data.xml", location.Id)),
                                            location.Data);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                        if (hfnReceivePorts != null)
                        {
                            hfnReceivePorts.SortChildren();
                        }
                    }
                    #endregion
                    this.UpdatePercentageComplete(60);

                    #region Schema

                    //============================================
                    // Schema
                    //============================================
                    if (application.Schemas.Count > 0)
                    {
                        bool rootNodeAdded = false;

                        HelpFileNode hfnSchema = null;
                        HelpFileNode hfnSchemaProperty = null;
                        HelpFileNode hfnSchemaDocument = null;
                        HelpFileNode hfnSchemaEnvelope = null;

                        foreach (Schema schema in application.Schemas)
                        {
                            if (schema.XmlContent != string.Empty && !rootNodeAdded)
                            {
                                hfnSchema = applicationNode.CreateChild("Schema");
                                rootNodeAdded = true;
                            }

                            if (rootNodeAdded)
                            {
                                string fileName = "Schema\\" + schema.Id + ".htm";
                                string srcName = "Schema\\" + schema.Id + ".xml";

                                switch (schema.SchemaType)
                                {
                                    case SchemaType.Property:
                                        if (hfnSchemaProperty == null)
                                        {
                                            hfnSchemaProperty = hfnSchema.CreateChild("Property Schema");
                                        }
                                        hfnWorkingNode = hfnSchemaProperty;
                                        break;

                                    case SchemaType.Envelope:
                                        if (hfnSchemaEnvelope == null)
                                        {
                                            hfnSchemaEnvelope = hfnSchema.CreateChild("Envelope Schema");
                                        }
                                        hfnWorkingNode = hfnSchemaEnvelope;
                                        break;

                                    default:
                                        if (hfnSchemaDocument == null)
                                        {
                                            hfnSchemaDocument = hfnSchema.CreateChild("Document Schema");
                                        }
                                        hfnWorkingNode = hfnSchemaDocument;
                                        break;
                                }

                                // Write the index page entry
                                HelpFileNode hfnChildNode = hfnWorkingNode.CreateChild(schema.Name, fileName);
                                hfnChildNode.CreateChild("Source", srcName);

                                this.WriteTransformedXmlDataToFile(
                                    Path.Combine(this.targetDir, fileName),
                                    schema.GetXml(),
                                    this.schemaTransform,
                                    xsltArgs);

                                this.WriteDataToFile(Path.Combine(this.targetDir, srcName), schema.XmlContent);
                            }
                        }

                        if (hfnSchemaProperty != null)
                        {
                            hfnSchemaProperty.SortChildren();
                        }

                        if (hfnSchemaEnvelope != null)
                        {
                            hfnSchemaEnvelope.SortChildren();
                        }

                        if (hfnSchemaDocument != null)
                        {
                            hfnSchemaDocument.SortChildren();
                        }
                    }

                    #endregion
                    this.UpdatePercentageComplete(70);

                    #region Maps
                    //============================================
                    // Maps
                    //============================================
 
                    if (application.Maps.Count > 0)
                    {
                        HelpFileNode hfnMaps = applicationNode.CreateChild("Maps");

                        foreach (Transform transform in application.Maps)
                        {
                            string mapXmlFileName = Path.Combine(targetDir, "Map\\" + transform.Id + ".xml");
                            string mapHtmlFileName = Path.Combine(targetDir, "Map\\" + transform.Id + ".html");
                            string map2HtmlFileName = Path.Combine(targetDir, "Map\\" + transform.Id + "_2.html");

                            HelpFileNode mapSourceNode = hfnMaps.CreateChild(transform.Name, mapHtmlFileName);
                            mapSourceNode.CreateChild("Detail Formatted", map2HtmlFileName);
                            mapSourceNode.CreateChild("Source", mapXmlFileName);

                            WriteTransformedXmlDataToFile(
                                Path.Combine(targetDir, mapHtmlFileName),
                                transform.GetXml(),
                                mapTransform,
                                xsltArgs);

                            WriteTransformedXmlDataToFile(
                                Path.Combine(targetDir, map2HtmlFileName),
                                transform.GetXml(),
                                map2Transform,
                                xsltArgs);

                            WriteDataToFile(mapXmlFileName, transform.XsltSource);
                        }

                        hfnMaps.ChildNodes.Sort(new SortNodes());
                    }

                    #endregion
                    this.UpdatePercentageComplete(80);

                    #region Pipelines
                    //============================================
                    // Pipelines
                    //============================================                    
                    if (application.Pipelines.Count > 0)
                    {
                        HelpFileNode hfnPipeline = null;
                        HelpFileNode hfnSendPipelines = null;
                        HelpFileNode hfnReceivePipelines = null;

                        foreach (Pipeline pl in application.Pipelines)
                        {
                            if (hfnPipeline == null)
                            {
                                hfnPipeline = applicationNode.CreateChild("Pipelines");
                            }

                            //XmlNode pipelineNode = installationDoc.SelectSingleNode("BizTalkBaseObject/Pipelines/Pipeline[Name='" + pl.Name + "']");

                            string fileName = "Pipeline/" + pl.Id + ".htm";

                            switch (pl.PipelineType)
                            {
                                case PipelineType.Send:
                                    if (hfnSendPipelines == null)
                                    {
                                        hfnSendPipelines = hfnPipeline.CreateChild("Send Pipelines");
                                    }
                                    hfnWorkingNode = hfnSendPipelines;
                                    break;

                                default:
                                    if (hfnReceivePipelines == null)
                                    {
                                        hfnReceivePipelines = hfnPipeline.CreateChild("Receive Pipelines");
                                    }
                                    hfnWorkingNode = hfnReceivePipelines;
                                    break;
                            }

                            // Write the index page entry
                            hfnWorkingNode.CreateChild(pl.Name, fileName);

                            string pipelineFlow = string.Empty;

                            if (pl.ViewData != null && pl.ViewData != string.Empty)
                            {
                                pipelineFlow = this.WriteTransformedXmlDataToString(pl.ViewData, this.pipelineDisplayTransform, xsltArgs).Substring(3);
                            }

                            string pipelineData = this.WriteTransformedXmlDataToString(pl.GetXml(), this.pipelineTransform, xsltArgs).Substring(3);

                            this.WriteDataToFile(
                                Path.Combine(this.targetDir, fileName),
                                pipelineData.Replace("${PROCESS_FLOW}", pipelineFlow));
                        }

                        if (hfnSendPipelines != null)
                        {
                            hfnSendPipelines.SortChildren();
                        }

                        if (hfnReceivePipelines != null)
                        {
                            hfnReceivePipelines.SortChildren();
                        }

                    }
                    #endregion
                    this.UpdatePercentageComplete(90);

                    #region SSO

                    //============================================
                    // SSO
                    //============================================  

                    HelpFileNode hfnSso = null;

                    foreach (string ssoConfig in this.sso)
                    {
                        if (!string.IsNullOrEmpty(ssoConfig))
                        {
                            if (hfnSso == null) hfnSso = applicationNode.CreateChild("SSO");
                            string ssoName = Path.GetFileNameWithoutExtension(ssoConfig);
                            string ssoXmlFileName = Path.Combine(this.targetDir, "SSO\\" + ssoName + ".xml");
                            string ssoHtmlFileName = Path.Combine(this.targetDir, "SSO\\" + ssoName + ".html");

                            HelpFileNode ssoSourceNode = hfnSso.CreateChild(ssoName, ssoHtmlFileName);
                            ssoSourceNode.CreateChild("Source", ssoXmlFileName);

                            string ssoXml = File.ReadAllText(ssoConfig);

                            WriteTransformedXmlDataToFile(Path.Combine(this.targetDir, ssoHtmlFileName), ssoXml,
                                                          this.ssoTransform, xsltArgs);

                            WriteDataToFile(ssoXmlFileName, ssoXml);
                        }
                    }

                    #endregion //MTB 08/03/2014
                    this.UpdatePercentageComplete(95);

                }

                //hfn.SortChildren();
            }
        }

        private void WriteConfiguration(XsltArgumentList xsltArgs)
        {
            if (!string.IsNullOrEmpty(bizTalkConfigPath))
            {

                string filename = Path.GetFileNameWithoutExtension(this.bizTalkConfigPath);
                string configXmlFileName = Path.Combine(this.targetDir, "Config\\" + filename + ".xml");
                string configHtmlFileName = Path.Combine(this.targetDir, "Config\\" + filename + ".html");
                File.Copy(this.bizTalkConfigPath, configXmlFileName);

                StreamReader sr = new StreamReader(configXmlFileName);
                string xmlString = sr.ReadToEnd().Replace("&amp;", "");
                sr.Close();
                HelpFileNode hfnConfig = this.hfw.RootNode.CreateChild("Installation Configuration", configHtmlFileName);
                hfnConfig.CreateChild("Source", configXmlFileName);

                WriteTransformedXmlDataToFile(Path.Combine(targetDir, configHtmlFileName), xmlString, configTransform, xsltArgs);

                WriteDataToFile(configXmlFileName, xmlString);

            }
        } //MTB 08/03/2014

        private void WriteSso(XsltArgumentList xsltArgs)
        {
            HelpFileNode hfnSsoApps = null;


            foreach (string ssoApp in ssoApps)
            {
                if (hfnSsoApps == null) hfnSsoApps = this.hfw.RootNode.CreateChild("SSO Applications");

                string ssoXmlFileName = Path.Combine(this.targetDir, "SSOMulti\\" + ssoApp + ".xml");
                string ssoHtmlFileName = Path.Combine(this.targetDir, "SSOMulti\\" + ssoApp + ".html");

                string xmlString;
                try
                {
                    xmlString = ConfigManager.ExportApplicationAsXmlString(ssoApp);
                }
                catch (Exception)
                {
                    xmlString = "<xml>Unable to retrieve SSO Application data: " + ssoApp + "</xml>";
                }


                StreamWriter sw = new StreamWriter(ssoXmlFileName);
                sw.Write(xmlString);
                sw.Close();

                HelpFileNode ssoSourceNode = hfnSsoApps.CreateChild(ssoApp, ssoHtmlFileName);
                ssoSourceNode.CreateChild("Source", ssoXmlFileName);

                WriteTransformedXmlDataToFile(Path.Combine(this.targetDir, ssoHtmlFileName), xmlString,
                                  this.ssoTransform, xsltArgs);

                WriteDataToFile(ssoXmlFileName, xmlString);

            }

        }//MTB 08/03/2014

        private void WriteAdditionalNotes()
        {
            HelpFileNode hfnDevNotes;
            if (!string.IsNullOrEmpty(this.resourceFolder) && Directory.GetFiles(this.resourceFolder, "*.htm*").Length > 0)
            {
                hfnDevNotes = this.hfw.RootNode.CreateChild("Additional Notes");
                foreach (string filename in Directory.GetFiles(this.resourceFolder, "*.htm*"))
                {
                    string devNote = Path.GetFileName(filename);
                    if (devNote != "titlePage.htm")
                        hfnDevNotes.CreateChild(Path.GetFileNameWithoutExtension(filename), Path.Combine(this.targetDir, devNote));
                }
            }
        } //MTB 08/03/2014

        private XsltArgumentList CreateTransformParameterSets()
        {
            XsltArgumentList xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("GenDate", "", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            xsltArgs.AddParam("DocVersion", "", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            HelpFileNode hfnWorkingNode = null;
            return xsltArgs;
        }

        private void LoadStylesheets()
        {
            Assembly a = Assembly.GetExecutingAssembly();


            XmlTextReader xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.SendPort.xslt"));
            // sendPortTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            sendPortTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Application.xslt"));
            //appTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013 
            appTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.SendPortGroup.xslt"));
            //sendPortGroupTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            sendPortGroupTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Assembly.xslt"));
            //asmTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            asmTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Host.xslt"));
            //hostTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            hostTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.ReceivePort.xslt"));
            //rpTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            rpTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Orchestration.xslt"));
            //orchTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            orchTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Pipeline.xslt"));
            //pipelineTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            pipelineTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.PipelineDisplay.xslt"));
            //pipelineDisplayTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            pipelineDisplayTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Schema.xslt"));
            //schemaTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            schemaTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Protocol.xslt"));
            //protocolTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            protocolTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Policy.xslt"));
            //policyTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            policyTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Vocabulary.xslt"));
            //vocabularyTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            vocabularyTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.OrchestrationImage.xslt"));
            //orchestrationImageTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            orchestrationImageTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Map.xslt"));
            //mapTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            mapTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Map2.xslt"));
            map2Transform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.OrchestrationCorrelationTypes.xslt"));
            //correlationTypeTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            correlationTypeTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Config.xslt"));
            //configTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            configTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.OrchestrationCode.xslt"));
            //orchCodeTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            orchCodeTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Party.xslt"));
            //partyTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            partyTransform.Load(xr);
            xr.Close();

            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.Role.xslt"));
            //roleTransform.Load(xr, new XmlUrlResolver(), a.Evidence); MTB 30/06/2013
            roleTransform.Load(xr);
            xr.Close();

            //MTB 09/03/2013
            xr = new XmlTextReader(a.GetManifestResourceStream(resourcePrefix + ".XSLT.SSO.xslt"));
            ssoTransform.Load(xr);
            xr.Close();

            xr = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowOutput()
        {
            if (hfw != null)
            {
                hfw.DisplayHelpFile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Cleanup()
        {
            this.bi = null;

            if (Directory.Exists(targetDir))
            {
                try
                {
#if (!DEBUG)
                    Directory.Delete(targetDir, true);
#endif
                }
                catch
                {
                }
            }

            return true;
        }

        #endregion

        #region PrepareFilesAndDirectories

        private void SaveEmbeddedGraphicToDisk(string graphicName, Assembly assembly, string targetDirectory)
        {
            Bitmap bmp = new Bitmap(assembly.GetManifestResourceStream(resourcePrefix + ".Res." + graphicName));
            bmp.Save(Path.Combine(targetDirectory, graphicName));
            bmp.Dispose();
        }

        private bool PrepareFilesAndDirectories(string reportTitle)
        {
            //targetDir = Path.Combine(Path.GetTempPath(), "BTS2K4Doc");

            try
            {
                hfw = new HelpFileWriter();

                //============================================
                // Delete the target directory if it
                // already exists
                //============================================
                if (Directory.Exists(targetDir))
                {
                    Directory.Delete(targetDir, true);
                }

                //============================================
                // Create the main and supporting directories
                //============================================
                Directory.CreateDirectory(targetDir);
                Directory.CreateDirectory(Path.Combine(targetDir, "Application"));
                Directory.CreateDirectory(Path.Combine(targetDir, "SendPortGroup"));
                Directory.CreateDirectory(Path.Combine(targetDir, "SendPort"));
                Directory.CreateDirectory(Path.Combine(targetDir, "ReceivePort"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Host"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Map"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Assembly"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Orchestration"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Schema"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Party"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Protocol"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Pipeline"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Policy"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Role"));
                Directory.CreateDirectory(Path.Combine(targetDir, "Vocabulary"));
                Directory.CreateDirectory(Path.Combine(targetDir, "SSO")); //MTB 09/03/2014
                Directory.CreateDirectory(Path.Combine(targetDir, "SSOMulti")); //MTB 09/03/2014
                Directory.CreateDirectory(Path.Combine(targetDir, "Config")); //MTB 09/03/2014
                //============================================
                // Read images and CSS files from resource file 
                // and write the to disk in temporary location
                //============================================
                Assembly a = Assembly.GetExecutingAssembly();

                StreamReader sr = new StreamReader(a.GetManifestResourceStream(resourcePrefix + ".Res.titlePage.htm"));
                string html = sr.ReadToEnd();
                sr.Close();

                sr = new StreamReader(a.GetManifestResourceStream(resourcePrefix + ".Res.styles.css"));
                string css = sr.ReadToEnd();
                sr.Close();

                this.SaveEmbeddedGraphicToDisk("Assemble.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Assembly.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Assembly.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("bts.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("CorrelationSetSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Decode.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Disassemble.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Encode.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("FilterSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Host.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Map.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("MapSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("MessageSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("OrchPortSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Orchestration.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("OrchestrationSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Pipeline.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("PreAssemble.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Protocol.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("ReceivePort.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("ReceivePortSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("ResolveParty.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleAnd.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleIf.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleNot.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleOr.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleRule.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleThen.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("RuleVocab.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Schema.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("SchemaSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("SendPort1.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("SendPort2.gif", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("SendPortSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("title.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("TransportSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("Validate.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("VariableSmall.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("divBlank.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("divBot.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("divMid.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("divTop.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("topBackground.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("topRight.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("topRight.jpg", a, targetDir);
                this.SaveEmbeddedGraphicToDisk("topSpacer.gif", a, targetDir);


                //============================================
                // Write out the CSS style sheet
                //============================================
                StreamWriter cssWriter = new StreamWriter(File.Create(Path.Combine(targetDir, "CommentReport.css")));
                cssWriter.Write(css);
                cssWriter.Close();

                //============================================
                // Copy the resourceFolder contents
                //============================================
                string tmpHtml = this.SetUpResourceFolder(this.resourceFolder);
                if (tmpHtml != string.Empty)
                {
                    html = tmpHtml;
                }

                //============================================
                // Write out the main title page
                //============================================
                StreamWriter titleWriter = new StreamWriter(File.Create(Path.Combine(targetDir, "CWP0.HTM")));
                html = html.Replace("#GENDATE#", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                html = html.Replace("#DOCVERSION#", a.GetName().Version.ToString());
                html = html.Replace("#SERVER#", this.bi.Server);
                html = html.Replace("#DATABASE#", this.bi.MgmtDatabaseName);

                if (this.bi.Application != null)
                {
                    html = html.Replace("#APPLICATION#", "Application: " + this.bi.Application.Name);
                }
                else
                {
                    html = html.Replace("#APPLICATION#", "");
                }

                titleWriter.Write(html);
                titleWriter.Close();

                //============================================
                // Add the help file contents page meta data
                //============================================
                if (reportTitle == null || reportTitle == string.Empty)
                {
                    hfw.RootNode.Caption = "BizTalk Configuration - " + this.bi.Server;
                    hfw.Title = "BizTalk Configuration - " + this.bi.Server;
                }
                else
                {
                    hfw.RootNode.Caption = reportTitle;
                    hfw.Title = reportTitle;
                }

                //============================================
                // Add the help file project meta data
                //============================================
                if (!Directory.Exists(btsOutputDir))
                {
                    Directory.CreateDirectory(btsOutputDir);
                }

                if (!Directory.Exists(Path.GetDirectoryName(FILE_NAME)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FILE_NAME));
                }

                hfw.CompiledFileName = FILE_NAME;
                hfw.ContentsFileName = Path.Combine(targetDir, "BTS2K4Doc.hhc");
                hfw.RootNode.Url = "CWP0.HTM";
                hfw.ProjectFileName = Path.Combine(targetDir, "BTS2K4Doc.hhp");

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceFolderName"></param>
        private string SetUpResourceFolder(string resourceFolderName)
        {
            string html = string.Empty;
            if (Directory.Exists(resourceFolderName))
            {
                DirectoryInfo di = new DirectoryInfo(resourceFolder);

                foreach (FileInfo fi in di.GetFiles())
                {
                    if (string.Compare(fi.Name, "titlePage.htm", true) == 0)
                    {
                        StreamReader customTitleReader = new StreamReader(fi.FullName);
                        html = customTitleReader.ReadToEnd();
                        customTitleReader.Close();
                    }
                    else
                    {
                        fi.CopyTo(Path.Combine(targetDir, fi.Name), true);
                    }
                }

                foreach (DirectoryInfo sdi in di.GetDirectories())
                {
                    this.CopyDirectory(sdi.FullName, Path.Combine(targetDir, sdi.Name));
                }
            }
            return html;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFolderName"></param>
        /// <param name="destinationFolderName"></param>
        private void CopyDirectory(string sourceFolderName, string destinationFolderName)
        {
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolderName);
            DirectoryInfo destinationFolderInfo = Directory.CreateDirectory(destinationFolderName);

            foreach (DirectoryInfo subFolder in sourceFolderInfo.GetDirectories())
            {
                Directory.CreateDirectory(Path.Combine(destinationFolderName, subFolder.Name));
                this.CopyDirectory(subFolder.FullName, Path.Combine(destinationFolderName, subFolder.Name));
            }

            foreach (FileInfo fi in sourceFolderInfo.GetFiles())
            {
                fi.CopyTo(Path.Combine(destinationFolderName, fi.Name), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage"></param>
        private void UpdatePercentageComplete(int percentage)
        {
            if (this.PercentageDocumentationComplete != null)
            {
                this.PercentageDocumentationComplete(percentage);
            }
        }
    }
}

public class SortNodes : IComparer
{

    // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
    int IComparer.Compare(Object x, Object y)
    {
        HelpFileNode hfnX = (HelpFileNode)x;
        HelpFileNode hfnY = (HelpFileNode)y;
        return ((new CaseInsensitiveComparer()).Compare(hfnX.Caption, hfnY.Caption));
    }

}