using System;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity.Structures
{
    internal unsafe struct BaseWeapon
    {
        ///// <summary>
        ///// Хандл оружий
        ///// </summary>
        //public int WeaponEntity;

        ///// <summary>
        ///// Инициализация оружий
        ///// </summary>
        ///// <param name="WeaponIndex">Хандл оружий</param>
        //public void GetWeaponBase(ref int Index) => WeaponEntity = UIMemory.Read<int>(ClientAddress + OffsetDumper.OEntityList + (((UIMemory.Read<int>(VClient.LocalPlayer + OffsetDumper.OMyWeapon + (Index++ * 0x4)) & 0xFFF) - 1) * 0x10));

        //public void GetWeaponBase(int Player,int Index) => WeaponEntity = UIMemory.Read<int>(ClientAddress + OffsetDumper.OEntityList + (((UIMemory.Read<int>(Player + OffsetDumper.OMyWeapon + (Index * 0x4)) & 0xFFF) - 1) * 0x10));

        #region Active Weapon
        /// <summary>
        /// Handle активного оружия
        /// </summary>
        public static void* ActiveWeaponHandle => IClientEntityList.GetClientEntityFromHandle((void*)Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OActiveWeapon));

        ///// <summary>
        ///// Индекс модели
        ///// </summary>
        //public static int ActiveModelIndex
        //{
        //    get => UIMemory.Read<int>((uint)ActiveWeaponHandle + OffsetDumper.OModelIndex);
        //    set => UIMemory.Write((int)ActiveWeaponHandle + OffsetDumper.OModelIndex, value);
        //}

        /// <summary>
        /// Активное оружие
        /// </summary>
        public static WeaponID ActiveWeaponIndex
        {
            get => (WeaponID)Memory.Read<short>((IntPtr)ActiveWeaponHandle + OffsetDumper.OItemDefinitionIndex);
            set => Memory.Write((IntPtr)ActiveWeaponHandle + OffsetDumper.OItemDefinitionIndex, (short)value);
        }

        ///// <summary>
        ///// Индекс показывающейся модели
        ///// </summary>
        //public static int ActiveViewModelIndex
        //{
        //    get => UIMemory.Read<int>((uint)ActiveWeaponHandle + OffsetDumper.OViewModelIndex);
        //    set => UIMemory.Write((int)ActiveWeaponHandle + OffsetDumper.OViewModelIndex, value);
        //}

        /// <summary>
        /// Патронов в обойме оружия
        /// </summary>
        public static uint ActiveWeaponClip => Memory.Read<uint>((IntPtr)ActiveWeaponHandle + OffsetDumper.OClip);
        #endregion

        //    #region My Weapon
        //    / <summary>
        //    / Данные игрока в Low
        //    / </summary>
        //    public int OriginalOwnerXuidLow
        //    {
        //        get
        //        {
        //            fixed (void* Class = &this)
        //            {
        //                return UIMemory.Read<int>((uint)Class + OffsetDumper.OOriginalOwnerXuidLow);
        //            }
        //        }

        //        set
        //        {
        //            fixed (void* Class = &this)
        //            {
        //                UIMemory.Write((int)Class + OffsetDumper.OOriginalOwnerXuidLow, value);
        //            }
        //        }
        //    }

        //    / <summary>
        //    / Данные игрока в High
        //    / </summary>
        //    public int OriginalOwnerXuidHigh
        //    {
        //        get => UIMemory.Read<int>(WeaponEntity + OffsetDumper.OOriginalOwnerXuidHigh);
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OOriginalOwnerXuidHigh, value);
        //    }

        //    / <summary>
        //    / ID игрока
        //    / </summary>
        //    public int AccountId
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OAccountID, value);
        //    }

        //    / <summary>
        //    / Индекс оружия
        //    / </summary>
        //    public WeaponID ItemDefinitionIndex
        //    {
        //        get => (WeaponID)UIMemory.Read<short>(WeaponEntity + OffsetDumper.OItemDefinitionIndex);
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OItemDefinitionIndex, (short)value);
        //    }

        //    / <summary>
        //    / Резервные значения для скина
        //    / </summary>
        //    public int ItemIdHigh
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OItemIdHigh, value);
        //    }

        //    / <summary>
        //    / ID скина
        //    / </summary>
        //    public float PaintWear
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OPaintWear, value);
        //    }

        //    / <summary>
        //    / ID скина
        //    / </summary>
        //    public int PaintKit
        //    {
        //        get => UIMemory.Read<int>(WeaponEntity + OffsetDumper.OPaintKit);
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OPaintKit, value);
        //    }

        //    / <summary>
        //    / ID прорисовки
        //    / </summary>
        //    public int PaintSeed
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OPaintSeed, value);
        //    }

        //    / <summary>
        //    / Счетчик StatTrack
        //    / </summary>
        //    public int StatTrack
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OStatTrack, value);
        //    }

        //    /// <summary>
        //    /// Имя оружия
        //    /// </summary>
        //    public string CustomName
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OCustomName, value);
        //    }

        //    / <summary>
        //    / Индекс модели
        //    / </summary>
        //    public int ModelIndex
        //    {
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OModelIndex, value);
        //    }

        //    /// <summary>
        //    /// Индекс показывающейся модели
        //    /// </summary>
        //    public int ViewModelIndex
        //    {
        //        get => UIMemory.Read<int>(WeaponEntity + OffsetDumper.OViewModelIndex);
        //        set => UIMemory.Write(WeaponEntity + OffsetDumper.OViewModelIndex, value);
        //    }
        //    #endregion

        //    #region Knife
        //    /// <summary>
        //    /// Хандл ножа и замена индекса
        //    /// </summary>
        //    public static int KnifeIndex
        //    {
        //        get => UIMemory.Read<int>(ClientAddress + OffsetDumper.OEntityList + ((UIMemory.Read<int>(VClient.LocalPlayer + OffsetDumper.OViewModel) & 0xFFF) - 1) * 0x10);
        //        set => UIMemory.Write(UIMemory.Read<int>(ClientAddress + OffsetDumper.OEntityList + ((UIMemory.Read<int>(VClient.LocalPlayer + OffsetDumper.OViewModel) & 0xFFF) - 1) * 0x10) + OffsetDumper.OModelIndex, value);
        //    }

        //    public static int KnifeSequence
        //    {
        //        get => UIMemory.Read<int>(KnifeIndex + OffsetDumper.OSequence);
        //        set => UIMemory.Write(KnifeIndex + OffsetDumper.OSequence, value);
        //    }
        //    #endregion
    }
}