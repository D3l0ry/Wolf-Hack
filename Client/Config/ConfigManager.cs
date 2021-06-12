using System;
using System.IO;
using System.Xml.Serialization;

using Wolf_Hack.SDK.Interfaces.Enums;

namespace Wolf_Hack.Client.Config
{
    internal class ConfigManager
    {
        private static string ConfigPath<T>() => $@"Config\{typeof(T).Name}.xml";

        private static string ConfigPath(object ObjectSave) => $@"Config\{ObjectSave.GetType().Name}.xml";

        #region Visual
        public static CVisualGlowObjectManager CVisualGlowObjectManager = new CVisualGlowObjectManager()
        {
            Red = 255,
            Alpha = 255
        };
        public static CVisualChamsColor CVisualChamsColor = new CVisualChamsColor()
        {
            Red = 255
        };
        public static CVisualMisc CVisualMisc = new CVisualMisc();
        #endregion

        #region Aim
        public static CAim CAim = new CAim();
        public static CAimWeapon[] CAimWeapon = new CAimWeapon[35]
         {
        new CAimWeapon(WeaponID.WEAPON_HKP2000),
        new CAimWeapon(WeaponID.WEAPON_GLOCK),
        new CAimWeapon(WeaponID.WEAPON_USP_SILENCER),
        new CAimWeapon(WeaponID.WEAPON_ELITE),
        new CAimWeapon(WeaponID.WEAPON_P250),
        new CAimWeapon(WeaponID.WEAPON_FIVESEVEN),
        new CAimWeapon(WeaponID.WEAPON_TEC9),
        new CAimWeapon(WeaponID.WEAPON_CZ75A),
        new CAimWeapon(WeaponID.WEAPON_DEAGLE),
        new CAimWeapon(WeaponID.WEAPON_REVOLVER),
        new CAimWeapon(WeaponID.WEAPON_NOVA),
        new CAimWeapon(WeaponID.WEAPON_XM1014),
        new CAimWeapon(WeaponID.WEAPON_MAG7),
        new CAimWeapon(WeaponID.WEAPON_SAWEDOFF),
        new CAimWeapon(WeaponID.WEAPON_M249),
        new CAimWeapon(WeaponID.WEAPON_NEGEV),
        new CAimWeapon(WeaponID.WEAPON_MAC10),
        new CAimWeapon(WeaponID.WEAPON_MP9),
        new CAimWeapon(WeaponID.WEAPON_MP7),
        new CAimWeapon(WeaponID.WEAPON_MP5SD),
        new CAimWeapon(WeaponID.WEAPON_UMP45),
        new CAimWeapon(WeaponID.WEAPON_P90),
        new CAimWeapon(WeaponID.WEAPON_BIZON),
        new CAimWeapon(WeaponID.WEAPON_FAMAS),
        new CAimWeapon(WeaponID.WEAPON_GALILAR),
        new CAimWeapon(WeaponID.WEAPON_M4A1_SILENCER),
        new CAimWeapon(WeaponID.WEAPON_AK47),
        new CAimWeapon(WeaponID.WEAPON_M4A1),
        new CAimWeapon(WeaponID.WEAPON_AUG),
        new CAimWeapon(WeaponID.WEAPON_SG553),
        new CAimWeapon(WeaponID.WEAPON_SSG08),
        new CAimWeapon(WeaponID.WEAPON_AWP),
        new CAimWeapon(WeaponID.WEAPON_SCAR20),
        new CAimWeapon(WeaponID.WEAPON_G3SG1),
        new CAimWeapon(WeaponID.WEAPON_NONE)
        {
            WeaponBone = 8
        }
         };
        public static CAimMisc CAimMisc = new CAimMisc();
        #endregion

        #region Misc
        public static CMisc CMisc = new CMisc();
        #endregion

        public static void SaveConfig()
        {
            Object[] ObjectType =
            {
                CVisualGlowObjectManager,
                CVisualChamsColor,
                CVisualMisc,

                CAim,
                CAimWeapon,
                CAimMisc,

                CMisc
            };

            if (!Directory.Exists("Config"))
            {
                Directory.CreateDirectory("Config");
            }

            foreach (var ObjectSave in ObjectType)
            {
                using (FileStream FileStream = new FileStream(ConfigPath(ObjectSave), FileMode.Create, FileAccess.Write))
                {
                    new XmlSerializer(ObjectSave.GetType()).Serialize(FileStream, ObjectSave);
                }
            }
        }

        public static T LoadConfig<T>()
        {
        TryLoad:
            try
            {
                using (FileStream FileStream = new FileStream(ConfigPath<T>(), FileMode.Open, FileAccess.Read))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(FileStream);
                }
            }
            catch
            {
                Object[] ObjectType =
                {
                CVisualGlowObjectManager,
                CVisualChamsColor,
                CVisualMisc,

                CAim,
                CAimWeapon,
                CAimMisc,

                CMisc
                };

                if (!Directory.Exists("Config"))
                {
                    Directory.CreateDirectory("Config");
                }

                using (FileStream FileStream = new FileStream(ConfigPath<T>(), FileMode.Create, FileAccess.Write))
                {
                    foreach (var ObjectSave in ObjectType)
                    {
                        if (typeof(T).Name == ObjectSave.GetType().Name)
                        {
                            new XmlSerializer(typeof(T)).Serialize(FileStream, ObjectSave);
                        }
                    }
                }

                goto TryLoad;
            }
        }
    }
}