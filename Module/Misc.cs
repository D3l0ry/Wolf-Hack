﻿using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.WinAPI;

namespace Wolf_Hack.Module
{
    internal class Misc
    {
        public void Tick()
        {
            if (ConfigManager.CMisc.BunnyHop)
            {
                if(NativeMethods.GetAsyncKeyState(KeysCode.VK_SPACE) && !Base.LocalPlayer.BhopFlag() && Base.LocalPlayer.Velocity > 25)
                {
                    VClient.SetForceJump(VClient.SUserCMD.ButtonID.IN_JUMP);
                }
            }

            if (ConfigManager.CMisc.NoFlash)
            {
                Base.LocalPlayer.FlashMax = 0f;
            }
        }
    }
}