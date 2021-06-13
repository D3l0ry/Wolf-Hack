using System;
using System.Diagnostics;
using System.MemoryInteraction;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack.SDK.Interfaces
{
    internal class Base
    {
        public static Process process = Process.GetProcessesByName("csgo")[0];
        public static MemoryManager Memory = process.GetMemoryManager();

        public static ModuleManager ClientModule = Memory["client.dll"];
        public static ModuleManager EngineModule = Memory["engine.dll"];

        public static PlayerBase LocalPlayer = IntPtr.Zero;
    }
}