
namespace Microsoft.Services.Tools.BizTalkOM
{
    /// <summary>
    /// Summary description for RuleArtifact.
    /// </summary>
    public class RuleArtifact : BizTalkBaseObject
    {
        private int majorVersion;
        private int minorVersion;

        public RuleArtifact()
        {
        }

        public int MajorVersion
        {
            get { return this.majorVersion; }
            set { this.majorVersion = value; }
        }

        public int MinorVersion
        {
            get { return this.minorVersion; }
            set { this.minorVersion = value; }
        }

        public string XmlFileName
        {
            get { return string.Format("{0}.xml", this.FullName); }
        }

        public string HtmlFileName
        {
            get { return string.Format("{0}.html", this.FullName); }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}.{2}", this.Name, this.MajorVersion, this.MinorVersion); }
        }
    }
}
