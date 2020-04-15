using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Mathematics;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Engine
{


    internal unsafe struct VEngineClient
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);


        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* CreateInterfaceFn([MarshalAs(UnmanagedType.LPStr)]string Function, IntPtr Index);
        #endregion

        #region Private Methods
        private static CreateInterfaceFn GetInterface(IntPtr Library) => Marshal.GetDelegateForFunctionPointer<CreateInterfaceFn>(GetProcAddress(Library, "CreateInterface"));

        private static void* GetInterface(IntPtr Library, string Function) => GetInterface(Library)(Function, IntPtr.Zero);
        #endregion

        #region Structures
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
        #endregion

        /// <summary>
        /// Доступ к движку
        /// </summary>
        public static IntPtr EngineBase => Memory.Read<IntPtr>(EngineAddress + OffsetDumper.OClientState);

        /// <summary>
        /// Командная строка игры
        /// </summary>
        /// <param name="cmd">аргумент</param>
        /// <param name="encoding">кодировка</param>
        public static void ClientCmd(string cmd)
        {
           Memory.GetExecutor().CallFunction(OffsetDumper.dwClientCmd,Encoding.UTF8.GetBytes(cmd));
        }

        /// <summary>
        /// Получение индекса игрока
        /// </summary>
        public static int GetLocalPlayer => Memory.Read<int>(EngineBase + OffsetDumper.OGetLocalPlayer) + 0x1;

        /// <summary>
        /// Информация об игроке
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static SPlayerInfo PlayerInfo(int Index) => Memory.Read<SPlayerInfo>((IntPtr)Memory.Read<int>((IntPtr)Memory.Read<int>((IntPtr)Memory.Read<int>((IntPtr)Memory.Read<int>(EngineBase + OffsetDumper.OPlayerInfo) + 0x40) + 0xC) + 0x28 + ((Index - 0x1) * 0x34)));

        /// <summary>
        /// Максимальное число игроков
        /// </summary>
        public static int MaxPlayers => Memory.Read<int>(EngineBase + OffsetDumper.OMaxPlayer);

        /// <summary>
        /// Номер пропуска
        /// </summary>
        public static int CurrentSequenceNumber => Memory.Read<int>(EngineBase + OffsetDumper.OCurrentSequenceNumber) + 0x2;

        /// <summary>
        /// Пакеты для сервера
        /// </summary>
        public static byte SendPacket
        {
            set => Memory.Write(EngineAddress + OffsetDumper.OSendPacket, value);
        }

        /// <summary>
        /// Обновление движка
        /// </summary>
        public static int DeltaTick
        {
            set => Memory.Write(EngineBase + OffsetDumper.ODeltaTick, value);
        }

        /// <summary>
        /// Углы игрока
        /// </summary>
        public static Vector3 ViewAngels
        {
            get => Memory.Read<Vector3>(EngineBase + OffsetDumper.OVecViewAngles);
            set => Memory.Write(EngineBase + OffsetDumper.OVecViewAngles, value.ClampAngle().NormalizeAngle());
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

        //    IntPtr alloc = Memory.GetAllocator().Alloc((uint)shellCode.Length);
        //    uint dwClanTag = (uint)OffsetDumper.dwSetClanTag;

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

        //    Memory.GetExecutor().Execute(alloc, IntPtr.Zero);
        //}
    }
}