using System;
using System.Linq;
using System.Numerics;
using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Enums;
using Wolf_Hack.SDK.Mathematics;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity.Structures
{
    internal unsafe class PlayerBase : IComparable<PlayerBase>
    {
        public readonly IntPtr m_Class;

        private PlayerBase(IntPtr classPtr) => m_Class = classPtr;

        public static implicit operator PlayerBase(IntPtr Value) => new PlayerBase(Value);

        public static explicit operator IntPtr(PlayerBase value) => value.m_Class;

        public static implicit operator bool(PlayerBase value) => value.m_Class != IntPtr.Zero;

        public float Fov => Vector.GetFov(VEngineClient.ViewAngels, AngleToTarget(false, 6, 0, false));

        /// <summary>
        /// Команда игрока
        /// </summary>
        public TeamID Team
        {
            get
            {
                return (TeamID)Memory.Read<int>(m_Class + Offsets.OTeamNum);
            }
        }

        /// <summary>
        /// Является ли объект игроком
        /// </summary>
        public bool IsPlayer
        {
            get
            {
                TeamID team = Team;

                return team == TeamID.CTTeam || team == TeamID.TTeam;
            }
        }

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
                return Memory.Read<int>(m_Class + Offsets.OSpottedByMask) & (1 << Memory.Read<int>((IntPtr)LocalPlayer + 0x64) - 1);
            }

            set
            {
                Memory.Write(m_Class + Offsets.OSpottedByMask, value);
            }
        }

        /// <summary>
        /// Видимость игроков через радар
        /// </summary>
        public byte Spotted
        {
            get
            {
                return Memory.Read<byte>(m_Class + Offsets.OSpotted);
            }

            set
            {
                Memory.Write(m_Class + Offsets.OSpotted, value);
            }
        }

        /// <summary>
        /// Активность игроков
        /// </summary>
        public bool Dormant
        {
            get
            {
                return Memory.Read<bool>(m_Class + Offsets.ODormant);
            }
        }

        /// <summary>
        /// Жизни игроков
        /// </summary>
        public uint Health
        {
            get
            {
                return Memory.Read<uint>(m_Class + Offsets.OHealth);
            }
        }

        /// <summary>
        /// Флаг Bhop
        /// </summary>
        /// <returns></returns>
        public bool InAir()
        {
            int PlayerFlag = Memory.Read<int>(m_Class + Offsets.OFlags);

            return PlayerFlag == 256 || PlayerFlag == 262;
        }

        /// <summary>
        /// Свечение объекта
        /// </summary>
        /// <param name="SGlowObjectManager"></param>
        /// <param name="Health"></param>
        public void GlowRender(CVisualGlowObjectManager SGlowObjectManager, bool Health = false)
        {
            IntPtr glowObjectDefinitionPtr = (IntPtr)(VClient.GlowObjectManager + Memory.Read<int>(m_Class + Offsets.OGlowIndex) * 0x38);

            VClient.GlowObjectDefinition glowObjectDefinition = Memory.Read<VClient.GlowObjectDefinition>(glowObjectDefinitionPtr);

            if (Health)
            {
                glowObjectDefinition.Red = 1 - (this.Health / 100f);
                glowObjectDefinition.Green = this.Health / 100f;
                glowObjectDefinition.Blue = 0;
            }
            else
            {
                glowObjectDefinition.Red = SGlowObjectManager.Red / 255f;
                glowObjectDefinition.Green = SGlowObjectManager.Green / 255f;
                glowObjectDefinition.Blue = SGlowObjectManager.Blue / 255f;
            }

            glowObjectDefinition.Alpha = SGlowObjectManager.Alpha / 255f;

            glowObjectDefinition.RenderWhenOccluded = true;
            glowObjectDefinition.RenderWhenUnoccluded = false;

            glowObjectDefinition.FullBloomRender = SGlowObjectManager.FullBloom;

            Memory.Write(glowObjectDefinitionPtr, glowObjectDefinition);
        }

        /// <summary>
        /// Изменение модели объекта
        /// </summary>
        /// <param name="CVisualChamsColor"></param>
        /// <param name="Health"></param>
        public void ChamsRender(CVisualChamsColor CVisualChamsColor, bool Health = false)
        {
            if (Health)
            {
                VClient.ChamsObjectDefinition chamsObjectDefinition = new VClient.ChamsObjectDefinition()
                {
                    Red = (byte)(255 - this.Health),
                    Green = (byte)(this.Health * 2.55),
                };

                Memory.Write(m_Class + Offsets.OChamsRender, chamsObjectDefinition);
            }
            else
            {

                VClient.ChamsObjectDefinition chamsObjectDefinition = new VClient.ChamsObjectDefinition()
                {
                    Red = CVisualChamsColor.Red,
                    Green = CVisualChamsColor.Green,
                    Blue = CVisualChamsColor.Blue
                };

                Memory.Write(m_Class + Offsets.OChamsRender, chamsObjectDefinition);
            }
        }

        /// <summary>
        /// Позиция игроков
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return Memory.Read<Vector3>(m_Class + Offsets.OVecOrigin);
            }
        }

        /// <summary>
        /// Позиция костей игроков
        /// </summary>
        /// <param name="Bone">Кость</param>
        /// <returns></returns>
        public Vector3 GetBonePosition(int Bone)
        {
            IntPtr boneVectorPtr = Memory.Read<IntPtr>(m_Class + Offsets.OBoneMatrix) + 0x30 * Bone;

            return new Vector3
            {
                X = Memory.Read<float>(boneVectorPtr + 0xC),
                Y = Memory.Read<float>(boneVectorPtr + 0x1C),
                Z = Memory.Read<float>(boneVectorPtr + 0x2C)
            };
        }

        public int CrosshairId => Memory.Read<int>(m_Class + Offsets.OCrosshair);

        /// <summary>
        /// Флаг Bhop
        /// </summary>
        /// <returns></returns>
        public bool BhopFlag()
        {
            int PlayerFlag = Memory.Read<int>(m_Class + Offsets.OFlags);

            return PlayerFlag == 256 || PlayerFlag == 262;
        }

        /// <summary>
        /// Ослепление
        /// </summary>
        public float FlashMax
        {
            get => Memory.Read<float>(m_Class + Offsets.OFlash);
            set => Memory.Write(m_Class + Offsets.OFlash, value);
        }

        /// <summary>
        /// Проверка на активность для тихого выстрела
        /// </summary>
        public bool CheckSilent(int CurrentSequenceNumber) => Memory.Read<int>((IntPtr)VClient.Input.m_pCommands + (CurrentSequenceNumber % 150) * 0x64 + 0x4) < CurrentSequenceNumber;

        /// <summary>
        /// Пули игрока
        /// </summary>
        public bool Bullet(bool CheckRcs) => CheckRcs ? Memory.Read<int>(m_Class + Offsets.OShootFired) > 1 : Memory.Read<int>(m_Class + Offsets.OShootFired) == 0;

        /// <summary>
        /// Камера игрока
        /// </summary>
        public Vector3 VecView => Memory.Read<Vector3>(m_Class + Offsets.OVecViewOffset);

        /// <summary>
        /// Позиция разброса
        /// </summary>
        public Vector3 MyPunch => Memory.Read<Vector3>(m_Class + Offsets.OVecPunch);

        /// <summary>
        /// Скорость игрока
        /// </summary>
        public int Velocity
        {
            get
            {
                Vector3 VecVelocity = Memory.Read<Vector3>(m_Class + Offsets.OVecVelocity);

                return (int)Math.Sqrt(VecVelocity.X * VecVelocity.X + VecVelocity.Y * VecVelocity.Y);
            }
        }

        /// <summary>
        /// Обновление свечения
        /// </summary>
        public void GlowUpdate(ref bool Active)
        {
            if (LocalPlayer.Health > 0 && Active)
            {
                ClientModule.Write<byte>((IntPtr)Offsets.OGlowUpdate, 0xEB);
                Active = false;
            }
            else if (LocalPlayer.Health == 0 && !Active)
            {
                ClientModule.Write<byte>((IntPtr)Offsets.OGlowUpdate, 0x74);
                Active = true;
            }
        }

        /// <summary>
        /// Угол до цели
        /// </summary>
        /// <param name="Bone">Индекс кости</param>
        /// <param name="RcsValue">Значение RCS</param>
        /// <param name="RCS">Контроль спрея</param>
        /// <returns></returns>
        public Vector3 AngleToTarget(bool nearestBone, int bone, float RcsValue, bool RCS)
        {
            return AngleToTarget(nearestBone, GetBonePosition(bone), RcsValue, RCS);
        }

        /// <summary>
        /// Угол до цели
        /// </summary>
        /// <param name="Bone">Индекс кости</param>
        /// <param name="RcsValue">Значение RCS</param>
        /// <param name="RCS">Контроль спрея</param>
        /// <returns></returns>
        private Vector3 AngleToTarget(bool nearestBone, Vector3 bonePosition, float RcsValue, bool RCS)
        {
            if (RCS)
            {
                if (nearestBone)
                {
                    return ((LocalPlayer.Position + LocalPlayer.VecView).CalcAngle(NearestBone()) + (Vector3.Zero - (LocalPlayer.MyPunch * RcsValue))).NormalizeAngle();
                }

                return ((LocalPlayer.Position + LocalPlayer.VecView).CalcAngle(bonePosition) + (Vector3.Zero - (LocalPlayer.MyPunch * RcsValue))).NormalizeAngle();
            }
            else
            {
                if (nearestBone)
                {
                    return (LocalPlayer.Position + LocalPlayer.VecView).CalcAngle(NearestBone()).NormalizeAngle();
                }

                return (LocalPlayer.Position + LocalPlayer.VecView).CalcAngle(bonePosition).NormalizeAngle();
            }
        }

        public Vector3 NearestBone()
        {
            Vector3[] bones = new Vector3[6];

            int boneIndex = 3;

            for (int index = 0; index < 6; index++)
            {
                bones[index] = GetBonePosition(boneIndex++);
            }

            return bones.Aggregate((current, next) => Vector.GetFov(VEngineClient.ViewAngels, AngleToTarget(false, current, 0, false)) < Vector.GetFov(VEngineClient.ViewAngels, AngleToTarget(false, next, 0, false)) ? current : next);
        }

        public int CompareTo(PlayerBase other) => Fov.CompareTo(other.Fov);
    }
}