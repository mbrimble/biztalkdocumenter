
namespace Microsoft.Services.Tools.BizTalkOM
{
    public class NameIdPair
    {
        private string name;
        private string id;

        public NameIdPair()
        {
        }

        public NameIdPair(string name, string id)
        {
            this.name = name;
            this.id = id;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
