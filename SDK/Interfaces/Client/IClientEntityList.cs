using System;
using Wolf_Hack.SDK.Dumpers;
using Wolf_Hack.SDK.Interfaces.Client.Entity;

namespace Wolf_Hack.SDK.Interfaces.Client
{
    internal unsafe struct IClientEntityList
    {
        public static IClientEntity* GetClientEntity(int Index) => (IClientEntity*)Base.Memory.Read<int>(Base.ClientAddress + OffsetDumper.OEntityList + (Index) * 0x10);

        public static IClientEntity* GetClientEntityFromHandle(void* Handle) => (IClientEntity*)Base.Memory.Read<int>(Base.ClientAddress + OffsetDumper.OEntityList + (int)((((uint)Handle & 0xFFF) - 1) * 0x10));
    }
}