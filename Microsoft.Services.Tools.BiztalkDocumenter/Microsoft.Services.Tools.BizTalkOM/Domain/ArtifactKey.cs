
namespace Microsoft.Services.Tools.BizTalkOM
{
    public class ArtifactKey
    {
        public string QualifiedName;
        public string ArtifactName;

        public ArtifactKey(string sourceQualifiedName, string sourceArtifactName)
        {
            this.QualifiedName = sourceQualifiedName;
            this.ArtifactName = sourceArtifactName;
        }


    }
}
