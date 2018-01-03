using System;

namespace Lateetud.NServiceBus.Classes.MsmqReturnToSourceQueue
{
    [Serializable]
    public class HeaderInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}