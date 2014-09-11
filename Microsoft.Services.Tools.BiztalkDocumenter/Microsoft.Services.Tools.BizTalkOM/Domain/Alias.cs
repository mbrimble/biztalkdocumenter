
namespace Microsoft.Services.Tools.BizTalkOM
{

    /// <summary>
    /// Summary description for Alias.
    /// </summary>
    public class Alias : BizTalkBaseObject
    {
        private bool isAutoCreated;
        private string qualifier;
        private string value;

        /// <summary>
        /// 
        /// </summary>
        public Alias()
        {
        }

        #region Public Properties

        public bool IsAutoCreated
        {
            get { return this.isAutoCreated; }
            set { this.isAutoCreated = value; }
        }

        public string Qualifier
        {
            get { return this.qualifier; }
            set { this.qualifier = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #endregion
    }
}
