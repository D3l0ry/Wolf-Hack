using System.Diagnostics;
using System.Threading;
using Wolf_Hack.Module.Aim;
using Wolf_Hack.Module.Visual;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Client;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Engine;

namespace Wolf_Hack.Module
{
    internal static unsafe class ModuleManager
    {
        private static bool IsConnected = false;
        private static volatile int m_MaxPlayers = 0;

        private static readonly VisualESP m_Visual = new VisualESP();
        private static readonly Legit m_Legit = new Legit();
        private static readonly Misc m_Misc = new Misc();

        private static readonly Thread m_AimTask = new Thread(() =>
        {
            while (true)
            {
                if (!Base.process.IsActiveWindow() || m_MaxPlayers == 0 || !Base.LocalPlayer || Base.LocalPlayer.Health == 0)
                {
                    continue;
                }

                m_Legit.Tick();
            }
        });

        private static readonly Thread m_VisalTask = new Thread(() =>
        {
            int index = 0;

            while (true)
            {
                m_MaxPlayers = VEngineClient.MaxPlayers;

                if (!Base.process.IsActiveWindow())
                {
                    continue;
                }

                if (!IsConnected && m_MaxPlayers != 0)
                {
                    Base.LocalPlayer = IClientEntityList.GetClientEntity(VEngineClient.GetLocalPlayer).GetPlayer;

                    if (!Base.LocalPlayer)
                    {
                        continue;
                    }

                    IsConnected = true;
                }

                if (m_MaxPlayers == 0)
                {
                    IsConnected = false;

                    continue;
                }

                if (Base.LocalPlayer.Health == 0)
                {
                    continue;
                }

                PlayerBase currentEntity = IClientEntityList.GetClientEntity(index).GetPlayer;

                m_Visual.Tick(currentEntity);

                m_Misc.Tick();

                index = index < m_MaxPlayers ? index + 1 : 0;
            }
        });

        public static void Run()
        {
            m_VisalTask.Priority = ThreadPriority.Lowest;
            m_AimTask.Priority = ThreadPriority.Lowest;

            m_VisalTask.Start();
            m_AimTask.Start();
        }

        public static void Abort()
        {
            m_VisalTask.Abort();
            m_AimTask.Abort();
        }
    }
}