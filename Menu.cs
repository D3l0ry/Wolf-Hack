using System;
using System.Drawing;
using System.Windows.Forms;

using Wolf_Hack.Client.Config;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.WinAPI;
using Wolf_Hack.Module;
using Wolf_Hack.SDK.Interfaces.Enum.EModule;
using Wolf_Hack.Module.Aim;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack
{
    internal unsafe partial class Menu : Form
    {
        #region Menu
        public Menu() => InitializeComponent();

        private void Menu_Load(object sender, EventArgs e)
        {
            Base.Initialize();//Убрать

            CheckProcessTimer.Start();

            Starting.Run();

            #region VisualESP
            ConfigManager.CVisualGlowObjectManager = ConfigManager.LoadConfig<CVisualGlowObjectManager>();
            ConfigManager.CVisualChamsColor = ConfigManager.LoadConfig<CVisualChamsColor>();
            ConfigManager.CVisualESP = ConfigManager.LoadConfig<CVisualESP>();
            ConfigManager.CVisualMisc = ConfigManager.LoadConfig<CVisualMisc>();

            #region Glow
            GlowActive.Checked = ConfigManager.CVisualGlowObjectManager.GlowActive;
            GlowHPActive.Checked = ConfigManager.CVisualGlowObjectManager.GlowHPActive;
            GlowFullBloomActive.Checked = ConfigManager.CVisualGlowObjectManager.FullBloom;

            GlowEnemyColorPanel.BackColor = Color.FromArgb(ConfigManager.CVisualGlowObjectManager.Allow, ConfigManager.CVisualGlowObjectManager.Red, ConfigManager.CVisualGlowObjectManager.Green, ConfigManager.CVisualGlowObjectManager.Blue);
            #endregion

            #region Chams
            ChamsActive.Checked = ConfigManager.CVisualChamsColor.ChamsActive;
            ChamsHPActive.Checked = ConfigManager.CVisualChamsColor.ChamsHPActive;

            ChamsEnemyColorPanel.BackColor = Color.FromArgb(ConfigManager.CVisualChamsColor.Red, ConfigManager.CVisualChamsColor.Green, ConfigManager.CVisualChamsColor.Blue);
            #endregion

            #region ESP
            VisualESPActive.Checked = ConfigManager.CVisualESP.ESPActive;
            VisualESPTracerActive.Checked = ConfigManager.CVisualESP.TracerActive;
            VisualESPNameActive.Checked = ConfigManager.CVisualESP.NameActive;
            VisualESPHealthActive.Checked = ConfigManager.CVisualESP.HealthActive;
            VisualESPDistanceActive.Checked = ConfigManager.CVisualESP.DistanceActive;
            VisualESPFovActive.Checked = ConfigManager.CVisualESP.FovActive;
            #endregion

            #region Misc
            VisualMiscRadarActive.Checked = ConfigManager.CVisualMisc.RadarActive;
            VisualMiscWaterMarkActive.Checked = ConfigManager.CVisualMisc.WaterMark;
            VisualMiscHitSoundActive.Checked = ConfigManager.CVisualMisc.HitSound;
            VisualMiscDangerZoneActive.Checked = ConfigManager.CVisualMisc.DangerZone;
            #endregion

            #endregion

            #region Aim
            ConfigManager.CAim = ConfigManager.LoadConfig<CAim>();
            ConfigManager.CAimWeapon = ConfigManager.LoadConfig<CAimWeapon[]>();
            ConfigManager.CAimMisc = ConfigManager.LoadConfig<CAimMisc>();

            AimWeaponComboBox.SelectedIndex = 0;

            #region Aim Main
            AimActive.Checked = ConfigManager.CAim.AimActive;
            #endregion

            #region Aim Misc
            AimMouseAttackActive.Checked = ConfigManager.CAimMisc.MouseAttackActive;
            AimPlayerInAirActive.Checked = ConfigManager.CAimMisc.PlayerInAirActive;
            AimEnemyInAirActive.Checked = ConfigManager.CAimMisc.EnemyInAirActive;
            AimDangerZoneActive.Checked = ConfigManager.CAimMisc.DangerZoneActive;

            switch (ConfigManager.CAimMisc.VisibleID)
            {
                case VisibleID.Spotted:
                    AimVisibleSpottedActive.Checked = true;
                    break;
                case VisibleID.SpottedByMask:
                    AimVisibleSpottedByMaskActive.Checked = true;
                    break;
            }
            #endregion

            #endregion

            #region Trigger
            ConfigManager.CTrigger = ConfigManager.LoadConfig<CTrigger>();

            TriggerActive.Checked = ConfigManager.CTrigger.TriggerActive;
            #endregion

            #region Misc
            ConfigManager.CMisc = ConfigManager.LoadConfig<CMisc>();

            BunnyHopActive.Checked = ConfigManager.CMisc.BunnyHop;
            NoFlashActive.Checked = ConfigManager.CMisc.NoFlash;
            #endregion
        }

        private void CheckProcessTimer_Tick(object sender, EventArgs e)
        {
            if (Base.Memory.ProcessMemory.HasExited)
            {
                ConfigManager.SaveConfig();

                Starting.TaskModule.Abort();
                Base.Memory.Dispose();

                Environment.Exit(0);
            }
        }
        #endregion

        #region TopPanel
        private void ExitLabel_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveConfig();

            Base.Memory.Dispose();
            Starting.TaskModule.Abort();

            Environment.Exit(0);
        }

        private void CollapseLabel_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.PostMessage(Handle, 0x0112, (IntPtr)0xF012, (IntPtr)0xF008);
        }
        #endregion

        #region Visual

        #region Glow
        private void GlowActive_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.CVisualGlowObjectManager.GlowActive = GlowActive.Checked;

            VEngineClient.DeltaTick = -1;
        }

        private void GlowHPActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualGlowObjectManager.GlowHPActive = GlowHPActive.Checked;

        private void GlowFullBloomActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualGlowObjectManager.FullBloom = GlowFullBloomActive.Checked;

        private void GlowEnemyColorPanel_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == EnemyColorDialog.ShowDialog())
            {
                ConfigManager.CVisualGlowObjectManager.Red = EnemyColorDialog.Color.R;
                ConfigManager.CVisualGlowObjectManager.Green = EnemyColorDialog.Color.G;
                ConfigManager.CVisualGlowObjectManager.Blue = EnemyColorDialog.Color.B;
                ConfigManager.CVisualGlowObjectManager.Allow = EnemyColorDialog.Color.A;

                GlowEnemyColorPanel.BackColor = EnemyColorDialog.Color;
            }
        }
        #endregion

        #region Chams
        private void ChamsActive_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.CVisualChamsColor.ChamsActive = ChamsActive.Checked;

            VEngineClient.DeltaTick = -1;
        }

        private void ChamsHPActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualChamsColor.ChamsHPActive = ChamsHPActive.Checked;

        private void ChamsEnemyColorPanel_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == EnemyColorDialog.ShowDialog())
            {
                ConfigManager.CVisualChamsColor.Red = EnemyColorDialog.Color.R;
                ConfigManager.CVisualChamsColor.Green = EnemyColorDialog.Color.G;
                ConfigManager.CVisualChamsColor.Blue = EnemyColorDialog.Color.B;

                ChamsEnemyColorPanel.BackColor = EnemyColorDialog.Color;
            }
        }
        #endregion

        #region ESP
        private void VisualESPActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.ESPActive = VisualESPActive.Checked;

        private void VisualESPTracerActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.TracerActive = VisualESPTracerActive.Checked;

        private void VisualESPNameActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.NameActive = VisualESPNameActive.Checked;

        private void VisualESPHealthActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.HealthActive = VisualESPHealthActive.Checked;

        private void VisualESPDistanceActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.DistanceActive = VisualESPDistanceActive.Checked;

        private void VisualESPCrosshairNoneRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void VisualESPCrosshairCrossRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void VisualESPCrosshairDotRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region Misc
        private void VisualMiscRadarActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualMisc.RadarActive = VisualMiscRadarActive.Checked;

        private void VisualMiscWaterMarkActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualMisc.WaterMark = VisualMiscWaterMarkActive.Checked;

        private void VisualMiscHitSoundActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualMisc.HitSound = VisualMiscHitSoundActive.Checked;

        private void VisualESPFovActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualESP.FovActive = VisualESPFovActive.Checked;

        private void VisualMiscDangerZoneActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualMisc.DangerZone = VisualMiscDangerZoneActive.Checked;
        #endregion

        #endregion

        #region Aim

        #region Aim Main
        private void AimActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAim.AimActive = AimActive.Checked;
        #endregion

        #region Weapon
        private void AimWeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AimWeaponActive.Checked = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponActive;

            SilentBoneTrackBar.Value = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].SilentBone;

            AimFovLabel.Text = $"Fov: {AimFovTrackBar.Value = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponFov}";
            AimBoneLabel.Text = $"Кость: {Legit.Bone[AimBoneTrackBar.Value = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponBone]}";
            AimSmoothTrackBar.Value = (int)Math.Round(ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].Smooth / 0.1f);
            AimSmoothLabel.Text = $"Плавность: {ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].Smooth}";
            AimKillDelayLabel.Text = $"Задержка после убийства: {AimKillDelayTrackBar.Value = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].KillDelay}";


            AimSilentActive.Checked = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].SilentActive;
            SilentBoneLabel.Text = $"Кость: {Legit.Bone[SilentBoneTrackBar.Value = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].SilentBone]}";

            AimRcsStandaloneActive.Checked = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneActive;
            AimRcsStandaloneXTrackBar.Value = (int)Math.Round(ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneX / 0.1f);
            AimRcsStandaloneYTrackBar.Value = (int)Math.Round(ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneY / 0.1f);

            AimRcsActive.Checked = ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsActive;
            AimRcsTrackBar.Value = (int)Math.Round(ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsValue / 0.1f);
        }

        private void AimWeaponActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponActive = AimWeaponActive.Checked;

        private void AimFovTrackBar_Scroll(object sender, EventArgs e) => AimFovLabel.Text = $"Fov: {ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponFov = AimFovTrackBar.Value}";

        private void AimBoneTrackBar_Scroll(object sender, EventArgs e) => AimBoneLabel.Text = $"Кость: {Legit.Bone[ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].WeaponBone = AimBoneTrackBar.Value]}";

        private void AimSmoothTrackBar_Scroll(object sender, EventArgs e) => AimSmoothLabel.Text = $"Плавность: {ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].Smooth = AimSmoothTrackBar.Value * 0.1f}";

        private void AimKillDelayTrackBar_Scroll(object sender, EventArgs e) => AimKillDelayLabel.Text = $"Задержка после убийства: {ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].KillDelay = AimKillDelayTrackBar.Value}";

        private void AimSilentActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].SilentActive = AimSilentActive.Checked;

        private void SilentBoneTrackBar_Scroll(object sender, EventArgs e) => SilentBoneLabel.Text = $"Кость: {Legit.Bone[ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].SilentBone = SilentBoneTrackBar.Value]}";

        private void AimRcsActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsActive = AimRcsActive.Checked;

        private void AimRcsTrackBar_Scroll(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsValue = AimRcsTrackBar.Value * 0.1f;
        #endregion

        #region Misc
        private void AimMouseAttackActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.MouseAttackActive = AimMouseAttackActive.Checked;

        private void AimPlayerInAirActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.PlayerInAirActive = AimPlayerInAirActive.Checked;

        private void AimEnemyInAirActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.EnemyInAirActive = AimEnemyInAirActive.Checked;

        private void AimDangerZoneActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.DangerZoneActive = AimDangerZoneActive.Checked;

        private void AimRcsStandaloneActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneActive = AimRcsStandaloneActive.Checked;

        private void AimRcsStandaloneXTrackBar_Scroll(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneX = AimRcsStandaloneXTrackBar.Value * 0.1f;

        private void AimRcsStandaloneYTrackBar_Scroll(object sender, EventArgs e) => ConfigManager.CAimWeapon[AimWeaponComboBox.SelectedIndex].RcsStandaloneY = AimRcsStandaloneYTrackBar.Value * 0.1f;

        private void AimVisibleSpottedActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.VisibleID = VisibleID.Spotted;

        private void AimVisibleSpottedByMaskActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CAimMisc.VisibleID = VisibleID.SpottedByMask;
        #endregion

        #endregion

        #region Trigger
        private void TriggerActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CTrigger.TriggerActive = TriggerActive.Checked;
        #endregion

        #region SkinChanger

        //#region SkinChanger Main
        //private void SkinChangerActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChanger.SkinChangerActive = SkinChangerActive.Checked;
        //#endregion

        //#region Weapon
        //private void SkinChangerWeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (SkinChangerWeaponComboBox.SelectedIndex)
        //    {
        //        case 0:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Granite Marbleized",
        //                "Silver",
        //                "Scorpion",
        //                "Grassland",
        //                "Grassland Leaves",
        //                "Corticera",
        //                "Ocean Foam",
        //                "Amber Fade",
        //                "Red FragCam",
        //                "Pulse",
        //                "Chainmail",
        //                "Coach Class",
        //                "Ivory",
        //                "Fire Elemental",
        //                "Pathfinder",
        //                "Handgun",
        //                "Imperial",
        //                "Oceanic",
        //                "Imperial Dragon",
        //                "Turf",
        //                "Woodsman",
        //                "Urban Hazard"
        //            });
        //            break;
        //        case 1:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Groundwater",
        //    "Candy Apple",
        //    "Fade",
        //    "Night",
        //    "Dragon Tattoo",
        //    "Brass",
        //    "Sand Dune",
        //    "Steel Disruption",
        //    "Blue Fissure",
        //    "Death Rattle",
        //    "Water Elemental",
        //    "Reactor",
        //    "Grinder",
        //    "Catacombs",
        //    "Twilight Galaxy",
        //    "Bunsen Burner",
        //    "Wraiths",
        //    "Royal Legion",
        //    "Wasteland Rebel",
        //    "Weasel",
        //    "Ironwork",
        //    "Off World",
        //    "Moonrise",
        //    "Warhawk",
        //    "Nuclear Garden",
        //    "High Beam",
        //    "Oxide Blaze"
        //            });
        //            break;
        //        case 2:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Forest Leaves",
        //    "Dark Water",
        //    "Overgrowth",
        //    "Blood Tiger",
        //    "Serum",
        //    "Night Ops",
        //    "Guardian",
        //    "Stainless",
        //    "Orion",
        //    "Road Rash",
        //    "Royal Blue",
        //    "Caiman",
        //    "Business Class",
        //    "Para Green",
        //    "Torque",
        //    "Kill Confirmed",
        //    "Lead Conduit",
        //    "Cyrex",
        //    "Neo-Noir",
        //    "Blueprint",
        //    "Cortex",
        //    "Check Engine",
        //    "Flashback"
        //            });
        //            break;
        //        case 3:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Anodized Navy",
        //    "Stained",
        //    "Contractor",
        //    "Colony",
        //    "Demolition",
        //    "Black Limba",
        //    "Hemoglobin",
        //    "Cobalt Quartz",
        //    "Marina",
        //    "Panther",
        //    "Retribution",
        //    "Briar",
        //    "Urban Shock",
        //    "Dualing Dragons",
        //    "Cartel",
        //    "Ventilators",
        //    "Royal Consorts",
        //    "Cobra Strike",
        //    "Shred",
        //    "Twin Turbo"
        //            });
        //            break;
        //        case 4:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Gunsmoke",
        //    "Bone Mask",
        //    "Metallic DDPAT",
        //    "Boreal Forest",
        //    "Splash",
        //    "Modern Hunter",
        //    "Nuclear Threat",
        //    "Facets",
        //    "Sand Dune",
        //    "Hive",
        //    "Steel Disruption",
        //    "Whiteout",
        //    "Mehndi",
        //    "Undertow",
        //    "Franklin",
        //    "Supernova",
        //    "Contamination",
        //    "Cartel",
        //    "Muertos",
        //    "Valence",
        //    "Neon Kimono",
        //    "Crimson Kimono",
        //    "Mint Kimono",
        //    "Asiimov",
        //    "Iron Clad",
        //    "Ripple",
        //    "Red Rock",
        //    "See Ya Later",
        //    "Vino Primo",
        //    "Facility Draft",
        //    "Exchanger",
        //    "Nevermore"
        //            });
        //            break;
        //        case 5:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Candy Apple",
        //    "Case Hardened",
        //    "Contractor",
        //    "Forest Night",
        //    "Orange Peel",
        //    "Jungle",
        //    "Anodized Gunmetal",
        //    "Nightshade",
        //    "Silver Quartz",
        //    "Nitro",
        //    "Kami",
        //    "Copper Galaxy",
        //    "Fowl Play",
        //    "Urban Hazard",
        //    "Hot Shot",
        //    "Monkey Business",
        //    "Neon Kimono",
        //    "Retrobution",
        //    "Triumvirate",
        //    "Violent Daimyo",
        //    "Scumbria",
        //    "Capillary",
        //    "Hyper Beast",
        //    "Flame Test",
        //    "Coolant"
        //            });
        //            break;
        //        case 6:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Urban DDPAT",
        //    "Ossified",
        //    "Tornado",
        //    "Brass",
        //    "Nuclear Threat",
        //    "Groundwater",
        //    "Blue Titanium",
        //    "VariCamo",
        //    "Army Mesh",
        //    "Red Quartz",
        //    "Sandstorm",
        //    "Titanium Bit",
        //    "Isaac",
        //    "Toxic",
        //    "Hades",
        //    "Bamboo Forest",
        //    "Terrace",
        //    "Avalanche",
        //    "Jambiya",
        //    "Re-Entry",
        //    "Ice Cap",
        //    "Fuel Injector",
        //    "Cut Out",
        //    "Cracked Opal",
        //    "Snek-9",
        //    "Remote Control",
        //    "Fubar"
        //            });
        //            break;
        //        case 7:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Crimson Web",
        //    "Hexane",
        //    "Nitro",
        //    "Tread Plate",
        //    "The Fuschia Is Now",
        //    "Victoria",
        //    "Tuxedo",
        //    "Army Sheen",
        //    "Poison Dart",
        //    "Chalice",
        //    "Twist",
        //    "Tigris",
        //    "Green Plaid",
        //    "Pole Position",
        //    "Emerald",
        //    "Yellow Jacket",
        //    "Red Astor",
        //    "Imprint",
        //    "Polymer",
        //    "Xiangliu",
        //    "Tacticat",
        //    "Eco"
        //            });
        //            break;
        //        case 8:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Crimson Web",
        //    "Urban DDPAT",
        //    "Blaze",
        //    "Night",
        //    "Hypnotic",
        //    "Mudder",
        //    "Golden Koi",
        //    "Cobalt Disruption",
        //    "Urban Rubble",
        //    "Heirloom",
        //    "Meteorite",
        //    "Hand Cannon",
        //    "Pilot",
        //    "Conspiracy",
        //    "Naga",
        //    "Bronze Deco",
        //    "Midnight Storm",
        //    "Sunset Storm 壱",
        //    "Sunset Storm 弐",
        //    "Kumicho Dragon",
        //    "Directive",
        //    "Oxide Blaze",
        //    "Corinthian",
        //    "Code Red",
        //    "Mecha Industries"
        //            });
        //            break;
        //        case 9:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Crimson Web",
        //    "Bone Mask",
        //    "Urban Perforated",
        //    "Waves Perforated",
        //    "Orange Peel",
        //    "Urban Masked",
        //    "Jungle Dashed",
        //    "Sand Dashed",
        //    "Urban Dashed",
        //    "Dry Season",
        //    "Fade",
        //    "Amber Fade",
        //    "Reboot",
        //    "Llama Cannon",
        //    "Grip",
        //    "Survivalist",
        //    "Nitro"
        //            });
        //            break;
        //        case 10:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Candy Apple",
        //    "Forest Leaves",
        //    "Bloomstick",
        //    "Polar Mesh",
        //    "Walnut",
        //    "Modern Hunter",
        //    "Blaze Orange",
        //    "Predator",
        //    "Tempest",
        //    "Sand Dune",
        //    "Graphite",
        //    "Ghost Camo",
        //    "Rising Skull",
        //    "Antique",
        //    "Green Apple",
        //    "Caged Steel",
        //    "Koi",
        //    "Moon in Libra",
        //    "Ranger",
        //    "Hyper Beast",
        //    "Exo",
        //    "Gila",
        //    "Wild Six",
        //    "Toy Soldier",
        //    "Mandrel",
        //    "Wood Fired"
        //            });
        //            break;
        //        case 11:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Blue Steel",
        //    "Grassland",
        //    "Blue Spruce",
        //    "Urban Perforated",
        //    "Jungle",
        //    "Blaze Orange",
        //    "Fallout Warning",
        //    "VariCamo Blue",
        //    "CaliCamo",
        //    "Heaven Guard",
        //    "Red Python",
        //    "Red Leather",
        //    "Bone Machine",
        //    "Tranquility",
        //    "Quicksilver",
        //    "Scumbria",
        //    "Teclu Burner",
        //    "Black Tie",
        //    "Slipstream",
        //    "Seasons",
        //    "Ziggy",
        //    "Oxide Blaze"
        //            });
        //            break;
        //        case 12:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Silver",
        //    "Metallic DDPAT",
        //    "Bulldozer",
        //    "Storm",
        //    "Irradiated Alert",
        //    "Memento",
        //    "Hazard",
        //    "Sand Dune",
        //    "Heaven Guard",
        //    "Firestarter",
        //    "Heat",
        //    "Counter Terrace",
        //    "Seabird",
        //    "Cobalt Core",
        //    "Praetorian",
        //    "Petroglyph",
        //    "Sonar",
        //    "Hard Water",
        //    "SWAG-7",
        //    "Rust Coat",
        //    "Core Breach"
        //            });
        //            break;
        //        case 13:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Forest DDPAT",
        //    "Snake Camo",
        //    "Copper",
        //    "Orange DDPAT",
        //    "Sage Spray",
        //    "Irradiated Alert",
        //    "Rust Coat",
        //    "Mosaico",
        //    "Amber Fade",
        //    "Full Stop",
        //    "The Kraken",
        //    "First Class",
        //    "Highwayman",
        //    "Serenity",
        //    "Origami",
        //    "Bamboo Shadow",
        //    "Yorick",
        //    "Fubar",
        //    "Limelight",
        //    "Wasteland Princess",
        //    "Zander",
        //    "Morris",
        //    "Devourer",
        //    "Brake Light",
        //    "Black Sand"
        //            });
        //            break;
        //        case 14:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "NULL"
        //            });
        //            break;
        //        case 15:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "NULL"
        //            });
        //            break;
        //        case 16:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Candy Apple",
        //    "Urban DDPAT",
        //    "Silver",
        //    "Fade",
        //    "Ultraviolet",
        //    "Tornado",
        //    "Palm",
        //    "Graven",
        //    "Amber Fade",
        //    "Heat",
        //    "Curse",
        //    "Indigo",
        //    "Tatter",
        //    "Commuter",
        //    "Nuclear Garden",
        //    "Malachite",
        //    "Neon Rider",
        //    "Rangeen",
        //    "Lapis Gator",
        //    "Carnivore",
        //    "Last Dive",
        //    "Aloha",
        //    "Oceanic",
        //    "Carnivore",
        //    "Last Dive",
        //    "Aloha",
        //    "Calf Skin",
        //    "Pipe Down"
        //            });
        //            break;
        //        case 17:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Hot Rod",
        //    "Bulldozer",
        //    "Hypnotic",
        //    "Storm",
        //    "Orange Peel",
        //    "Sand Dashed",
        //    "Dry Season",
        //    "Rose Iron",
        //    "Dark Age",
        //    "Green Plaid",
        //    "Setting Sun",
        //    "Dart",
        //    "Deadly Poison",
        //    "Pandora\'s Box",
        //    "Ruby Poison Dart",
        //    "Bioleak",
        //    "Airlock",
        //    "Sand Scale",
        //    "Goo",
        //    "Black Sand",
        //    "Capillary",
        //    "Slide",
        //    "Modest Threat"
        //            });
        //            break;
        //        case 18:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Forest DDPAT",
        //    "Skulls",
        //    "Gunsmoke",
        //    "Anodized Navy",
        //    "Whiteout",
        //    "Orange Peel",
        //    "Groundwater",
        //    "Ocean Foam",
        //    "Army Recon",
        //    "Full Stop",
        //    "Urban Hazard",
        //    "Olive Plaid",
        //    "Armor Core",
        //    "Asterion",
        //    "Nemesis",
        //    "Special Delivery",
        //    "Impire",
        //    "Cirrus",
        //    "Akoben",
        //    "Bloodsport",
        //    "Powercore",
        //    "Powercore",
        //    "Fade",
        //    "Motherboard"
        //            });
        //            break;
        //        case 19:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "NULL"
        //            });
        //            break;
        //        case 20:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Gunsmoke",
        //    "Urban DDPAT",
        //    "Blaze",
        //    "Carbon Fiber",
        //    "Caramel",
        //    "Fallout Warning",
        //    "Scorched",
        //    "Bone Pile",
        //    "Corporal",
        //    "Indigo",
        //    "Labyrinth",
        //    "Delusion",
        //    "Grand Prix",
        //    "Minotaur\'s Labyrinth",
        //    "Riot",
        //    "Primal Saber",
        //    "Briefing",
        //    "Scaffold",
        //    "Metal Flowers",
        //    "Exposure",
        //    "Arctic Wolf",
        //    "Facility Dark",
        //    "Momentum"
        //            });
        //            break;
        //        case 21:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Virus",
        //    "Cold Blooded",
        //    "Storm",
        //    "Glacier Mesh",
        //    "Sand Spray",
        //    "Death by Kitty",
        //    "Fallout Warning",
        //    "Scorched",
        //    "Emerald Dragon",
        //    "Blind Spot",
        //    "Ash Wood",
        //    "Teardown",
        //    "Trigon",
        //    "Desert Warfare",
        //    "Module",
        //    "Leather",
        //    "Asiimov",
        //    "Elite Build",
        //    "Shapewood",
        //    "Chopper",
        //    "Grim",
        //    "Shallow Grave",
        //    "Death Grip",
        //    "Traction",
        //    "Facility Negative"
        //            });
        //            break;
        //        case 22:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Blue Streak",
        //    "Forest Leaves",
        //    "Carbon Fiber",
        //    "Jungle Dashed",
        //    "Urban Dashed",
        //    "Sand Dashed",
        //    "Brass",
        //    "Modern Hunter",
        //    "Irradiated Alert",
        //    "Rust Coat",
        //    "Water Sigil",
        //    "Night Ops",
        //    "Cobalt Halftone",
        //    "Antique",
        //    "Osiris",
        //    "Chemical Green",
        //    "Bamboo Print",
        //    "Fuel Rod",
        //    "Phonic Zone",
        //    "Judgement of Anubis",
        //    "Harvester",
        //    "Jungle Slipstream",
        //    "High Roller",
        //    "Facility"
        //            });
        //            break;
        //        case 23:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Contrast Spray",
        //    "Colony",
        //    "Cyanospatter",
        //    "Afterimage",
        //    "Doomkitty",
        //    "Spitfire",
        //    "Hexane",
        //    "Teardown",
        //    "Pulse",
        //    "Sergeant",
        //    "Styx",
        //    "Djinn",
        //    "Neural Net",
        //    "Survivor Z",
        //    "Valence",
        //    "Roll Cage",
        //    "Mecha Industries",
        //    "Macabre",
        //    "Eye of Athena"
        //            });
        //            break;
        //        case 24:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Winter Forest",
        //    "Orange DDPAT",
        //    "Sage Spray",
        //    "Shattered",
        //    "Blue Titanium",
        //    "VariCamo",
        //    "Urban Rubble",
        //    "Hunting Blind",
        //    "Sandstorm",
        //    "Kami",
        //    "Tuxedo",
        //    "Cerberus",
        //    "Chatterbox",
        //    "Eco",
        //    "Aqua Terrace",
        //    "Rocket Pop",
        //    "Srone Cold",
        //    "Firefight",
        //    "Black Sand",
        //    "Crimson Tsunami",
        //    "Sugar Rush",
        //    "Cold Fusion",
        //    "Signal"
        //            });
        //            break;
        //        case 25:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Dark Water",
        //    "Boreal Forest",
        //    "Bright Water",
        //    "Blood Tiger",
        //    "VariCamo",
        //    "Nitro",
        //    "Guardian",
        //    "Atomic Alloy",
        //    "Master Piece",
        //    "Knight",
        //    "Cyrex",
        //    "Basilisk",
        //    "Hyper Beast",
        //    "Icarus Fell",
        //    "Hot Rod",
        //    "Golden Coil",
        //    "Chantico\'s Fire",
        //    "Mecha Industries",
        //    "Flashback",
        //    "Decimator",
        //    "Briefing",
        //    "Leaded Glass",
        //    "Nightmare",
        //    "Control Panel"
        //            });
        //            break;
        //        case 26:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Red Laminate",
        //    "Case hardened",
        //    "Safari Mesh",
        //    "Jungle Spray",
        //    "Predator",
        //    "Black Laminate",
        //    "Fire Serpent",
        //    "Blue Laminate",
        //    "Redline",
        //    "Emerald Pinstripe",
        //    "Jaguar",
        //    "Vulcan",
        //    "Jet Set",
        //    "First Class",
        //    "Wasteland Rebel",
        //    "Cartel",
        //    "Elite Build",
        //    "Hydroponic",
        //    "Aquamarine Revenge",
        //    "Frontside Misty",
        //    "Point Disarray",
        //    "Fuel Injector",
        //    "Neon Revolution",
        //    "Bloodsport",
        //    "Orbit Mk01",
        //    "Crimson Web",
        //    "The Empress",
        //    "Neon Rider",
        //    "Safety Net",
        //    "Asiimov"
        //            });
        //            break;
        //        case 27:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Desert Storm",
        //    "Jungle Tiger",
        //    "Urban DDPAT",
        //    "Tornado",
        //    "Bullet Rain",
        //    "Modern Hunter",
        //    "Radiation Hazard",
        //    "Faded Zebra",
        //    "Zirka",
        //    "X-Ray",
        //    "Asiimov",
        //    "Howl",
        //    "Desert-Strike",
        //    "Griffin",
        //    "Dragon King",
        //    "Poseidon",
        //    "Daybreak",
        //    "Evil Daimyo",
        //    "Royal Paladin",
        //    "The Battlestar",
        //    "Desolate Space",
        //    "Buzz Kill",
        //    "Hellfire",
        //    "Neo-Noir",
        //    "Mainframe",
        //    "Converter",
        //    "Magnesium"
        //            });
        //            break;
        //        case 28:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Bengal Tiger",
        //    "Copperhead",
        //    "Anodized Navy",
        //    "Hot Rod",
        //    "Contractor",
        //    "Colony",
        //    "Wings",
        //    "Storm",
        //    "Condemned",
        //    "Radiation Hazard",
        //    "Chameleon",
        //    "Torque",
        //    "Daedalus",
        //    "Akihabara Accept",
        //    "Ricochet",
        //    "Fleet Flock",
        //    "Aristocrat",
        //    "Syd Mead",
        //    "Triqua",
        //    "Stymphalian",
        //    "Amber Slipstream",
        //    "Random Access",
        //    "Sweeper"
        //            });
        //            break;
        //        case 29:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Anodized Navy",
        //    "Bulldozer",
        //    "Ultraviolet",
        //    "Tornado",
        //    "Waves Perforated",
        //    "Fallout Warning",
        //    "Wave Spray",
        //    "Gator Mesh",
        //    "Damascus Steel",
        //    "Pulse",
        //    "Army Sheen",
        //    "Traveler",
        //    "Cyrex",
        //    "Tiger Moth",
        //    "Atlas",
        //    "Aerial",
        //    "Triarch",
        //    "Phantom",
        //    "Aloha",
        //    "Integrale",
        //    "Danger Close"
        //            });
        //            break;
        //        case 30:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Lichen Dashed",
        //    "Dark Water",
        //    "Blue Spruce",
        //    "Splashed",
        //    "Mayan Dreams",
        //    "Sand Dune",
        //    "Blood in the Water",
        //    "Tropical Storm",
        //    "Acid Fade",
        //    "Detour",
        //    "Abyss",
        //    "Big Iron",
        //    "Necropos",
        //    "Ghost Crusader",
        //    "Dragonfire",
        //    "Death's Head"
        //            });
        //            break;
        //        case 31:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Snake Camo",
        //    "Lightning Strike",
        //    "Safari Mesh",
        //    "Pink DDPAT",
        //    "BOOM",
        //    "Corticera",
        //    "Graphite",
        //    "Electric Hive",
        //    "Pit Viper",
        //    "Redline",
        //    "Asiimov",
        //    "Dragon Lore",
        //    "Man-o\'-war",
        //    "Worm God",
        //    "Medusa",
        //    "Sun in Leo",
        //    "Hyper Beast",
        //    "Elite Build",
        //    "Phobos",
        //    "Fever Dream",
        //    "Oni Taiji",
        //    "Mortis",
        //    "PAW",
        //    "Acheron",
        //    "Neo-Noir"
        //            });
        //            break;
        //        case 32:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Crimson Web",
        //    "Contractor",
        //    "Carbon Fiber",
        //    "Storm",
        //    "Sand Mesh",
        //    "Palm",
        //    "Splash Jam",
        //    "Emerald",
        //    "Army Sheen",
        //    "Cyrex",
        //    "Cardiac",
        //    "Grotto",
        //    "Green Marine",
        //    "Outbreak",
        //    "Bloodsport",
        //    "Powercore",
        //    "Blueprint",
        //    "Jungle Slipstream"
        //            });
        //            break;
        //        case 33:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Arctic Camo",
        //    "Desert Storm",
        //    "Contractor",
        //    "Safari Mesh",
        //    "Polar Camo",
        //    "Jungle Dashed",
        //    "Demeter",
        //    "Azure Zebra",
        //    "VariCamo",
        //    "Green Apple",
        //    "Murky",
        //    "Chronos",
        //    "Orange Kimono",
        //    "Flux",
        //    "Corinthian",
        //    "The Executioner",
        //    "Orange Crash",
        //    "Ventilator",
        //    "Stinger",
        //    "Hunter",
        //    "High Seas",
        //    "Scavenger"
        //            });
        //            break;
        //        case 34:
        //        case 35:
        //        case 36:
        //        case 37:
        //        case 38:
        //        case 39:
        //        case 40:
        //        case 41:
        //        case 42:
        //        case 43:
        //            WolfClient.AddItems(SkinChangerSkinComboBox, new object[]
        //            {
        //                "Forest DDPAT",
        //    "Crimson Web",
        //    "Fade",
        //    "Night",
        //    "Blue Steel",
        //    "Stained",
        //    "Case Hardened",
        //    "Slaughter",
        //    "Safari Mesh",
        //    "Boreal Forest",
        //    "Ultraviolet",
        //    "Urban Masked",
        //    "Scorched",
        //    "Tiger Tooth",
        //    "Marble Fade",
        //    "Rust Coat"
        //            });
        //            break;
        //    }

        //    SkinChangerSkinComboBox.Text = $"{ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID}";
        //    SkinChangerWearTrackBar.Value = (int)Math.Round(ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinWear / 0.01f);
        //    SkinChangerWeaponNameTextBox.Text = ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].WeaponName;
        //}

        //private void SkinChangerSkinComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (SkinChangerWeaponComboBox.SelectedIndex)
        //    {
        //        case 0:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Granite Marbleized":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "Urban DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 17;
        //                    break;
        //                case "Blaze":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 37;
        //                    break;
        //                case "Night":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 40;
        //                    break;
        //                case "Hypnotic":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 61;
        //                    break;
        //                case "Mudder":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 90;
        //                    break;
        //                case "Golden Koi":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 185;
        //                    break;
        //                case "Cobalt Disruption":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 231;
        //                    break;
        //                case "Urban Rubble":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 237;
        //                    break;
        //                case "Heirloom":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 273;
        //                    break;
        //                case "Meteorite":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 296;
        //                    break;
        //                case "Hand Cannon":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 328;
        //                    break;
        //                case "Pilot":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 347;
        //                    break;
        //                case "Conspiracy":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 351;
        //                    break;
        //                case "Naga":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 397;
        //                    break;
        //                case "Bronze Deco":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 425;
        //                    break;
        //                case "Midnight Storm":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 468;
        //                    break;
        //                case "Sunset Storm 壱":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 469;
        //                    break;
        //                case "Sunset Storm 弐":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 470;
        //                    break;
        //                case "Corinthian":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 509;
        //                    break;
        //                case "Kumicho Dragon":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 527;
        //                    break;
        //                case "Directive":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 603;
        //                    break;
        //                case "Oxide Blaze":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 645;
        //                    break;
        //                case "Code Red":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 711;
        //                    break;
        //                case "Mecha Industries":
        //                    ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = 805;
        //                    break;
        //            }
        //            break;
        //        case 1:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Anodized Navy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 28;
        //                    break;
        //                case "Stained":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 43;
        //                    break;
        //                case "Contractor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 46;
        //                    break;
        //                case "Colony":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 47;
        //                    break;
        //                case "Demolition":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 153;
        //                    break;
        //                case "Black Limba":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 172;
        //                    break;
        //                case "Hemoglobin":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 220;
        //                    break;
        //                case "Cobalt Quartz":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 249;
        //                    break;
        //                case "Marina":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 261;
        //                    break;
        //                case "Panther":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 276;
        //                    break;
        //                case "Retribution":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 307;
        //                    break;
        //                case "Briar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 330;
        //                    break;
        //                case "Urban Shock":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 396;
        //                    break;
        //                case "Dualing Dragons":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 491;
        //                    break;
        //                case "Cartel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 528;
        //                    break;
        //                case "Ventilators":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 544;
        //                    break;
        //                case "Royal Consorts":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 625;
        //                    break;
        //                case "Cobra Strike":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 658;
        //                    break;
        //                case "Shred":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 710;
        //                    break;
        //                case "Twin Turbo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 747;
        //                    break;
        //            }
        //            break;
        //        case 2:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Candy Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 3;
        //                    break;
        //                case "Case Hardened":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 44;
        //                    break;
        //                case "Contractor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 46;
        //                    break;
        //                case "Forest Night":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 78;
        //                    break;
        //                case "Orange Peel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 141;
        //                    break;
        //                case "Jungle":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 151;
        //                    break;
        //                case "Anodized Gunmetal":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 210;
        //                    break;
        //                case "Nightshade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 223;
        //                    break;
        //                case "Silver Quartz":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 252;
        //                    break;
        //                case "Nitro":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 254;
        //                    break;
        //                case "Kami":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 265;
        //                    break;
        //                case "Copper Galaxy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 274;
        //                    break;
        //                case "Fowl Play":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 352;
        //                    break;
        //                case "Urban Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 387;
        //                    break;
        //                case "Hot Shot":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 377;
        //                    break;
        //                case "Monkey Business":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 427;
        //                    break;
        //                case "Neon Kimono":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 464;
        //                    break;
        //                case "Retrobution":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 510;
        //                    break;
        //                case "Triumvirate":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 530;
        //                    break;
        //                case "Violent Daimyo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 585;
        //                    break;
        //                case "Scumbria":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 605;
        //                    break;
        //                case "Capillary":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 646;
        //                    break;
        //                case "Hyper Beast":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 660;
        //                    break;
        //                case "Flame Test":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 693;
        //                    break;
        //                case "Coolant":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 784;
        //                    break;
        //            }
        //            break;
        //        case 3:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Groundwater":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 2;
        //                    break;
        //                case "Candy Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 3;
        //                    break;
        //                case "Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 38;
        //                    break;
        //                case "Night":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 40;
        //                    break;
        //                case "Dragon Tattoo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 48;
        //                    break;
        //                case "Brass":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 159;
        //                    break;
        //                case "Sand Dune":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 208;
        //                    break;
        //                case "Steel Disruption":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 230;
        //                    break;
        //                case "Blue Fissure":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 278;
        //                    break;
        //                case "Death Rattle":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 293;
        //                    break;
        //                case "Water Elemental":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 353;
        //                    break;
        //                case "Reactor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 367;
        //                    break;
        //                case "Grinder":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 381;
        //                    break;
        //                case "Catacombs":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 399;
        //                    break;
        //                case "Twilight Galaxy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 437;
        //                    break;
        //                case "Bunsen Burner":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 479;
        //                    break;
        //                case "Wraiths":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 495;
        //                    break;
        //                case "Royal Legion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 532;
        //                    break;
        //                case "Wasteland Rebel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 586;
        //                    break;
        //                case "Weasel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 607;
        //                    break;
        //                case "Ironwork":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 623;
        //                    break;
        //                case "Off World":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 680;
        //                    break;
        //                case "Moonrise":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 694;
        //                    break;
        //                case "Warhawk":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 713;
        //                    break;
        //                case "Nuclear Garden":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 789;
        //                    break;
        //                case "High Beam":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 799;
        //                    break;
        //                case "Oxide Blaze":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 808;
        //                    break;
        //            }
        //            break;
        //        case 4:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Red Laminate":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 14;
        //                    break;
        //                case "Case hardened":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 44;
        //                    break;
        //                case "Safari Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 72;
        //                    break;
        //                case "Jungle Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 122;
        //                    break;
        //                case "Predator":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 170;
        //                    break;
        //                case "Black Laminate":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 172;
        //                    break;
        //                case "Fire Serpent":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 180;
        //                    break;
        //                case "Blue Laminate":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 226;
        //                    break;
        //                case "Redline":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 282;
        //                    break;
        //                case "Emerald Pinstripe":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 300;
        //                    break;
        //                case "Jaguar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 316;
        //                    break;
        //                case "Vulcan":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 302;
        //                    break;
        //                case "Jet Set":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 340;
        //                    break;
        //                case "First Class":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 341;
        //                    break;
        //                case "Wasteland Rebel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 380;
        //                    break;
        //                case "Cartel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 394;
        //                    break;
        //                case "Elite Build":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 422;
        //                    break;
        //                case "Hydroponic":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 456;
        //                    break;
        //                case "Aquamarine Revenge":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 474;
        //                    break;
        //                case "Frontside Misty":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 490;
        //                    break;
        //                case "Point Disarray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 506;
        //                    break;
        //                case "Fuel Injector":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 524;
        //                    break;
        //                case "Neon Revolution":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 600;
        //                    break;
        //                case "Bloodsport":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 639;
        //                    break;
        //                case "Orbit Mk01":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 656;
        //                    break;
        //                case "Crimson Web":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "The Empress":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 675;
        //                    break;
        //                case "Neon Rider":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 707;
        //                    break;
        //                case "Safety Net":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 795;
        //                    break;
        //                case "Asiimov":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 801;
        //                    break;
        //            }
        //            break;
        //        case 5:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Bengal Tiger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 9;
        //                    break;
        //                case "Copperhead":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 10;
        //                    break;
        //                case "Anodized Navy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 28;
        //                    break;
        //                case "Hot Rod":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 33;
        //                    break;
        //                case "Contractor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 46;
        //                    break;
        //                case "Colony":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 47;
        //                    break;
        //                case "Wings":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 73;
        //                    break;
        //                case "Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 100;
        //                    break;
        //                case "Condemned":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 110;
        //                    break;
        //                case "Radiation Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 167;
        //                    break;
        //                case "Chameleon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 280;
        //                    break;
        //                case "Torque":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 305;
        //                    break;
        //                case "Daedalus":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 444;
        //                    break;
        //                case "Akihabara Accept":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 455;
        //                    break;
        //                case "Ricochet":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 507;
        //                    break;
        //                case "Fleet Flock":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 541;
        //                    break;
        //                case "Aristocrat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 583;
        //                    break;
        //                case "Syd Mead":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 601;
        //                    break;
        //                case "Triqua":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 674;
        //                    break;
        //                case "Stymphalian":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 690;
        //                    break;
        //                case "Amber Slipstream":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 708;
        //                    break;
        //                case "Random Access":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 779;
        //                    break;
        //                case "Sweeper":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 794;
        //                    break;
        //            }
        //            break;
        //        case 6:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Snake Camo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 30;
        //                    break;
        //                case "Lightning Strike":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 51;
        //                    break;
        //                case "Safari Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 72;
        //                    break;
        //                case "Pink DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 84;
        //                    break;
        //                case "BOOM":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 174;
        //                    break;
        //                case "Corticera":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 181;
        //                    break;
        //                case "Graphite":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 212;
        //                    break;
        //                case "Electric Hive":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 227;
        //                    break;
        //                case "Pit Viper":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 251;
        //                    break;
        //                case "Redline":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 259;
        //                    break;
        //                case "Asiimov":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 279;
        //                    break;
        //                case "Dragon Lore":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 344;
        //                    break;
        //                case "Man-o'-war":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 395;
        //                    break;
        //                case "Worm God":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 424;
        //                    break;
        //                case "Medusa":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 446;
        //                    break;
        //                case "Sun in Leo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 451;
        //                    break;
        //                case "Hyper Beast":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 475;
        //                    break;
        //                case "Elite Build":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 525;
        //                    break;
        //                case "Phobos":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 584;
        //                    break;
        //                case "Fever Dream":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 640;
        //                    break;
        //                case "Oni Taiji":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 662;
        //                    break;
        //                case "Mortis":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 691;
        //                    break;
        //                case "PAW":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 718;
        //                    break;
        //                case "Acheron":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 788;
        //                    break;
        //                case "Neo-Noir":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 803;
        //                    break;
        //            }
        //            break;
        //        case 7:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Contrast Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 22;
        //                    break;
        //                case "Colony":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 47;
        //                    break;
        //                case "Cyanospatter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 92;
        //                    break;
        //                case "Afterimage":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 154;
        //                    break;
        //                case "Doomkitty":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 178;
        //                    break;
        //                case "Spitfire":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 194;
        //                    break;
        //                case "Hexane":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 218;
        //                    break;
        //                case "Teardown":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 244;
        //                    break;
        //                case "Pulse":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 260;
        //                    break;
        //                case "Sergeant":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 288;
        //                    break;
        //                case "Styx":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 371;
        //                    break;
        //                case "Djinn":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 429;
        //                    break;
        //                case "Neural Net":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 477;
        //                    break;
        //                case "Survivor Z":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 492;
        //                    break;
        //                case "Valence":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 529;
        //                    break;
        //                case "Roll Cage":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 604;
        //                    break;
        //                case "Mecha Industries":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 626;
        //                    break;
        //                case "Macabre":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 659;
        //                    break;
        //                case "Eye of Athena":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 723;
        //                    break;
        //            }
        //            break;
        //        case 8:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Arctic Camo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 6;
        //                    break;
        //                case "Desert Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 8;
        //                    break;
        //                case "Contractor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 46;
        //                    break;
        //                case "Safari Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 72;
        //                    break;
        //                case "Polar Camo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 74;
        //                    break;
        //                case "Jungle Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 147;
        //                    break;
        //                case "Demeter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 195;
        //                    break;
        //                case "Azure Zebra":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 229;
        //                    break;
        //                case "VariCamo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 235;
        //                    break;
        //                case "Green Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 294;
        //                    break;
        //                case "Murky":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 382;
        //                    break;
        //                case "Chronos":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 438;
        //                    break;
        //                case "Orange Kimono":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 465;
        //                    break;
        //                case "Flux":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 493;
        //                    break;
        //                case "Corinthian":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 509;
        //                    break;
        //                case "The Executioner":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 511;
        //                    break;
        //                case "Orange Crash":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 545;
        //                    break;
        //                case "Ventilator":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 606;
        //                    break;
        //                case "Stinger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 628;
        //                    break;
        //                case "Hunter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 677;
        //                    break;
        //                case "High Seas":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 712;
        //                    break;
        //                case "Scavenger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 806;
        //                    break;
        //            }
        //            break;
        //        case 9:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Winter Forest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 76;
        //                    break;
        //                case "Orange DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 83;
        //                    break;
        //                case "Sage Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 119;
        //                    break;
        //                case "Shattered":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 192;
        //                    break;
        //                case "Blue Titanium":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 216;
        //                    break;
        //                case "VariCamo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 235;
        //                    break;
        //                case "Urban Rubble":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 237;
        //                    break;
        //                case "Hunting Blind":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 241;
        //                    break;
        //                case "Sandstorm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 264;
        //                    break;
        //                case "Kami":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 265;
        //                    break;
        //                case "Tuxedo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 297;
        //                    break;
        //                case "Cerberus":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 379;
        //                    break;
        //                case "Chatterbox":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 398;
        //                    break;
        //                case "Eco":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 428;
        //                    break;
        //                case "Aqua Terrace":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 460;
        //                    break;
        //                case "Rocket Pop":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 478;
        //                    break;
        //                case "Srone Cold":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 494;
        //                    break;
        //                case "Firefight":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 546;
        //                    break;
        //                case "Black Sand":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 629;
        //                    break;
        //                case "Crimson Tsunami":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 647;
        //                    break;
        //                case "Sugar Rush":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 661;
        //                    break;
        //                case "Cold Fusion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 790;
        //                    break;
        //                case "Signal":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 807;
        //                    break;
        //            }
        //            break;
        //        case 10:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Desert Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 8;
        //                    break;
        //                case "Jungle Tiger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 16;
        //                    break;
        //                case "Urban DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 17;
        //                    break;
        //                case "Tornado":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 101;
        //                    break;
        //                case "Bullet Rain":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 155;
        //                    break;
        //                case "Modern Hunter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 164;
        //                    break;
        //                case "Radiation Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 167;
        //                    break;
        //                case "Faded Zebra":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 176;
        //                    break;
        //                case "Zirka":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 187;
        //                    break;
        //                case "X-Ray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 215;
        //                    break;
        //                case "Asiimov":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 255;
        //                    break;
        //                case "Howl":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 309;
        //                    break;
        //                case "Desert-Strike":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 336;
        //                    break;
        //                case "Griffin":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 384;
        //                    break;
        //                case "Dragon King":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 400;
        //                    break;
        //                case "Poseidon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 449;
        //                    break;
        //                case "Daybreak":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 471;
        //                    break;
        //                case "Evil Daimyo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 480;
        //                    break;
        //                case "Royal Paladin":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 512;
        //                    break;
        //                case "The Battlestar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 533;
        //                    break;
        //                case "Desolate Space":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 588;
        //                    break;
        //                case "Buzz Kill":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 632;
        //                    break;
        //                case "Hellfire":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 664;
        //                    break;
        //                case "Neo-Noir":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 653;
        //                    break;
        //                case "Mainframe":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 780;
        //                    break;
        //                case "Converter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 793;
        //                    break;
        //                case "Magnesium":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 811;
        //                    break;
        //            }
        //            break;
        //        case 11:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Candy Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 3;
        //                    break;
        //                case "Urban DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 17;
        //                    break;
        //                case "Silver":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 32;
        //                    break;
        //                case "Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 38;
        //                    break;
        //                case "Ultraviolet":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 98;
        //                    break;
        //                case "Tornado":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 101;
        //                    break;
        //                case "Palm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 157;
        //                    break;
        //                case "Graven":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 188;
        //                    break;
        //                case "Amber Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 246;
        //                    break;
        //                case "Heat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 284;
        //                    break;
        //                case "Curse":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 310;
        //                    break;
        //                case "Indigo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 333;
        //                    break;
        //                case "Tatter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 337;
        //                    break;
        //                case "Commuter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 343;
        //                    break;
        //                case "Nuclear Garden":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 372;
        //                    break;
        //                case "Malachite":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 402;
        //                    break;
        //                case "Neon Rider":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 433;
        //                    break;
        //                case "Rangeen":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 498;
        //                    break;
        //                case "Lapis Gator":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 534;
        //                    break;
        //                case "Carnivore":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 589;
        //                    break;
        //                case "Last Dive":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 651;
        //                    break;
        //                case "Aloha":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 665;
        //                    break;
        //                case "Oceanic":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 550;
        //                    break;
        //                case "Calf Skin":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 748;
        //                    break;
        //                case "Pipe Down":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 812;
        //                    break;
        //            }
        //            break;
        //        case 12:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Virus":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 20;
        //                    break;
        //                case "Cold Blooded":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 67;
        //                    break;
        //                case "Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 100;
        //                    break;
        //                case "Glacier Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 111;
        //                    break;
        //                case "Sand Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 124;
        //                    break;
        //                case "Death by Kitty":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 156;
        //                    break;
        //                case "Fallout Warning":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 169;
        //                    break;
        //                case "Scorched":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 175;
        //                    break;
        //                case "Emerald Dragon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 182;
        //                    break;
        //                case "Blind Spot":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 228;
        //                    break;
        //                case "Ash Wood":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 234;
        //                    break;
        //                case "Teardown":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 244;
        //                    break;
        //                case "Trigon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 283;
        //                    break;
        //                case "Desert Warfare":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 311;
        //                    break;
        //                case "Module":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 335;
        //                    break;
        //                case "Leather":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 342;
        //                    break;
        //                case "Asiimov":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 359;
        //                    break;
        //                case "Elite Build":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 486;
        //                    break;
        //                case "Shapewood":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 516;
        //                    break;
        //                case "Chopper":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 593;
        //                    break;
        //                case "Grim":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 611;
        //                    break;
        //                case "Shallow Grave":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 636;
        //                    break;
        //                case "Death Grip":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 669;
        //                    break;
        //                case "Traction":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 717;
        //                    break;
        //                case "Facility Negative":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 776;
        //                    break;
        //            }
        //            break;
        //        case 13:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Gunsmoke":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 15;
        //                    break;
        //                case "Urban DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 17;
        //                    break;
        //                case "Blaze":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 37;
        //                    break;
        //                case "Carbon Fiber":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 70;
        //                    break;
        //                case "Caramel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 93;
        //                    break;
        //                case "Fallout Warning":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 169;
        //                    break;
        //                case "Scorched":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 175;
        //                    break;
        //                case "Bone Pile":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 193;
        //                    break;
        //                case "Corporal":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 281;
        //                    break;
        //                case "Indigo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 333;
        //                    break;
        //                case "Labyrinth":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 362;
        //                    break;
        //                case "Delusion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 392;
        //                    break;
        //                case "Grand Prix":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 436;
        //                    break;
        //                case "Minotaur's Labyrinth":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 441;
        //                    break;
        //                case "Riot":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 488;
        //                    break;
        //                case "Primal Saber":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 556;
        //                    break;
        //                case "Briefing":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 615;
        //                    break;
        //                case "Scaffold":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 652;
        //                    break;
        //                case "Metal Flowers":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 672;
        //                    break;
        //                case "Exposure":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 688;
        //                    break;
        //                case "Arctic Wolf":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 704;
        //                    break;
        //                case "Facility Dark":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 778;
        //                    break;
        //                case "Momentum":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 802;
        //                    break;
        //            }
        //            break;
        //        case 14:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Blue Steel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 42;
        //                    break;
        //                case "Grassland":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 95;
        //                    break;
        //                case "Blue Spruce":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 96;
        //                    break;
        //                case "Urban Perforated":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 135;
        //                    break;
        //                case "Jungle":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 151;
        //                    break;
        //                case "Blaze Orange":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 166;
        //                    break;
        //                case "Fallout Warning":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 169;
        //                    break;
        //                case "VariCamo Blue":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 238;
        //                    break;
        //                case "CaliCamo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 240;
        //                    break;
        //                case "Heaven Guard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 314;
        //                    break;
        //                case "Red Python":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 320;
        //                    break;
        //                case "Red Leather":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 348;
        //                    break;
        //                case "Bone Machine":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 370;
        //                    break;
        //                case "Tranquility":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 393;
        //                    break;
        //                case "Quicksilver":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 407;
        //                    break;
        //                case "Scumbria":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 505;
        //                    break;
        //                case "Teclu Burner":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 521;
        //                    break;
        //                case "Black Tie":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 557;
        //                    break;
        //                case "Slipstream":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 616;
        //                    break;
        //                case "Seasons":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 654;
        //                    break;
        //                case "Ziggy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 689;
        //                    break;
        //                case "Oxide Blaze":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 706;
        //                    break;
        //            }
        //            break;
        //        case 15:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Blue Streak":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 13;
        //                    break;
        //                case "Forest Leaves":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 25;
        //                    break;
        //                case "Carbon Fiber":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 70;
        //                    break;
        //                case "Jungle Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 147;
        //                    break;
        //                case "Urban Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 149;
        //                    break;
        //                case "Sand Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 148;
        //                    break;
        //                case "Brass":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 159;
        //                    break;
        //                case "Modern Hunter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 164;
        //                    break;
        //                case "Irradiated Alert":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 171;
        //                    break;
        //                case "Rust Coat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 203;
        //                    break;
        //                case "Water Sigil":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 224;
        //                    break;
        //                case "Night Ops":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 236;
        //                    break;
        //                case "Cobalt Halftone":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 267;
        //                    break;
        //                case "Antique":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 306;
        //                    break;
        //                case "Osiris":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 349;
        //                    break;
        //                case "Chemical Green":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 376;
        //                    break;
        //                case "Bamboo Print":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 457;
        //                    break;
        //                case "Fuel Rod":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 508;
        //                    break;
        //                case "Phonic Zone":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 526;
        //                    break;
        //                case "Judgement of Anubis":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 542;
        //                    break;
        //                case "Harvester":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 594;
        //                    break;
        //                case "Jungle Slipstream":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 641;
        //                    break;
        //                case "High Roller":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 676;
        //                    break;
        //                case "Night Riot":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 692;
        //                    break;
        //                case "Facility":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 775;
        //                    break;
        //            }
        //            break;
        //        case 16:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Silver":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 32;
        //                    break;
        //                case "Metallic DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 34;
        //                    break;
        //                case "Bulldozer":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 39;
        //                    break;
        //                case "Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 100;
        //                    break;
        //                case "Irradiated Alert":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 171;
        //                    break;
        //                case "Memento":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 177;
        //                    break;
        //                case "Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 198;
        //                    break;
        //                case "Sand Dune":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 208;
        //                    break;
        //                case "Heaven Guard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 291;
        //                    break;
        //                case "Firestarter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 385;
        //                    break;
        //                case "Heat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 431;
        //                    break;
        //                case "Counter Terrace":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 462;
        //                    break;
        //                case "Seabird":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 473;
        //                    break;
        //                case "Cobalt Core":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 499;
        //                    break;
        //                case "Praetorian":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 535;
        //                    break;
        //                case "Petroglyph":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 608;
        //                    break;
        //                case "Sonar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 633;
        //                    break;
        //                case "Hard Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 666;
        //                    break;
        //                case "SWAG-7":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 703;
        //                    break;
        //                case "Rust Coat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 754;
        //                    break;
        //                case "Core Breach":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 787;
        //                    break;
        //            }
        //            break;
        //        case 17:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Forest DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 5;
        //                    break;
        //                case "Snake Camo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 30;
        //                    break;
        //                case "Copper":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 41;
        //                    break;
        //                case "Orange DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 83;
        //                    break;
        //                case "Sage Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 119;
        //                    break;
        //                case "Irradiated Alert":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 171;
        //                    break;
        //                case "Rust Coat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 203;
        //                    break;
        //                case "Mosaico":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 204;
        //                    break;
        //                case "Amber Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 246;
        //                    break;
        //                case "Full Stop":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 250;
        //                    break;
        //                case "The Kraken":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 256;
        //                    break;
        //                case "First Class":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 345;
        //                    break;
        //                case "Highwayman":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 390;
        //                    break;
        //                case "Serenity":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 405;
        //                    break;
        //                case "Origami":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 434;
        //                    break;
        //                case "Bamboo Shadow":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 458;
        //                    break;
        //                case "Yorick":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 517;
        //                    break;
        //                case "Fubar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 552;
        //                    break;
        //                case "Limelight":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 596;
        //                    break;
        //                case "Wasteland Princess":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 638;
        //                    break;
        //                case "Zander":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 655;
        //                    break;
        //                case "Morris":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 673;
        //                    break;
        //                case "Devourer":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 720;
        //                    break;
        //                case "Brake Light":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 797;
        //                    break;
        //                case "Black Sand":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 814;
        //                    break;
        //            }
        //            break;
        //        case 18:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Urban DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 17;
        //                    break;
        //                case "Ossified":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 36;
        //                    break;
        //                case "Tornado":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 101;
        //                    break;
        //                case "Brass":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 159;
        //                    break;
        //                case "Nuclear Threat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 179;
        //                    break;
        //                case "Groundwater":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 209;
        //                    break;
        //                case "Blue Titanium":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 216;
        //                    break;
        //                case "VariCamo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 235;
        //                    break;
        //                case "Army Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 242;
        //                    break;
        //                case "Red Quartz":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 248;
        //                    break;
        //                case "Sandstorm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 264;
        //                    break;
        //                case "Titanium Bit":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 272;
        //                    break;
        //                case "Isaac":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 303;
        //                    break;
        //                case "Toxic":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 374;
        //                    break;
        //                case "Hades":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 439;
        //                    break;
        //                case "Bamboo Forest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 459;
        //                    break;
        //                case "Terrace":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 463;
        //                    break;
        //                case "Avalanche":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 520;
        //                    break;
        //                case "Jambiya":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 539;
        //                    break;
        //                case "Re-Entry":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 555;
        //                    break;
        //                case "Ice Cap":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 599;
        //                    break;
        //                case "Fuel Injector":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 614;
        //                    break;
        //                case "Cut Out":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 671;
        //                    break;
        //                case "Cracked Opal":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 684;
        //                    break;
        //                case "Snek-9":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 722;
        //                    break;
        //                case "Remote Control":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 791;
        //                    break;
        //                case "Fubar":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 816;
        //                    break;
        //            }
        //            break;
        //        case 19:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Granite Marbleized":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 21;
        //                    break;
        //                case "Silver":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 32;
        //                    break;
        //                case "Scorpion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 71;
        //                    break;
        //                case "Grassland":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 95;
        //                    break;
        //                case "Grassland Leaves":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 104;
        //                    break;
        //                case "Corticera":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 184;
        //                    break;
        //                case "Ocean Foam":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 211;
        //                    break;
        //                case "Amber Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 246;
        //                    break;
        //                case "Red FragCam":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 275;
        //                    break;
        //                case "Pulse":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 287;
        //                    break;
        //                case "Chainmail":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 327;
        //                    break;
        //                case "Coach Class":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 346;
        //                    break;
        //                case "Ivory":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 357;
        //                    break;
        //                case "Fire Elemental":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 389;
        //                    break;
        //                case "Pathfinder":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 443;
        //                    break;
        //                case "Handgun":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 485;
        //                    break;
        //                case "Imperial":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 515;
        //                    break;
        //                case "Oceanic":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 550;
        //                    break;
        //                case "Imperial Dragon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 591;
        //                    break;
        //                case "Turf":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 635;
        //                    break;
        //                case "Woodsman":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 667;
        //                    break;
        //                case "Urban Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 700;
        //                    break;
        //            }
        //            break;
        //        case 20:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Forest DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 5;
        //                    break;
        //                case "Skulls":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 11;
        //                    break;
        //                case "Gunsmoke":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 15;
        //                    break;
        //                case "Anodized Navy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 28;
        //                    break;
        //                case "Whiteout":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 102;
        //                    break;
        //                case "Orange Peel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 141;
        //                    break;
        //                case "Groundwater":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 209;
        //                    break;
        //                case "Ocean Foam":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 213;
        //                    break;
        //                case "Army Recon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 245;
        //                    break;
        //                case "Full Stop":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 250;
        //                    break;
        //                case "Urban Hazard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 354;
        //                    break;
        //                case "Olive Plaid":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 365;
        //                    break;
        //                case "Armor Core":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 423;
        //                    break;
        //                case "Asterion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 442;
        //                    break;
        //                case "Nemesis":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 481;
        //                    break;
        //                case "Special Delivery":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 500;
        //                    break;
        //                case "Impire":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 536;
        //                    break;
        //                case "Cirrus":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 627;
        //                    break;
        //                case "Akoben":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 649;
        //                    break;
        //                case "Bloodsport":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 696;
        //                    break;
        //                case "Powercore":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 719;
        //                    break;
        //                case "Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 752;
        //                    break;
        //                case "Motherboard":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 782;
        //                    break;
        //            }
        //            break;
        //        case 21:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Hot Rod":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 33;
        //                    break;
        //                case "Bulldozer":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 39;
        //                    break;
        //                case "Hypnotic":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 61;
        //                    break;
        //                case "Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 100;
        //                    break;
        //                case "Orange Peel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 141;
        //                    break;
        //                case "Sand Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 148;
        //                    break;
        //                case "Dry Season":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 199;
        //                    break;
        //                case "Rose Iron":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 262;
        //                    break;
        //                case "Dark Age":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 329;
        //                    break;
        //                case "Green Plaid":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 366;
        //                    break;
        //                case "Setting Sun":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 368;
        //                    break;
        //                case "Dart":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 386;
        //                    break;
        //                case "Deadly Poison":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 403;
        //                    break;
        //                case "Pandora's Box":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 448;
        //                    break;
        //                case "Ruby Poison Dart":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 482;
        //                    break;
        //                case "Bioleak":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 549;
        //                    break;
        //                case "Airlock":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 609;
        //                    break;
        //                case "Sand Scale":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 630;
        //                    break;
        //                case "Goo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 679;
        //                    break;
        //                case "Black Sand":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 697;
        //                    break;
        //                case "Capillary":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 715;
        //                    break;
        //                case "Slide":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 755;
        //                    break;
        //                case "Modest Threat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 804;
        //                    break;
        //            }
        //            break;
        //        case 22:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Candy Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 3;
        //                    break;
        //                case "Forest Leaves":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 25;
        //                    break;
        //                case "Bloomstick":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 62;
        //                    break;
        //                case "Polar Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 107;
        //                    break;
        //                case "Walnut":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 158;
        //                    break;
        //                case "Modern Hunter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 164;
        //                    break;
        //                case "Blaze Orange":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 166;
        //                    break;
        //                case "Predator":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 170;
        //                    break;
        //                case "Tempest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 191;
        //                    break;
        //                case "Sand Dune":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 208;
        //                    break;
        //                case "Graphite":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 214;
        //                    break;
        //                case "Ghost Camo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 225;
        //                    break;
        //                case "Rising Skull":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 263;
        //                    break;
        //                case "Antique":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 286;
        //                    break;
        //                case "Green Apple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 294;
        //                    break;
        //                case "Caged Steel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 299;
        //                    break;
        //                case "Koi":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 356;
        //                    break;
        //                case "Moon in Libra":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 450;
        //                    break;
        //                case "Ranger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 484;
        //                    break;
        //                case "Hyper Beast":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 537;
        //                    break;
        //                case "Exo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 590;
        //                    break;
        //                case "Gila":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 634;
        //                    break;
        //                case "Wild Six":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 699;
        //                    break;
        //                case "Toy Soldier":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 716;
        //                    break;
        //                case "Mandrel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 785;
        //                    break;
        //                case "Wood Fired":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 809;
        //                    break;
        //            }
        //            break;
        //        case 23:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Gunsmoke":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 15;
        //                    break;
        //                case "Bone Mask":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 27;
        //                    break;
        //                case "Metallic DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 34;
        //                    break;
        //                case "Boreal Forest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 77;
        //                    break;
        //                case "Splash":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 162;
        //                    break;
        //                case "Modern Hunter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 164;
        //                    break;
        //                case "Nuclear Threat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 168;
        //                    break;
        //                case "Facets":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 207;
        //                    break;
        //                case "Sand Dune":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 208;
        //                    break;
        //                case "Hive":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 219;
        //                    break;
        //                case "Steel Disruption":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 230;
        //                    break;
        //                case "Whiteout":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 102;
        //                    break;
        //                case "Mehndi":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 258;
        //                    break;
        //                case "Undertow":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 271;
        //                    break;
        //                case "Franklin":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 295;
        //                    break;
        //                case "Supernova":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 358;
        //                    break;
        //                case "Contamination":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 373;
        //                    break;
        //                case "Cartel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 388;
        //                    break;
        //                case "Muertos":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 404;
        //                    break;
        //                case "Valence":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 426;
        //                    break;
        //                case "Neon Kimono":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 464;
        //                    break;
        //                case "Crimson Kimono":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 466;
        //                    break;
        //                case "Mint Kimono":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 467;
        //                    break;
        //                case "Asiimov":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 551;
        //                    break;
        //                case "Iron Clad":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 592;
        //                    break;
        //                case "Ripple":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 650;
        //                    break;
        //                case "Red Rock":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 668;
        //                    break;
        //                case "See Ya Later":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 678;
        //                    break;
        //                case "Vino Primo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 749;
        //                    break;
        //                case "Facility Draft":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 777;
        //                    break;
        //                case "Exchanger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 786;
        //                    break;
        //                case "Nevermore":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 813;
        //                    break;
        //            }
        //            break;
        //        case 24:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Crimson Web":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "Contractor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 46;
        //                    break;
        //                case "Carbon Fiber":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 70;
        //                    break;
        //                case "Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 100;
        //                    break;
        //                case "Sand Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 116;
        //                    break;
        //                case "Palm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 157;
        //                    break;
        //                case "Splash Jam":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 165;
        //                    break;
        //                case "Emerald":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 196;
        //                    break;
        //                case "Army Sheen":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 298;
        //                    break;
        //                case "Cyrex":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 312;
        //                    break;
        //                case "Cardiac":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 391;
        //                    break;
        //                case "Grotto":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 406;
        //                    break;
        //                case "Green Marine":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 502;
        //                    break;
        //                case "Outbreak":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 518;
        //                    break;
        //                case "Bloodsport":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 597;
        //                    break;
        //                case "Powercore":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 612;
        //                    break;
        //                case "Blueprint":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 642;
        //                    break;
        //                case "Jungle Slipstream":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 641;
        //                    break;
        //            }
        //            break;
        //        case 25:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Anodized Navy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 28;
        //                    break;
        //                case "Bulldozer":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 39;
        //                    break;
        //                case "Ultraviolet":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 98;
        //                    break;
        //                case "Tornado":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 101;
        //                    break;
        //                case "Waves Perforated":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 136;
        //                    break;
        //                case "Fallout Warning":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 378;
        //                    break;
        //                case "Wave Spray":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 186;
        //                    break;
        //                case "Gator Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 243;
        //                    break;
        //                case "Damascus Steel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 247;
        //                    break;
        //                case "Pulse":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 287;
        //                    break;
        //                case "Army Sheen":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 298;
        //                    break;
        //                case "Traveler":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 363;
        //                    break;
        //                case "Cyrex":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 487;
        //                    break;
        //                case "Tiger Moth":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 519;
        //                    break;
        //                case "Atlas":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 553;
        //                    break;
        //                case "Aerial":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 598;
        //                    break;
        //                case "Triarch":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 613;
        //                    break;
        //                case "Phantom":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 686;
        //                    break;
        //                case "Aloha":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 702;
        //                    break;
        //                case "Integrale":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 750;
        //                    break;
        //                case "Danger Close":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 815;
        //                    break;
        //            }
        //            break;
        //        case 26:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Lichen Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 26;
        //                    break;
        //                case "Dark Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 60;
        //                    break;
        //                case "Blue Spruce":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 96;
        //                    break;
        //                case "Splashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 162;
        //                    break;
        //                case "Mayan Dreams":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 200;
        //                    break;
        //                case "Sand Dune":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 208;
        //                    break;
        //                case "Blood in the Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 222;
        //                    break;
        //                case "Tropical Storm":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 233;
        //                    break;
        //                case "Acid Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 253;
        //                    break;
        //                case "Detour":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 319;
        //                    break;
        //                case "Abyss":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 361;
        //                    break;
        //                case "Big Iron":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 503;
        //                    break;
        //                case "Necropos":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 538;
        //                    break;
        //                case "Ghost Crusader":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 554;
        //                    break;
        //                case "Dragonfire":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 624;
        //                    break;
        //                case "Death's Head":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 670;
        //                    break;
        //                case "Hand Brake":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 751;
        //                    break;
        //            }
        //            break;
        //        case 27:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Dark Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 60;
        //                    break;
        //                case "Boreal Forest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 77;
        //                    break;
        //                case "Bright Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 189;
        //                    break;
        //                case "Blood Tiger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 217;
        //                    break;
        //                case "VariCamo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 235;
        //                    break;
        //                case "Nitro":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 254;
        //                    break;
        //                case "Guardian":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 257;
        //                    break;
        //                case "Atomic Alloy":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 301;
        //                    break;
        //                case "Master Piece":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 321;
        //                    break;
        //                case "Knight":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 326;
        //                    break;
        //                case "Cyrex":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 360;
        //                    break;
        //                case "Basilisk":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 383;
        //                    break;
        //                case "Hyper Beast":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 430;
        //                    break;
        //                case "Icarus Fell":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 440;
        //                    break;
        //                case "Hot Rod":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 445;
        //                    break;
        //                case "Golden Coil":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 497;
        //                    break;
        //                case "Chantico's Fire":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 548;
        //                    break;
        //                case "Mecha Industries":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 587;
        //                    break;
        //                case "Flashback":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 631;
        //                    break;
        //                case "Decimator":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 644;
        //                    break;
        //                case "Briefing":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 615;
        //                    break;
        //                case "Leaded Glass":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 681;
        //                    break;
        //                case "Nightmare":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 714;
        //                    break;
        //                case "Control Panel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 792;
        //                    break;
        //            }
        //            break;
        //        case 28:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Forest Leaves":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 25;
        //                    break;
        //                case "Dark Water":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 60;
        //                    break;
        //                case "Overgrowth":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 183;
        //                    break;
        //                case "Blood Tiger":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 217;
        //                    break;
        //                case "Serum":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 221;
        //                    break;
        //                case "Night Ops":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 236;
        //                    break;
        //                case "Guardian":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 290;
        //                    break;
        //                case "Stainless":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 277;
        //                    break;
        //                case "Orion":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 313;
        //                    break;
        //                case "Road Rash":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 318;
        //                    break;
        //                case "Royal Blue":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 332;
        //                    break;
        //                case "Caiman":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 339;
        //                    break;
        //                case "Business Class":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 364;
        //                    break;
        //                case "Para Green":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 454;
        //                    break;
        //                case "Torque":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 489;
        //                    break;
        //                case "Kill Confirmed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 504;
        //                    break;
        //                case "Lead Conduit":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 540;
        //                    break;
        //                case "Cyrex":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 637;
        //                    break;
        //                case "Neo-Noir":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 653;
        //                    break;
        //                case "Blueprint":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 657;
        //                    break;
        //                case "Cortex":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 705;
        //                    break;
        //                case "Check Engine":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 796;
        //                    break;
        //                case "Flashback":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 817;
        //                    break;
        //            }
        //            break;
        //        case 29:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Crimson Web":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "Hexane":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 218;
        //                    break;
        //                case "Nitro":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 322;
        //                    break;
        //                case "Tread Plate":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 268;
        //                    break;
        //                case "The Fuschia Is Now":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 269;
        //                    break;
        //                case "Victoria":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 270;
        //                    break;
        //                case "Tuxedo":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 297;
        //                    break;
        //                case "Army Sheen":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 298;
        //                    break;
        //                case "Poison Dart":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 315;
        //                    break;
        //                case "Chalice":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 325;
        //                    break;
        //                case "Twist":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 334;
        //                    break;
        //                case "Tigris":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 350;
        //                    break;
        //                case "Green Plaid":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 366;
        //                    break;
        //                case "Pole Position":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 435;
        //                    break;
        //                case "Emerald":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 453;
        //                    break;
        //                case "Yellow Jacket":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 476;
        //                    break;
        //                case "Red Astor":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 543;
        //                    break;
        //                case "Imprint":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 602;
        //                    break;
        //                case "Polymer":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 622;
        //                    break;
        //                case "Xiangliu":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 643;
        //                    break;
        //                case "Tacticat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 687;
        //                    break;
        //                case "Eco":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 709;
        //                    break;
        //            }
        //            break;
        //        case 30:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Crimson Web":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "Bone Mask":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 27;
        //                    break;
        //                case "Urban Perforated":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 135;
        //                    break;
        //                case "Waves Perforated":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 136;
        //                    break;
        //                case "Orange Peel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 141;
        //                    break;
        //                case "Urban Masked":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 143;
        //                    break;
        //                case "Jungle Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 147;
        //                    break;
        //                case "Sand Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 148;
        //                    break;
        //                case "Urban Dashed":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 149;
        //                    break;
        //                case "Dry Season":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 199;
        //                    break;
        //                case "Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 522;
        //                    break;
        //                case "Amber Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 523;
        //                    break;
        //                case "Reboot":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 595;
        //                    break;
        //                case "Llama Cannon":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 683;
        //                    break;
        //                case "Grip":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 701;
        //                    break;
        //                case "Survivalist":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 721;
        //                    break;
        //                case "Nitro":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 798;
        //                    break;
        //            }
        //            break;
        //        case 31:
        //            switch (SkinChangerSkinComboBox.Text)
        //            {
        //                case "Forest DDPAT":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 5;
        //                    break;
        //                case "Crimson Web":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 12;
        //                    break;
        //                case "Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 38;
        //                    break;
        //                case "Night":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 40;
        //                    break;
        //                case "Blue Steel":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 42;
        //                    break;
        //                case "Stained":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 43;
        //                    break;
        //                case "Case Hardened":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 44;
        //                    break;
        //                case "Slaughter":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 59;
        //                    break;
        //                case "Safari Mesh":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 72;
        //                    break;
        //                case "Boreal Forest":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 77;
        //                    break;
        //                case "Ultraviolet":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 98;
        //                    break;
        //                case "Urban Masked":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 143;
        //                    break;
        //                case "Scorched":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 175;
        //                    break;
        //                case "Tiger Tooth":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 409;
        //                    break;
        //                case "Marble Fade":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 413;
        //                    break;
        //                case "Rust Coat":
        //                    ConfigManager.CSkinChangerWeapon[AimWeaponComboBox.SelectedIndex].SkinID = 414;
        //                    break;
        //            }
        //            break;
        //    }
        //}

        //private void SkinChangerSkinComboBox_TextUpdate(object sender, EventArgs e)
        //{
        //    if (int.TryParse(SkinChangerSkinComboBox.Text, out int Result))
        //    {
        //        ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinID = Result;
        //    }
        //}

        //private void SkinChangerWearTrackBar_Scroll(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].SkinWear = SkinChangerWearTrackBar.Value * 0.01f;

        //private void SkinChangerWeaponNameTextBox_TextChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].WeaponName = SkinChangerWeaponNameTextBox.Text;

        //private void SkinChangerKnifeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //switch (SkinChangerKnifeComboBox.SelectedIndex)
        //    //{
        //    //    case 0:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_BAYONET;
        //    //        break;
        //    //    case 1:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_SURVIVAL_BOWIE;
        //    //        break;
        //    //    case 2:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_BUTTERFLY;
        //    //        break;
        //    //    case 3:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_FALCHION;
        //    //        break;
        //    //    case 4:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_FLIP;
        //    //        break;
        //    //    case 5:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_GUT;
        //    //        break;
        //    //    case 6:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_TACTICAL;
        //    //        break;
        //    //    case 7:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_KARAMBIT;
        //    //        break;
        //    //    case 8:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_M9_BAYONET;
        //    //        break;
        //    //    case 9:
        //    //        ConfigManager.CSkinChangerKnife.WeaponID = WeaponID.WEAPON_KNIFE_PUSH;
        //    //        break;
        //    //}

        //    //SkinChangerKnifeSkinComboBox.Text = ConfigManager.CSkinChangerKnife.SkinID.ToString();

        //    //CEngine.DeltaTick = -1;
        //}

        //private void SkinChangerKnifeSkinComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}

        //private void SkinChangerKnifeSkinComboBox_TextUpdate(object sender, EventArgs e)
        //{
        //    if (int.TryParse(SkinChangerKnifeSkinComboBox.Text, out int Result))
        //    {
        //        //ConfigManager.CSkinChangerKnife.SkinID = Result;
        //    }
        //}
        //#endregion

        //#region Quality
        //private void SkinChangerNormalQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Normal;

        //private void SkinChangerGenuineQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Genuine;

        //private void SkinChangerVintageQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Vintage;

        //private void SkinChangerUnusualQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Unusual;

        //private void SkinChangerCommunityQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Community;

        //private void SkinChangerDeveloperQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Developer;

        //private void SkinChangerSelfMadeQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.SelfMade;

        //private void SkinChangerCustomizedQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Customized;

        //private void SkinChangerStrangeQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Strange;

        //private void SkinChangerCompletedQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Completed;

        //private void SkinChangerTournamentQualityRadioButton_CheckedChanged(object sender, EventArgs e) => ConfigManager.CSkinChangerWeapon[SkinChangerWeaponComboBox.SelectedIndex].QualityID = QualityID.Tournament;
        //#endregion

        #endregion

        #region Misc
        private void BunnyHopActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CMisc.BunnyHop = BunnyHopActive.Checked;

        private void NoFlashActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CMisc.NoFlash = NoFlashActive.Checked;
        #endregion
    }
}