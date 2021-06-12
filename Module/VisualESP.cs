using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Enums;

namespace Wolf_Hack.Module.Visual
{
    internal unsafe class VisualESP
    {
        private bool GlowUpdate = true;

        public void Tick(EntityBase entity)
        {
            if (!entity)
            {
                return;
            }

            TeamID localPlayerTeam = Base.LocalPlayer.Team;
            TeamID entityTeam = entity.Team;

            if (ConfigManager.CVisualGlowObjectManager.GlowActive)
            {
                if ((entityTeam != localPlayerTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    entity.GlowRender(ConfigManager.CVisualGlowObjectManager, ConfigManager.CVisualGlowObjectManager.GlowHPActive);
                }
                else if ((entityTeam == localPlayerTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    entity.GlowRender(new CVisualGlowObjectManager());
                }
                else if (ConfigManager.CVisualMisc.DangerZone)
                {
                    entity.GlowRender(ConfigManager.CVisualGlowObjectManager, ConfigManager.CVisualGlowObjectManager.GlowHPActive);
                }

                Base.LocalPlayer.GlowUpdate(ref GlowUpdate);
            }

            if (ConfigManager.CVisualChamsColor.ChamsActive)
            {
                if ((entityTeam != localPlayerTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    entity.ChamsRender(ConfigManager.CVisualChamsColor, ConfigManager.CVisualChamsColor.ChamsHPActive);
                }
                else if (ConfigManager.CVisualMisc.DangerZone)
                {
                    entity.ChamsRender(ConfigManager.CVisualChamsColor, ConfigManager.CVisualChamsColor.ChamsHPActive);
                }
            }

            if (ConfigManager.CVisualMisc.RadarActive)
            {
                entity.Spotted = 1;
            }
        }
    }
}