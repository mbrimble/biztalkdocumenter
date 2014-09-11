using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.BizTalk.SnapIn.Framework;

namespace Microsoft.Services.Tools.BiztalkDocumenter.SnapIn
{
    [Guid("66D98E3D-69FD-4b2e-8541-5B7A6CB2BC75"), ComVisible(true)]
    internal class DocNode : ScopeNode
    {
        private static NodeDefinition NodeDefinition;
        public static MgmtDBConnection conn = null;

        public string DatabaseName
        {
            get { return conn.DatabaseName; }
        }

        public string ServerDisplayName
        {
            get { return conn.ServerName; }
        }

        public MgmtDBConnection MgmtDBConnection
        {
            get { return conn; }
        }

        static DocNode()
        {
            DocNode.NodeDefinition = new NodeDefinition(
                new NodeDefinition.Label(
                    typeof(DocNode),
                    AppResources.DocNodeTitle,
                    AppResources.Manager,
                    AppResources.DocNodeIcon),
                null,
                new Guid(AppConstants.DocResultViewGuid),
                null);

            try
            {
                conn = new MgmtDBConnection();
                conn.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentData"></param>
        public DocNode(ComponentData componentData)
            : base(0, DocNode.NodeDefinition, componentData)
        {
        }

        //public override void OnExpand(ref IConsoleNameSpace2 consoleNameSpace, int parentHScopeItem)
        //{
        //    this.HScopeItem = parentHScopeItem;

        //    if (this.HintContainsFolders)
        //    {
        //        this.IsScopeExpanded = this.CreateChildren();
        //        this.ExpandScopeChildren(ref consoleNameSpace);
        //    }

        //    return;
        //}

        //public override int Expand(ref IConsoleNameSpace2 consoleNameSpace, int parentHScopeItem)
        //{
        //    if (-1 == parentHScopeItem)
        //    {
        //        return -1;
        //    }
        //    if (!base.IsMarkedDelete && (-1 != this.HScopeItem))
        //    {
        //        return this.HScopeItem;
        //    }
        //    SCOPEDATAITEM scopedataitem1 = new SCOPEDATAITEM();
        //    scopedataitem1.mask = 0;
        //    scopedataitem1.mask |= 2;
        //    scopedataitem1.displayname = (IntPtr)(-1);
        //    scopedataitem1.mask |= 0x20;
        //    scopedataitem1.lParam = base.Cookie;
        //    if (-1 != base.ImageIndex)
        //    {
        //        scopedataitem1.mask |= 4;
        //        scopedataitem1.nImage = base.ImageIndex;
        //    }
        //    if (-1 != this.SelectedImageIndex)
        //    {
        //        scopedataitem1.mask |= 8;
        //        scopedataitem1.nOpenImage = this.SelectedImageIndex;
        //    }
        //    scopedataitem1.relativeID = parentHScopeItem;
        //    if (!this.HintContainsFolders)
        //    {
        //        scopedataitem1.mask |= 0x40;
        //        scopedataitem1.cChildren = 0;
        //    }
        //    consoleNameSpace.InsertItem(ref scopedataitem1);
        //    return scopedataitem1.ID;
        //}
    }
}
