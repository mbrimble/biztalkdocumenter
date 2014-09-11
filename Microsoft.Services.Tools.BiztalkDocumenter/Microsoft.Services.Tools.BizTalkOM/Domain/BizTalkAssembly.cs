
using Microsoft.BizTalk.ExplorerOM;
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;
    using Microsoft.XLANGs.BaseTypes;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for DeployAssembly.
    /// </summary>
    public class BizTalkAssembly : BizTalkBaseObject
    {
        private NameIdPair nameIdPair;
        private string path;
        private AssemblyName assemblyName;
        private bool validBizTalkAssembly;
        private string[] references;
        private string culture;
        private string displayName;
        private string version;
        private string publicKeyToken;
        private NameIdPairCollection schemas;
        private NameIdPairCollection maps;
        private NameIdPairCollection pipelines;
        private NameIdPairCollection orchestrations;

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="BizTalkAssembly"/>
        /// </summary>
        public BizTalkAssembly()
        {
            this.schemas = new NameIdPairCollection();
            this.maps = new NameIdPairCollection();
            this.pipelines = new NameIdPairCollection();
            this.orchestrations = new NameIdPairCollection();
        }

        /// <summary>
        /// Creates a new <see cref="BizTalkAssembly"/>
        /// </summary>
        /// <param name="btsAssembly"></param>
        public BizTalkAssembly(BizTalkCore.BtsAssembly btsAssembly)
            : this()
        {

            this.culture = btsAssembly.Culture;
            this.displayName = btsAssembly.DisplayName;
            this.version = btsAssembly.Version;
            this.publicKeyToken = btsAssembly.PublicKeyToken;

            //this.QualifiedName = String.Format("{0},{1},{2},{3}", this.displayName, this.version, this.culture, this.PublicKeyToken);
            this.QualifiedName = this.DisplayName;

            Assembly assembly = null;

            try
            {
                // Load the assembly type information
                assembly = Assembly.Load(btsAssembly.DisplayName);

                // Initialize from the assembly type information
                this.Initialize(assembly);
            }
            catch (FileNotFoundException ex)
            {
                // The assembly cannot be loaded from the GAC...
                TraceManager.SmartTrace.TraceError("The assembly could not be loaded from the GAC");
                TraceManager.SmartTrace.TraceError(ex);
            }
            finally
            {
                assembly = null;
            }
        }

        #endregion

        #region Public Properties

        [XmlIgnore()]
        public new NameIdPair NameIdPair
        {
            get
            {
                if (this.nameIdPair == null)
                {
                    this.nameIdPair = new NameIdPair(this.displayName, this.Id);
                }

                return this.nameIdPair;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PublicKeyToken
        {
            get { return this.publicKeyToken; }
            set { this.publicKeyToken = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Schema", typeof(NameIdPair))]
        public NameIdPairCollection Schemas
        {
            get { return this.schemas; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Transform", typeof(NameIdPair))]
        public NameIdPairCollection Maps
        {
            get { return this.maps; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Orchestration", typeof(NameIdPair))]
        public NameIdPairCollection Orchestrations
        {
            get { return this.orchestrations; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("Pipeline", typeof(NameIdPair))]
        public NameIdPairCollection Pipelines
        {
            get { return this.pipelines; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Culture
        {
            get { return this.culture; }
            set { this.culture = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem("ReferencedAssembly", typeof(string))]
        public string[] References
        {
            get { return this.references; }
            set { this.references = value; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="schema"></param>
        public void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.BtsAssembly assembly)
        {
            this.Name = assembly.Name;
            this.displayName = assembly.DisplayName;
        }

        /// <summary>
        /// Initializes various member fields by reflecting on assembly
        /// type information.
        /// </summary>
        /// <param name="assembly">
        /// The assembly type information to initialize from.
        /// </param>
        private void Initialize(Assembly assembly)
        {
            // Store the assembly name
            this.assemblyName = assembly.GetName();

            // Is this assembly a BizTalk assembly?
            this.validBizTalkAssembly = assembly.IsDefined(
                typeof(BizTalkAssemblyAttribute),
                false);

            // Get the assembly references
            AssemblyName[] refs = assembly.GetReferencedAssemblies();
            this.references = new string[refs.Length];
            for (int pos = 0; pos < refs.Length; pos++)
            {
                this.references[pos] = refs[pos].Name;//.FullName;
            }
            refs = null;

            // Store the path to the assembly
            this.path = assembly.Location;
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            BizTalkCore.BtsAssembly assembly = this.GetAssemblyByDisplayName(explorer);

            if (assembly != null)
            {
                FixSchemaReferences(assembly);
                FixMapReferences(assembly);
                FixPipelineReferences(assembly);
                FixOrchReferences(assembly);
            }
            else
            {
                TraceManager.SmartTrace.TraceError("Could not locate assembly. References will not be fixed.");
            }

            TraceManager.SmartTrace.TraceOut();
            return;
        }

        private void FixOrchReferences(BtsAssembly assembly)
        {
            foreach (BizTalkCore.BtsOrchestration orchestration in assembly.Orchestrations)
            {
                //Orchestration o = this.Application.ParentInstallation.Orchestrations[orchestration.FullName] as Orchestration;

                Orchestration o = this.GetOrchByFullyQualifiedName(orchestration.FullName, assembly.DisplayName);

                if (o != null)
                {
                    o.ParentAssembly = this.NameIdPair;
                    this.orchestrations.Add(o.NameIdPair);
                }
                else
                {
                    TraceManager.SmartTrace.TraceError("Could not locate Orch. References will not be fixed.");
                }
            }
        }

        private void FixPipelineReferences(BtsAssembly assembly)
        {
            foreach (BizTalkCore.Pipeline pipeline in assembly.Pipelines)
            {
                //Pipeline p = this.Application.ParentInstallation.Pipelines[pipeline.FullName] as Pipeline;
                Pipeline p = this.GetPipelineByFullyQualifiedName(pipeline.FullName, assembly.DisplayName);

                if (p != null)
                {
                    p.ParentAssembly = this.NameIdPair;
                    this.pipelines.Add(p.NameIdPair);
                }
                else
                {
                    TraceManager.SmartTrace.TraceError("Could not locate pipeline. References will not be fixed.");
                }

            }
        }

        private void FixMapReferences(BtsAssembly assembly)
        {
            foreach (BizTalkCore.Transform transform in assembly.Transforms)
            {
                //Transform t = this.Application.ParentInstallation.Maps[transform.FullName] as Transform;
                Transform t = this.GetMapByFullyQualifiedName(transform.FullName, assembly.DisplayName);

                if (t != null)
                {
                    t.ParentAssembly = this.NameIdPair;
                    this.maps.Add(t.NameIdPair);
                }
                else
                {
                    TraceManager.SmartTrace.TraceError("Could not locate map. References will not be fixed.");
                }
            }
        }

        private void FixSchemaReferences(BtsAssembly assembly)
        {
            foreach (BizTalkCore.Schema schema in assembly.Schemas)
            {
                //Schema s = this.Application.ParentInstallation.Schemas[schema.FullName] as Schema;
                Schema s = this.GetSchemaByFullyQualifiedName(schema, assembly.DisplayName);

                if (s != null)
                {
                    s.ParentAssembly = this.NameIdPair;
                    this.schemas.Add(s.NameIdPair);
                }
                else
                {
                    TraceManager.SmartTrace.TraceError("Could not locate schema. References will not be fixed.");
                }

            }
        }

        private BizTalkCore.BtsAssembly GetAssemblyByDisplayName(BizTalkCore.BtsCatalogExplorer explorer)
        {
            foreach (BtsAssembly assembly in explorer.Assemblies)
            {
                if (assembly.DisplayName.Equals(this.DisplayName))
                {
                    return assembly;
                }
            }

            return null;
        }

        private Orchestration GetOrchByFullyQualifiedName(string orchFullName, string assemblyName)
        {
            string qName = orchFullName + "," + assemblyName;
            foreach (Orchestration o in this.Application.ParentInstallation.Orchestrations)
            {
                if (o.Name.Equals(orchFullName) && o.QualifiedName.Equals(qName))
                {
                    return o;

                }
            }
            return null;
        }

        private Schema GetSchemaByFullyQualifiedName(BizTalkCore.Schema schema, string assemblyName)
        {
            string qName;
            if (!String.IsNullOrEmpty(schema.RootName))
            {
                qName = String.Format("{0},{1}#{2}", schema.AssemblyQualifiedName, schema.TargetNameSpace,
                                                  schema.RootName);
            }
            else
            {
                qName = String.Format("{0},{1}", schema.AssemblyQualifiedName, schema.TargetNameSpace);

            }
            foreach (Schema s in this.Application.ParentInstallation.Schemas)
            {
                if (s.Name.Equals(schema.FullName) && s.QualifiedName.Equals(qName))
                {
                    return s;
                }
            }

            return null;
        }

        private Transform GetMapByFullyQualifiedName(string mapName, string assemblyName)
        {
            string qName = mapName + "," + assemblyName;

            foreach (Transform t in this.Application.ParentInstallation.Maps)
            {
                if (t.Name.Equals(mapName) && t.QualifiedName.Equals(qName))
                {
                    return t;
                }
            }

            return null;
        }

        private Pipeline GetPipelineByFullyQualifiedName(string pipeName, string assemblyName)
        {
            string qName = pipeName + ", " + assemblyName;

            foreach (Pipeline p in this.Application.ParentInstallation.Pipelines)
            {
                if (p.Name.Equals(pipeName, StringComparison.OrdinalIgnoreCase) && p.QualifiedName.Equals(qName, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }

            return null;
        }
    }
}
