using System;

namespace Wolf_Hack.Client.Config
{
    [Serializable]
    public struct CVisualGlowObjectManager
    {
        public bool GlowActive;
        public bool GlowHPActive;

        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Allow;

        public bool FullBloom;
    }

    [Serializable]
    public struct CVisualChamsColor
    {
        public bool ChamsActive;
        public bool ChamsHPActive;

        public byte Red;
        public byte Green;
        public byte Blue;
    }

    public struct CVisualESP
    {
        public bool ESPActive;
        public bool TracerActive;
        public bool NameActive;
        public bool HealthActive;
        public bool DistanceActive;
        public bool FovActive;
    }

    [Serializable]
    public struct CVisualMisc
    {
        public bool RadarActive;
        public bool WaterMark;
        public bool HitSound;
        public bool DangerZone;
    }
}