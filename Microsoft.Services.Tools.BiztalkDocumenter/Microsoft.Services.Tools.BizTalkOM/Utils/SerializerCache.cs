
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public delegate void XmlSerializationError(Type type, string message);

    /// <summary>
    /// Summary description for AssemblyCache.
    /// </summary>
    public class XmlSerializerCache : ManagedCache
    {
        /// <summary>
        /// 
        /// </summary>
        public event XmlSerializationError OnXmlSerializationError;

        public XmlSerializerCache()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheTimeoutSeconds"></param>
        public XmlSerializerCache(int cacheTimeoutSeconds)
            : base(cacheTimeoutSeconds)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public XmlSerializer GetCachedSerializer(Type type)
        {
            XmlSerializer serializer = null;

            if (type != null)
            {
                lock (this.SyncRoot)
                {
                    if (!this.ContainsKey(type.FullName))
                    {
                        serializer = new XmlSerializer(type);
                        this.CacheObject(type.FullName, serializer);
                    }
                    else
                    {
                        serializer = this.GetObject(type.FullName) as XmlSerializer;
                    }
                }
            }

            return serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object DeserializeObject(string xmlData, Type type)
        {
            try
            {
                XmlSerializer serializer = this.GetCachedSerializer(type);
                object obj = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlData)));
                return obj;
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeObject(object obj)
        {
            try
            {
                XmlSerializer serializer = this.GetCachedSerializer(obj.GetType());
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                string xmlData = sr.ReadToEnd();
                sr.Close();
                return xmlData;
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                if (this.OnXmlSerializationError != null)
                {
                    this.OnXmlSerializationError(obj.GetType(), ex.ToString());
                }
            }

            return string.Empty;
        }
    }
}
