using System;
using System.Collections.Specialized;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using Microsoft.EnterpriseSingleSignOn.Interop;


namespace BizTalk.Utilities.SSO.Core
{
    public static class ConfigManager
    {
        private static string[] _applications;
        //don't actually need a GUID value
        private const string IdenifierGuid = "ConfigProperties";

        /// <summary>
        /// Creates a new SSO ConfigStore application.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="description"></param>
        /// <param name="uAccountName"></param>
        /// <param name="adminAccountName"></param>
        /// <param name="propertiesBag"></param>
        /// <param name="maskArray"></param>
        public static void CreateConfigStoreApplication(string appName, string description, string uAccountName, string adminAccountName, SsoPropBag propertiesBag, ArrayList maskArray)
        {
            int appFlags = 0;

            //bitwise operation for flags
            appFlags |= SSOFlag.SSO_FLAG_APP_CONFIG_STORE;
            appFlags |= SSOFlag.SSO_FLAG_SSO_WINDOWS_TO_EXTERNAL;
            appFlags |= SSOFlag.SSO_FLAG_APP_ALLOW_LOCAL;

            ISSOAdmin ssoAdmin = new ISSOAdmin();

            //create app
            ssoAdmin.CreateApplication(appName, description, "", uAccountName, adminAccountName, appFlags, propertiesBag.PropertyCount);

            //create property fields
            int counter = 0;

            //create dummy field in first slot
            ssoAdmin.CreateFieldInfo(appName, "dummy", 0);
            //create real fields
            foreach (DictionaryEntry de in propertiesBag.Properties)
            {
                string propName = de.Key.ToString();
                int fieldFlags = 0;
                fieldFlags |= Convert.ToInt32(maskArray[counter]);

                //create property
                ssoAdmin.CreateFieldInfo(appName, propName, fieldFlags);

                counter++;
            }

            //enable application
            ssoAdmin.UpdateApplication(appName, null, null, null, null, SSOFlag.SSO_FLAG_ENABLED, SSOFlag.SSO_FLAG_ENABLED);

        }

        /// <summary>
        /// Set values for application fields
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="propertyBag"></param>
        public static void SetConfigProperties(string appName, SsoPropBag propertyBag)
        {
            ISSOConfigStore configStore = new ISSOConfigStore();

            configStore.SetConfigInfo(appName, IdenifierGuid, propertyBag);
 
        }

        public static string[] GetCurrentUserApplications()
        {
            if (_applications == null)
            {
                ISSOMapper mapper = new ISSOMapper();

                AffiliateApplicationType appTypes = AffiliateApplicationType.ConfigStore;

                IPropertyBag propBag = (IPropertyBag)mapper;

                uint appFilterFlagMask = SSOFlag.SSO_FLAG_APP_FILTER_BY_TYPE;

                uint appFilterFlags = (uint)appTypes;

                object appFilterFlagsObj = appFilterFlags;

                object appFilterFlagMaskObj = appFilterFlagMask;

                propBag.Write("AppFilterFlags", ref appFilterFlagsObj);

                propBag.Write("AppFilterFlagMask", ref appFilterFlagMaskObj);

                string[] descs;
                string[] contacts;
                
                mapper.GetApplications(out _applications, out descs, out contacts);
            }
            return _applications;
        }

 
        /// <summary>
        /// Retrieve dictionary of field/value pairs
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="description"></param>
        /// <param name="contactInfo"></param>
        /// <param name="appUserAcct"></param>
        /// <param name="appAdminAcct"></param>
        /// <returns></returns>
        private static HybridDictionary GetConfigProperties(string appName, out string description, out string contactInfo, out string appUserAcct, out string appAdminAcct)
        {
            int flags;
            int count;

            //get config info
            ISSOAdmin ssoAdmin = new ISSOAdmin();
            ssoAdmin.GetApplicationInfo(appName, out description, out contactInfo, out appUserAcct, out appAdminAcct, out flags, out count);
            
            //get properties
            ISSOConfigStore configStore = new ISSOConfigStore();
            SsoPropBag propertiesBag = new SsoPropBag();

            configStore.GetConfigInfo(appName, IdenifierGuid, SSOFlag.SSO_FLAG_RUNTIME, propertiesBag);

            return propertiesBag.Properties;
        }

        public static HybridDictionary GetApplicationKeyValuePairs(string appName)
        {
            string description;
            string contactInfo;
            string appUserAcct;
            string appAdminAcct;


            return GetConfigProperties(appName, out description, out contactInfo, out appUserAcct, out appAdminAcct);
        }

        public static string ExportApplicationAsXmlString(string applicationName)
        {
            string xmlString = null;

            string description;
            string contactInfo;
            string appUserAcct;
            string appAdminAcct;

            HybridDictionary appValuePairs = GetConfigProperties(applicationName, out description, out contactInfo, out appUserAcct, out appAdminAcct);

            sso app2Export = new sso();
            sso.applicationRow applicationRow = app2Export.application.AddapplicationRow(description, appUserAcct, appAdminAcct, applicationName);
            
            int loopCounter=0;
            foreach (DictionaryEntry valueKeyPair in appValuePairs)
            {
                app2Export.field.AddfieldRow(loopCounter++.ToString(), valueKeyPair.Key.ToString(), "no",
                                             valueKeyPair.Value.ToString(), applicationRow);
            }

            app2Export.flags.AddflagsRow("yes", "yes", "yes", applicationRow);
            
            app2Export.AcceptChanges();

            xmlString = app2Export.GetXml();

            return xmlString;

        }



        public static string GetConfigValue(string sAppName, string sKey)
        {
            string description;
            string contactInfo;
            string appUserAcct;
            string appAdminAcct;
            HybridDictionary objResult = GetConfigProperties(sAppName, out description, out contactInfo, out appUserAcct, out appAdminAcct);
            object objValue = objResult[sKey];
            return objValue == null ? string.Empty : objValue.ToString();
        }


        public static string GetConfigValueInstance(string sAppName, string sKey)
        {
            string description;
            string contactInfo;
            string appUserAcct;
            string appAdminAcct;
            HybridDictionary objResult = GetConfigProperties(sAppName, out description, out contactInfo, out appUserAcct, out appAdminAcct);
            object objValue = objResult[sKey];
            return objValue == null ? string.Empty : objValue.ToString();
        }

        /// <summary>
        /// Remove the application
        /// </summary>
        /// <param name="appName"></param>
        public static void DeleteApplication(string appName)
        {
            ISSOAdmin ssoAdmin = new ISSOAdmin();

            ssoAdmin.DeleteApplication(appName);
        }
    }

    public class SsoPropBag : IPropertyBag
    {
        internal readonly HybridDictionary Properties;

        public SsoPropBag()
        {
            Properties = new HybridDictionary();
        }

        #region IPropertyBag Members

        public void Read(string propName, out object ptrVar, int errorLog)
        {
            ptrVar = Properties[propName];
        }

        public void Write(string propName, ref object ptrVar)
        {
            Properties.Add(propName, ptrVar);
        }

        public int PropertyCount
        {
            get
            {
                return Properties.Count;
            }
        }

        #endregion
    }

}

