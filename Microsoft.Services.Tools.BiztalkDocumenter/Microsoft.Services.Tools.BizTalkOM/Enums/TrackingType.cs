
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
 
    [Flags()]
    public enum TrackingType
    {        
        // Fields
        AfterReceivePipeline = 2,
        AfterSendPipeline = 8,
        BeforeReceivePipeline = 1,
        BeforeSendPipeline = 4,
        TrackPropertiesAfterReceivePipeline = 0x20,
        TrackPropertiesAfterSendPipeline = 0x80,
        TrackPropertiesBeforeReceivePipeline = 0x10,
        TrackPropertiesBeforeSendPipeline = 0x40
    }
}
