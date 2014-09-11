using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.BizTalk.SnapIn.Framework;
using Microsoft.BizTalk.SnapIn.Framework.Forms;
using Microsoft.Services.Tools.BiztalkDocumenter.Core;
using Microsoft.Services.Tools.BiztalkDocumenter.Publishers;
using Microsoft.Services.Tools.BizTalkOM;


namespace Microsoft.Services.Tools.BiztalkDocumenter.SnapIn
{
    [Guid("44D98E3D-69FD-4b2e-8541-5B7A6CB2BC75"), ComVisible(true)]
    public partial class DocResultView : FormView
    {
        private string appName;
        private string server;
        private string mgmtDatabaseName;
        Documenter documenter = new Documenter();

        public DocResultView()
        {
            InitializeComponent();
        }

        private void Init()
        {
            documenter.SetDefaults();
            this.txtOutputFolderName.Text = documenter.OutputDir;
            this.txtReportTitle.Text = documenter.ReportName;
            comboBox2.SelectedIndex = 0;
        }

        #region IFormViewControl Methods

        public override void Initialize(Microsoft.BizTalk.SnapIn.Framework.ScopeNode node, string state)
        {
            base.Initialize(node, state);

            if (node is DocNode)
            {
                this.server = ((DocNode)node).ServerDisplayName;
                this.mgmtDatabaseName = ((DocNode)node).DatabaseName;



                //MessageBox.Show("Node: " + node.DisplayName);

                //if (node.Parent != null)
                //{
                //    MessageBox.Show("Parent: " + node.Parent.DisplayName);
                //    //this.appName = node.Parent.DisplayName;

                //    if (node.Parent.Parent != null)
                //    {
                //        MessageBox.Show("Parent.Parent: " + node.Parent.Parent.DisplayName);
                //        this.appName = node.Parent.Parent.DisplayName;
                //    }
                //}






                documenter.Server = this.server;
                documenter.Database = this.mgmtDatabaseName;
            }


            ExplorerOMProvider explorerOMProvider = new ExplorerOMProvider(((DocNode)node).MgmtDBConnection);

            foreach (Microsoft.BizTalk.ExplorerOM.Application app in explorerOMProvider.Explorer.Applications)
            {
                //txtInfo.Text += Environment.NewLine + app.Name;
                //node.Parent.DisplayName;

                this.appName = app.Name;
                this.appName = "BizTalk Application 1";

            }

            documenter.ReportName = "BizTalk Documentation - Application " + this.appName;
            this.Init();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void GenerateDocumentation()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                linkLabel6.Visible = false;
                progressBar1.Visible = true;

                if (!Utility.ValidateReportName(documenter.ReportName))
                {
                    throw new ApplicationException("Report title contains some invalid characters.");
                }

                BizTalkInstallation bi = new BizTalkInstallation();
                bi.Server = this.server;
                bi.MgmtDatabaseName = this.mgmtDatabaseName;
                //documenter.Application = bi.GetApplicationDetail(this.appName);

                documenter.PercentageDocumentationComplete += new UpdatePercentageComplete(DocumenterPercentageDocumentationComplete);

                //if (radioAssembly.Checked == true)
                //{
                //    if (clbApplications.CheckedItems.Count == 0)
                //    {
                //        MessageBox.Show("No applications have been selected for documentation", "Error Generating Documentation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }

                //    foreach (object item in clbApplications.CheckedItems)
                //    {
                //        string appName = item.ToString();

                //        BizTalkInstallation bi = new BizTalkInstallation();
                //        bi.Server = this.txtServerName.Text;
                //        bi.MgmtDatabaseName = this.textBox1.Text;
                //        documenter.Application = bi.GetApplicationDetail(appName);

                //        break;
                //    }

                //    documenter.PublishType = PublishType.SpecificApplication;
                //}
                //else if (radioSchema.Checked == true)
                //{
                //    documenter.PublishType = PublishType.SchemaOnly;
                //}

                //if (executionMode == ExecutionMode.Interactive)
                //{
                //    documenter.Publisher = DeterminePublisher(this.comboBox1.SelectedItem.ToString());
                //}

                //documenter.PublishType = PublishType.SpecificApplication;
                documenter.PublishType = PublishType.EntireConfiguration;

                documenter.ResourceFolder = this.txtResourceFolderName.Text;
                documenter.Publisher = DeterminePublisher(this.comboBox2.SelectedItem.ToString());
                documenter.GenerateDocumentation();
            }
            catch (Exception ex)
            {
#if(DEBUG)
                MessageBox.Show(ex.ToString());
#endif
                MessageBox.Show(ex.Message, "Error Generating Documentation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                linkLabel6.Visible = true;
                Cursor.Current = Cursors.Default;
            }

            return;
        }

        #region DeterminePublisher
        private static IPublisher DeterminePublisher(string publisherName)
        {
            switch (publisherName.ToLower())
            {
                case "compiled help":
                case "chm":
                    return new CompiledHelpPublisher();

                //case "word 2003 xml":
                //case "word":
                //    return new WordXmlPublisher();

                // Default to CHM
                default:
                    return new CompiledHelpPublisher();
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage"></param>
        void DocumenterPercentageDocumentationComplete(int percentage)
        {
            progressBar1.Value = percentage;
        }

        private void BtnGenerateClicked(object sender, EventArgs e)
        {
            this.GenerateDocumentation();
        }

        private void ProviderSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            switch (cb.SelectedItem.ToString().ToLower())
            {
                case "compiled help":
                    this.txtResourceFolderName.Enabled = true;
                    break;

                default:
                    this.txtResourceFolderName.Enabled = false;
                    break;
            }
            return;
        }

        private void OutputFolderBrowseClick(object sender, System.EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {
                txtOutputFolderName.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ResourceFolderBrowseClick(object sender, System.EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {
                txtResourceFolderName.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}