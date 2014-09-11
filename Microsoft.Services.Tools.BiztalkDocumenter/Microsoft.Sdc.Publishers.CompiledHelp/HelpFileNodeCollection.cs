
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    using System.Collections;

    /// <summary>
    /// 
    /// </summary>
    public class HelpFileNodeCollection : CollectionBase
    {
        /// <summary>
        /// 
        /// </summary>
        public HelpFileNodeCollection()
        {
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public HelpFileNode this[int index]
        {
            get { return (HelpFileNode)this.List[index]; }
            set { this.List[index] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Add(HelpFileNode obj)
        {
            this.List.Add(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        public void Insert(int index, HelpFileNode obj)
        {
            this.List.Insert(index, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(HelpFileNode obj)
        {
            this.List.Remove(obj);
        }

        /// <summary>
        /// Copies the collection into an array of objects
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="index"></param>
        public void CopyTo(HelpFileNode[] objects, int index)
        {
            ((ICollection)this).CopyTo(objects, index);
        }

        /// <summary>
        /// IndexOf
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int IndexOf(HelpFileNode obj)
        {
            return this.List.IndexOf(obj);
        }

        /// <summary>
        /// Checks to see if the specified object exsits in the collection
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Contains(HelpFileNode obj)
        {
            return this.List.Contains(obj);
        }

        /// <summary>
        /// Retrieves an item from the collection with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HelpFileNode GetItemByName(string caption)
        {
            foreach (HelpFileNode o in this.List)
            {
                if (o.Caption == caption)
                {
                    return o;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer comparer)
        {
            this.InnerList.Sort(comparer);
        }
    }
}
