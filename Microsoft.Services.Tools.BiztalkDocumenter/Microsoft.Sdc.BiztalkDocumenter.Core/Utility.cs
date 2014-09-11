
namespace Microsoft.Services.Tools.BiztalkDocumenter.Core
{

    public class Utility
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public static bool ValidateReportName(string reportName)
        {
            if (reportName.IndexOfAny(new char[] { "\"".ToCharArray()[0], '/', '*', ':', '?', '"', '<', '>', '|' }) >= 0)
            {
                return false;
            }
            return true;
        }
    }
}
