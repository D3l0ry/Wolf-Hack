using System;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Enums;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity.Structures
{
    internal unsafe class BaseWeapon
    {
        #region Active Weapon
        /// <summary>
        /// Handle активного оружия
        /// </summary>
        public static IClientEntity ActiveWeaponHandle => IClientEntityList.GetClientEntityFromHandle(Memory.Read<int>(VClient.LocalPlayer + Offsets.OActiveWeapon));

        /// <summary>
        /// Активное оружие
        /// </summary>
        public static WeaponID ActiveWeaponIndex
        {
            get => (WeaponID)Memory.Read<short>((IntPtr)ActiveWeaponHandle + Offsets.OItemDefinitionIndex);
            set => Memory.Write((IntPtr)ActiveWeaponHandle + Offsets.OItemDefinitionIndex, (short)value);
        }

        /// <summary>
        /// Патронов в обойме оружия
        /// </summary>
        public static uint ActiveWeaponClip => Memory.Read<uint>((IntPtr)ActiveWeaponHandle + Offsets.OClip);
        #endregion
    }
}