
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using Microsoft.Services.Tools.BiztalkDocumenter.Publishers;
    using Microsoft.Services.Tools.BizTalkOM;
    using Microsoft.Services.Tools.BizTalkOM.Diagnostics;
    using Microsoft.Win32;

    /// <summary>
    /// Summary description for Documenter.
    /// </summary>
    public class Documenter
    {
        public bool ShowOutput = true;
        public string Server = string.Empty;
        public string ResourceFolder = string.Empty;
        public string CustomDescriptionsFileName = string.Empty;
        public string ConfigFrameworkFileName = string.Empty;
        public string UserName = string.Empty;
        public string Password = string.Empty;
        public string Database = string.Empty;
        public string RulesServer = string.Empty;
        public string RulesDatabase = string.Empty;
        public string SsoStage = string.Empty;
        public string SsoTest = string.Empty;
        public string SsoProd = string.Empty;
        public string[] SsoApplications = new string[] { };
        public string BizTalkXmlConfig; //MTB08/03/2014
        public ArrayList Applications;
        public bool IncludeReferences = true;
        public string OutputDir = @"C:\Temp";
        public string ReportName = @"";
        public PublishType PublishType;
        public IPublisher Publisher = null;
        public bool DocumentRules = false;
        public string[] RulesPolicyFilters = new string[] { };         // PCA 2015-01-06
        public string[] RulesVocabularyFilters = new string[] { };     // PCA 2015-01-06
        public string[] HostFilters = new string[] { };                // PCA 2015-01-06
        public string[] AdapterFilters = new string[] { };             // PCA 2015-01-06

        public event UpdatePercentageComplete PercentageDocumentationComplete;

        /// <summary>
        /// 
        /// </summary>
        public Documenter()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            this.Applications = new ArrayList();
            RegistryKey bizTalkKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0");

            string btsInstallDirectory = (string)bizTalkKey.GetValue("InstallPath", @"C:\Program Files\Microsoft BizTalk Server 2006\");

            string privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            privateBinPath += String.Format(";{0}", Path.Combine(btsInstallDirectory, @"Tracking\Control"));
            AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = privateBinPath;

            RegistryKey bizTalkAdminKey = bizTalkKey.OpenSubKey(@"Administration");

            RegistryKey bizTalkRulesKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BusinessRules\3.0");

            this.Server = (string)bizTalkAdminKey.GetValue("MgmtDBServer", Environment.MachineName);
            this.Database = (string)bizTalkAdminKey.GetValue("MgmtDBName", "BizTalkMgmtDb");
            this.RulesServer = (string)bizTalkRulesKey.GetValue("DatabaseServer", Environment.MachineName);
            this.RulesDatabase = (string)bizTalkRulesKey.GetValue("DatabaseName", "BizTalkRuleEngineDb");
            this.OutputDir = Path.GetTempPath();
            this.ReportName = "BizTalk Documentation - " + this.Server;
            this.PublishType = PublishType.EntireConfiguration;
            //this.Publisher = new CompiledHelpPublisher();

            bizTalkKey.Close();
            bizTalkAdminKey.Close();
            bizTalkRulesKey.Close();

            try
            {
                AppDomain.CurrentDomain.Load(@"Microsoft.BizTalk.XLangView");
                Assembly.LoadFrom(Path.Combine(btsInstallDirectory, @"Tracking\Control") + @"\Microsoft.BizTalk.XLangView.dll");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateDocumentation()
        {
            try
            {
                if (this.Publisher == null)
                {
                    throw new ApplicationException("Error initialising documentation publisher");
                }

                this.Publisher.PercentageDocumentationComplete += new UpdatePercentageComplete(Publisher_PercentageDocumentationComplete);

                //============================================
                // Prepare the environment for the doc run
                //============================================
                if (!Publisher.Prepare())
                {
                    return;
                }

                Publisher_PercentageDocumentationComplete(10);

                BizTalkInstallation bi = new BizTalkInstallation();

                if (string.Compare("localhost", this.Server, true) == 0 ||
                    string.Compare("(local)", this.Server, true) == 0 ||
                    string.Compare(".", this.Server, true) == 0)
                {
                    this.Server = Environment.GetEnvironmentVariable("COMPUTERNAME");
                }

                bi.Server = this.Server;
                bi.MgmtDatabaseName = this.Database;
                bi.RulesServer = this.RulesServer;
                bi.RulesDatabase = this.RulesDatabase;

                if (this.PublishType == PublishType.SpecificApplication)
                {
                    bi.LoadConfig(this.Applications, this.IncludeReferences);
                }
                else
                {
                    bi.LoadConfig();
                }

                string[] ssoLocations = { SsoStage, SsoTest, SsoProd };

               this.Publisher.Publish(bi, this.PublishType, this.ResourceFolder, this.OutputDir, this.ReportName, this.DocumentRules, ssoLocations, this.SsoApplications, this.BizTalkXmlConfig
                   , RulesPolicyFilters, RulesVocabularyFilters, HostFilters, AdapterFilters); // PCA 2015-01-06

                //this.Publisher.Publish(bi, this.PublishType, this.ResourceFolder, this.OutputDir, this.ReportName, this.DocumentRules); MTB 20130308

                bi = null;

                if (this.ShowOutput)
                {
                    this.Publisher.ShowOutput();
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                //============================================
                // And cleanup - Done!
                //============================================
                if (this.Publisher != null)
                {
                    this.Publisher.Cleanup();
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage"></param>
        private void Publisher_PercentageDocumentationComplete(int percentage)
        {
            if (this.PercentageDocumentationComplete != null)
            {
                this.PercentageDocumentationComplete(percentage);
            }
            return;
        }
    }
}
