using System.Threading;

using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.Module.Visual;
using Wolf_Hack.Module.Aim;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using Wolf_Hack.Client.Config;

namespace Wolf_Hack.Module
{
    internal unsafe struct Starting
    {

        public static Thread TaskModule = new Thread(() =>
        {
            while (true)
            {
                if (!Base.Memory.GetProcessInfo().IsActiveWindow())
                {
                    continue;
                }

                if (BasePlayer.Health == 0)
                {
                    continue;
                }

                for (int index = 0; index < VEngineClient.MaxPlayers; index++)
                {
                    EntityBase* Entity = IClientEntityList.GetClientEntity(index)->GetPlayer;

                    if (Entity == null)
                    {
                        continue;
                    }

                    if (!Entity->IsValid)
                    {
                        continue;
                    }

                    TeamID EntityTeam = Entity->Team;
                    TeamID PlayerBaseTeam = BasePlayer.Team;

                    VisualESP.VisualInitialize(Entity, EntityTeam, PlayerBaseTeam);

                    if (ConfigManager.CAim.AimActive)
                    {
                        Legit.LegitInitialize(Entity, EntityTeam, PlayerBaseTeam);
                    }

                    if (ConfigManager.CTrigger.TriggerActive)
                    {
                        Trigger.TriggerInitialize(PlayerBaseTeam);
                    }

                    Misc.Misc.MiscInitialize();
                }

                Thread.Sleep(1);
            }
        });

        public static void Run()
        {
            TaskModule.Priority = ThreadPriority.Lowest;
            TaskModule.Start();
        }

        public static int GetWeaponID(WeaponID WeaponID)
        {
            switch (WeaponID)
            {
                case WeaponID.WEAPON_HKP2000:
                    return 0;
                case WeaponID.WEAPON_GLOCK:
                    return 1;
                case WeaponID.WEAPON_USP_SILENCER:
                    return 2;
                case WeaponID.WEAPON_ELITE:
                    return 3;
                case WeaponID.WEAPON_P250:
                    return 4;
                case WeaponID.WEAPON_FIVESEVEN:
                    return 5;
                case WeaponID.WEAPON_TEC9:
                    return 6;
                case WeaponID.WEAPON_CZ75A:
                    return 7;
                case WeaponID.WEAPON_DEAGLE:
                    return 8;
                case WeaponID.WEAPON_REVOLVER:
                    return 9;
                case WeaponID.WEAPON_NOVA:
                    return 10;
                case WeaponID.WEAPON_XM1014:
                    return 11;
                case WeaponID.WEAPON_MAG7:
                    return 12;
                case WeaponID.WEAPON_SAWEDOFF:
                    return 13;
                case WeaponID.WEAPON_M249:
                    return 14;
                case WeaponID.WEAPON_NEGEV:
                    return 15;
                case WeaponID.WEAPON_MAC10:
                    return 16;
                case WeaponID.WEAPON_MP9:
                    return 17;
                case WeaponID.WEAPON_MP7:
                    return 18;
                case WeaponID.WEAPON_MP5SD:
                    return 19;
                case WeaponID.WEAPON_UMP45:
                    return 20;
                case WeaponID.WEAPON_P90:
                    return 21;
                case WeaponID.WEAPON_BIZON:
                    return 22;
                case WeaponID.WEAPON_FAMAS:
                    return 23;
                case WeaponID.WEAPON_GALILAR:
                    return 24;
                case WeaponID.WEAPON_M4A1_SILENCER:
                    return 25;
                case WeaponID.WEAPON_AK47:
                    return 26;
                case WeaponID.WEAPON_M4A1:
                    return 27;
                case WeaponID.WEAPON_AUG:
                    return 28;
                case WeaponID.WEAPON_SG553:
                    return 29;
                case WeaponID.WEAPON_SSG08:
                    return 30;
                case WeaponID.WEAPON_AWP:
                    return 31;
                case WeaponID.WEAPON_SCAR20:
                    return 32;
                case WeaponID.WEAPON_G3SG1:
                    return 33;
                case WeaponID.WEAPON_KNIFE: return 34;
                case WeaponID.WEAPON_KNIFE_T: return 34;
                case WeaponID.WEAPON_KNIFE_KARAMBIT: return 34;
                case WeaponID.WEAPON_KNIFE_GUT: return 34;
                case WeaponID.WEAPON_KNIFE_FLIP: return 34;
                case WeaponID.WEAPON_KNIFE_BAYONET: return 34;
                case WeaponID.WEAPON_KNIFE_M9_BAYONET: return 34;
                case WeaponID.WEAPON_KNIFE_TACTICAL: return 34;
                case WeaponID.WEAPON_KNIFE_BUTTERFLY: return 34;
                case WeaponID.WEAPON_KNIFE_FALCHION: return 34;
                case WeaponID.WEAPON_KNIFE_URSUS: return 34;
                case WeaponID.WEAPON_KNIFE_SURVIVAL_BOWIE: return 34;
                default:
                    return 0;
            }
        }
    }
}