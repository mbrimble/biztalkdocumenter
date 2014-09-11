
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Xml.Serialization;

    public enum FilterOperator
    {
        Equal = 0,
        NotEqual = 5,
        LessThan = 1,
        LessThanOrEqual = 2,
        GreaterThan = 3,
        GreaterThanOrEqual = 4,
        Exists = 6

    }

    /// <summary>
    /// 
    /// </summary>
    public class FilterGroup : BizTalkBaseObject
    {
        private BizTalkBaseObjectCollectionEx filters;

        public FilterGroup()
        {
            this.filters = new BizTalkBaseObjectCollectionEx();
        }

        [XmlArrayItem("Filter", typeof(Filter))]
        public BizTalkBaseObjectCollectionEx Filter
        {
            get { return this.filters; }
            set { this.filters = value; }
        }
    }

    /// <summary>
    /// Summary description for Filter.
    /// </summary>
    public class Filter : BizTalkBaseObject
    {
        private string property;
        private string value = string.Empty;
        private FilterOperator filterOperator;

        public Filter()
        {
        }

        public string Property
        {
            get { return this.property; }
            set { this.property = value; }
        }

        public FilterOperator FilterOperator
        {
            get { return this.filterOperator; }
            set { this.filterOperator = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
