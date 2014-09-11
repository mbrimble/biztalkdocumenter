namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class BizTalkBaseObjectCollectionEx : CollectionBase
    {
        //private List<ArtifactEntry> artifactEntries;
        private ArtifactEntries artifactEntries;

        /// <summary>
        /// Creates a new <see cref="BizTalkBaseObjectCollection"/>
        /// </summary>
        public BizTalkBaseObjectCollectionEx()
        {
            //this.artifactEntries = new List<ArtifactEntry>();
            this.artifactEntries = new ArtifactEntries();
        }

        internal event ObjectAddedEvent OnObjectAdded;

        public int ObjectCount
        {
            get
            {
                return this.artifactEntries.Count;
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public BizTalkBaseObject this[int index]
        {
            get { return (BizTalkBaseObject)this.artifactEntries[index].artifact; }

            set
            {
                this.InnerList[index] = value;
                if (value is BizTalkBaseObject && !this.ItemExists(value))
                {
                    this.AddItemToCollection(value);
                }
            }
        }

        [XmlIgnore]
        public BizTalkBaseObject this[string name]
        {

            get
            {
                if ((this.artifactEntries.Count == 0) || (this.ItemIndex(name) == -1))
                    return null;
                int index = this.ItemIndex(name);
                return this.artifactEntries[index].artifact as BizTalkBaseObject;
            }

        }

        public bool ItemExists(BizTalkBaseObject obj)
        {
            return this.artifactEntries.ItemExists(obj);
        }


        /// <summary>
        /// Adds a new object to the list
        /// </summary>
        /// <param name="obj"></param>
        public void Add(BizTalkBaseObject obj)
        {
            this.Add(obj, false);
        }

        public void Add(BizTalkBaseObject obj, bool allowDuplicates)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Cannot add null objects to collection");
            }

            if (this.ItemExists(obj))
            {
                string msg = "Warning! Attempted to add duplicate item name :" + obj.Name + " QName : " + obj.QualifiedName;
                TraceManager.SmartTrace.TraceError(msg);
                if (!allowDuplicates)
                {
                    throw new InvalidOperationException("DUPLICATE ADDITION DETECTED : Cannot add duplicate items to collection. Attempted to add item name :" + obj.Name + " QName : " + obj.QualifiedName);
                }
            }

            ArtifactKey key;
            if (obj is BizTalkBaseObject)
            {
                if (!string.IsNullOrEmpty(obj.Name))
                {
                    this.InnerList.Add(obj);
                    this.AddItemToCollection(obj);

                }
            }

            if (this.OnObjectAdded != null)
            {
                this.OnObjectAdded(obj);
            }

        }

        /// <summary>
        /// Sorts the collection
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer comparer)
        {
            //this.InnerList.Sort(comparer);

            this.artifactEntries.Sort(new ArtifactEntryComparer());
        }

        private void AddItemToCollection(BizTalkBaseObject obj)
        {
            this.artifactEntries.AddItem(obj);
        }

        private int ItemIndex(string itemName)
        {
            return this.artifactEntries.ItemIndex(itemName);
        }

    }
}
