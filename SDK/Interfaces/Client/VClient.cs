using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Wolf_Hack.SDK.Dumpers;
using static Wolf_Hack.SDK.Interfaces.Base;

namespace Wolf_Hack.SDK.Interfaces.Client
{
    internal unsafe class VClient
    {
        public static int CurrentSequenceNumber;

        [StructLayout(LayoutKind.Explicit)]
        internal struct SInput
        {
            [FieldOffset(0x0)]
            public int m_pVftable;

            [FieldOffset(0x4)]
            public bool m_bTrackIRAvailable;

            [FieldOffset(0x05)]
            public bool m_bMouseInitialized;

            [FieldOffset(0x06)]
            public bool m_bMouseActive;

            [FieldOffset(0x07)]
            public bool m_bJoystickAdvancedInit;

            [FieldOffset(0x08)]
            public uint Unk1;

            [FieldOffset(0x34)]
            public int m_pKeys;

            [FieldOffset(0x38)]
            public uint Unk2;

            [FieldOffset(0x9C)]
            public bool m_bCameraInterceptingMouse;

            [FieldOffset(0x9D)]
            public bool m_bCameraInThirdPerson;

            [FieldOffset(0x9E)]
            public bool m_bCameraMovingWithMouse;

            [FieldOffset(0xA0)]
            public Vector3 m_vecCameraOffset;

            [FieldOffset(0xAC)]
            public bool m_bCameraDistanceMove;

            [FieldOffset(0xB0)]
            public int m_nCameraOldX;

            [FieldOffset(0xB4)]
            public int m_nCameraOldY;

            [FieldOffset(0xB8)]
            public int m_nCameraX;

            [FieldOffset(0xBC)]
            public int m_nCameraY;

            [FieldOffset(0xC0)]
            public bool m_bCameraIsOrthographic;

            [FieldOffset(0xC4)]
            public Vector3 m_vecPreviousViewAngles;

            [FieldOffset(0xD0)]
            public Vector3 m_vecPreviousViewAnglesTilt;

            [FieldOffset(0xDC)]
            public float m_flLastForwardMove;

            [FieldOffset(0xE0)]
            public Int32 m_nClearInputState;

            [FieldOffset(0xE4)]
            public uint Unk3;

            [FieldOffset(0xF4)]
            public int m_pCommands;

            [FieldOffset(0xF8)]
            public int m_pVerifiedCommands;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct SUserCMD
        {
            public enum ButtonID : int
            {
                IN_NOATTACK = 0,
                IN_ATTACK = 1 << 0,
                IN_JUMP = 1 << 1,
                IN_USE = 1 << 5,
                IN_ATTACK2 = 1 << 11,
                IN_SCORE = 1 << 16,
                IN_BULLRUSH = 1 << 22
            };

            [FieldOffset(0x0)]
            private UIntPtr pVft;

            [FieldOffset(0x04)]
            public Int32 m_iCmdNumber;

            [FieldOffset(0x08)]
            public Int32 m_iTickCount;

            [FieldOffset(0x0C)]
            public Vector3 m_vecViewAngles;

            [FieldOffset(0x18)]
            public Vector3 m_vecAimDirection;

            [FieldOffset(0x24)]
            public float m_flForwardmove;

            [FieldOffset(0x28)]
            public float m_flSidemove;

            [FieldOffset(0x2C)]
            public float m_flUpmove;

            [FieldOffset(0x30)]
            public ButtonID m_iButtons;

            [FieldOffset(0x34)]
            public uint m_bImpulse;

            [FieldOffset(0x38)]
            public Int32 m_iWeaponSelect;

            [FieldOffset(0x3C)]
            public Int32 m_iWeaponSubtype;

            [FieldOffset(0x40)]
            public Int32 m_iRandomSeed;

            [FieldOffset(0x44)]
            public Int16 m_siMouseDx;

            [FieldOffset(0x46)]
            public Int16 m_siMouseDy;

            [FieldOffset(0x48)]
            public Int16 m_bHasBeenPredicted;
        }

        [StructLayout(LayoutKind.Sequential, Size = 0x38)]
        public struct GlowObjectDefinition
        {
            public int Entity;

            public float Red;
            public float Green;
            public float Blue;
            public float Alpha;

            fixed byte pad[4];

            float m_flSomeFloat;

            float bloomAmount;

            float m_flAnotherFloat;

            [MarshalAs(UnmanagedType.I1)]
            public bool RenderWhenOccluded;

            [MarshalAs(UnmanagedType.I1)]
            public bool RenderWhenUnoccluded;

            [MarshalAs(UnmanagedType.I1)]
            public bool FullBloomRender;

            byte pad1;

            int fullBloomStencilTestValue;

            public int glowStyle;

            int splitScreenSlot;

            int nextFreeSlot;
        };

        /// <summary>
        /// Локальный игрок
        /// </summary>
        public static IntPtr LocalPlayer => ClientModule.Read<IntPtr>((IntPtr)Offsets.OLocalPlayer);

        /// <summary>
        /// Вход для серверов
        /// </summary>
        public static SInput Input => ClientModule.Read<SInput>((IntPtr)Offsets.OInput);

        /// <summary>
        /// Клиентский запрос
        /// </summary>
        public static SUserCMD UserCmd
        {
            get => Memory.Read<SUserCMD>((IntPtr)Input.m_pCommands + ((CurrentSequenceNumber - 1) % 150) * 0x64);
            set
            {
                Memory.Write((IntPtr)Input.m_pCommands + (CurrentSequenceNumber - 1) % 150 * 0x64, value);
                Memory.Write((IntPtr)Input.m_pVerifiedCommands + (CurrentSequenceNumber - 1) % 150 * 0x68, value);
            }
        }

        /// <summary>
        /// Прыжок
        /// </summary>
        public static void SetForceJump(SUserCMD.ButtonID value) => ClientModule.Write((IntPtr)Offsets.OForceJump, (int)value);

        /// <summary>
        /// Атака
        /// </summary>
        public static void SetForceAttack(SUserCMD.ButtonID value) => ClientModule.Write((IntPtr)Offsets.OForceAttack, (int)value);

        /// <summary>
        /// Объекты свечения
        /// </summary>
        public static int GlowObjectManager => ClientModule.Read<int>((IntPtr)Offsets.OGlowObjectManager);

        ///// <summary>
        ///// Объекты свечения
        ///// </summary>
        //public static int GlowObjectCount => ClientModule.Read<int>((IntPtr)Offsets.OGlowObjectManager + 0x4);
    }
}