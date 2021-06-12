using System;
using Wolf_Hack.SDK.Interfaces.Enums;

namespace Wolf_Hack.Client.Config
{
    [Serializable]
    public struct CAim
    {
        public bool AimActive;
    }

    [Serializable]
    public class CAimWeapon
    {
        public WeaponID WeaponID;

        public bool WeaponActive;

        public int WeaponFov;
        public int WeaponBone = 7;

        public bool SilentActive;
        public int SilentBone = 8;

        public float Smooth = 1;

        public int KillDelay = 0;

        public bool RcsStandaloneActive;
        public float RcsStandaloneX;
        public float RcsStandaloneY;

        public bool RcsActive;
        public float RcsValue;

        public CAimWeapon() : this(WeaponID.WEAPON_NONE) { }

        public CAimWeapon(WeaponID WeaponID) => this.WeaponID = WeaponID;
    }

    [Serializable]
    public struct CAimMisc
    {
        public bool MouseAttackActive;
        public bool PlayerInAirActive;
        public bool EnemyInAirActive;
        public bool DangerZoneActive;
    }
}