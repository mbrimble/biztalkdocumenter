
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Collections;

    public class NameIdPairCollection : CollectionBase
    {

        public NameIdPairCollection()
        {
        }

        public NameIdPair this[int index]
        {
            get { return (NameIdPair)this.List[index]; }
            set { this.List[index] = value; }
        }

        public void Add(NameIdPair obj)
        {
            if (obj != null)
            {
                this.List.Add(obj);
            }
        }
    }
}
