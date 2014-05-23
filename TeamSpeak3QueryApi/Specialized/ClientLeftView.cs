﻿using System;

namespace TeamSpeak3QueryApi.Net.Specialized
{
    public class ClientLeftView : Notify
    {
        [QuerySerialize("cfid")]
        public int SourceChannelId;

        [QuerySerialize("ctid")]
        public int TargetChannelId; // „0“ bei Verlassen

        [QuerySerialize("reasonid")]
        public ReasonId Reason;

        [QuerySerialize("clid")]
        public int ClientId;

        [QuerySerialize("invokerid")]
        public int InvokerId;

        [QuerySerialize("invokername")]
        public string InvokerName;

        [QuerySerialize("invokeruid")]
        public string InvokerUid;

        [QuerySerialize("reasonmsg")]
        public string ReasonMessage;

        [QuerySerialize("bantime")]
        public TimeSpan BanTime;
    }
}
