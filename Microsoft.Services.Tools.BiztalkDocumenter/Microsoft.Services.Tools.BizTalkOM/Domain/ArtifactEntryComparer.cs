namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Collections.Generic;

    internal class ArtifactEntryComparer : IComparer<ArtifactEntry>
    {
        #region IComparer<ArtifactEntry> Members

        public int Compare(ArtifactEntry x, ArtifactEntry y)
        {
            // check if the keys are equal
            ArtifactKeyComparer keyComparer = new ArtifactKeyComparer();
            if (keyComparer.Equals(x.artifactKey, y.artifactKey))
            {
                return 0;
            }

            // if the keys are not equal, then compare the Name
            if (x.artifactKey.ArtifactName.CompareTo(y.artifactKey.ArtifactName) == 0)
            {
                return 0;
            }

            // if we get this far then compare the QName
            return x.artifactKey.QualifiedName.CompareTo(y.artifactKey.QualifiedName);
        }

        #endregion
    }
}
