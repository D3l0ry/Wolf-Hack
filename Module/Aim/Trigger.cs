using System.Threading.Tasks;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack.Module.Aim
{
    internal struct Trigger
    {
        public static void TriggerInitialize(TeamID PlayerBaseTeam)
        {
            int Crosshair = BasePlayer.Crosshair;

            if (Crosshair > 0 && Crosshair < VEngineClient.MaxPlayers)
            {
                int CrosshairEntity = BasePlayer.CrosshairEntity(Crosshair);

                if (BasePlayer.CrosshairTeam(CrosshairEntity) != PlayerBaseTeam)
                {
                    Shot();
                }
            }
        }

        private static async void Shot()
        {
            VClient.ForceAttack = VClient.SUserCMD.ButtonID.IN_ATTACK;

            await Task.Delay(25);

            VClient.ForceAttack = VClient.SUserCMD.ButtonID.IN_NOATTACK;

            await Task.Delay(15);
        }
    }
}