

namespace Microsoft.Services.Tools.BiztalkDocumenter.Publishers
{
    using Microsoft.Services.Tools.BizTalkOM;

    /// <summary>
    /// Summary description for IPublisher.
    /// </summary>
    public interface IPublisher
    {
        bool Prepare();
        //void Publish(BizTalkInstallation bi, PublishType publishType, string resourceFolder, string publishFolder, string reportTitle, bool publishRules); MTB 08/03/2014
        void Publish(BizTalkInstallation bi, PublishType publishType, string resourceFolder, string publishFolder, string reportTitle, bool publishRules, string[] ssoLocations, string[] ssoApplications, string bizTalkConfigurationPath);
        bool Cleanup();
        void ShowOutput();

        event UpdatePercentageComplete PercentageDocumentationComplete;
    }
}
