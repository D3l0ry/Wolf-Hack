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
        public byte Alpha;

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

    [Serializable]
    public struct CVisualMisc
    {
        public bool RadarActive;
        public bool DangerZone;
    }
}