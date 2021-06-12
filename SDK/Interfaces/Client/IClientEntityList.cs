using System;

using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Client.Entity;

namespace Wolf_Hack.SDK.Interfaces.Client
{
    internal unsafe class IClientEntityList
    {
        public static IClientEntity GetClientEntity(int index) => Base.ClientModule.Read<IntPtr>((IntPtr)Offsets.OEntityList + (index - 1) * 0x10);

        public static IClientEntity GetClientEntityFromHandle(int Handle) => Base.ClientModule.Read<IntPtr>((IntPtr)Offsets.OEntityList + (int)((((uint)Handle & 0xFFF) - 1) * 0x10));
    }
}