
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Reflection;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Pipeline
    /// </summary>
    public class Pipeline : BizTalkBaseObject
    {
        private PipelineType pipelineType;
        private string viewData = string.Empty;
        private NameIdPair parentAssembly;
        private NameIdPairCollection receiveLocations;

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Pipeline"/>
        /// </summary>
        public Pipeline()
        {
            this.receiveLocations = new NameIdPairCollection();
        }

        /// <summary>
        /// Creates a new <see cref="Pipeline"/>
        /// </summary>
        public Pipeline(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion

        #region Public properties

        public PipelineType PipelineType
        {
            get { return this.pipelineType; }
            set { this.pipelineType = value; }
        }

        [XmlIgnore()]
        public string ViewData
        {
            get { return this.viewData; }
            set { this.viewData = value; }
        }

        public NameIdPair ParentAssembly
        {
            get { return this.parentAssembly; }
            set { this.parentAssembly = value; }
        }

        public NameIdPairCollection ReceiveLocations
        {
            get { return this.receiveLocations; }
            set { this.receiveLocations = value; }
        }

        #endregion

        public void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.Pipeline pipeline)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            this.Name = pipeline.FullName;
            this.QualifiedName = pipeline.AssemblyQualifiedName;
            this.AssemblyName = pipeline.AssemblyQualifiedName;
            this.PipelineType = (PipelineType)Enum.Parse(typeof(PipelineType), ((int)pipeline.Type).ToString());
            this.ApplicationName = pipeline.Application.Name;
            this.CustomDescription = pipeline.Description;  // PCA 2015-01-06

            this.PrepareViewData(pipeline.AssemblyQualifiedName);

            TraceManager.SmartTrace.TraceOut();
        }

        private void PrepareViewData(string assemblyQualifiedName)
        {
            TraceManager.SmartTrace.TraceIn(assemblyQualifiedName);

            try
            {
                string[] info = assemblyQualifiedName.Split(new char[] { ',' }, 2);
                string asmName = info[1].Trim();
                Assembly asm = Assembly.Load(asmName);
                object o = asm.CreateInstance(info[0]);

                PropertyInfo pi = o.GetType().GetProperty("XmlContent", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                object viewData = pi.GetValue(o, null);

                string tmpViewData = viewData != null ? viewData.ToString() : string.Empty;
                this.viewData = tmpViewData.Replace("utf-16", "utf-8");
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
