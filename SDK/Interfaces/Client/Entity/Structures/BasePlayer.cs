using System;
using System.Numerics;

using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity.Structures
{
    internal struct BasePlayer
    {
        /// <summary>
        /// Команда игрока
        /// </summary>
        public static TeamID Team => (TeamID)Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OTeamNum);

        /// <summary>
        /// Жизни игрока
        /// </summary>
        public static uint Health => Memory.Read<uint>(VClient.LocalPlayer + OffsetDumper.OHealth);

        /// <summary>
        /// Флаг Bhop
        /// </summary>
        /// <returns></returns>
        public static bool BhopFlag()
        {
            int PlayerFlag = Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OFlags);

            return PlayerFlag == 256 || PlayerFlag == 262;
        }

        /// <summary>
        /// Тип камеры игрока
        /// </summary>
        public static ObserverModeID ObserverMode
        {
            get => (ObserverModeID)Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OObserverMode);
            set => Memory.Write(VClient.LocalPlayer + OffsetDumper.OObserverMode, (int)value);
        }

        /// <summary>
        /// Ослепление
        /// </summary>
        public static float FlashMax
        {
            get => Memory.Read<float>(VClient.LocalPlayer + OffsetDumper.OFlash);
            set => Memory.Write(VClient.LocalPlayer + OffsetDumper.OFlash, value);
        }

        #region CrosshairID
        /// <summary>
        /// Индекс противника в прицеле
        /// </summary>
        public static int Crosshair => Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OCrosshair);

        /// <summary>
        /// Объект в прицеле
        /// </summary>
        public static int CrosshairEntity(int Crosshair) => Memory.Read<int>(ClientAddress + OffsetDumper.OEntityList + (Crosshair - 1) * 0x10);

        /// <summary>
        /// Команда объекта в прицеле
        /// </summary>
        public static TeamID CrosshairTeam(int CrosshairEntity) => (TeamID)Memory.Read<int>((IntPtr)CrosshairEntity + OffsetDumper.OTeamNum);
        #endregion

        /// <summary>
        /// Число попаданий в игре
        /// </summary>
        public static int TotalHitsOnServer
        {
            get => Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OTotalHitsOnServer);
            set => Memory.Write(VClient.LocalPlayer + OffsetDumper.OTotalHitsOnServer, value);
        }

        /// <summary>
        /// Проверка на активность для тихого выстрела
        /// </summary>
        public static bool CheckSilent(int CurrentSequenceNumber) => Memory.Read<int>((IntPtr)VClient.Input.m_pCommands + (CurrentSequenceNumber % 150) * 0x64 + 0x4) < CurrentSequenceNumber;

        /// <summary>
        /// Пули игрока
        /// </summary>
        public static bool Bullet(bool CheckRcs) => CheckRcs ? Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OShootFired) > 1 : Memory.Read<int>(VClient.LocalPlayer + OffsetDumper.OShootFired) == 0;

        /// <summary>
        /// Камера игрока
        /// </summary>
        public static Vector3 VecView => Memory.Read<Vector3>(VClient.LocalPlayer + OffsetDumper.OVecViewOffset);

        /// <summary>
        /// Позиция игрока
        /// </summary>
        public static Vector3 Position => Memory.Read<Vector3>(VClient.LocalPlayer + OffsetDumper.OVecOrigin);

        /// <summary>
        /// Позиция разброса
        /// </summary>
        public static Vector3 MyPunch => Memory.Read<Vector3>(VClient.LocalPlayer + OffsetDumper.OVecPunch);

        /// <summary>
        /// Скорость игрока
        /// </summary>
        public static int Velocity
        {
            get
            {
                Vector3 VecVelocity = Memory.Read<Vector3>(VClient.LocalPlayer + OffsetDumper.OVecVelocity);

                return (int)Math.Sqrt(VecVelocity.X * VecVelocity.X + VecVelocity.Y * VecVelocity.Y);
            }
        }

        /// <summary>
        /// Дистанция рук игрока
        /// </summary>
        public static int Fov
        {
            set => Memory.Write(VClient.LocalPlayer + OffsetDumper.OFov,value);
        }

        /// <summary>
        /// Обновление свечения
        /// </summary>
        public static void GlowUpdate(ref bool Active)
        {
            if (Health > 0 && Active)
            {
                Memory.Write<byte>(ClientAddress + OffsetDumper.OGlowUpdate, 0xEB);
                Active = false;
            }
            else if (Health == 0 && !Active)
            {
                Memory.Write<byte>(ClientAddress + OffsetDumper.OGlowUpdate, 0x74);
                Active = true;
            }
        }
    }
}