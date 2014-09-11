namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections.Generic;

    internal class ArtifactEntries : List<ArtifactEntry>
    {
        internal bool ItemExists(BizTalkBaseObject obj)
        {
            if (this.Count == 0) return false;
            foreach (ArtifactEntry entry in this)
            {
                if (entry.Contains(obj))
                {
                    return true;
                }
            }
            return false;

        }

        internal void AddItem(BizTalkBaseObject obj)
        {
            ArtifactEntry entry = new ArtifactEntry(obj);
            this.Add(entry);
        }

        internal int ItemIndex(string itemName)
        {
            int index = -1;

            foreach (ArtifactEntry entry in this)
            {
                index++;
                if (String.Compare(entry.artifactKey.ArtifactName, itemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    break;
                }
            }

            return index;


        }

    }
}
