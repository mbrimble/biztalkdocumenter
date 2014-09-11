
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.Xml;
    using System.Xml.Serialization;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// Summary description for Schema.
    /// </summary>
    public class Schema : BizTalkBaseObject
    {
        private string xmlContent;
        private SchemaType schemaType;
        private ArrayList properties;
        private ArrayList importedProperties;
        private BizTalkBaseObjectCollectionEx importedSchema;
        private bool envelope;
        private string targetNamespace = "";
        private string rootName = "";
        private NameIdPair parentAssembly;
        private bool nested;

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Schema"/>
        /// </summary>
        public Schema()
        {
            this.xmlContent = string.Empty;
            this.properties = new ArrayList();
            this.importedProperties = new ArrayList();
            this.importedSchema = new BizTalkBaseObjectCollectionEx();
        }

        /// <summary>
        /// Creates a new <see cref="Schema"/>
        /// </summary>
        /// <param name="nested"></param>
        public Schema(bool nested)
            : this()
        {
            this.nested = nested;
        }

        /// <summary>
        /// Creates a new <see cref="Schema"/>
        /// </summary>
        /// <param name="name"></param>
        public Schema(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion

        #region Public properties

        public NameIdPair ParentAssembly
        {
            get { return this.parentAssembly; }
            set { this.parentAssembly = value; }
        }

        public string TargetNamespace
        {
            get { return this.targetNamespace; }
            set { this.targetNamespace = value; }
        }

        public string RootName
        {
            get { return this.rootName; }
            set { this.rootName = value; }
        }

        public bool Envelope
        {
            get { return this.envelope; }
            set { this.envelope = value; }
        }

        [XmlArrayItem("Property", typeof(string))]
        public ArrayList Properties
        {
            get { return this.properties; }
            set { this.properties = value; }
        }

        [XmlArrayItem("ImportedProperty", typeof(string))]
        public ArrayList ImportedProperties
        {
            get { return this.importedProperties; }
            set { this.importedProperties = value; }
        }

        [XmlIgnore()]
        public string XmlContent
        {
            get { return this.xmlContent; }
            set { this.xmlContent = value; }
        }

        [XmlArrayItem("Schema", typeof(Schema))]
        public BizTalkBaseObjectCollectionEx ImportedSchema
        {
            get { return this.importedSchema; }
            set { this.importedSchema = value; }
        }

        public SchemaType SchemaType
        {
            get { return this.schemaType; }
            set { this.schemaType = value; }
        }

        public bool Nested
        {
            get { return this.nested; }
            set { this.nested = value; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="schema"></param>
        public void Load(BizTalkCore.BtsCatalogExplorer explorer, BizTalkCore.Schema schema)
        {
            LoadSchemaMetadata(schema);
            LoadSchemaProperties(schema);
            ParseSchemaXml(schema);
        }

        private void LoadSchemaMetadata(BizTalk.ExplorerOM.Schema schema)
        {
            this.AssemblyName = schema.AssemblyQualifiedName;
            this.schemaType = (SchemaType)Enum.Parse(typeof(SchemaType), ((int)schema.Type).ToString());
            this.ApplicationName = schema.Application.Name;
            this.CustomDescription = schema.Description;
            this.targetNamespace = schema.TargetNameSpace;
            this.rootName = String.IsNullOrEmpty(schema.RootName) ? String.Empty : schema.RootName;

            //            this.QualifiedName = schema.AssemblyQualifiedName;
            if (this.rootName.Length > 0)
            {
                this.QualifiedName = String.Format("{0},{1}#{2}", schema.AssemblyQualifiedName, schema.TargetNameSpace,
                                                  schema.RootName);
            }
            else
            {
                this.QualifiedName = String.Format("{0},{1}", schema.AssemblyQualifiedName, schema.TargetNameSpace);

            }
        }

        private void ParseSchemaXml(BizTalk.ExplorerOM.Schema schema)
        {
            try
            {
                this.xmlContent = schema.XmlContent.Replace("utf-16", "utf-8");
                this.xmlContent = this.xmlContent.Replace("UTF-16", "UTF-8");

                XmlDocument schemaDoc = new XmlDocument();
                schemaDoc.LoadXml(this.xmlContent);

                XmlNamespaceManager mgr = new XmlNamespaceManager(schemaDoc.NameTable);
                mgr.AddNamespace("b", "http://schemas.microsoft.com/BizTalk/2003");
                mgr.AddNamespace("x", "http://www.w3.org/2001/XMLSchema");

                LoadSchemaImports(schemaDoc, mgr);

                LoadSchemaEnvelope(schemaDoc, mgr);

                schemaDoc = null;
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }
        }

        private void LoadSchemaEnvelope(XmlDocument schemaDoc, XmlNamespaceManager mgr)
        {
            XmlNode envelopeNode = schemaDoc.SelectSingleNode("//x:appinfo/b:schemaInfo", mgr);

            if (envelopeNode != null)
            {
                XmlAttribute att = envelopeNode.Attributes.GetNamedItem("is_envelope") as XmlAttribute;

                if (att != null)
                {
                    this.envelope = att.Value == "yes" ? true : false;
                    this.SchemaType = SchemaType.Envelope;
                }
            }
        }

        private void LoadSchemaImports(XmlDocument schemaDoc, XmlNamespaceManager mgr)
        {
            XmlNodeList importedSchemaNodes = schemaDoc.SelectNodes("//x:appinfo/b:imports/b:namespace", mgr);
            foreach (XmlNode importedSchemaNode in importedSchemaNodes)
            {
                Schema importedSchemaObj = new Schema(true);
                importedSchemaObj.Name = importedSchemaNode.Attributes.GetNamedItem("location").Value;

                string importedPrefix = importedSchemaNode.Attributes.GetNamedItem("prefix").Value;

                // Select properties used by this schema that are declared in the property schema
                XmlNodeList importedPropertyNodes = schemaDoc.SelectNodes("//x:appinfo/b:properties/b:property", mgr);
                foreach (XmlNode importedPropertyNode in importedPropertyNodes)
                {
                    XmlAttribute nameAttribute = importedPropertyNode.Attributes.GetNamedItem("name") as XmlAttribute;

                    if (nameAttribute == null)
                    {
                        nameAttribute = importedPropertyNode.Attributes.GetNamedItem("distinguished") as XmlAttribute;
                    }

                    if (nameAttribute != null)
                    {
                        string propertyName = nameAttribute.Value;

                        if (propertyName.StartsWith(importedPrefix))
                        {
                            string[] nameParts = propertyName.Split(new char[] { ':' });

                            if (nameParts.Length > 0)
                            {
                                propertyName = nameParts[1];
                                importedSchemaObj.ImportedProperties.Add(propertyName);
                            }
                        }
                    }
                }

                this.importedSchema.Add(importedSchemaObj);
            }
        }

        private void LoadSchemaProperties(BizTalk.ExplorerOM.Schema schema)
        {
            if (schema.Properties != null)
            {
                foreach (DictionaryEntry de in schema.Properties)
                {
                    this.properties.Add(de.Key.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorer"></param>
        internal override void FixReferences(BizTalkCore.BtsCatalogExplorer explorer)
        {
            TraceManager.SmartTrace.TraceIn(explorer);

            foreach (Schema importedSchema in this.importedSchema)
            {
                Schema s = this.Application.ParentInstallation.Schemas[importedSchema.Name] as Schema;

                if (s != null)
                {
                    importedSchema.Id = s.Id;
                }
            }

            TraceManager.SmartTrace.TraceOut();
        }
    }
}
