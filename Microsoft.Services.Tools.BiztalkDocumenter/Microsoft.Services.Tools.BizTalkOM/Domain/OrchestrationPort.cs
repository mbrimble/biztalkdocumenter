
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using BizTalkCore = Microsoft.BizTalk.ExplorerOM;

	/// <summary>
	/// Summary description for OrchestrationPort.
	/// </summary>
	public sealed class OrchestrationPort : BizTalkBaseObject
	{
        private BindingType bindingType;
        private NameIdPair sendPortName;
        private NameIdPair sendPortGroupName;
        private NameIdPair receivePortName;

		public OrchestrationPort()
		{
		}

        public OrchestrationPort(string name) : this()
        {
            this.Name = name;
        }

        public BindingType BindingType
        {
            get { return this.bindingType; }
            set { this.bindingType = value; }
        }

        public NameIdPair SendPortName
        {
            get { return this.sendPortName; }
            set { this.sendPortName = value; }
        }

        public NameIdPair SendPortGroupName
        {
            get { return this.sendPortGroupName; }
            set { this.sendPortGroupName = value; }
        }

        public NameIdPair ReceivePortName
        {
            get { return this.receivePortName; }
            set { this.receivePortName = value; }
        }

        public void Load(BizTalkCore.OrchestrationPort port)
        {
            this.Name = port.Name;
            this.BindingType = (BindingType)Enum.Parse(typeof(BindingType), ((int)port.Binding).ToString());

            this.sendPortGroupName = port.SendPortGroup != null ? new NameIdPair(port.SendPortGroup.Name, "") : null;
            this.sendPortName = port.SendPort != null ? new NameIdPair(port.SendPort.Name, "") : null;
            this.receivePortName = port.ReceivePort != null ? new NameIdPair(port.ReceivePort.Name, "") : null;
        }
	}
}
