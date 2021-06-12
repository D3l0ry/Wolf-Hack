using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Wolf_Hack.SDK.Dumpers;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Engine
{
    internal unsafe class VEngineClient
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SPlayerInfo
        {
            public fixed byte __pad0[8];
            public int m_nXuidLow;
            public int m_nXuidHigh;

            public fixed byte m_szPlayerName[128];
            public int m_nUserID;

            public fixed byte m_szSteamID[33];
            public uint m_nSteam3ID;

            public fixed byte m_szFriendsName[128];

            public bool m_bIsFakePlayer;

            public bool m_bIsHLTV;

            public fixed int m_dwCustomFiles[4];
            public char m_FilesDownloaded;
        }

        /// <summary>
        /// Доступ к движку
        /// </summary>
        public static IntPtr EngineBase => EngineModule.Read<IntPtr>((IntPtr)Offsets.OClientState);

        ///// <summary>
        ///// Командная строка игры
        ///// </summary>
        ///// <param name="cmd">аргумент</param>
        ///// <param name="encoding">кодировка</param>
        //public static void ClientCmd(string cmd)
        //{
        //    new Executor(process).Execute(Offsets.dwClientCmd, new Allocator(process).Alloc());
        //}

        /// <summary>
        /// Получение индекса игрока
        /// </summary>
        public static int GetLocalPlayer => Memory.Read<int>(EngineBase + Offsets.OGetLocalPlayer) + 1;

        /// <summary>
        /// Максимальное число игроков
        /// </summary>
        public static int MaxPlayers => Memory.Read<int>(EngineBase + Offsets.OMaxPlayer);

        /// <summary>
        /// Номер пропуска
        /// </summary>
        public static int CurrentSequenceNumber => Memory.Read<int>(EngineBase + Offsets.OCurrentSequenceNumber) + 0x2;

        /// <summary>
        /// Пакеты для сервера
        /// </summary>
        public static void SetSendPacket(byte value) => EngineModule.Write((IntPtr)Offsets.OSendPacket, value);

        /// <summary>
        /// Обновление движка
        /// </summary>
        public static int DeltaTick
        {
            set
            {
                Memory.Write(EngineBase + Offsets.ODeltaTick, value);
            }
        }

        /// <summary>
        /// Углы игрока
        /// </summary>
        public static Vector3 ViewAngels
        {
            get => Memory.Read<Vector3>(EngineBase + Offsets.OVecViewAngles);
            set => Memory.Write(EngineBase + Offsets.OVecViewAngles, value);
        }

        //public static void SetClanTag([MarshalAs(UnmanagedType.LPStr)]string clanTag)
        //{
        //    byte[] shellCode =
        //    {
        //        0x51,
        //        0x52,
        //        0xB9,0x00,0x00,0x00,0x00,
        //        0xBA,0x00,0x00,0x00,0x00,
        //        0xE8,0x00,0x00,0x00,0x00,
        //        0x83,0x04,0x24,0x0A,
        //        0x68,0x00,0x00,0x00,0x00,
        //        0xC3,
        //        0x5A,
        //        0x59,
        //        0xC3,
        //        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
        //    };
        //    int size = shellCode.Length - 0x10;

        //    IntPtr alloc = new Allocator(process).Alloc(shellCode.Length);
        //    uint dwClanTag = (uint)Offsets.dwSetClanTag;

        //    uint tagAddress = (uint)(alloc + size);
        //    byte[] tagBytes = Encoding.UTF8.GetBytes(clanTag);

        //    fixed (byte* shellPtr = shellCode)
        //    {
        //        Buffer.MemoryCopy(&tagAddress, shellPtr + 0x3, 4, 4);
        //        Buffer.MemoryCopy(&tagAddress, shellPtr + 0x8, 4, 4);
        //        Buffer.MemoryCopy(&dwClanTag, shellPtr + 0x16, 4, 4);
        //        Buffer.BlockCopy(tagBytes, 0, shellCode, size, tagBytes.Length > 15 ? 15 : tagBytes.Length);
        //    }

        //    Memory.WriteBytes(alloc, shellCode);

        //    new Executor(process).Execute(alloc, IntPtr.Zero);
        //}
    }
}