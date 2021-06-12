using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Enums;
using Wolf_Hack.SDK.Mathematics;
using Wolf_Hack.SDK.WinAPI;

namespace Wolf_Hack.Module.Aim
{
    internal unsafe class Legit
    {
        public static readonly string[] Bone = { "1", "2", "3", "Таз", "Пупок", "Живот", "Грудь", "Шея", "Голова" };

        private bool IsClosed = false;
        private EntityBase m_Entity = IntPtr.Zero;

        private Vector3 OldAngle;
        private CAimWeapon ActiveWeapon;

        public void Tick() => Tick(Base.LocalPlayer.Team);

        public void Tick(TeamID playerBaseTeam)
        {
            if (ConfigManager.CAim.AimActive)
            {
                ActiveWeapon = ConfigManager.CAimWeapon[GetWeaponID(BaseWeapon.ActiveWeaponIndex)];

                if (NativeMethods.GetAsyncKeyState(KeysCode.MS_Click1))
                {
                    if (ActiveWeapon.WeaponActive && BaseWeapon.ActiveWeaponClip > 0)
                    {
                        if (!IsClosed)
                        {
                            if (Base.LocalPlayer.CrosshairId > 0 && Base.LocalPlayer.CrosshairId < VEngineClient.MaxPlayers)
                            {
                                EntityBase crosshairPlayer = IClientEntityList.GetClientEntity(Base.LocalPlayer.CrosshairId).GetPlayer;

                                m_Entity = GetTarget(crosshairPlayer, ActiveWeapon, crosshairPlayer.Team, playerBaseTeam);
                            }
                            else
                            {
                                m_Entity = NearestTarget();
                            }

                            IsClosed = true;
                        }

                        if (m_Entity && m_Entity.Health != 0)
                        {
                            if (ActiveWeapon.SilentActive && Base.LocalPlayer.Bullet(false))
                            {
                                SetViewAngelSilent(m_Entity.AngleToTarget(false, ActiveWeapon.WeaponBone, ActiveWeapon.RcsValue, ActiveWeapon.RcsActive));
                            }
                            else
                            {
                                VEngineClient.ViewAngels = Vector.SmoothAngle(VEngineClient.ViewAngels, m_Entity.AngleToTarget(true, ActiveWeapon.WeaponBone, ActiveWeapon.RcsValue, ActiveWeapon.RcsActive), ActiveWeapon.Smooth);
                            }
                        }
                        else
                        {
                            IsClosed = false;
                        }

                        if (ActiveWeapon.RcsStandaloneActive && Base.LocalPlayer.Bullet(true))
                        {
                            RcsStandalone(ActiveWeapon.RcsStandaloneX, ActiveWeapon.RcsStandaloneY);
                        }
                    }

                    if (ConfigManager.CAimMisc.MouseAttackActive)
                    {
                        VClient.SetForceAttack(VClient.SUserCMD.ButtonID.IN_ATTACK);
                    }
                }
                else
                {
                    OldAngle.X = OldAngle.Y = 0.0f;
                    IsClosed = false;

                    VClient.SetForceAttack(VClient.SUserCMD.ButtonID.IN_NOATTACK);
                }
            }
        }

        public EntityBase NearestTarget()
        {
            List<EntityBase> entities = GetTargets();

            if (entities.Count == 0)
            {
                return IntPtr.Zero;
            }

            return entities.Aggregate((current, next) => current.Fov < next.Fov ? current : next);
        }

        /// <summary>
        /// Получение цели
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        private EntityBase GetTarget(EntityBase entity, CAimWeapon weapon, TeamID entityTeam, TeamID playerBaseTeam)
        {
            if ((entityTeam == playerBaseTeam && !ConfigManager.CAimMisc.DangerZoneActive) || (ConfigManager.CAimMisc.PlayerInAirActive && Base.LocalPlayer.BhopFlag()) || (ConfigManager.CAimMisc.EnemyInAirActive && entity.InAir()))
            {
                return IntPtr.Zero;
            }
            else if (Vector.GetFov(VEngineClient.ViewAngels, entity.AngleToTarget(true,weapon.WeaponBone, weapon.RcsValue, weapon.RcsActive)) <= weapon.WeaponFov)
            {
                if (entity.SpottedByMask > 0)
                {
                    return entity;
                }
            }

            return IntPtr.Zero;
        }

        private List<EntityBase> GetTargets()
        {
            List<EntityBase> targets = new List<EntityBase>();

            for (int index = 0; index < VEngineClient.MaxPlayers; index++)
            {
                EntityBase selectedTarget = IClientEntityList.GetClientEntity(index).GetPlayer;

                if (!selectedTarget || !selectedTarget.IsValid)
                {
                    continue;
                }

                EntityBase target = GetTarget(selectedTarget, ActiveWeapon, selectedTarget.Team, Base.LocalPlayer.Team);

                if (target)
                {
                    targets.Add(target);
                }
            }

            return targets;
        }

        /// <summary>
        /// Применение тихого угла
        /// </summary>
        /// <param name="Angle">Угол</param>
        private void SetViewAngelSilent(Vector3 Angle)
        {
            VClient.CurrentSequenceNumber = VEngineClient.CurrentSequenceNumber;

            VEngineClient.SetSendPacket(0);

            while (Base.LocalPlayer.CheckSilent(VClient.CurrentSequenceNumber))
            {
                Thread.Yield();
            }

            VClient.SUserCMD oldUserCmd = VClient.UserCmd;

            Angle = Angle.ClampAngle();
            Angle = Angle.NormalizeAngle();

            oldUserCmd.m_vecViewAngles = Angle;
            oldUserCmd.m_iButtons |= VClient.SUserCMD.ButtonID.IN_ATTACK;

            VClient.UserCmd = oldUserCmd;

            VEngineClient.SetSendPacket(1);
        }

        /// <summary>
        /// Контроль спрея без наводки
        /// </summary>
        /// <param name="RcsX"></param>
        /// <param name="RcsY"></param>
        private void RcsStandalone(float RcsX, float RcsY)
        {
            Vector3 AimView = VEngineClient.ViewAngels;
            Vector3 RcsPunch = Base.LocalPlayer.MyPunch;

            AimView.X += OldAngle.X;
            AimView.Y += OldAngle.Y;

            AimView.X -= RcsPunch.X * RcsX;
            AimView.Y -= RcsPunch.Y * RcsY;

            VEngineClient.ViewAngels = AimView.ClampAngle().NormalizeAngle();

            OldAngle.X = RcsPunch.X * RcsX;
            OldAngle.Y = RcsPunch.Y * RcsY;
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