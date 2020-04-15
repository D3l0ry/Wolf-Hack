using System.Numerics;
using System.Threading;

using NativeManager.WinApi;
using NativeManager.WinApi.Enums;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Client.Entity;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using Wolf_Hack.SDK.Mathematics;
using Wolf_Hack.SDK.WinAPI;

namespace Wolf_Hack.Module.Aim
{
    internal unsafe struct Legit
    {
        public static readonly string[] Bone = { "1", "2", "3", "Таз", "Пупок", "Живот", "Грудь", "Шея", "Голова" };

        private static bool IsClosed = false;
        private static EntityBase* entity = null;

        private static Vector3 OldAngle;
        private static CAimWeapon ActiveWeapon;

        public static void LegitInitialize(EntityBase* EntityBase, TeamID EntityTeam, TeamID PlayerBaseTeam)
        {
            ActiveWeapon = ConfigManager.CAimWeapon[Starting.GetWeaponID(BaseWeapon.ActiveWeaponIndex)];

            if (NativeMethods.GetAsyncKeyState(KeysCode.MS_Click1))
            {
                if (ActiveWeapon.WeaponActive && BaseWeapon.ActiveWeaponClip > 0)
                {
                    if (!IsClosed)
                    {
                        entity = GetTarget(EntityBase, ActiveWeapon, EntityTeam, PlayerBaseTeam);
                        IsClosed = true;
                    }

                    if (entity != null && entity->Health > 0)
                    {
                        if (ActiveWeapon.SilentActive && BasePlayer.Bullet(false))
                        {
                            SetViewAngelSilent(AngelToTarget(entity, ActiveWeapon.WeaponBone, ActiveWeapon.RcsValue, ActiveWeapon.RcsActive));
                        }
                        else
                        {
                            VEngineClient.ViewAngels = Vector.SmoothAngle(VEngineClient.ViewAngels, AngelToTarget(entity, ActiveWeapon.WeaponBone, ActiveWeapon.RcsValue, ActiveWeapon.RcsActive), ActiveWeapon.Smooth);
                        }
                    }
                    else
                    {
                        IsClosed = false;
                    }

                    if (ActiveWeapon.RcsStandaloneActive && BasePlayer.Bullet(true))
                    {
                        RcsStandalone(ActiveWeapon.RcsStandaloneX, ActiveWeapon.RcsStandaloneY);
                    }
                }

                if (ConfigManager.CAimMisc.MouseAttackActive)
                {
                    VClient.ForceAttack = VClient.SUserCMD.ButtonID.IN_ATTACK;
                }
            }
            else
            {
                OldAngle.X = OldAngle.Y = 0.0f;
                IsClosed = false;

                VClient.ForceAttack = VClient.SUserCMD.ButtonID.IN_NOATTACK;
            }
        }

        /// <summary>
        /// Получение цели
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="CAimWeapon"></param>
        /// <returns></returns>
        private static EntityBase* GetTarget(EntityBase* Entity, CAimWeapon CAimWeapon, TeamID EntityTeam, TeamID PlayerBaseTeam)
        {
            if ((EntityTeam == PlayerBaseTeam && !ConfigManager.CAimMisc.DangerZoneActive) || (ConfigManager.CAimMisc.PlayerInAirActive && BasePlayer.BhopFlag()) || (ConfigManager.CAimMisc.EnemyInAirActive && Entity->InAir()))
            {
                return null;
            }
            else if (Vector.GetFov(VEngineClient.ViewAngels, AngelToTarget(Entity, CAimWeapon.WeaponBone, CAimWeapon.RcsValue, CAimWeapon.RcsActive)) <= CAimWeapon.WeaponFov)
            {
                if (ConfigManager.CAimMisc.VisibleID == VisibleID.Spotted && Entity->Spotted == 1)
                {
                    return Entity;
                }
                else if (ConfigManager.CAimMisc.VisibleID == VisibleID.SpottedByMask && Entity->SpottedByMask > 0)
                {
                    return Entity;
                }
            }

            return null;
        }

        /// <summary>
        /// Угол до цели
        /// </summary>
        /// <param name="Player">Индекс игрока</param>
        /// <param name="Bone">Индекс кости</param>
        /// <param name="RcsValue">Значение RCS</param>
        /// <param name="RCS">Контроль спрея</param>
        /// <returns></returns>
        private static Vector3 AngelToTarget(EntityBase* EntityBase, int Bone, float RcsValue, bool RCS) => RCS ? ((BasePlayer.Position + BasePlayer.VecView).CalcAngle(EntityBase->GetBonePosition(Bone)) + (Vector3.Zero - BasePlayer.MyPunch * RcsValue)).NormalizeAngle() : (BasePlayer.Position + BasePlayer.VecView).CalcAngle(EntityBase->GetBonePosition(Bone)).NormalizeAngle();

        /// <summary>
        /// Применение тихого угла
        /// </summary>
        /// <param name="Angle">Угол</param>
        private static void SetViewAngelSilent(Vector3 Angle)
        {
            VClient.CurrentSequenceNumber = VEngineClient.CurrentSequenceNumber;

            VEngineClient.SendPacket = 0;

            while (BasePlayer.CheckSilent(VClient.CurrentSequenceNumber))
            {
                Thread.Yield();
            }

            VClient.SUserCMD OldUserCmd = VClient.OldUserCmd;

            Angle = Angle.ClampAngle();
            Angle = Angle.NormalizeAngle();

            OldUserCmd.m_vecViewAngles = Angle;
            OldUserCmd.m_iButtons |= VClient.SUserCMD.ButtonID.IN_ATTACK;

            VClient.UserCmd = OldUserCmd;

            VEngineClient.SendPacket = 1;
        }

        /// <summary>
        /// Контроль спрея без наводки
        /// </summary>
        /// <param name="RcsX"></param>
        /// <param name="RcsY"></param>
        private static void RcsStandalone(float RcsX, float RcsY)
        {
            Vector3 AimView = VEngineClient.ViewAngels;
            Vector3 RcsPunch = BasePlayer.MyPunch;
            Vector3 Angle = new Vector3();

            AimView.X += OldAngle.X;
            AimView.Y += OldAngle.Y;

            Angle.X = AimView.X - RcsPunch.X * RcsX;
            Angle.Y = AimView.Y - RcsPunch.Y * RcsY;

            Angle = Angle.ClampAngle().NormalizeAngle();

            VEngineClient.ViewAngels = Angle;

            OldAngle.X = RcsPunch.X * RcsX;
            OldAngle.Y = RcsPunch.Y * RcsY;
        }
    }
}