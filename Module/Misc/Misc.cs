using NativeManager.WinApi;
using NativeManager.WinApi.Enums;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.WinAPI;

namespace Wolf_Hack.Module.Misc
{
    internal struct Misc
    {
        private static int LastHit;

        public static void MiscInitialize()
        {
            if (ConfigManager.CMisc.BunnyHop)
            {
                VClient.ForceJump = (NativeMethods.GetAsyncKeyState(KeysCode.VK_SPACE) && !BasePlayer.BhopFlag() && BasePlayer.Velocity > 25) ? VClient.SUserCMD.ButtonID.IN_JUMP : VClient.SUserCMD.ButtonID.IN_NOATTACK;
            }

            if (ConfigManager.CMisc.NoFlash)
            {
                BasePlayer.FlashMax = 0f;
            }

            if (ConfigManager.CVisualMisc.HitSound && BasePlayer.TotalHitsOnServer != LastHit && BasePlayer.TotalHitsOnServer > 0 && BasePlayer.Health > 0)
            {
                LastHit = BasePlayer.TotalHitsOnServer;

                VEngineClient.ClientCmd("play buttons/arena_switch_press_02");
            }
        }
    }
}