
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Microsoft.Services.Tools.BiztalkDocumenter.Publishers;
    using Microsoft.Services.Tools.BiztalkDocumenter.Publishers.Word;
    using Microsoft.Services.Tools.BizTalkOM;
    using Microsoft.Services.Tools.BizTalkOM.Diagnostics;
    using BizTalk.Utilities.SSO.Core;

    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        #region private fields
        private static Documenter documenter = null;
        private static ExecutionMode executionMode = ExecutionMode.CommandLine;
        private static bool stop;
        private static bool showUsage = false;
        private static BizTalkInstallation bizTalkInstallation = new BizTalkInstallation();

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbIncludeReferences;
        private System.Windows.Forms.CheckBox cbRulesConfig;
        private System.Windows.Forms.CheckBox cbShowOutput;
        private System.Windows.Forms.CheckedListBox clbApplications;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox grpBoxAdvanced;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkLabel12;
        private System.Windows.Forms.LinkLabel linkLabel13;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.LinkLabel linkLabel9;
        private System.Windows.Forms.OpenFileDialog dlgConfigFrameworkOpenFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton radioAssembly;
        private System.Windows.Forms.RadioButton radioEntire;
        private System.Windows.Forms.SaveFileDialog dlgConfigFrameworkSaveFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.TextBox txtReportTitle;
        private System.Windows.Forms.TextBox txtResourceFolder;
        private System.Windows.Forms.TextBox txtRulesDatabase;
        private System.Windows.Forms.TextBox txtRulesServer;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.TreeView tvOrchs;
        private SaveFileDialog dlgResultFileSave;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private TextBox txtConfigFrameworkFile;
        //Added for SSO Config Documenter
        private LinkLabel linkLabel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.TabPage tabPage5;
        private CheckedListBox listBoxSsoApplications;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private GroupBox grpBoxSSO;
        private TextBox textBoxSsoProd;
        private TextBox textBoxSsoTest;
        private TextBox textBoxSsoStage;
        //private TextBox textBoxSsoBuild;
        private Button buttonSsoProd;
        private Button buttonSsoTest;
        private Button buttonSsoStage;
        private LinkLabel linkAdditionalFilters;
        private TabPage tabAdditionalFilters;
        private GroupBox grpBoxAdditionalFilters;
        private TextBox txtBREPolicyFilters;
        private Label label12;
        private Label label15;
        private Label label13;
        private TextBox txtAdapterFilters;
        private TextBox txtHostFilters;
        private TextBox txtBREVocabularyFilters;
        private Label label14;
        private Label label16;
        private Button buttonSsoBuild;
        #endregion


        public Form1()
        {
            InitializeComponent();
            //comboBox1.SelectedIndex = 0;

            this.textBox1.Text = documenter.Database;
            this.txtServerName.Text = documenter.Server;
            this.txtOutputDir.Text = documenter.OutputDir;
            this.txtReportTitle.Text = documenter.ReportName;
            this.txtRulesServer.Text = documenter.RulesServer;
            this.txtRulesDatabase.Text = documenter.RulesDatabase;

            documenter.PercentageDocumentationComplete += new UpdatePercentageComplete(Documenter_PercentageDocumentationComplete);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            TraceManager.SmartTrace.TraceIn();

            documenter = new Documenter();

            if (args.Length > 0)
            {
                ProcessArgs(args);
                // there may have been some overrides of the defaults (in the config)
                ProcessOverrides();
            }
            else
            {
                stop = true;
                executionMode = ExecutionMode.Interactive;
            }



            if (showUsage)
            {
                ShowUsage();
                return;
            }
            else
            {
                if (executionMode == ExecutionMode.Interactive)
                {
                    documenter.SetDefaults();
                    Application.Run(new Form1());
                }
                else
                {
                    Form1 f1 = new Form1();
                    f1.GenerateDocumentation();
                }
            }
            System.Diagnostics.Trace.WriteLine("Completed processing");
            return;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label25 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.txtConfigFrameworkFile = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtRulesDatabase = new System.Windows.Forms.TextBox();
            this.txtRulesServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbRulesConfig = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.grpBoxAdvanced = new System.Windows.Forms.GroupBox();
            this.cbIncludeReferences = new System.Windows.Forms.CheckBox();
            this.radioEntire = new System.Windows.Forms.RadioButton();
            this.clbApplications = new System.Windows.Forms.CheckedListBox();
            this.radioAssembly = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.tvOrchs = new System.Windows.Forms.TreeView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtResourceFolder = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbShowOutput = new System.Windows.Forms.CheckBox();
            this.txtReportTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.grpBoxSSO = new System.Windows.Forms.GroupBox();
            this.textBoxSsoProd = new System.Windows.Forms.TextBox();
            this.textBoxSsoTest = new System.Windows.Forms.TextBox();
            this.textBoxSsoStage = new System.Windows.Forms.TextBox();
            this.buttonSsoProd = new System.Windows.Forms.Button();
            this.buttonSsoTest = new System.Windows.Forms.Button();
            this.buttonSsoStage = new System.Windows.Forms.Button();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.listBoxSsoApplications = new System.Windows.Forms.CheckedListBox();
            this.tabAdditionalFilters = new System.Windows.Forms.TabPage();
            this.grpBoxAdditionalFilters = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAdapterFilters = new System.Windows.Forms.TextBox();
            this.txtHostFilters = new System.Windows.Forms.TextBox();
            this.txtBREVocabularyFilters = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBREPolicyFilters = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dlgConfigFrameworkOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgConfigFrameworkSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.linkLabel12 = new System.Windows.Forms.LinkLabel();
            this.linkLabel13 = new System.Windows.Forms.LinkLabel();
            this.dlgResultFileSave = new System.Windows.Forms.SaveFileDialog();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.buttonSsoBuild = new System.Windows.Forms.Button();
            this.linkAdditionalFilters = new System.Windows.Forms.LinkLabel();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.grpBoxAdvanced.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.grpBoxSSO.SuspendLayout();
            this.tabAdditionalFilters.SuspendLayout();
            this.grpBoxAdditionalFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(552, 432);
            this.panel2.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabAdditionalFilters);
            this.tabControl1.Location = new System.Drawing.Point(-8, -24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(568, 464);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.label25);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.txtConfigFrameworkFile);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.txtRulesDatabase);
            this.tabPage1.Controls.Add(this.txtRulesServer);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.cbRulesConfig);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.txtServerName);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(560, 438);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label25.Location = new System.Drawing.Point(112, 351);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(384, 32);
            this.label25.TabIndex = 59;
            this.label25.Text = "Optional: Include the ConfigFramework.exe output XML to generate a system configu" +
    "ration overview";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Silver;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(429, 399);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 19);
            this.button2.TabIndex = 58;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label24.Location = new System.Drawing.Point(112, 401);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(160, 16);
            this.label24.TabIndex = 57;
            this.label24.Text = "ConfigFramework Output XML:";
            // 
            // txtConfigFrameworkFile
            // 
            this.txtConfigFrameworkFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfigFrameworkFile.Location = new System.Drawing.Point(272, 399);
            this.txtConfigFrameworkFile.Name = "txtConfigFrameworkFile";
            this.txtConfigFrameworkFile.Size = new System.Drawing.Size(152, 20);
            this.txtConfigFrameworkFile.TabIndex = 56;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(48)))), ((int)(((byte)(21)))));
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.label26);
            this.panel3.Controls.Add(this.label19);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(560, 40);
            this.panel3.TabIndex = 55;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 38);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label26
            // 
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(48, 24);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(100, 23);
            this.label26.TabIndex = 2;
            this.label26.Text = "Server Options";
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(48, 6);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(392, 23);
            this.label19.TabIndex = 1;
            this.label19.Text = "Microsoft Services BizTalk Documenter";
            // 
            // txtRulesDatabase
            // 
            this.txtRulesDatabase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRulesDatabase.Enabled = false;
            this.txtRulesDatabase.Location = new System.Drawing.Point(272, 224);
            this.txtRulesDatabase.Name = "txtRulesDatabase";
            this.txtRulesDatabase.Size = new System.Drawing.Size(152, 20);
            this.txtRulesDatabase.TabIndex = 52;
            // 
            // txtRulesServer
            // 
            this.txtRulesServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRulesServer.Enabled = false;
            this.txtRulesServer.Location = new System.Drawing.Point(272, 200);
            this.txtRulesServer.Name = "txtRulesServer";
            this.txtRulesServer.Size = new System.Drawing.Size(152, 20);
            this.txtRulesServer.TabIndex = 49;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label7.Location = new System.Drawing.Point(64, 226);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(192, 16);
            this.label7.TabIndex = 51;
            this.label7.Text = "Rules Engine Database Name";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label8.Location = new System.Drawing.Point(80, 202);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(176, 16);
            this.label8.TabIndex = 50;
            this.label8.Text = "Rules Engine Server Name";
            // 
            // cbRulesConfig
            // 
            this.cbRulesConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRulesConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.cbRulesConfig.Location = new System.Drawing.Point(80, 160);
            this.cbRulesConfig.Name = "cbRulesConfig";
            this.cbRulesConfig.Size = new System.Drawing.Size(240, 24);
            this.cbRulesConfig.TabIndex = 48;
            this.cbRulesConfig.Text = "Include Rules Engine Documentation";
            this.cbRulesConfig.CheckedChanged += new System.EventHandler(this.IncludeRulesChecked);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(272, 120);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 20);
            this.textBox1.TabIndex = 40;
            // 
            // txtServerName
            // 
            this.txtServerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerName.Location = new System.Drawing.Point(272, 96);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(152, 20);
            this.txtServerName.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label3.Location = new System.Drawing.Point(64, 120);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(192, 16);
            this.label3.TabIndex = 39;
            this.label3.Text = "Management Database Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label2.Location = new System.Drawing.Point(80, 96);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(176, 16);
            this.label2.TabIndex = 38;
            this.label2.Text = "Management Server Name";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.panel6);
            this.tabPage2.Controls.Add(this.grpBoxAdvanced);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(560, 438);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(48)))), ((int)(((byte)(21)))));
            this.panel6.Controls.Add(this.pictureBox2);
            this.panel6.Controls.Add(this.label27);
            this.panel6.Controls.Add(this.label22);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(560, 40);
            this.panel6.TabIndex = 55;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(8, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 38);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // label27
            // 
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(48, 24);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(152, 23);
            this.label27.TabIndex = 3;
            this.label27.Text = "Select Documentation Type";
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(48, 6);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(392, 23);
            this.label22.TabIndex = 1;
            this.label22.Text = "Microsoft Services BizTalk Documenter";
            // 
            // grpBoxAdvanced
            // 
            this.grpBoxAdvanced.Controls.Add(this.cbIncludeReferences);
            this.grpBoxAdvanced.Controls.Add(this.radioEntire);
            this.grpBoxAdvanced.Controls.Add(this.clbApplications);
            this.grpBoxAdvanced.Controls.Add(this.radioAssembly);
            this.grpBoxAdvanced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAdvanced.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.grpBoxAdvanced.Location = new System.Drawing.Point(21, 56);
            this.grpBoxAdvanced.Name = "grpBoxAdvanced";
            this.grpBoxAdvanced.Size = new System.Drawing.Size(499, 365);
            this.grpBoxAdvanced.TabIndex = 32;
            this.grpBoxAdvanced.TabStop = false;
            this.grpBoxAdvanced.Text = "Options";
            // 
            // cbIncludeReferences
            // 
            this.cbIncludeReferences.AutoSize = true;
            this.cbIncludeReferences.Checked = true;
            this.cbIncludeReferences.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeReferences.Location = new System.Drawing.Point(15, 334);
            this.cbIncludeReferences.Name = "cbIncludeReferences";
            this.cbIncludeReferences.Size = new System.Drawing.Size(174, 17);
            this.cbIncludeReferences.TabIndex = 4;
            this.cbIncludeReferences.Text = "Include referenced applications";
            // 
            // radioEntire
            // 
            this.radioEntire.Checked = true;
            this.radioEntire.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioEntire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.radioEntire.Location = new System.Drawing.Point(16, 24);
            this.radioEntire.Name = "radioEntire";
            this.radioEntire.Size = new System.Drawing.Size(248, 24);
            this.radioEntire.TabIndex = 3;
            this.radioEntire.TabStop = true;
            this.radioEntire.Text = "Document Entire Configuration";
            // 
            // clbApplications
            // 
            this.clbApplications.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.clbApplications.CheckOnClick = true;
            this.clbApplications.Enabled = false;
            this.clbApplications.FormattingEnabled = true;
            this.clbApplications.HorizontalScrollbar = true;
            this.clbApplications.Location = new System.Drawing.Point(16, 78);
            this.clbApplications.Name = "clbApplications";
            this.clbApplications.Size = new System.Drawing.Size(472, 169);
            this.clbApplications.Sorted = true;
            this.clbApplications.TabIndex = 2;
            // 
            // radioAssembly
            // 
            this.radioAssembly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioAssembly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.radioAssembly.Location = new System.Drawing.Point(16, 46);
            this.radioAssembly.Name = "radioAssembly";
            this.radioAssembly.Size = new System.Drawing.Size(248, 24);
            this.radioAssembly.TabIndex = 0;
            this.radioAssembly.Text = "Specific BizTalk Application";
            this.radioAssembly.CheckedChanged += new System.EventHandler(this.AdvancedCheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.panel5);
            this.tabPage3.Controls.Add(this.linkLabel9);
            this.tabPage3.Controls.Add(this.linkLabel8);
            this.tabPage3.Controls.Add(this.linkLabel7);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.tvOrchs);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(560, 438);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Orchestration Info";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(48)))), ((int)(((byte)(21)))));
            this.panel5.Controls.Add(this.pictureBox3);
            this.panel5.Controls.Add(this.label29);
            this.panel5.Controls.Add(this.label21);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(560, 40);
            this.panel5.TabIndex = 65;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(8, 1);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(40, 38);
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // label29
            // 
            this.label29.ForeColor = System.Drawing.Color.White;
            this.label29.Location = new System.Drawing.Point(48, 24);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(152, 23);
            this.label29.TabIndex = 4;
            this.label29.Text = "Orchestration Info";
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(48, 6);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(392, 23);
            this.label21.TabIndex = 1;
            this.label21.Text = "Microsoft Services BizTalk Documenter";
            // 
            // linkLabel9
            // 
            this.linkLabel9.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel9.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel9.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel9.Image")));
            this.linkLabel9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel9.LinkArea = new System.Windows.Forms.LinkArea(0, 19);
            this.linkLabel9.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel9.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel9.Location = new System.Drawing.Point(16, 120);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(139, 23);
            this.linkLabel9.TabIndex = 64;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "List Orchestrations";
            this.linkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel9.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel9.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel9_LinkClicked);
            // 
            // linkLabel8
            // 
            this.linkLabel8.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel8.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel8.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel8.Image")));
            this.linkLabel8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel8.LinkArea = new System.Windows.Forms.LinkArea(0, 28);
            this.linkLabel8.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel8.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel8.Location = new System.Drawing.Point(236, 368);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(205, 23);
            this.linkLabel8.TabIndex = 63;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "Save Selected Orchestrations";
            this.linkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel8.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel8_LinkClicked);
            // 
            // linkLabel7
            // 
            this.linkLabel7.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel7.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel7.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel7.Image")));
            this.linkLabel7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel7.LinkArea = new System.Windows.Forms.LinkArea(0, 26);
            this.linkLabel7.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel7.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel7.Location = new System.Drawing.Point(20, 368);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(187, 23);
            this.linkLabel7.TabIndex = 62;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "View Current Orchestration";
            this.linkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel7.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel7_LinkClicked);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label11.Location = new System.Drawing.Point(24, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(496, 40);
            this.label11.TabIndex = 60;
            this.label11.Text = resources.GetString("label11.Text");
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tvOrchs
            // 
            this.tvOrchs.CheckBoxes = true;
            this.tvOrchs.Location = new System.Drawing.Point(16, 152);
            this.tvOrchs.Name = "tvOrchs";
            this.tvOrchs.Size = new System.Drawing.Size(504, 200);
            this.tvOrchs.Sorted = true;
            this.tvOrchs.TabIndex = 0;
            this.tvOrchs.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvOrchsAfterCheck);
            this.tvOrchs.DoubleClick += new System.EventHandler(this.TvOrchsDoubleClick);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.panel4);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.txtResourceFolder);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.cbShowOutput);
            this.tabPage4.Controls.Add(this.txtReportTitle);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.comboBox1);
            this.tabPage4.Controls.Add(this.txtOutputDir);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.btnBrowse);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(560, 438);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Output";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(389, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 19);
            this.button1.TabIndex = 57;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(48)))), ((int)(((byte)(21)))));
            this.panel4.Controls.Add(this.pictureBox4);
            this.panel4.Controls.Add(this.label30);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(560, 40);
            this.panel4.TabIndex = 61;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(8, 1);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(40, 38);
            this.pictureBox4.TabIndex = 5;
            this.pictureBox4.TabStop = false;
            // 
            // label30
            // 
            this.label30.ForeColor = System.Drawing.Color.White;
            this.label30.Location = new System.Drawing.Point(48, 24);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(152, 23);
            this.label30.TabIndex = 4;
            this.label30.Text = "Output Options";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(48, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(392, 23);
            this.label18.TabIndex = 1;
            this.label18.Text = "Microsoft Services BizTalk Documenter";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label5.Location = new System.Drawing.Point(64, 368);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 58;
            this.label5.Text = "Resource Folder";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label10.Location = new System.Drawing.Point(8, 217);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(545, 111);
            this.label10.TabIndex = 60;
            this.label10.Text = resources.GetString("label10.Text");
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtResourceFolder
            // 
            this.txtResourceFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResourceFolder.Location = new System.Drawing.Point(176, 366);
            this.txtResourceFolder.Name = "txtResourceFolder";
            this.txtResourceFolder.Size = new System.Drawing.Size(208, 20);
            this.txtResourceFolder.TabIndex = 56;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label9.Location = new System.Drawing.Point(22, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(515, 27);
            this.label9.TabIndex = 59;
            this.label9.Text = "If you are using the CHM output provider you may specify a resource folder contai" +
    "ning a \'titlePage.htm\' and any associated images to produce a custom look and fe" +
    "el to your CHM title page.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbShowOutput
            // 
            this.cbShowOutput.Checked = true;
            this.cbShowOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.cbShowOutput.Location = new System.Drawing.Point(184, 165);
            this.cbShowOutput.Name = "cbShowOutput";
            this.cbShowOutput.Size = new System.Drawing.Size(200, 24);
            this.cbShowOutput.TabIndex = 55;
            this.cbShowOutput.Text = "Show Output On Completion";
            // 
            // txtReportTitle
            // 
            this.txtReportTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReportTitle.Location = new System.Drawing.Point(184, 80);
            this.txtReportTitle.Name = "txtReportTitle";
            this.txtReportTitle.Size = new System.Drawing.Size(227, 20);
            this.txtReportTitle.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label6.Location = new System.Drawing.Point(104, 82);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 54;
            this.label6.Text = "Report Title";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label4.Location = new System.Drawing.Point(80, 130);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 52;
            this.label4.Text = "Output Provider";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(184, 128);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(229, 21);
            this.comboBox1.TabIndex = 51;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ProviderSelectedIndexChanged);
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputDir.Location = new System.Drawing.Point(184, 104);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(200, 20);
            this.txtOutputDir.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label1.Location = new System.Drawing.Point(96, 106);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "Output Folder";
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.Silver;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(388, 104);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 19);
            this.btnBrowse.TabIndex = 49;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Controls.Add(this.panel7);
            this.tabPage5.Controls.Add(this.label33);
            this.tabPage5.Controls.Add(this.grpBoxSSO);
            this.tabPage5.Controls.Add(this.listBoxSsoApplications);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(560, 438);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "SSO Config";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(48)))), ((int)(((byte)(21)))));
            this.panel7.Controls.Add(this.pictureBox5);
            this.panel7.Controls.Add(this.label31);
            this.panel7.Controls.Add(this.label32);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(560, 40);
            this.panel7.TabIndex = 55;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(8, 1);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(40, 38);
            this.pictureBox5.TabIndex = 4;
            this.pictureBox5.TabStop = false;
            // 
            // label31
            // 
            this.label31.ForeColor = System.Drawing.Color.White;
            this.label31.Location = new System.Drawing.Point(48, 24);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(152, 23);
            this.label31.TabIndex = 3;
            this.label31.Text = "Select SSO Configuration";
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(48, 6);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(392, 23);
            this.label32.TabIndex = 1;
            this.label32.Text = "Microsoft Services BizTalk Documenter";
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label33.Location = new System.Drawing.Point(25, 47);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(493, 33);
            this.label33.TabIndex = 91;
            this.label33.Text = "Select the SSO Applications whose key/value pairs you would like included in the " +
    "documentation (Included under separate heading)";
            // 
            // grpBoxSSO
            // 
            this.grpBoxSSO.Controls.Add(this.textBoxSsoProd);
            this.grpBoxSSO.Controls.Add(this.textBoxSsoTest);
            this.grpBoxSSO.Controls.Add(this.textBoxSsoStage);
            this.grpBoxSSO.Controls.Add(this.buttonSsoProd);
            this.grpBoxSSO.Controls.Add(this.buttonSsoTest);
            this.grpBoxSSO.Controls.Add(this.buttonSsoStage);
            this.grpBoxSSO.Controls.Add(this.label34);
            this.grpBoxSSO.Controls.Add(this.label35);
            this.grpBoxSSO.Controls.Add(this.label36);
            this.grpBoxSSO.Controls.Add(this.label37);
            this.grpBoxSSO.Controls.Add(this.label38);
            this.grpBoxSSO.Location = new System.Drawing.Point(16, 260);
            this.grpBoxSSO.Name = "grpBoxSSO";
            this.grpBoxSSO.Size = new System.Drawing.Size(519, 175);
            this.grpBoxSSO.TabIndex = 78;
            this.grpBoxSSO.TabStop = false;
            this.grpBoxSSO.Text = "Include exported SSO application configurations";
            // 
            // textBoxSsoProd
            // 
            this.textBoxSsoProd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSsoProd.Location = new System.Drawing.Point(161, 147);
            this.textBoxSsoProd.Name = "textBoxSsoProd";
            this.textBoxSsoProd.Size = new System.Drawing.Size(310, 20);
            this.textBoxSsoProd.TabIndex = 99;
            // 
            // textBoxSsoTest
            // 
            this.textBoxSsoTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSsoTest.Location = new System.Drawing.Point(161, 121);
            this.textBoxSsoTest.Name = "textBoxSsoTest";
            this.textBoxSsoTest.Size = new System.Drawing.Size(310, 20);
            this.textBoxSsoTest.TabIndex = 98;
            // 
            // textBoxSsoStage
            // 
            this.textBoxSsoStage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSsoStage.Location = new System.Drawing.Point(162, 95);
            this.textBoxSsoStage.Name = "textBoxSsoStage";
            this.textBoxSsoStage.Size = new System.Drawing.Size(310, 20);
            this.textBoxSsoStage.TabIndex = 97;
            // 
            // buttonSsoProd
            // 
            this.buttonSsoProd.BackColor = System.Drawing.Color.Silver;
            this.buttonSsoProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSsoProd.Location = new System.Drawing.Point(478, 146);
            this.buttonSsoProd.Name = "buttonSsoProd";
            this.buttonSsoProd.Size = new System.Drawing.Size(35, 22);
            this.buttonSsoProd.TabIndex = 95;
            this.buttonSsoProd.Text = "...";
            this.buttonSsoProd.UseVisualStyleBackColor = false;
            this.buttonSsoProd.Click += new System.EventHandler(this.ButtonSsoClick);
            // 
            // buttonSsoTest
            // 
            this.buttonSsoTest.BackColor = System.Drawing.Color.Silver;
            this.buttonSsoTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSsoTest.Location = new System.Drawing.Point(478, 120);
            this.buttonSsoTest.Name = "buttonSsoTest";
            this.buttonSsoTest.Size = new System.Drawing.Size(35, 22);
            this.buttonSsoTest.TabIndex = 94;
            this.buttonSsoTest.Text = "...";
            this.buttonSsoTest.UseVisualStyleBackColor = false;
            this.buttonSsoTest.Click += new System.EventHandler(this.ButtonSsoClick);
            // 
            // buttonSsoStage
            // 
            this.buttonSsoStage.BackColor = System.Drawing.Color.Silver;
            this.buttonSsoStage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSsoStage.Location = new System.Drawing.Point(478, 94);
            this.buttonSsoStage.Name = "buttonSsoStage";
            this.buttonSsoStage.Size = new System.Drawing.Size(35, 22);
            this.buttonSsoStage.TabIndex = 93;
            this.buttonSsoStage.Text = "...";
            this.buttonSsoStage.UseVisualStyleBackColor = false;
            this.buttonSsoStage.Click += new System.EventHandler(this.ButtonSsoClick);
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label34.Location = new System.Drawing.Point(12, 22);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(493, 42);
            this.label34.TabIndex = 90;
            this.label34.Text = resources.GetString("label34.Text");
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label35.Location = new System.Drawing.Point(12, 148);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(143, 18);
            this.label35.TabIndex = 84;
            this.label35.Text = "SSO Configuration PROD:";
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label36.Location = new System.Drawing.Point(12, 122);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(143, 18);
            this.label36.TabIndex = 82;
            this.label36.Text = "SSO Configuration TEST:";
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label37.Location = new System.Drawing.Point(12, 96);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(143, 18);
            this.label37.TabIndex = 80;
            this.label37.Text = "SSO Configuration STAGE:";
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label38.Location = new System.Drawing.Point(12, 70);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(143, 18);
            this.label38.TabIndex = 78;
            this.label38.Text = "SSO Configuration BUILD:";
            // 
            // listBoxSsoApplications
            // 
            this.listBoxSsoApplications.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSsoApplications.CheckOnClick = true;
            this.listBoxSsoApplications.FormattingEnabled = true;
            this.listBoxSsoApplications.Location = new System.Drawing.Point(31, 80);
            this.listBoxSsoApplications.Name = "listBoxSsoApplications";
            this.listBoxSsoApplications.Size = new System.Drawing.Size(487, 167);
            this.listBoxSsoApplications.TabIndex = 92;
            // 
            // tabAdditionalFilters
            // 
            this.tabAdditionalFilters.BackColor = System.Drawing.Color.White;
            this.tabAdditionalFilters.Controls.Add(this.grpBoxAdditionalFilters);
            this.tabAdditionalFilters.Location = new System.Drawing.Point(4, 22);
            this.tabAdditionalFilters.Name = "tabAdditionalFilters";
            this.tabAdditionalFilters.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdditionalFilters.Size = new System.Drawing.Size(560, 438);
            this.tabAdditionalFilters.TabIndex = 5;
            this.tabAdditionalFilters.Text = "tabAdditionalFilters";
            // 
            // grpBoxAdditionalFilters
            // 
            this.grpBoxAdditionalFilters.Controls.Add(this.label16);
            this.grpBoxAdditionalFilters.Controls.Add(this.label15);
            this.grpBoxAdditionalFilters.Controls.Add(this.label13);
            this.grpBoxAdditionalFilters.Controls.Add(this.txtAdapterFilters);
            this.grpBoxAdditionalFilters.Controls.Add(this.txtHostFilters);
            this.grpBoxAdditionalFilters.Controls.Add(this.txtBREVocabularyFilters);
            this.grpBoxAdditionalFilters.Controls.Add(this.label14);
            this.grpBoxAdditionalFilters.Controls.Add(this.txtBREPolicyFilters);
            this.grpBoxAdditionalFilters.Controls.Add(this.label12);
            this.grpBoxAdditionalFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAdditionalFilters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.grpBoxAdditionalFilters.Location = new System.Drawing.Point(31, 37);
            this.grpBoxAdditionalFilters.Name = "grpBoxAdditionalFilters";
            this.grpBoxAdditionalFilters.Size = new System.Drawing.Size(499, 274);
            this.grpBoxAdditionalFilters.TabIndex = 33;
            this.grpBoxAdditionalFilters.TabStop = false;
            this.grpBoxAdditionalFilters.Text = "Additional Filters";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label16.Location = new System.Drawing.Point(6, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(487, 93);
            this.label16.TabIndex = 61;
            this.label16.Text = resources.GetString("label16.Text");
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label15.Location = new System.Drawing.Point(6, 140);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label15.Size = new System.Drawing.Size(125, 20);
            this.label15.TabIndex = 41;
            this.label15.Text = "Adapters Filter";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label13.Location = new System.Drawing.Point(6, 193);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label13.Size = new System.Drawing.Size(125, 20);
            this.label13.TabIndex = 41;
            this.label13.Text = "BRE Vocabularies Filter";
            // 
            // txtAdapterFilters
            // 
            this.txtAdapterFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdapterFilters.Location = new System.Drawing.Point(137, 138);
            this.txtAdapterFilters.Name = "txtAdapterFilters";
            this.txtAdapterFilters.Size = new System.Drawing.Size(343, 20);
            this.txtAdapterFilters.TabIndex = 40;
            // 
            // txtHostFilters
            // 
            this.txtHostFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHostFilters.Location = new System.Drawing.Point(137, 112);
            this.txtHostFilters.Name = "txtHostFilters";
            this.txtHostFilters.Size = new System.Drawing.Size(343, 20);
            this.txtHostFilters.TabIndex = 39;
            // 
            // txtBREVocabularyFilters
            // 
            this.txtBREVocabularyFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBREVocabularyFilters.Location = new System.Drawing.Point(137, 191);
            this.txtBREVocabularyFilters.Name = "txtBREVocabularyFilters";
            this.txtBREVocabularyFilters.Size = new System.Drawing.Size(343, 20);
            this.txtBREVocabularyFilters.TabIndex = 42;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label14.Location = new System.Drawing.Point(6, 114);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label14.Size = new System.Drawing.Size(125, 20);
            this.label14.TabIndex = 40;
            this.label14.Text = "Hosts Filter";
            // 
            // txtBREPolicyFilters
            // 
            this.txtBREPolicyFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBREPolicyFilters.Location = new System.Drawing.Point(137, 165);
            this.txtBREPolicyFilters.Name = "txtBREPolicyFilters";
            this.txtBREPolicyFilters.Size = new System.Drawing.Size(343, 20);
            this.txtBREPolicyFilters.TabIndex = 41;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.label12.Location = new System.Drawing.Point(6, 167);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label12.Size = new System.Drawing.Size(125, 20);
            this.label12.TabIndex = 40;
            this.label12.Text = "BRE Policies Filter";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            // 
            // dlgConfigFrameworkOpenFile
            // 
            this.dlgConfigFrameworkOpenFile.Filter = "XML Files|*.xml";
            this.dlgConfigFrameworkOpenFile.Title = "Select configuration file";
            this.dlgConfigFrameworkOpenFile.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgConfigFrameworkOpenFile_FileOk);
            // 
            // dlgConfigFrameworkSaveFile
            // 
            this.dlgConfigFrameworkSaveFile.Filter = "XML Files|*.xml";
            this.dlgConfigFrameworkSaveFile.Title = "Save configuration file";
            this.dlgConfigFrameworkSaveFile.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgConfigFrameworkSaveFile_FileOk);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
            this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(0, 14);
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel1.Location = new System.Drawing.Point(22, 506);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(120, 23);
            this.linkLabel1.TabIndex = 25;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Output Options";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel2.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel2.Image")));
            this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel2.LinkArea = new System.Windows.Forms.LinkArea(0, 18);
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel2.Location = new System.Drawing.Point(238, 506);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(269, 23);
            this.linkLabel2.TabIndex = 26;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Select Assemblies And Orchestrations";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel2.UseCompatibleTextRendering = true;
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel3.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel3.Image")));
            this.linkLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel3.LinkArea = new System.Windows.Forms.LinkArea(0, 25);
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel3.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel3.Location = new System.Drawing.Point(238, 482);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(191, 23);
            this.linkLabel3.TabIndex = 27;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Select Documentation Scope";
            this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel3.UseCompatibleTextRendering = true;
            this.linkLabel3.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(20, 324);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(208, 23);
            this.label17.TabIndex = 30;
            this.label17.Text = "Advanced Documentation Options:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Location = new System.Drawing.Point(216, 332);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 3);
            this.panel1.TabIndex = 31;
            // 
            // linkLabel6
            // 
            this.linkLabel6.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel6.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel6.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel6.Image")));
            this.linkLabel6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel6.LinkArea = new System.Windows.Forms.LinkArea(0, 22);
            this.linkLabel6.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel6.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel6.Location = new System.Drawing.Point(22, 562);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(176, 23);
            this.linkLabel6.TabIndex = 32;
            this.linkLabel6.TabStop = true;
            this.linkLabel6.Text = "Generate Documentation";
            this.linkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel6.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel6_LinkClicked);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(249, 564);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(288, 16);
            this.progressBar1.TabIndex = 55;
            this.progressBar1.Visible = false;
            // 
            // linkLabel12
            // 
            this.linkLabel12.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel12.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel12.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel12.Image")));
            this.linkLabel12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel12.LinkArea = new System.Windows.Forms.LinkArea(0, 14);
            this.linkLabel12.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel12.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel12.Location = new System.Drawing.Point(22, 482);
            this.linkLabel12.Name = "linkLabel12";
            this.linkLabel12.Size = new System.Drawing.Size(120, 23);
            this.linkLabel12.TabIndex = 56;
            this.linkLabel12.TabStop = true;
            this.linkLabel12.Text = "Server Options";
            this.linkLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel12.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel12.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel12_LinkClicked);
            // 
            // linkLabel13
            // 
            this.linkLabel13.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel13.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel13.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel13.Image")));
            this.linkLabel13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel13.LinkArea = new System.Windows.Forms.LinkArea(0, 4);
            this.linkLabel13.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel13.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel13.Location = new System.Drawing.Point(238, 559);
            this.linkLabel13.Name = "linkLabel13";
            this.linkLabel13.Size = new System.Drawing.Size(56, 23);
            this.linkLabel13.TabIndex = 57;
            this.linkLabel13.TabStop = true;
            this.linkLabel13.Text = "Quit";
            this.linkLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel13.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel13.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel13_LinkClicked);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(552, 674);
            this.shapeContainer1.TabIndex = 58;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 10;
            this.lineShape1.X2 = 519;
            this.lineShape1.Y1 = 456;
            this.lineShape1.Y2 = 455;
            // 
            // linkLabel4
            // 
            this.linkLabel4.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel4.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel4.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel4.Image")));
            this.linkLabel4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel4.LinkArea = new System.Windows.Forms.LinkArea(0, 18);
            this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel4.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkLabel4.Location = new System.Drawing.Point(238, 529);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(201, 17);
            this.linkLabel4.TabIndex = 59;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Select SSO Configuration";
            this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel4.UseCompatibleTextRendering = true;
            this.linkLabel4.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // buttonSsoBuild
            // 
            this.buttonSsoBuild.BackColor = System.Drawing.Color.Silver;
            this.buttonSsoBuild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSsoBuild.Location = new System.Drawing.Point(478, 68);
            this.buttonSsoBuild.Name = "buttonSsoBuild";
            this.buttonSsoBuild.Size = new System.Drawing.Size(35, 22);
            this.buttonSsoBuild.TabIndex = 92;
            this.buttonSsoBuild.Text = "...";
            this.buttonSsoBuild.UseVisualStyleBackColor = false;
            this.buttonSsoBuild.Click += new System.EventHandler(this.ButtonSsoClick);
            // 
            // linkAdditionalFilters
            // 
            this.linkAdditionalFilters.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkAdditionalFilters.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkAdditionalFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkAdditionalFilters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkAdditionalFilters.Image = ((System.Drawing.Image)(resources.GetObject("linkAdditionalFilters.Image")));
            this.linkAdditionalFilters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkAdditionalFilters.LinkArea = new System.Windows.Forms.LinkArea(0, 14);
            this.linkAdditionalFilters.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkAdditionalFilters.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(119)))), ((int)(((byte)(153)))));
            this.linkAdditionalFilters.Location = new System.Drawing.Point(22, 529);
            this.linkAdditionalFilters.Name = "linkAdditionalFilters";
            this.linkAdditionalFilters.Size = new System.Drawing.Size(122, 23);
            this.linkAdditionalFilters.TabIndex = 60;
            this.linkAdditionalFilters.TabStop = true;
            this.linkAdditionalFilters.Text = "Additional Filters";
            this.linkAdditionalFilters.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkAdditionalFilters.UseCompatibleTextRendering = true;
            this.linkAdditionalFilters.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkAdditionalFilters.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAdditionalFilters_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.linkLabel13;
            this.ClientSize = new System.Drawing.Size(552, 674);
            this.Controls.Add(this.linkAdditionalFilters);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.linkLabel13);
            this.Controls.Add(this.linkLabel12);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.linkLabel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Microsoft Services BizTalk Documenter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.grpBoxAdvanced.ResumeLayout(false);
            this.grpBoxAdvanced.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.grpBoxSSO.ResumeLayout(false);
            this.grpBoxSSO.PerformLayout();
            this.tabAdditionalFilters.ResumeLayout(false);
            this.grpBoxAdditionalFilters.ResumeLayout(false);
            this.grpBoxAdditionalFilters.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region DeterminePublisher
        private static IPublisher DeterminePublisher(string publisherName)
        {
            if (String.IsNullOrEmpty(publisherName) ||
                publisherName.ToLower().Contains("compiled") ||
                publisherName.ToLower().Contains("help") ||
                publisherName.ToLower().Contains("chm"))
            {
                return new CompiledHelpPublisher();
            }
            else
            {
                return new WordXmlPublisher();
            }

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
                linkLabel13.Visible = false;
                progressBar1.Visible = true;

                if (!ValidateReportName(documenter.ReportName))
                {
                    throw new ApplicationException("Report title contains some invalid characters.");
                }

                if (radioAssembly.Checked == true)
                {
                    if (clbApplications.CheckedItems.Count == 0)
                    {
                        MessageBox.Show("No applications have been selected for documentation", "Error Generating Documentation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    documenter.Applications.Clear();

                    foreach (object item in clbApplications.CheckedItems)
                    {
                        documenter.Applications.Add(item.ToString());
                    }

                    documenter.IncludeReferences = this.cbIncludeReferences.Checked;
                    documenter.PublishType = PublishType.SpecificApplication;
                }

                if (executionMode == ExecutionMode.Interactive)
                {
                    documenter.Publisher = DeterminePublisher(this.comboBox1.SelectedItem.ToString());
                }

                documenter.GenerateDocumentation();
            }
            catch (Exception ex)
            {
#if(DEBUG)
                MessageBox.Show(ex.ToString());
#endif
                if (executionMode == ExecutionMode.Interactive)
                {
                    MessageBox.Show(ex.Message, "Error Generating Documentation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }
            finally
            {
                progressBar1.Visible = false;
                linkLabel13.Visible = true;
                Cursor.Current = Cursors.Default;
            }

            return;
        }

        #region btnBrowse_Click
        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            DialogResult res = this.folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.txtOutputDir.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
        #endregion

        #region btnGenerate_Click
        private void btnGenerate_Click(object sender, System.EventArgs e)
        {
            documenter.Server = this.txtServerName.Text;
            documenter.Database = this.textBox1.Text;
            documenter.OutputDir = txtOutputDir.Text;
            documenter.ReportName = this.txtReportTitle.Text;
            documenter.ShowOutput = this.cbShowOutput.Checked;
            documenter.RulesServer = this.txtRulesServer.Text;
            documenter.RulesDatabase = this.txtRulesDatabase.Text;
            documenter.DocumentRules = this.cbRulesConfig.Checked;
            documenter.ResourceFolder = this.txtResourceFolder.Text;
            documenter.IncludeReferences = this.cbIncludeReferences.Checked;
            this.GenerateDocumentation();
        }
        #endregion

        #region ProcessArgs

        private static void ProcessArgs(string[] args)
        {
            if (args.Length == 0)
                stop = true;

            ArgParser parser = new ArgParser(args);
            ParserError error;

            //Help is unique as it is not a classic 'argument' with a name and a value
            if (args[0].Equals("/help") || args[0].Equals("/h") || args[0].Equals("/?"))
            {
                Form1.showUsage = true;
                return;
            }

            bool useDefaults = parser.Exists("defaults", "def");
            if (useDefaults)
            {
                // Use the documenters in-built defaults where possible
                documenter.SetDefaults();

                documenter.Publisher = DeterminePublisher("Compiled Help");
                documenter.ShowOutput = true;
            }
            else
            {
                documenter.Server = GetSafeString(parser.GetValue("server", "s", out error));
                documenter.Database = GetSafeString(parser.GetValue("database", "d", out error));
                documenter.RulesServer = GetSafeString(parser.GetValue("rs", out error));
                documenter.RulesDatabase = GetSafeString(parser.GetValue("rd", out error));

                documenter.OutputDir = GetSafeString(parser.GetValue("outputdir", "o", out error));
                documenter.ReportName = GetSafeString(parser.GetValue("title", "t", out error));
                documenter.CustomDescriptionsFileName = GetSafeString(parser.GetValue("commentfile", "c", out error));
                documenter.DocumentRules = parser.Exists("rules");
                documenter.ShowOutput = parser.Exists("show");
            }

            string applications = GetSafeString(parser.GetValue("applications", "a", out error));
            if (!String.IsNullOrEmpty(applications))
            {
                documenter.PublishType = PublishType.SpecificApplication;
                documenter.Applications = Form1.SplitStringToArrayList(applications, ",", true);
            }

            string provider = GetSafeString(parser.GetValue("/provider", "p", out error));
            documenter.Publisher = Form1.DeterminePublisher(provider);

            // Allow a user specified folder even if the rest is set to defaults
            string outDir = GetSafeString(parser.GetValue("outputdir", "o", out error));
            if (!String.IsNullOrEmpty(outDir))
            {
                if (!Directory.Exists(outDir))
                {
                    //TraceManager.SmartTrace.TraceInfo("attempting to create the specified folder");
                    Directory.CreateDirectory(outDir);
                }
                documenter.OutputDir = outDir;
            }

            // check if a report name is specified
            string reportName = GetSafeString(parser.GetValue("title", "t", out error));
            if (!String.IsNullOrEmpty(reportName))
            {
                documenter.ReportName = reportName;

            }

            // check if rules are specified
            documenter.DocumentRules = parser.Exists("rules");

            // Finally , check if any overrides are specified in the config file
        }

        private static void ProcessOverrides()
        {
            bool allowOverrides = false;
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["allow_cmdline_overrides"]))
            {
                allowOverrides = Boolean.Parse(ConfigurationManager.AppSettings["allow_cmdline_overrides"]);
            }
            // currently we only support specific config overrides
            if (allowOverrides)
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["default_output_folder"]))
                {
                    documenter.OutputDir = ConfigurationManager.AppSettings["default_output_folder"];
                }

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["default_report_name"]))
                {
                    documenter.ReportName = ConfigurationManager.AppSettings["default_report_name"];
                }

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["default_publisher"]))
                {
                    documenter.Publisher =
                        Form1.DeterminePublisher(ConfigurationManager.AppSettings["default_publisher"]);
                }

            }
        }

        #endregion

        private static string GetSafeString(string source)
        {
            if (String.IsNullOrEmpty(source))
                return String.Empty;
            else
            {
                return source;

            }
        }


        private static ArrayList SplitStringToArrayList(string source, string delimiter)
        {
            return SplitStringToArrayList(source, delimiter, false);
        }


        private static ArrayList SplitStringToArrayList(string source, string delimiter, bool forceLowerCase)
        {
            string[] sourceArray = source.Split(delimiter.ToCharArray());
            ArrayList results = new ArrayList();
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (forceLowerCase)
                {
                    results.Add(sourceArray[i].ToLower());
                }
                else
                {
                    results.Add(sourceArray[i]);
                }

            }

            return results;
        }

        private static void ShowUsage()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            StreamReader sr = new StreamReader(a.GetManifestResourceStream("Microsoft.Services.Tools.BiztalkDocumenter.Res.usage.txt"));
            string help = sr.ReadToEnd();
            sr.Close();

            MessageBox.Show(help, "Microsoft.Services.Tools.BiztalkDocumenter Usage");
        }



        #region AdvancedCheckedChanged

        private void AdvancedCheckedChanged(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (((RadioButton)sender).Checked == true)
            {
                this.clbApplications.Items.Clear();

                BizTalkInstallation bi = new BizTalkInstallation();
                bi.Server = this.txtServerName.Text;
                bi.MgmtDatabaseName = this.textBox1.Text;

                ArrayList applications = bi.GetApplicationNames();

                int appCount = applications.Count;

                if (appCount > 0)
                {
                    clbApplications.Enabled = true;
                    clbApplications.BackColor = SystemColors.Window;

                    foreach (object o in applications)
                    {
                        this.clbApplications.Items.Add(o);
                    }
                }
                else
                {
                    this.clbApplications.Items.Add("No BizTalk Applications found, select 'Application Definition' to configure applications");
                }
            }
            else
            {
                clbApplications.BackColor = SystemColors.InactiveBorder;
                clbApplications.Enabled = false;
                clbApplications.Items.Clear();
            }
            Cursor.Current = Cursors.Default;
            return;
        }

        #endregion

        private void Documenter_PercentageDocumentationComplete(int percentage)
        {
            progressBar1.Value = percentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        private bool ValidateReportName(string reportName)
        {
            if (reportName.IndexOfAny(new char[] { "\"".ToCharArray()[0], '/', '*', ':', '?', '"', '<', '>', '|' }) >= 0)
            {
                return false;
            }
            return true;
        }

        private void TvOrchsDoubleClick(object sender, System.EventArgs e)
        {
            DisplayOrchestration();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayOrchestration()
        {
            try
            {
                if (this.tvOrchs.SelectedNode != null &&
                    this.tvOrchs.SelectedNode.Tag != null)
                {
                    string tagString = this.tvOrchs.SelectedNode.Tag as string;
                    string[] nameParts = tagString.Split(new char[] { '|' }, 2);
                    string asmName = nameParts[0];
                    string orchName = nameParts[1];

                    BizTalkInstallation bizTalkInstallation = new BizTalkInstallation();
                    bizTalkInstallation.Server = this.txtServerName.Text;
                    bizTalkInstallation.MgmtDatabaseName = this.textBox1.Text;

                    Orchestration o = bizTalkInstallation.GetOrchestration(asmName, orchName);
                    OrchestrationViewer ov = new OrchestrationViewer(o);
                    ov.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying orchestration: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TvOrchsAfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            foreach (TreeNode tn in e.Node.Nodes)
            {
                tn.Checked = e.Node.Checked;
            }
        }

        private void IncludeRulesChecked(object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            label7.Enabled = cb.Checked;
            label8.Enabled = cb.Checked;
            txtRulesDatabase.Enabled = cb.Checked;
            txtRulesServer.Enabled = cb.Checked;
            documenter.DocumentRules = cb.Checked;
        }

        private void btnBrowse2_Click(object sender, System.EventArgs e)
        {
            DialogResult res = this.folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.txtResourceFolder.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void ProviderSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            switch (cb.SelectedItem.ToString().ToLower())
            {
                case "compiled help":
                    this.txtResourceFolder.Enabled = true;
                    break;

                default:
                    this.txtResourceFolder.Enabled = false;
                    break;
            }
            return;
        }

        #region Link Buttons

        private void linkLabel7_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            DisplayOrchestration();
        }

        private void linkLabel13_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel12_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage1;
        }

        private void linkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage2;
        }

        private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage3;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage5;
           //MTB 8/03/2014 - Populate SSO Applications
            if (listBoxSsoApplications.Items.Count == 0)
            {
                Wait();
                tabControl1.SelectedTab = tabPage5;
                string[] currentUserApplications = ConfigManager.GetCurrentUserApplications();
                foreach (string currentUserApplication in currentUserApplications)
                {
                    if (!currentUserApplication.StartsWith("{") && !currentUserApplication.EndsWith("}"))
                        listBoxSsoApplications.Items.Add(currentUserApplication);
                }
                Go();
            }
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage4;
        }

        private void linkLabel6_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            documenter.Server = this.txtServerName.Text;
            documenter.Database = this.textBox1.Text;
            documenter.OutputDir = txtOutputDir.Text;
            documenter.ReportName = this.txtReportTitle.Text;
            documenter.ShowOutput = this.cbShowOutput.Checked;
            documenter.RulesServer = this.txtRulesServer.Text;
            documenter.RulesDatabase = this.txtRulesDatabase.Text;
            documenter.DocumentRules = this.cbRulesConfig.Checked;
            documenter.ResourceFolder = this.txtResourceFolder.Text;
            documenter.ConfigFrameworkFileName = this.txtConfigFrameworkFile.Text;
            documenter.IncludeReferences = this.cbIncludeReferences.Checked;

            if (!String.IsNullOrEmpty(this.txtHostFilters.Text)) // PCA 2015-01-06
                documenter.HostFilters = this.txtHostFilters.Text.Split('|');
            else
                documenter.HostFilters = new string[0] { };

            if (!String.IsNullOrEmpty(this.txtAdapterFilters.Text)) // PCA 2015-01-06
                documenter.AdapterFilters = this.txtAdapterFilters.Text.Split('|');
            else
                documenter.AdapterFilters = new string[0] { };

            if (!String.IsNullOrEmpty(this.txtBREPolicyFilters.Text)) // PCA 2015-01-06
                documenter.RulesPolicyFilters = this.txtBREPolicyFilters.Text.Split('|');
            else
                documenter.RulesPolicyFilters = new string[0] { };

            if (!String.IsNullOrEmpty(this.txtBREVocabularyFilters.Text)) // PCA 2015-01-06
                documenter.RulesVocabularyFilters = this.txtBREVocabularyFilters.Text.Split('|');
            else
                documenter.RulesVocabularyFilters = new string[0] { };


            //TO BE ADDED FROM Red Eyed Monster version
            //documenter.SsoBuild = textBoxSsoBuild.Text;
            documenter.SsoStage = textBoxSsoStage.Text;
            documenter.SsoTest = textBoxSsoTest.Text;
            documenter.SsoProd = textBoxSsoProd.Text;


            if (listBoxSsoApplications.CheckedItems.Count > 0)
            {
                string[] ssoApplications = new string[listBoxSsoApplications.CheckedItems.Count];
                int loopIndex = 0;
                foreach (string checkedItem in listBoxSsoApplications.CheckedItems)
                {
                    ssoApplications[loopIndex++] = checkedItem;
                }
                documenter.SsoApplications = ssoApplications;
            }

            this.GenerateDocumentation();


           



        }

        #endregion

        private void linkLabel8_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int numOrchsToSave = 0;
                foreach (TreeNode asmNode in this.tvOrchs.Nodes)
                {
                    foreach (TreeNode tn in asmNode.Nodes)
                    {
                        if (tn.Checked && tn.Tag != null)
                        {
                            numOrchsToSave++;
                        }
                    }
                }

                if (numOrchsToSave > 0)
                {
                    DialogResult res = this.folderBrowserDialog1.ShowDialog();

                    if (res == DialogResult.OK)
                    {

                        Cursor.Current = Cursors.WaitCursor;
                        progressBar1.Visible = true;

                        string dirName = this.folderBrowserDialog1.SelectedPath;

                        if (Directory.Exists(dirName))
                        {
                            BizTalkInstallation bizTalkInstallation = new BizTalkInstallation();
                            bizTalkInstallation.Server = this.txtServerName.Text;
                            bizTalkInstallation.MgmtDatabaseName = this.textBox1.Text;

                            int counter = 0;
                            foreach (TreeNode asmNode in this.tvOrchs.Nodes)
                            {
                                foreach (TreeNode tn in asmNode.Nodes)
                                {
                                    if (tn.Checked && tn.Tag != null)
                                    {
                                        string tagString = tn.Tag as string;
                                        string[] nameParts = tagString.Split(new char[] { '|' }, 2);
                                        string asmName = nameParts[0];
                                        string orchName = nameParts[1];

                                        Orchestration o = bizTalkInstallation.GetOrchestration(asmName, orchName);
                                        //OrchestrationViewer ov = new OrchestrationViewer(o);
                                        //ov.ShowDialog(this);   

                                        //Orchestration o = tn.Tag as Orchestration;
                                        string fileName = Path.Combine(dirName, o.Name + ".jpg");
                                        o.SaveAsImage(fileName);
                                        counter++;
                                        Documenter_PercentageDocumentationComplete(100 / numOrchsToSave * counter);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Listing Saving Orchestrations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                Cursor.Current = Cursors.Default;
            }
            return;
        }

        /// <summary>
        /// List Orchestrations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel9_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.tvOrchs.Nodes.Clear();
                bizTalkInstallation = new BizTalkInstallation();
                bizTalkInstallation.Server = this.txtServerName.Text;
                bizTalkInstallation.MgmtDatabaseName = this.textBox1.Text;

                ArrayList names = bizTalkInstallation.GetOrchestrationNames();
                string asmName = string.Empty;
                string orchName = string.Empty;
                TreeNode asmNode = null;

                foreach (string orchAsmNameCombo in names)
                {
                    string[] nameParts = orchAsmNameCombo.Split(new char[] { '|' }, 2);
                    asmName = nameParts[0];
                    orchName = nameParts[1];

                    if (asmNode == null || asmName != asmNode.Text)
                    {
                        asmNode = new TreeNode(asmName, 0, 0);
                        this.tvOrchs.Nodes.Add(asmNode);
                    }

                    TreeNode orchNode = new TreeNode(orchName, 1, 1);
                    orchNode.Tag = orchAsmNameCombo;
                    asmNode.Nodes.Add(orchNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Listing Deployed Orchestrations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            DialogResult res = dlgConfigFrameworkOpenFile.ShowDialog();

            if (res == DialogResult.OK)
            {
                txtConfigFrameworkFile.Text = dlgConfigFrameworkOpenFile.FileName;
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.
                AddRange(new object[] {
                            "Compiled Help","Word 2003 Xml"});
            this.comboBox1.SelectedIndex = 0;

        }

        private void dlgConfigFrameworkOpenFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void dlgConfigFrameworkSaveFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowseReportFileName_Click(object sender, EventArgs e)
        {
            dlgResultFileSave.ShowDialog();
            string resultFileName = dlgResultFileSave.FileName;
            txtOutputDir.Text = Path.GetDirectoryName(resultFileName);
        }

        private void Go()
        {
            Cursor = Cursors.Default;
            Enabled = true;
        }

        private void Wait()
        {
            Cursor = Cursors.WaitCursor;
            Enabled = false;
        }

        private void RenderButtons()
        {
            //linkLabelNext.Visible = tabControl1.SelectedIndex != 3;
            //linkLabelPrevious.Visible = tabControl1.SelectedIndex != 0;
            //linkLabelGenerate.Visible = tabControl1.SelectedIndex == 3;
            RenderTabPage();
        }

        //TO be deleted
        private void RenderTabPage()
        {
            //switch (tabControl1.SelectedIndex)
            //{
            //    case 2:
                    if (listBoxSsoApplications.Items.Count == 0)
                    {
                        Wait();
                        tabControl1.SelectedTab = tabPage5;
                        string[] currentUserApplications = ConfigManager.GetCurrentUserApplications();
                        foreach (string currentUserApplication in currentUserApplications)
                        {
                            if (!currentUserApplication.StartsWith("{") && !currentUserApplication.EndsWith("}"))
                                listBoxSsoApplications.Items.Add(currentUserApplication);
                        }
                        Go();
                    }
            //        break;
            //}
        }

        private void ButtonSsoClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML|*.xml",
                Multiselect = false,
                Title = "Select SSO Configuration"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Button thisButton =
                    (Button)sender;

                switch (thisButton.Name)
                {
                    //case "buttonSsoBuild":
                    //    textBoxSsoBuild.Text = ofd.FileName;
                    //    break;
                    case "buttonSsoStage":
                        textBoxSsoStage.Text = ofd.FileName;
                        break;
                    case "buttonSsoTest":
                        textBoxSsoTest.Text = ofd.FileName;
                        break;
                    case "buttonSsoProd":
                        textBoxSsoProd.Text = ofd.FileName;
                        break;


                }


            }
        }

        private void linkAdditionalFilters_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            this.tabControl1.SelectedTab = tabAdditionalFilters;        
        }


    }
}
