
namespace Microsoft.Services.Tools.BiztalkDocumenter.SnapIn
{
    using System.Resources;

    internal class AppResources
    {
        private static ResourceManager manager;

        public static ResourceManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = new ResourceManager("Microsoft.Services.Tools.BiztalkDocumenter.SnapIn.Resources", typeof(AppResources).Assembly);
                }
                return manager;
            }
        }

        public static string DocNodeTitle
        {
            get { return Manager.GetString("DocNodeTitle"); }
        }

        public static System.Drawing.Icon DocNodeIcon
        {
            //get { return (System.Drawing.Icon)Manager.GetObject("DocNodeIcon"); }
            get { return (System.Drawing.Icon)Manager.GetObject("ui1"); }
        }
    }
}
