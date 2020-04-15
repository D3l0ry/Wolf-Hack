using System;
using System.Text;
using System.Media;
using System.Windows.Forms;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using System.Numerics;
using Wolf_Hack.SDK.Mathematics;
using Wolf_Hack.SDK.Interfaces.Client.Entity;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack.Module.Visual
{
    internal unsafe struct VisualESP
    {
        private static bool GlowUpdate = true;

        //public OverlayWindow OverlayWindow;
        //public Graphics Graphics;

        //public static System.Drawing.Rectangle ScreenPrimary = Screen.PrimaryScreen.Bounds;

        //public VisualESP()
        //{
        //    OverlayWindow = new OverlayWindow(ScreenPrimary.Left, ScreenPrimary.Top, ScreenPrimary.Width, ScreenPrimary.Height)
        //    {
        //        IsVisible = true,
        //        IsTopmost = true
        //    };

        //    Graphics = new Graphics()
        //    {
        //        MeasureFPS = true,
        //        Height = OverlayWindow.Height,
        //        PerPrimitiveAntiAliasing = true,
        //        TextAntiAliasing = true,
        //        UseMultiThreadedFactories = true,
        //        VSync = true,
        //        Width = OverlayWindow.Width,
        //        WindowHandle = IntPtr.Zero
        //    };

        //    OverlayWindow.CreateWindow();

        //    Graphics.WindowHandle = OverlayWindow.Handle;
        //    Graphics.Setup();
        //}

        //public SolidBrush GetBrushColor(Color Color) => Graphics.CreateSolidBrush(Color.R, Color.G, Color.B, Color.A);

        public static void VisualInitialize(EntityBase* Entity,TeamID EntityTeam,TeamID PlayerBaseTeam,float[] ViewMatrix = null)
        {
            if (ConfigManager.CVisualGlowObjectManager.GlowActive)
            {
                if ((EntityTeam != PlayerBaseTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->GlowRender(Entity,ConfigManager.CVisualGlowObjectManager, ConfigManager.CVisualGlowObjectManager.GlowHPActive);
                }
                else if ((EntityTeam == PlayerBaseTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->GlowRender(Entity, new CVisualGlowObjectManager(), false);
                }
                else if (ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->GlowRender(Entity, ConfigManager.CVisualGlowObjectManager, ConfigManager.CVisualGlowObjectManager.GlowHPActive);
                }

                BasePlayer.GlowUpdate(ref GlowUpdate);
            }

            if (ConfigManager.CVisualChamsColor.ChamsActive)
            {
                if ((EntityTeam != PlayerBaseTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->ChamsRender(ConfigManager.CVisualChamsColor, ConfigManager.CVisualChamsColor.ChamsHPActive);
                }
                else if ((EntityTeam == PlayerBaseTeam) && !ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->ChamsRender(new CVisualChamsColor());
                }
                else if (ConfigManager.CVisualMisc.DangerZone)
                {
                    Entity->ChamsRender(ConfigManager.CVisualChamsColor, ConfigManager.CVisualChamsColor.ChamsHPActive);
                }
            }

            if (ConfigManager.CVisualMisc.RadarActive)
            {
                Entity->Spotted = 1;
            }
        }
    }
}