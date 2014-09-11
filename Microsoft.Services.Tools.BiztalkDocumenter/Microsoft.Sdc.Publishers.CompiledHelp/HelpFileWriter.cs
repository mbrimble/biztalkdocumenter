
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Summary description for HelpFileWriter.
    /// </summary>
    public class HelpFileWriter
    {
        private HelpFileNode rootNode;

        public HelpFileNode RootNode
        {
            get { return this.rootNode; }
            set { this.rootNode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Compatibility
        {
            get { return "1.1 or later"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultFont
        {
            get { return "Verdana,8,0"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayCompileProgres
        {
            get { return "No"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Language
        {
            get { return "0x809 English (United Kingdom)"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultTopic;
        /// <summary>
        /// 
        /// </summary>
        public string CompiledFileName = "";
        /// <summary>
        /// 
        /// </summary>
        public string ContentsFileName = "";
        /// <summary>
        /// 
        /// </summary>
        public string ProjectFileName = "";
        /// <summary>
        /// 
        /// </summary>
        public string Title = "";
        /// <summary>
        /// 
        /// </summary>
        public string OutputFile = "";

        /// <summary>
        /// 
        /// </summary>
        public string ContentsRootNodeCaption = "";

        /// <summary>
        /// 
        /// </summary>
        public ArrayList Files;

        /// <summary>
        /// 
        /// </summary>
        public HelpFileWriter()
        {
            this.rootNode = new HelpFileNode();
            Files = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hhcWriter"></param>
        /// <param name="hfn"></param>
        private void WriteNodeToContentsFile(StreamWriter hhcWriter, HelpFileNode hfn)
        {
            hhcWriter.WriteLine(hfn.ToString());

            if (hfn.ChildNodes.Count > 0)
            {
                hhcWriter.WriteLine("<UL>");

                foreach (HelpFileNode childHfn in hfn.ChildNodes)
                {
                    WriteNodeToContentsFile(hhcWriter, childHfn);
                }

                hhcWriter.WriteLine("</UL>");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hhcWriter"></param>
        /// <param name="hfn"></param>
        private void WriteNodeUrlToProjectFile(StreamWriter hhcWriter, HelpFileNode hfn)
        {
            if (hfn.Url != string.Empty)
            {
                hhcWriter.WriteLine(hfn.Url);
            }

            foreach (HelpFileNode childHfn in hfn.ChildNodes)
            {
                WriteNodeUrlToProjectFile(hhcWriter, childHfn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveContentsFile()
        {
            StreamWriter hhcWriter = new StreamWriter(File.Create(ContentsFileName));
            hhcWriter.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\"><HTML><BODY bgcolor=\"#c0c0c0\"><OBJECT type=\"text/site properties\">");
            hhcWriter.WriteLine("<param name=\"Window Styles\" value=\"0x800025\"></OBJECT><UL>");
            WriteNodeToContentsFile(hhcWriter, this.rootNode);
            hhcWriter.WriteLine("</UL></BODY>");
            hhcWriter.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveProjectFile()
        {
            StreamWriter hhpWriter = new StreamWriter(File.Create(ProjectFileName));
            hhpWriter.WriteLine("[OPTIONS]");
            hhpWriter.WriteLine("Compatibility={0}", Compatibility);
            hhpWriter.WriteLine("Contents file={0}", ContentsFileName);
            hhpWriter.WriteLine("Default Font={0}", DefaultFont);
            hhpWriter.WriteLine("Default topic={0}", this.rootNode.Url);
            hhpWriter.WriteLine("Display compile progress={0}", DisplayCompileProgres);
            hhpWriter.WriteLine("Language={0}", Language);
            hhpWriter.WriteLine("Title={0}", Title);
            hhpWriter.WriteLine("");
            hhpWriter.WriteLine("[FILES]");
            WriteNodeUrlToProjectFile(hhpWriter, this.rootNode);
            hhpWriter.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCompilerLocation()
        {
            string hhCompilerLocation = @"C:\Program Files\HTML Help Workshop\hhc.exe";

            try
            {
                hhCompilerLocation = new AppSettingsReader().GetValue("HelpCompilerLocation", typeof(string)).ToString();
            }
            catch (Exception)
            {
            }


            if (hhCompilerLocation == null || hhCompilerLocation == string.Empty)
            {
                throw new ApplicationException("Unable to locate the help compiler from config entry.");
            }

            if (File.Exists(hhCompilerLocation) == false)
            {
                hhCompilerLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "")) + "\\";
                hhCompilerLocation += "hhc.exe";

                if (File.Exists(hhCompilerLocation) == false)
                {
                    throw new ApplicationException("Unable to locate the help compiler executable.");
                }
            }

            return hhCompilerLocation;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Compile()
        {
            string hhCompilerLocation = GetCompilerLocation();

            SaveContentsFile();
            SaveProjectFile();

            ProcessStartInfo psi = new ProcessStartInfo("\"" + hhCompilerLocation + "\"", "\"" + ProjectFileName + "\"");
            psi.WorkingDirectory = new FileInfo(hhCompilerLocation).DirectoryName;

            psi.WindowStyle = ProcessWindowStyle.Hidden;

            psi.UseShellExecute = true;

            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
            p.Close();

            OutputFile = CompiledFileName;
            string fileToCopy = ProjectFileName.ToLower().Replace("hhp", "chm");

            File.Copy(fileToCopy, OutputFile, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisplayHelpFile()
        {
            if (File.Exists(OutputFile))
            {
                Process.Start(OutputFile);
            }
        }
    }
}

