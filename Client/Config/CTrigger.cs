using System;

using Wolf_Hack.SDK.Interfaces.Enum.EModule;

namespace Wolf_Hack.Client.Config
{
    [Serializable]
    public class CTrigger
    {
        public bool TriggerActive;

        public TriggerID TriggerID = TriggerID.MouseEvent;
    }
}