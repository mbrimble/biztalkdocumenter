
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for Orchestration.
    /// </summary>
    public class Orchestration : BizTalkBaseObject
    {
        private string parentAssemblyFormattedName;
        private string parentAssemblyId;
        private NameIdPair host;
        private string viewData = string.Empty;
        private string artifactData = string.Empty;
        private BizTalkBaseObjectCollectionEx ports;
        private BizTalkBaseObjectCollectionEx invokedOrchestrations;
        private BizTalkBaseObjectCollectionEx variables;
        private BizTalkBaseObjectCollectionEx messages;
        private BizTalkBaseObjectCollectionEx multiPartMessages;
        private BizTalkBaseObjectCollectionEx correlationSets;
        private BizTalkBaseObjectCollectionEx correlationSetTypes;
        private BizTalkBaseObjectCollectionEx roleLinks;
        private ArrayList calledRules;
        private NameIdPairCollection transforms;
        private ArrayList shapeMap;
        private Hashtable coverageShapes;
        private ArrayList trackedShapeIds;
        private ArrayList orchestrationErrors;
        private NameIdPair parentAssembly;

        public Orchestration()
        {
            this.ports = new BizTalkBaseObjectCollectionEx();
            this.invokedOrchestrations = new BizTalkBaseObjectCollectionEx();
            this.variables = new BizTalkBaseObjectCollectionEx();
            this.messages = new BizTalkBaseObjectCollectionEx();
            this.correlationSets = new BizTalkBaseObjectCollectionEx();
            this.correlationSetTypes = new BizTalkBaseObjectCollectionEx();
            this.calledRules = new ArrayList();
            this.transforms = new NameIdPairCollection();
            this.shapeMap = new ArrayList();
            this.coverageShapes = new Hashtable();
            this.trackedShapeIds = new ArrayList();
            this.orchestrationErrors = new ArrayList();
            this.multiPartMessages = new BizTalkBaseObjectCollectionEx();
            this.roleLinks = new BizTalkBaseObjectCollectionEx();
        }

        public Orchestration(string name)
            : this()
        {
            this.Name = name;
        }

        #region Public Properties

        public NameIdPair ParentAssembly
        {
            get { return this.parentAssembly; }
            set { this.parentAssembly = value; }
        }

        [XmlIgnore()]
        public Hashtable CoverageShapes
        {
            get { return this.coverageShapes; }
            set { this.coverageShapes = value; }
        }

        public string ParentAssemblyFormattedName
        {
            get { return this.parentAssemblyId; }
            set { this.parentAssemblyId = value; }
        }

        [XmlArrayItem("Error", typeof(OrchestrationError))]
        public ArrayList OrchestrationErrors
        {
            get { return this.orchestrationErrors; }
            set { this.orchestrationErrors = value; }
        }

        [XmlIgnore]
        //[XmlArrayItem("Shape", typeof(OrchShape))]
        public ArrayList ShapeMap
        {
            get { return this.shapeMap; }
            set { this.shapeMap = value; }
        }

        [XmlArrayItem("Transform", typeof(NameIdPair))]
        public NameIdPairCollection Transforms
        {
            get { return this.transforms; }
            set { this.transforms = value; }
        }

        [XmlArrayItem("Rule", typeof(string))]
        public ArrayList CalledRules
        {
            get { return this.calledRules; }
            set { this.calledRules = value; }
        }

        public string ParentAssemblyId
        {
            get { return this.parentAssemblyFormattedName; }
            set { this.parentAssemblyFormattedName = value; }
        }

        [XmlArrayItem("OrchestrationPort", typeof(OrchestrationPort))]
        public BizTalkBaseObjectCollectionEx Ports
        {
            get { return this.ports; }
            set { this.ports = value; }
        }

        [XmlArrayItem("Orchestration", typeof(Orchestration))]
        public BizTalkBaseObjectCollectionEx InvokedOrchestrations
        {
            get { return this.invokedOrchestrations; }
            set { this.invokedOrchestrations = value; }
        }

        [XmlArrayItem("Variable", typeof(OrchestrationVariable))]
        public BizTalkBaseObjectCollectionEx Variables
        {
            get { return this.variables; }
            set { this.variables = value; }
        }

        [XmlArrayItem("Message", typeof(OrchestrationMessage))]
        public BizTalkBaseObjectCollectionEx Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        [XmlArrayItem("MultiPartMessage", typeof(OrchestrationMultiPartMessage))]
        public BizTalkBaseObjectCollectionEx MultiPartMessages
        {
            get { return this.multiPartMessages; }
            set { this.multiPartMessages = value; }
        }

        [XmlArrayItem("RoleLink", typeof(OrchestrationRoleLink))]
        public BizTalkBaseObjectCollectionEx RoleLinks
        {
            get { return this.roleLinks; }
            set { this.roleLinks = value; }
        }

        [XmlArrayItem("CorrelationSet", typeof(CorrelationSet))]
        public BizTalkBaseObjectCollectionEx CorrelationSets
        {
            get { return this.correlationSets; }
            set { this.correlationSets = value; }
        }

        [XmlArrayItem("CorrelationSetType", typeof(CorrelationSet))]
        public BizTalkBaseObjectCollectionEx CorrelationSetTypes
        {
            get { return this.correlationSetTypes; }
            set { this.correlationSetTypes = value; }
        }

        public NameIdPair Host
        {
            get { return this.host; }
            set { this.host = value; }
        }

        [XmlIgnore()]
        public string ViewData
        {
            get { return this.viewData; }
            set { this.viewData = value; }
        }

        [XmlIgnore()]
        public string ArtifactData
        {
            get { return this.artifactData; }
            set { this.artifactData = value; }
        }

        #endregion

        public bool SaveAsImage(string fileName)
        {
            return OrchViewer.SaveOrchestrationToJpg(this, fileName);
        }

        public Bitmap GetImage()
        {
            return OrchViewer.GetOrchestationImage(this, false);
        }

        public Bitmap GetImage(bool includeCoverage)
        {
            return OrchViewer.GetOrchestationImage(this, includeCoverage);
        }

        public void ClearCoverageInfo()
        {
            this.coverageShapes.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Cleanup()
        {
            this.ViewData = null;
            this.ArtifactData = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrchestrationCoverageInfo GetCoverageInfo(string dtaServerName, string dtaDatabaseName, DateTime dateFrom, DateTime dateTo)
        {
            this.GetCoverageShapeInfo(dtaServerName, dtaDatabaseName, dateFrom, dateTo);

            OrchestrationCoverageInfo info = new OrchestrationCoverageInfo();

            info.totalCoverageCompletePercentage = Convert.ToInt32(this.GetCoveragePercentage());
            info.totalCoverageFailedPercentage = 100 - info.totalCoverageCompletePercentage;

            info.successRatePercentage = Convert.ToInt32(this.GetSuccessRate());
            info.failureRatePercentage = 100 - info.successRatePercentage;

            return info;
        }

        #region "Load Internal Data"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="orchestration"></param>
        internal void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.BtsOrchestration orchestration)
        {
            if (orchestration != null)
            {
                this.QualifiedName = orchestration.AssemblyQualifiedName;
                this.Name = orchestration.FullName;
                this.ApplicationName = orchestration.Application.Name;
                this.CustomDescription = orchestration.Description;
                this.AssemblyName = orchestration.AssemblyQualifiedName;

                if (orchestration.Host != null)
                {
                    this.host = new NameIdPair(orchestration.Host.Name, "");
                }

                foreach (BizTalkCore.OrchestrationPort port in orchestration.Ports)
                {
                    OrchestrationPort op = new OrchestrationPort(port.Name);
                    op.Load(port);
                    this.ports.Add(op);
                }

                this.GetInfo(orchestration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bto"></param>
        /// <returns></returns>
        private void GetInfo(BizTalkCore.BtsOrchestration bto)
        {
            this.LoadInternalData(bto.BtsAssembly.DisplayName);

            //foreach (BizTalkCore.BtsOrchestration invokedOrchestration in bto.InvokedOrchestrations)
            //{
            //    this.invokedOrchestrations.Add(GetInfo(invokedOrchestration));
            //}

            XmlDocument doc = null;
            XmlNamespaceManager mgr = null;
            CollectionSorter comparer = new CollectionSorter(CollectionSorter.SortOrder.Ascending, "Name");

            LoadMainOrchDocument(ref doc, ref mgr);

            LoadOrchestrationVariables(doc, mgr, comparer);

            LoadOrchMessages(doc, mgr, comparer);

            LoadOrchMultipartMessages(doc, mgr);

            LoadOrchCorrelationTypes(doc, mgr);

            LoadOrchCorrelationSets(doc, mgr);

            LoadOrchTransforms(doc, mgr);

            LoadRoleLinks(doc, mgr, comparer);

            LoadCalledRules(doc, mgr);

            doc = null;
            mgr = null;
        }

        private void LoadInternalData(string parentAssemblyName)
        {
            try
            {
                Assembly asm = Assembly.Load(parentAssemblyName);
                Type t = asm.GetType(this.Name);

                FieldInfo pi = t.GetField("_symInfo", BindingFlags.NonPublic | BindingFlags.Static);
                object viewData = pi.GetValue(t);
                this.viewData = viewData != null ? viewData.ToString() : string.Empty;

                FieldInfo fi = t.GetField("_symODXML", BindingFlags.NonPublic | BindingFlags.Static);
                object artifactData = fi.GetValue(t);
                this.artifactData = artifactData != null ? artifactData.ToString() : string.Empty;
                int pos = this.artifactData.IndexOf("?>");
                if (pos > 0)
                {
                    this.artifactData = this.artifactData.Substring(pos + 2);
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }
        }

        private void LoadMainOrchDocument(ref XmlDocument doc, ref XmlNamespaceManager mgr)
        {
            try
            {
                doc = new XmlDocument();
                doc.LoadXml(this.artifactData);

                mgr = new XmlNamespaceManager(doc.NameTable);
                mgr.AddNamespace("om", "http://schemas.microsoft.com/BizTalk/2003/DesignerData");
            }
            catch (Exception ex)
            {
                doc = null;
                mgr = null;
                TraceManager.SmartTrace.TraceError(ex);
            }
        }

        private void LoadOrchestrationVariables(XmlDocument doc, XmlNamespaceManager mgr, CollectionSorter comparer)
        {
            //=====================================
            // load orchestration variables
            //=====================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList varNodes = doc.SelectNodes(".//om:Element[@Type='VariableDeclaration']", mgr);

                    foreach (XmlNode varNode in varNodes)
                    {
                        OrchestrationVariable var = new OrchestrationVariable(varNode, mgr);
                        this.variables.Add(var, true);
                    }

                    this.variables.Sort(comparer);
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadOrchMessages(XmlDocument doc, XmlNamespaceManager mgr, CollectionSorter comparer)
        {
            //=====================================
            // load orchestration messages
            //=====================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList msgNodes = doc.SelectNodes(".//om:Element[@Type='MessageDeclaration']", mgr);

                    foreach (XmlNode msgNode in msgNodes)
                    {
                        OrchestrationMessage msg = new OrchestrationMessage(msgNode, mgr);
                        this.messages.Add(msg, true);
                    }

                    this.messages.Sort(comparer);
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadOrchMultipartMessages(XmlDocument doc, XmlNamespaceManager mgr)
        {
            //=====================================
            // load orchestration multipart messages
            //=====================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList msgNodes = doc.SelectNodes(".//om:Element[@Type='MultipartMessageType']", mgr);

                    foreach (XmlNode msgNode in msgNodes)
                    {
                        OrchestrationMultiPartMessage msg = new OrchestrationMultiPartMessage(msgNode, mgr);
                        this.multiPartMessages.Add(msg, true);
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadOrchCorrelationTypes(XmlDocument doc, XmlNamespaceManager mgr)
        {
            //=========================================
            // load orchestration correlation set types
            //=========================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList csNodes = doc.SelectNodes(".//om:Element[@Type='CorrelationType']", mgr);

                    foreach (XmlNode csNode in csNodes)
                    {
                        string typeName = csNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                        string orchName = csNode.ParentNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;

                        CorrelationSet cs = new CorrelationSet();
                        cs.Name = orchName + "." + typeName;

                        XmlNodeList csPropertyNodes = csNode.SelectNodes("om:Element[@Type='PropertyRef']", mgr);

                        foreach (XmlNode csPropertyNode in csPropertyNodes)
                        {
                            string propertyRef = csPropertyNode.SelectSingleNode("om:Property[@Name='Ref']", mgr).Attributes.GetNamedItem("Value").Value;
                            cs.CorrelatedProperties.Add(propertyRef);
                        }

                        this.correlationSetTypes.Add(cs, true);
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadOrchCorrelationSets(XmlDocument doc, XmlNamespaceManager mgr)
        {
            //=====================================
            // load orchestration correlation sets
            //=====================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList csNodes = doc.SelectNodes(".//om:Element[@Type='CorrelationDeclaration']", mgr);

                    foreach (XmlNode csNode in csNodes)
                    {
                        CorrelationSet cs = new CorrelationSet();
                        XmlAttribute scopeAtt = csNode.Attributes.GetNamedItem("ParentLink") as XmlAttribute;
                        if (scopeAtt != null &&
                            string.Compare(scopeAtt.Value, "Scope_CorrelationDeclaration", true) == 0)
                        {
                            cs.Scope = csNode.ParentNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                        }

                        cs.Name = csNode.SelectSingleNode("om:Property[@Name='Name']", mgr).Attributes.GetNamedItem("Value").Value;
                        cs.TypeName = csNode.SelectSingleNode("om:Property[@Name='Type']", mgr).Attributes.GetNamedItem("Value").Value;

                        XmlNode descNode = csNode.SelectSingleNode("om:Property[@Name='AnalystComments']", mgr);
                        if (descNode != null)
                        {
                            cs.Description = descNode.Attributes.GetNamedItem("Value").Value;
                        }

                        BizTalkBaseObject definingType = this.correlationSetTypes[cs.TypeName];
                        if (definingType != null)
                        {
                            cs.DefiningTypeId = definingType.Id;
                        }

                        this.correlationSets.Add(cs, true);
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadOrchTransforms(XmlDocument doc, XmlNamespaceManager mgr)
        {
            //=========================================
            // load orchestration transforms
            //=========================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList csNodes = doc.SelectNodes(".//om:Element[@Type='Transform']", mgr);

                    foreach (XmlNode csNode in csNodes)
                    {
                        string transformName = csNode.SelectSingleNode("om:Property[@Name='ClassName']", mgr).Attributes.GetNamedItem("Value").Value;
                        this.transforms.Add(new NameIdPair(transformName, ""));
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadRoleLinks(XmlDocument doc, XmlNamespaceManager mgr, CollectionSorter comparer)
        {
            //=====================================
            // load role links
            //=====================================
            if (doc != null)
            {
                try
                {
                    XmlNodeList linkNodes = doc.SelectNodes(".//om:Element[@Type='ServiceLinkDeclaration']", mgr);

                    foreach (XmlNode linkNode in linkNodes)
                    {
                        OrchestrationRoleLink link = new OrchestrationRoleLink(linkNode, mgr);
                        this.roleLinks.Add(link, true);
                    }

                    this.roleLinks.Sort(comparer);
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        private void LoadCalledRules(XmlDocument doc, XmlNamespaceManager mgr)
        {
            //=====================================
            // load called rules
            //=====================================
            if (doc != null)
            {
                try
                {
                    this.CalledRules.Clear();
                    XmlNodeList ruleNodes = doc.SelectNodes(@"//om:Property[@Name='PolicyName']/@Value", mgr);
                    foreach (System.Xml.XmlNode ruleNode in ruleNodes)
                    {
                        this.CalledRules.Add(ruleNode.Value.ToString());
                    };
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsBizTalkService(Type t)
        {
            while ((typeof(object) != t))
            {
                if ("Microsoft.BizTalk.XLANGs.BTXEngine.BTXService" == t.FullName)
                {
                    return true;
                }
                t = t.BaseType;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetTrackedShapeCount()
        {
            int count = 0;

            if (this.ViewData != null && this.ViewData.Length > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.ViewData);
                XmlNodeList trackedShapeNodes = doc.SelectNodes("XsymFile/Metadata/TrkMetadata/ShapeID");

                foreach (XmlNode shapeNode in trackedShapeNodes)
                {
                    // TODO - this will add the orch shape too - we must get rid of it somehow...
                    trackedShapeIds.Add(shapeNode.InnerText.Replace("'", ""));
                }

                count = trackedShapeNodes.Count;
            }

            return count;
        }

        /// <summary>
        /// Get the number of tracked shapes that have successfully initialized
        /// </summary>
        /// <returns></returns>
        private int GetInitializedTrackedShapeCount()
        {
            int count = 0;

            if (this.coverageShapes != null && this.coverageShapes.Count > 0)
            {
                foreach (OrchShape os in this.coverageShapes.Values)
                {
                    if (os.entryCount > 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Get the number of tracked shapes that have successfully completed
        /// </summary>
        /// <returns></returns>
        private int GetCompletedTrackedShapeCount()
        {
            int count = 0;

            if (this.coverageShapes != null && this.coverageShapes.Count > 0)
            {
                this.trackedShapeIds.Clear();
                int totalCount = this.GetTrackedShapeCount();  // From Xym File - xpath(XsymFile/Metadata/TrkMetadata/ShapeID)

                foreach (OrchShape os in this.coverageShapes.Values)
                {
                    if (os.exitCount > 0)
                    {
                        if (this.trackedShapeIds.Contains(os.Id))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetCoveragePercentage()
        {
            double percentage = 0;
            try
            {
                this.trackedShapeIds.Clear();
                int totalCount = this.GetTrackedShapeCount();  // From Xym File - xpath(XsymFile/Metadata/TrkMetadata/ShapeID)
                int completeCount = this.GetCompletedTrackedShapeCount();

                if (totalCount > 0)
                {
                    if (completeCount > totalCount)
                    {
                        completeCount = totalCount;
                    }

                    percentage = (((double)completeCount) / ((double)totalCount)) * 100;
                }
            }
            catch (DivideByZeroException ex)
            {
                percentage = 0;
                TraceManager.SmartTrace.TraceError(ex);
            }

            return Math.Abs(percentage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetSuccessRate()
        {
            int numComplete = 0;
            double percentage = 0;

            foreach (OrchShape os in this.coverageShapes.Values)
            {
                if (os.GetSuccessRate() == 100)
                {
                    numComplete++;
                }
            }

            if (this.coverageShapes.Count > 0 && numComplete > 0)
            {
                try
                {
                    int completedCount = this.GetCompletedTrackedShapeCount();

                    if (completedCount > 0)
                    {
                        percentage = (((double)numComplete) / ((double)completedCount)) * 100;
                    }
                }
                catch (DivideByZeroException ex)
                {
                    percentage = 0;
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }

            return Math.Abs(percentage);
        }

        #region PopulateErrorInfo

        public void PopulateErrorInfo(string dtaServerName, string dtaDatabaseName, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                #region SQL

                string resourcePrefix = "Microsoft.Services.Tools.BizTalkOM";
                Assembly a = Assembly.GetExecutingAssembly();
                StreamReader sqlReader = new StreamReader(a.GetManifestResourceStream(resourcePrefix + ".Res.SqlOrchErrorInfo.txt"));
                string sqlOrchTrackingMetrics = sqlReader.ReadToEnd();
                sqlReader.Close();

                sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$ParentAssemblyFormattedName$", this.ParentAssemblyFormattedName);
                sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$Name$", this.Name);
                sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$dateFrom$", dateFrom.ToString("dd MMM yy HH:mm:ss"));
                sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$dateTo$", dateTo.ToString("dd MMM yy HH:mm:ss"));

                #endregion

                SqlCommand cmd = new SqlCommand(sqlOrchTrackingMetrics);

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1000;

                using (SqlConnection conn = new SqlConnection(string.Format("server={0};database={1};integrated security=sspi", dtaServerName, dtaDatabaseName)))
                {
                    cmd.Connection = conn;
                    cmd.Connection.Open();

                    SqlDataReader sr = cmd.ExecuteReader();

                    string data1 = null;
                    string data2 = null;

                    while (sr.Read())
                    {
                        data1 = sr.GetString(0);

                        data1 = data1.Replace("\r\n", "^");
                        string[] dataParts = data1.Split(new char[] { '^' }, 2);

                        if (dataParts.Length >= 2)
                        {
                            if (string.Compare(data2, dataParts[0], true) != 0)
                            {
                                data2 = dataParts[0];
                                OrchestrationError error = new OrchestrationError(dataParts[0], dataParts[1]);
                                this.orchestrationErrors.Add(error);
                            }
                        }
                    } //while
                } //using
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            return;
        }

        #endregion

        #region GetMetrics

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtaServerName"></param>
        /// <param name="dtaDatabaseName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public OrchMetrics GetMetrics(string dtaServerName, string dtaDatabaseName, DateTime dateFrom, DateTime dateTo)
        {
            #region SQL

            string resourcePrefix = "Microsoft.Services.Tools.BizTalkOM";
            Assembly a = Assembly.GetExecutingAssembly();
            StreamReader sqlReader = new StreamReader(a.GetManifestResourceStream(resourcePrefix + ".Res.SqlOrchTrackingMetrics.txt"));
            string sqlOrchTrackingMetrics = sqlReader.ReadToEnd();
            sqlReader.Close();

            sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$ParentAssemblyFormattedName$", this.ParentAssemblyFormattedName);
            sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$Name$", this.Name);
            sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$dateFrom$", dateFrom.ToString("dd MMM yy HH:mm:ss"));
            sqlOrchTrackingMetrics = sqlOrchTrackingMetrics.Replace("$dateTo$", dateTo.ToString("dd MMM yy HH:mm:ss"));

            #endregion

            OrchMetrics metrics = new OrchMetrics();

            SqlCommand cmd = new SqlCommand(sqlOrchTrackingMetrics);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 1000;

            using (SqlConnection conn = new SqlConnection(string.Format("server={0};database={1};integrated security=sspi", dtaServerName, dtaDatabaseName)))
            {
                cmd.Connection = conn;
                cmd.Connection.Open();

                SqlDataReader sr = cmd.ExecuteReader();

                if (sr.HasRows)
                {
                    sr.Read();

                    metrics.minDurationMillis = sr.IsDBNull(2) ? 0 : sr.GetInt32(2);
                    metrics.maxDurationMillis = sr.IsDBNull(3) ? 0 : sr.GetInt32(3);
                    metrics.avgDurationMillis = sr.IsDBNull(4) ? 0 : sr.GetInt32(4);

                    metrics.numCompleted = sr.IsDBNull(5) ? 0 : sr.GetInt32(5);
                    metrics.numTerminated = sr.IsDBNull(6) ? 0 : sr.GetInt32(6);
                    metrics.numStarted = sr.IsDBNull(7) ? 0 : sr.GetInt32(7);
                }

                cmd.Connection.Close();
            }

            return metrics;
        }

        #endregion

        #region GetShapeMetricsAsDom

        /// <summary>
        /// 
        /// </summary>
        public XmlDocument GetShapeMetricsAsDom()
        {
            if (this.coverageShapes != null &&
                this.coverageShapes.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.ASCII);

                xtw.WriteStartDocument(true);
                xtw.WriteStartElement("CoverageShapes");

                foreach (DictionaryEntry de in this.coverageShapes)
                {
                    OrchShape os = de.Value as OrchShape;

                    if (os != null)
                    {
                        xtw.WriteStartElement("CoverageShape");

                        xtw.WriteElementString("Id", os.Id);

                        foreach (OrchShape shape in this.ShapeMap)
                        {
                            if (shape.Id == os.Id)
                            {
                                xtw.WriteElementString("Text", shape.Text);
                                break;
                            }
                        }

                        xtw.WriteElementString("EntryCount", os.entryCount.ToString());
                        xtw.WriteElementString("ExitCount", os.exitCount.ToString());

                        double successRate = 0;

                        if (os.entryCount > 0 && os.exitCount > 0)
                        {
                            successRate = (((double)os.exitCount) / ((double)os.entryCount)) * 100;
                            xtw.WriteElementString("SuccessRate", successRate.ToString("###"));
                        }
                        else
                        {
                            xtw.WriteElementString("SuccessRate", "0");
                        }

                        xtw.WriteElementString("MaxDurationMillis", os.maxDurationMillis.ToString());
                        xtw.WriteElementString("MinDurationMillis", os.minDurationMillis.ToString());
                        xtw.WriteElementString("AvgDurationMillis", os.avgDurationMillis.ToString());

                        xtw.WriteEndElement(); //CoverageShape
                    }
                }

                xtw.WriteEndElement(); //CoverageShapes
                xtw.WriteEndDocument();
                xtw.Flush();

                ms.Seek(0, SeekOrigin.Begin);
                XmlDocument doc = new XmlDocument();
                doc.Load(ms);
                xtw.Close();

                return doc;
            }

            return null;
        }

        #endregion

        #region GetCoverageShapeInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtaServerName"></param>
        /// <param name="dtaDatabaseName"></param>
        private void GetCoverageShapeInfo(string dtaServerName, string dtaDatabaseName, DateTime dateFrom, DateTime dateTo)
        {
            #region SQL

            string resourcePrefix = "Microsoft.Services.Tools.BizTalkOM";
            Assembly a = Assembly.GetExecutingAssembly();
            StreamReader sqlReader = new StreamReader(a.GetManifestResourceStream(resourcePrefix + ".Res.SqlOrchTrackingShapeMetrics.txt"));
            string sqlOrchTrackingShapeInfo = sqlReader.ReadToEnd();
            sqlReader.Close();

            sqlOrchTrackingShapeInfo = sqlOrchTrackingShapeInfo.Replace("$ParentAssemblyFormattedName$", this.ParentAssemblyFormattedName);
            sqlOrchTrackingShapeInfo = sqlOrchTrackingShapeInfo.Replace("$Name$", this.Name);
            sqlOrchTrackingShapeInfo = sqlOrchTrackingShapeInfo.Replace("$dateFrom$", dateFrom.ToString("dd MMM yy HH:mm:ss"));
            sqlOrchTrackingShapeInfo = sqlOrchTrackingShapeInfo.Replace("$dateTo$", dateTo.ToString("dd MMM yy HH:mm:ss"));

            #endregion

            SqlCommand cmd = new SqlCommand(sqlOrchTrackingShapeInfo);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 1000;

            using (SqlConnection conn = new SqlConnection(string.Format("server={0};database={1};integrated security=sspi", dtaServerName, dtaDatabaseName)))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.Connection.Open();

                    SqlDataReader sr = cmd.ExecuteReader();

                    if (sr.HasRows)
                    {
                        while (sr.Read())
                        {
                            this.LoadReaderIntoShapes(sr);
                        }
                    }

                }
                catch (Exception e)
                {

                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }


            }

            // Add the shapes that may not have been tracked yet
            AugmentCoverageShapesWithTrackingInfo();

            return;
        }

        private void LoadReaderIntoShapes(SqlDataReader sr)
        {
            string shapeId = sr.GetGuid(0).ToString();
            int inCount = sr.IsDBNull(1) ? 0 : sr.GetInt32(1);
            int outCount = sr.IsDBNull(2) ? 0 : sr.GetInt32(2);
            int minDuration = sr.IsDBNull(3) ? 0 : sr.GetInt32(3);
            int maxDuration = sr.IsDBNull(4) ? 0 : sr.GetInt32(4);
            int avgDuration = sr.IsDBNull(5) ? 0 : sr.GetInt32(5);

            if (!this.coverageShapes.ContainsKey(shapeId))
            {
                OrchShape os = new OrchShape();
                os.Id = shapeId;

                os.entryCount = inCount;
                os.exitCount = outCount;
                os.minDurationMillis = minDuration;
                os.maxDurationMillis = maxDuration;
                os.avgDurationMillis = avgDuration;

                this.coverageShapes.Add(os.Id, os);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AugmentCoverageShapesWithTrackingInfo()
        {
            if (this.ViewData != null && this.ViewData.Length > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.ViewData);
                XmlNodeList trackedShapeNodes = doc.SelectNodes("XsymFile/Metadata/TrkMetadata/ShapeID");

                foreach (XmlNode shapeNode in trackedShapeNodes)
                {
                    string shapeId = shapeNode.InnerText.Replace("'", "");

                    if (!this.coverageShapes.ContainsKey(shapeId))
                    {
                        OrchShape os = new OrchShape();
                        os.Id = shapeId;

                        string xpath = "//ShapeInfo[ShapeID='" + shapeId + "']/shapeText";
                        XmlNode node = doc.SelectSingleNode(xpath);

                        if (node != null)
                        {
                            os.Text = node.InnerText;
                            this.coverageShapes.Add(os.Id, os);
                        }
                    }
                }
            }
            return;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            // Host
            if (this.Host != null)
            {
                Host h = this.Application.ParentInstallation.Hosts[this.Host.Name] as Host;

                if (h != null)
                {
                    this.host = h.NameIdPair;
                    h.HostedOrchestrations.Add(this.NameIdPair);
                }
            }

            // Ports
            foreach (OrchestrationPort port in this.ports)
            {
                if (port.ReceivePortName != null)
                {
                    ReceivePort rp = this.Application.ParentInstallation.ReceivePorts[port.ReceivePortName.Name] as ReceivePort;

                    if (rp != null)
                    {
                        port.ReceivePortName = rp.NameIdPair;
                        rp.BoundOrchestrations.Add(this.NameIdPair);
                    }
                }

                if (port.SendPortName != null)
                {
                    SendPort sp = this.Application.ParentInstallation.SendPorts[port.SendPortName.Name] as SendPort;

                    if (sp != null)
                    {
                        port.SendPortName = sp.NameIdPair;
                        sp.BoundOrchestrations.Add(this.NameIdPair);
                    }
                }

                if (port.SendPortGroupName != null)
                {
                    SendPortGroup spg = this.Application.ParentInstallation.SendPortGroups[port.SendPortGroupName.Name] as SendPortGroup;

                    if (spg != null)
                    {
                        port.SendPortGroupName = spg.NameIdPair;
                        spg.BoundOrchestrations.Add(this.NameIdPair);
                    }
                }
            }

            // Maps
            foreach (NameIdPair transform in this.transforms)
            {
                Transform t = this.Application.ParentInstallation.Maps[transform.Name] as Transform;

                if (t != null)
                {
                    transform.Id = t.NameIdPair.Id;
                    t.Orchestrations.Add(this.NameIdPair);
                }
            }

            // Role links
            foreach (OrchestrationRoleLink roleLink in this.roleLinks)
            {
                if (roleLink.ConsumerRole != null)
                {
                    Role role = this.Application.ParentInstallation.Roles[roleLink.ConsumerRole.Name] as Role;

                    if (role != null)
                    {
                        roleLink.ConsumerRole.Id = role.Id;
                    }
                }

                if (roleLink.ProviderRole != null)
                {
                    Role role = this.Application.ParentInstallation.Roles[roleLink.ProviderRole.Name] as Role;

                    if (role != null)
                    {
                        roleLink.ProviderRole.Id = role.Id;
                    }
                }

                if (roleLink.InitiatingRole != null)
                {
                    Role role = this.Application.ParentInstallation.Roles[roleLink.InitiatingRole.Name] as Role;

                    if (role != null)
                    {
                        roleLink.InitiatingRole.Id = role.Id;
                    }
                }
            }

            // Shape Map
            if (this.artifactData != string.Empty && this.viewData != string.Empty)
            {
                try
                {
                    Bitmap bmp = this.GetImage();
                    bmp.Dispose();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(this.ArtifactData);

                    XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
                    xnm.AddNamespace("om", "http://schemas.microsoft.com/BizTalk/2003/DesignerData");

                    foreach (OrchShape os in this.ShapeMap)
                    {
                        switch (os.ShapeType)
                        {
                            case ShapeType.ReceiveShape:
                            case ShapeType.SendShape:
                                XmlNode shapeNode = doc.SelectSingleNode("//om:Element[@OID='" + os.Id + "']", xnm);

                                if (shapeNode != null)
                                {
                                    os.PortName = shapeNode.SelectSingleNode("om:Property[@Name='PortName']/@Value", xnm).Value;
                                    os.OperationName = shapeNode.SelectSingleNode("om:Property[@Name='OperationName']/@Value", xnm).Value;
                                    os.MessageName = shapeNode.SelectSingleNode("om:Property[@Name='MessageName']/@Value", xnm).Value;
                                }
                                break;
                            case ShapeType.TransformShape: // NJB TransformShape  -- CD Tidied this code.
                                XmlNode shapeNode2 = doc.SelectSingleNode("//om:Element[@OID='" + os.Id + "']", xnm);
                                XmlNode name = shapeNode2.SelectSingleNode("om:Property[@Name='ClassName']/@Value", xnm);

                                if (name != null) os.TransformName = name.Value;

                                XmlNodeList messagesIn = shapeNode2.SelectNodes("om:Element[@ParentLink='Transform_InputMessagePartRef']", xnm);

                                if (messagesIn != null)
                                    foreach (XmlNode selectNode in messagesIn)
                                    {
                                        foreach (XmlNode detail in selectNode.ChildNodes)
                                        {
                                            if (detail.Attributes != null)
                                                if (detail.Attributes["Name"].Value == "MessageRef")
                                                    os.InputMessage += detail.Attributes["Value"].Value + ',';
                                        }

                                    }

                                if (!string.IsNullOrEmpty(os.InputMessage)) os.InputMessage = os.InputMessage.Remove(os.InputMessage.LastIndexOf(","));

                                XmlNodeList messagesOut = shapeNode2.SelectNodes("om:Element[@ParentLink='Transform_OutputMessagePartRef']", xnm);

                                if (messagesOut != null)
                                    foreach (XmlNode selectNode in messagesOut)
                                    {
                                        foreach (XmlNode detail in selectNode.ChildNodes)
                                        {

                                            if (detail.Attributes != null)
                                                if (detail.Attributes["Name"].Value == "MessageRef")
                                                    os.OutputMessage += detail.Attributes["Value"].Value + ',';
                                        }
                                    }

                                if (!string.IsNullOrEmpty(os.OutputMessage)) os.OutputMessage = os.OutputMessage.Remove(os.OutputMessage.LastIndexOf(","));

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }

            TraceManager.SmartTrace.TraceOut();
            return;
        }
    }
}
