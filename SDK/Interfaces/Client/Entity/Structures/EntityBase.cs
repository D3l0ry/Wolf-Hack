using System;
using System.Numerics;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity.Structures
{
    internal unsafe struct EntityBase
    {
        /// <summary>
        /// Команда игрока
        /// </summary>
        public TeamID Team
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return (TeamID)Memory.Read<int>((IntPtr)Class + OffsetDumper.OTeamNum);
                }
            }
        }

        /// <summary>
        /// Является ли объект игроком
        /// </summary>
        public bool IsPlayer(TeamID Team) => Team == TeamID.CTTeam || Team == TeamID.TTeam ? true : false;

        /// <summary>
        /// Является ли игрок валидным
        /// </summary>
        public bool IsValid => Health > 0 && !Dormant;

        /// <summary>
        /// Видимость игрока через локального
        /// </summary>
        public int SpottedByMask
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return Memory.Read<int>((IntPtr)Class + OffsetDumper.OSpottedByMask) & (1 << Memory.Read<int>(VClient.LocalPlayer + 0x64) - 1);
                }
            }

            set
            {
                fixed (void* Class = &this)
                {
                    Memory.Write((IntPtr)Class + OffsetDumper.OSpottedByMask, value);
                }
            }
        }

        /// <summary>
        /// Видимость игроков через радар
        /// </summary>
        public byte Spotted
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return Memory.Read<byte>((IntPtr)Class + OffsetDumper.OSpotted);
                }
            }

            set
            {
                fixed (void* Class = &this)
                {
                    Memory.Write((IntPtr)Class + OffsetDumper.OSpotted, value);
                }
            }
        }

        /// <summary>
        /// Активность игроков
        /// </summary>
        public bool Dormant
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return Memory.Read<bool>((IntPtr)Class + OffsetDumper.ODormant);
                }
            }
        }

        /// <summary>
        /// Жизни игроков
        /// </summary>
        public uint Health
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return Memory.Read<uint>((IntPtr)Class + OffsetDumper.OHealth);
                }
            }
        }

        ///// <summary>
        ///// Активное оружие противника
        ///// </summary>
        //public WeaponID Weapon => (WeaponID)UIMemory.Read<short>(UIMemory.Read<int>(ClientAddress + OffsetDumper.OEntityList + ((UIMemory.Read<int>(Enemies + OffsetDumper.OActiveWeapon) & 0xFFF) - 1) * 0x10) + OffsetDumper.OItemDefinitionIndex);

        /// <summary>
        /// Флаг Bhop
        /// </summary>
        /// <returns></returns>
        public bool InAir()
        {
            fixed (void* Class = &this)
            {
                int PlayerFlag = Memory.Read<int>((IntPtr)Class + OffsetDumper.OFlags);

                return PlayerFlag == 256 || PlayerFlag == 262;
            }
        }

        /// <summary>
        /// Свечение объекта
        /// </summary>
        /// <param name="SGlowObjectManager"></param>
        /// <param name="Health"></param>
        public void GlowRender(EntityBase* entity,CVisualGlowObjectManager SGlowObjectManager, bool Health = false)
        {
            fixed (void* Class = &this)
            {
                if (!entity->Dormant)
                {
                    if (Health)
                    {
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x4), 1 - (this.Health / 100f));
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x8), this.Health / 100f);
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0xC), 0);
                    }
                    else
                    {
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x4), SGlowObjectManager.Red / 255f);
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x8), SGlowObjectManager.Green / 255f);
                        Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0xC), SGlowObjectManager.Blue / 255f);
                    }

                    Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x10), SGlowObjectManager.Allow / 255f);

                    Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x24), true);
                    Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x25), false);
                    Memory.Write((IntPtr)(VClient.GlowObjectManager + (Memory.Read<int>((IntPtr)Class + OffsetDumper.OGlowIndex) * 0x38) + 0x26), SGlowObjectManager.FullBloom);
                }
            }
        }

        /// <summary>
        /// Изменение модели объекта
        /// </summary>
        /// <param name="CVisualChamsColor"></param>
        /// <param name="Health"></param>
        public void ChamsRender(CVisualChamsColor CVisualChamsColor, bool Health = false)
        {
            fixed (void* Class = &this)
            {
                if (Health)
                {
                    Memory.Write((IntPtr)Class + OffsetDumper.OChamsRender, (byte)(255 - this.Health));
                    Memory.Write((IntPtr)Class + OffsetDumper.OChamsRender + 0x1, (byte)(this.Health * 2.55));
                    Memory.Write<byte>((IntPtr)Class + OffsetDumper.OChamsRender + 0x2, 0);
                }
                else
                {
                    Memory.Write((IntPtr)Class + OffsetDumper.OChamsRender, CVisualChamsColor.Red);
                    Memory.Write((IntPtr)Class + OffsetDumper.OChamsRender + 0x1, CVisualChamsColor.Green);
                    Memory.Write((IntPtr)Class + OffsetDumper.OChamsRender + 0x2, CVisualChamsColor.Blue);
                }
            }
        }

        /// <summary>
        /// Позиция игроков
        /// </summary>
        public Vector3 Position
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return Memory.Read<Vector3>((IntPtr)Class + OffsetDumper.OVecOrigin);
                }
            }
        }

        /// <summary>
        /// Позиция костей игроков
        /// </summary>
        /// <param name="PlayerTargetIndex">Индекс игрока</param>
        /// <param name="Bone">Кость</param>
        /// <returns></returns>
        public Vector3 GetBonePosition(int Bone)
        {
            fixed (void* Class = &this)
            {
                return new Vector3
                {
                    X = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)Class + OffsetDumper.OBoneMatrix) + 0x30 * Bone + 0xC),
                    Y = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)Class + OffsetDumper.OBoneMatrix) + 0x30 * Bone + 0x1C),
                    Z = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)Class + OffsetDumper.OBoneMatrix + 0x30 * Bone + 0x2C))
                };
            }
        }

        /// <summary>
        /// Позиция костей игроков
        /// </summary>
        /// <param name="PlayerTargetIndex">Индекс игрока</param>
        /// <param name="Bone">Кость</param>
        /// <returns></returns>
        public static Vector3 GetBonePosition(int PlayerTargetIndex, int Bone) => new Vector3
        {
            X = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)PlayerTargetIndex + OffsetDumper.OBoneMatrix) + 0x30 * Bone + 0xC),
            Y = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)PlayerTargetIndex + OffsetDumper.OBoneMatrix) + 0x30 * Bone + 0x1C),
            Z = Memory.Read<float>(Memory.Read<IntPtr>((IntPtr)PlayerTargetIndex + OffsetDumper.OBoneMatrix) + 0x30 * Bone + 0x2C),
        };
    }
}