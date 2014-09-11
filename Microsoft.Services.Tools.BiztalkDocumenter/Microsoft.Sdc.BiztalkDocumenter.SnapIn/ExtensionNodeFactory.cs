using System;
using Microsoft.BizTalk.SnapIn.Framework;

namespace Microsoft.Services.Tools.BiztalkDocumenter.SnapIn
{
    [CLSCompliant(false)]
    class ExtensionNodeFactory : NodeFactory
    {
        public ExtensionNodeFactory(ComponentData scope)
            : base(scope)
        {
        }
    }
}
