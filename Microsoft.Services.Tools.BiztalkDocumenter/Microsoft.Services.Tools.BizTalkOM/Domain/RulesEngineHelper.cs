
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.IO;
    using Microsoft.RuleEngine;

    /// <summary>
    /// Summary description for RulesEngineHelper.
    /// </summary>
    public class RulesEngineHelper
    {
        private static Hashtable vdefs;
        private string server;
        private string database;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public RulesEngineHelper(string server, string database)
        {
            this.server = server;
            this.database = database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BizTalkBaseObjectCollectionEx GetVocabularies()
        {
            TraceManager.SmartTrace.TraceIn();

            BizTalkBaseObjectCollectionEx vocabularies = new BizTalkBaseObjectCollectionEx();

            try
            {
                RuleSetDeploymentDriver rsdd = new RuleSetDeploymentDriver(this.server, this.database);
                RuleStore rs = rsdd.GetRuleStore();

                VocabularyInfoCollection vic = rs.GetVocabularies(Microsoft.RuleEngine.RuleStore.Filter.All);

                foreach (VocabularyInfo vi in vic)
                {
                    RuleArtifact ra = new RuleArtifact();
                    ra.Name = vi.Name;
                    ra.MajorVersion = vi.MajorRevision;
                    ra.MinorVersion = vi.MinorRevision;
                    ra.QualifiedName = ra.Name + "," + ra.MajorVersion + "," + ra.MinorVersion;
                    vocabularies.Add(ra);
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
            return vocabularies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BizTalkBaseObjectCollectionEx GetRuleSets()
        {
            TraceManager.SmartTrace.TraceIn();

            BizTalkBaseObjectCollectionEx ruleSets = new BizTalkBaseObjectCollectionEx();

            try
            {
                RuleSetDeploymentDriver rsdd = new RuleSetDeploymentDriver(this.server, this.database);
                RuleStore rs = rsdd.GetRuleStore();

                RuleSetInfoCollection rsic = rs.GetRuleSets(Microsoft.RuleEngine.RuleStore.Filter.All);

                foreach (RuleSetInfo rsi in rsic)
                {
                    RuleArtifact ra = new RuleArtifact();
                    ra.Name = rsi.Name;
                    ra.MajorVersion = rsi.MajorRevision;
                    ra.MinorVersion = rsi.MinorRevision;
                    ra.QualifiedName = ra.Name + "," + ra.MajorVersion + "," + ra.MinorVersion;
                    ruleSets.Add(ra);
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
            return ruleSets;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PrepareVocabs()
        {
            TraceManager.SmartTrace.TraceIn();
            vdefs = new Hashtable();

            try
            {
                RuleSetDeploymentDriver rsdd = new RuleSetDeploymentDriver(this.server, this.database);
                RuleStore store = rsdd.GetRuleStore();

                VocabularyInfoCollection vocabularyInfos = store.GetVocabularies(RuleStore.Filter.All);

                foreach (VocabularyInfo vi in vocabularyInfos)
                {
                    Vocabulary v = store.GetVocabulary(vi);

                    foreach (VocabularyDefinition o in v.Definitions)
                    {
                        if (!vdefs.ContainsKey(o.Id))
                        {
                            vdefs.Add(o.Id, o);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
            }

            TraceManager.SmartTrace.TraceOut();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public string ExportRuleSetToFile(RuleArtifact ruleSet, string outputFolder)
        {
            TraceManager.SmartTrace.TraceIn();
            string fileName = null;

            try
            {
                RuleSetDeploymentDriver rsdd = new RuleSetDeploymentDriver(this.server, this.database);
                RuleStore store = rsdd.GetRuleStore();

                RuleSetInfo rsi = new RuleSetInfo(ruleSet.Name, ruleSet.MajorVersion, ruleSet.MinorVersion);
                VocabularyInfoCollection referencedVocabs = store.GetReferencedVocabularies(rsi);
                RuleSet rs = store.GetRuleSet(rsi);

                fileName = Path.Combine(outputFolder, ruleSet.XmlFileName);
                rsdd.ExportRuleSetToFileRuleStore(rsi, fileName);


                fileName = Path.Combine(outputFolder, ruleSet.HtmlFileName);
                StreamWriter sw = new StreamWriter(fileName);
                RuleSetWriter writer = new RuleSetWriter(sw);

                writer.VocabularyDefinitions = vdefs;
                writer.WriteRuleSet(rs, referencedVocabs, sw);

                sw.Flush();
                sw.Close();


            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                TraceManager.SmartTrace.TraceError("FileName will be set to NULL");
            }

            TraceManager.SmartTrace.TraceOut();
            return fileName;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vocabulary"></param>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public string ExportVocabularyToFile(RuleArtifact vocabulary, string outputFolder)
        {
            string fileName = null;
            TraceManager.SmartTrace.TraceIn();

            try
            {
                RuleSetDeploymentDriver rsdd = new RuleSetDeploymentDriver(this.server, this.database);
                VocabularyInfo vi = new VocabularyInfo(vocabulary.Name, vocabulary.MajorVersion, vocabulary.MinorVersion);
                fileName = Path.Combine(outputFolder, vocabulary.XmlFileName);
                rsdd.ExportVocabularyToFileRuleStore(vi, fileName);
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                TraceManager.SmartTrace.TraceError("FileName will be set to NULL");
            }

            TraceManager.SmartTrace.TraceOut();
            return fileName;
        }
    }
}
