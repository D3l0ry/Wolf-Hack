using System;
using System.Collections.Generic;

using NativeManager.MemoryInteraction;

namespace Wolf_Hack.SDK.Interfaces
{
    internal struct Base
    {
        #region Class
        public static  MemoryManager Memory;
        #endregion

        #region Define
        public static IntPtr ClientAddress;
        public static IntPtr EngineAddress;
        #endregion

        public static void Initialize()
        {
            try
            {
                Memory = new MemoryManager("csgo");
                Dictionary<string, IntPtr> Modules = Memory.GetProcessInfo().GetModulesAddress();

                ClientAddress = Modules["client_panorama.dll"];
                EngineAddress = Modules["engine.dll"];
            }
            catch
            {
                Environment.Exit(-1);
            }
        }
    }
}