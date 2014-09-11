namespace Microsoft.Services.Tools.BizTalkOM
{

    internal class ArtifactEntry
    {
        internal ArtifactKey artifactKey;
        internal BizTalkBaseObject artifact;

        internal ArtifactEntry(BizTalkBaseObject obj)
        {
            artifactKey = new ArtifactKey(obj.QualifiedName, obj.Name);
            artifact = obj;
        }

        internal bool Contains(BizTalkBaseObject obj)
        {
            return this.artifactKey.QualifiedName == obj.QualifiedName
                && this.artifactKey.ArtifactName == obj.Name;
        }

    }
}
