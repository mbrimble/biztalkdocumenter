namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections.Generic;

    public class ArtifactKeyComparer : IEqualityComparer<ArtifactKey>
    {
        #region IEqualityComparer<ArtifactKey> Members

        public bool Equals(ArtifactKey x, ArtifactKey y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return ((x.ArtifactName == y.ArtifactName) && (x.QualifiedName == y.QualifiedName));
        }

        public int GetHashCode(ArtifactKey obj)
        {
            // Get the hash code for the artifact name if it is not null
            int hashArtifactName = obj.ArtifactName == null ? 0 : obj.ArtifactName.GetHashCode();

            // get the hash code for the assemblyQName if it is not null
            int hashAssemblyQName = obj.QualifiedName == null ? 0 : obj.QualifiedName.GetHashCode();

            // Calculate the hash code for the object
            return hashArtifactName ^ hashAssemblyQName;
        }

        #endregion
    }
}
