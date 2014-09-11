using System;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.SnapIn.Framework;

namespace Microsoft.Services.Tools.BiztalkDocumenter.SnapIn
{
    /// <summary>
    /// Implementation of MMC's IComponentData interface.  This
    /// class represents the Scope Pane (tree view) of a Snap-In.
    /// </summary>
    [ComVisible(true), Guid("20D92510-A213-4837-ACEC-8C7D5BE638DD"), CLSCompliant(false)]
    public class MyExtensionComponentData : ComponentData
    {
        public MyExtensionComponentData()
        {
        }

        public override NodeFactory Factory
        {
            get { return (base.Factory as ExtensionNodeFactory); }
        }

        /// <summary>
        // Create console's node factory.
        /// </summary>
        /// <returns>Snap-In's NodeFactory</returns>
        public override NodeFactory CreateNodeFactory()
        {
            return new ExtensionNodeFactory(this);
        }

        public override void Initialize(object pUnknown)
        {
            base.Initialize(pUnknown);
        }

        /// <summary>
        /// Create Snap-In root node - called by base.Initialize().
        /// </summary>
        /// <returns>Root node</returns>
        public override ScopeNode CreateRoot()
        {
            return (this.Factory.Create(typeof(DocNode)) as ScopeNode);
        }

        /// <summary>
        /// Override MMCN_EXPAND event handler for extending another
        /// Snap-In
        /// </summary>
        /// <param name="dataInterface"></param>
        /// <param name="consoleNamespace"></param>
        /// <param name="hScope"></param>
        public override void OnExpand(System.Runtime.InteropServices.ComTypes.IDataObject dataInterface, ref IConsoleNameSpace2 consoleNamespace, int hScope)
        {
            //if (IsDataObject(dataInterface))
            //{
            //    base.OnExpand(dataInterface, ref consoleNamespace, hScope);
            //}
            //else
            //{
            //    OnExtend(dataInterface, ref consoleNamespace, hScope);
            //}

            this.RootNode.HScopeItem = -1;
            //this.RootNode.OnExpand(ref consoleNamespace, hScope);
            this.RootNode.Expand(ref consoleNamespace, hScope);
        }

        /// <summary>
        /// Extend node in data with this Snap-In
        /// </summary>
        /// <param name="data">IDataObject of node to extend</param>
        /// <param name="cns">MMC console namespace</param>
        /// <param name="hScope">HScopeItem for this Snap-In's root node</param>
        public void OnExtend(System.Runtime.InteropServices.ComTypes.IDataObject data, ref IConsoleNameSpace2 cns, int hScope)
        {
            this.RootNode.OnExpand(ref cns, hScope);
        }

        //public override bool IsDataObject(System.Runtime.InteropServices.ComTypes.IDataObject dataInterface)
        //{
        //    System.Runtime.InteropServices.ComTypes.FORMATETC formatec = new System.Runtime.InteropServices.ComTypes.FORMATETC();
        //    System.Runtime.InteropServices.ComTypes.STGMEDIUM stgMedium = new System.Runtime.InteropServices.ComTypes.STGMEDIUM();

        //    dataInterface.GetDataHere(ref formatec, ref stgMedium);

        //    System.Windows.Forms.MessageBox.Show(dataInterface.GetType().ToString());
        //    System.Windows.Forms.MessageBox.Show("cfFormat: " + formatec.cfFormat.ToString());

        //    if (formatec.cfFormat == 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }
}
