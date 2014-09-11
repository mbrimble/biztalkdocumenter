
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;

    [Flags]
    public enum Capabilities
    {
//        ApplicationProtocol = 16,
//        DeleteProtected = 32,
//        InitInboundProtocolContext = 2048,
//        InitOutboundProtocolContext = 1024,
//        InitReceiveLocationContext = 4096,
//        InitTransmitLocationContext = 8192,
//        InitTransmitterOnServiceStart = 32768,
//        ReceiveIsCreatable = 8,
//        RequireSingleInstance = 4,
//        StaticHandlers = 64,
//        SupportsOrderedDelivery = 16384,
//        SupportsReceive = 1,
//        SupportsRequestResponse = 128,
//        SupportsSend = 2,
//        SupportsSoap = 512,
//        SupportsSolicitResponse = 256

        ApplicationProtocol = 0x10,
        DeleteProtected = 0x20,
        InitInboundProtocolContext = 0x800,
        InitOutboundProtocolContext = 0x400,
        InitReceiveLocationContext = 0x1000,
        InitTransmitLocationContext = 0x2000,
        InitTransmitterOnServiceStart = 0x8000,
        ReceiveIsCreatable = 8,
        RequireSingleInstance = 4,
        StaticHandlers = 0x40,
        SupportsOrderedDelivery = 0x4000,
        SupportsReceive = 1,
        SupportsRequestResponse = 0x80,
        SupportsSend = 2,
        SupportsSoap = 0x200,
        SupportsSolicitResponse = 0x100,
        //Unknown = 387,
        Unknown = 81163,
        Unknown2 = 80907,
        Unknown3 = 71689,

    }
}
