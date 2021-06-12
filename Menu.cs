using System;
using System.Drawing;
using System.Windows.Forms;
using Wolf_Hack.Client.Config;
using Wolf_Hack.Module;
using Wolf_Hack.Module.Aim;
using Wolf_Hack.SDK.Interfaces;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;
using Wolf_Hack.SDK.Interfaces.Engine;
using Wolf_Hack.SDK.WinAPI;

namespace Wolf_Hack
{
    internal unsafe partial class Menu : Form
    {
        #region Menu
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            CheckProcessTimer.Start();

            ModuleManager.Run();

            #region VisualESP
            ConfigManager.CVisualGlowObjectManager = ConfigManager.LoadConfig<CVisualGlowObjectManager>();
            ConfigManager.CVisualChamsColor = ConfigManager.LoadConfig<CVisualChamsColor>();
            ConfigManager.CVisualMisc = ConfigManager.LoadConfig<CVisualMisc>();

            #region Glow
            GlowActive.Checked = ConfigManager.CVisualGlowObjectManager.GlowActive;
            GlowHPActive.Checked = ConfigManager.CVisualGlowObjectManager.GlowHPActive;
            GlowFullBloomActive.Checked = ConfigManager.CVisualGlowObjectManager.FullBloom;

            GlowEnemyColorPanel.BackColor = Color.FromArgb(ConfigManager.CVisualGlowObjectManager.Alpha, ConfigManager.CVisualGlowObjectManager.Red, ConfigManager.CVisualGlowObjectManager.Green, ConfigManager.CVisualGlowObjectManager.Blue);
            #endregion

            #region Chams
            ChamsActive.Checked = ConfigManager.CVisualChamsColor.ChamsActive;
            ChamsHPActive.Checked = ConfigManager.CVisualChamsColor.ChamsHPActive;

            ChamsEnemyColorPanel.BackColor = Color.FromArgb(ConfigManager.CVisualChamsColor.Red, ConfigManager.CVisualChamsColor.Green, ConfigManager.CVisualChamsColor.Blue);
            #endregion

            #region Misc
            VisualMiscRadarActive.Checked = ConfigManager.CVisualMisc.RadarActive;
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
            #endregion

            #endregion

            #region Misc
            ConfigManager.CMisc = ConfigManager.LoadConfig<CMisc>();

            BunnyHopActive.Checked = ConfigManager.CMisc.BunnyHop;
            NoFlashActive.Checked = ConfigManager.CMisc.NoFlash;
            #endregion
        }

        private void CheckProcessTimer_Tick(object sender, EventArgs e)
        {
            if (Base.process.HasExited)
            {
                ConfigManager.SaveConfig();

                ModuleManager.Abort();

                Environment.Exit(0);
            }

            int weaponId = Legit.GetWeaponID(BaseWeapon.ActiveWeaponIndex);

            if (weaponId != 34)
            {
                AimWeaponComboBox.SelectedIndex = weaponId;
            }
        }
        #endregion

        #region TopPanel
        private void ExitLabel_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveConfig();

            ModuleManager.Abort();

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
                ConfigManager.CVisualGlowObjectManager.Alpha = EnemyColorDialog.Color.A;

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

        #region Misc
        private void VisualMiscRadarActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CVisualMisc.RadarActive = VisualMiscRadarActive.Checked;

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
        #endregion

        #endregion

        #region Misc
        private void BunnyHopActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CMisc.BunnyHop = BunnyHopActive.Checked;

        private void NoFlashActive_CheckedChanged(object sender, EventArgs e) => ConfigManager.CMisc.NoFlash = NoFlashActive.Checked;
        #endregion
    }
}