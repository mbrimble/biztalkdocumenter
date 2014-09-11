
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    using Microsoft.Services.Tools.BizTalkOM;

    /// <summary>
    /// Summary description for HelpFileNode.
    /// </summary>
    public class HelpFileNode
    {
        private string caption = string.Empty;
        private string url = string.Empty;
        private HelpFileNodeCollection childNodes;
        private HelpFileNode parentNode;

        public HelpFileNode()
        {
            childNodes = new HelpFileNodeCollection();
        }

        public HelpFileNode(string caption)
            : this()
        {
            this.caption = caption;
        }

        public HelpFileNode(string caption, string url)
            : this(caption)
        {
            this.url = url;
        }

        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public HelpFileNodeCollection ChildNodes
        {
            get { return this.childNodes; }
        }

        public HelpFileNode CreateChild(string caption)
        {
            return CreateChild(caption, string.Empty);
        }

        public HelpFileNode CreateChild(string caption, string url)
        {
            HelpFileNode hfn = new HelpFileNode(caption, url);
            hfn.parentNode = this;
            this.childNodes.Add(hfn);
            return hfn;
        }

        public void SortChildren()
        {
            CollectionSorter cs = new CollectionSorter(CollectionSorter.SortOrder.Ascending, "Caption");

            if (this.ChildNodes.Count > 0)
            {
                this.ChildNodes.Sort(cs);
            }
        }

        public override string ToString()
        {
            if (this.url == string.Empty)
            {
                return string.Format("<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"{0}\"></OBJECT>", this.caption);
            }
            else
            {
                return string.Format("<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"{0}\"><param name=\"Local\" value=\"{1}\"><param name=\"ImageNumber\" value=\"auto\"></OBJECT>",
                    this.caption,
                    this.url);
            }
        }

    }
}
