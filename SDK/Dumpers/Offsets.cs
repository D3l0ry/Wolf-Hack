namespace Wolf_Hack.SDK.Dumpers
{
#if DEBUG
    internal static class Offsets
    {
        public static int
            OLocalPlayer = 0xD892CC,
            OBoneMatrix = 0x26A8,
            OForceJump = 0x524BF4C,
            OForceAttack = 0x31D2690,
            OGlowObjectManager = 0x52EA5D0,
            OEntityList = 0x4DA215C,
            OPlayerResource = OffsetDumper.GetOffset()["dwPlayerResource"],
            OViewMatrix = OffsetDumper.GetOffset()["dwViewMatrix"],
            OInput = 0x51F3720,
            OGlowUpdate = 0x3AFECA,
            OCurrentSequenceNumber = 0x4D2C;

        public static int
            OVecViewOffset = 0x108,
            OVecVelocity = 0x114,
            OVecOrigin = 0x138,
            OShootFired = 0xA390,
            OFlags = 0x104,
            OFlash = 0xA41C,
            OCrosshair = 0xB3E8,
            OActiveWeapon = 0x2EF8,
            OMyWeapon = 0x2DF8,
            OVecPunch = 0x302C,
            OTeamNum = 0xF4,
            OHealth = 0x100,
            OSpotted = 0x93D,
            OSpottedByMask = 0x980,
            ODormant = 0xED,
            OObserverMode = OffsetDumper.GetOffset()["m_iObserverMode"],
            ORank = OffsetDumper.GetOffset()["m_iCompetitiveRanking"],
            OWins = OffsetDumper.GetOffset()["m_iCompetitiveWins"],
            OLifeState = OffsetDumper.GetOffset()["m_lifeState"],
            OChamsRender = 0x70,
            OGlowIndex = 0xA438;

        public static int
            OOriginalOwnerXuidHigh = OffsetDumper.GetOffset()["m_OriginalOwnerXuidHigh"],
            OOriginalOwnerXuidLow = OffsetDumper.GetOffset()["m_OriginalOwnerXuidLow"],
            OAccountID = OffsetDumper.GetOffset()["m_iAccountID"],
            OEntityQuality = OffsetDumper.GetOffset()["m_iEntityQuality"],
            OItemDefinitionIndex = 0x2FAA,
            OItemIdHigh = OffsetDumper.GetOffset()["m_iItemIDHigh"],
            OPaintWear = OffsetDumper.GetOffset()["m_flFallbackWear"],
            OPaintKit = OffsetDumper.GetOffset()["m_nFallbackPaintKit"],
            OPaintSeed = OffsetDumper.GetOffset()["m_nFallbackSeed"],
            OCustomName = OffsetDumper.GetOffset()["m_szCustomName"],
            OStatTrack = OffsetDumper.GetOffset()["m_nFallbackStatTrak"],
            OClip = 0x3264;

        public static int
            OClientState = 0x588FEC,
            OGetLocalPlayer = 0x180,
            OVecViewAngles = 0x4D90,
            ODeltaTick = 0x174,
            OPlayerInfo = 0x52C0,
            OMaxPlayer = 0x388,
            OSendPacket = 0xD76DA;

        //public static IntPtr dwClientCmd = Base.Memory.GetPatternManager().FindPattern("engine.dll", "55 8B EC 8B 0D ? ? ? ? 81 F9 ? ? ? ? 75 0C A1 ? ? ? ? 35 ? ? ? ? EB 05 8B 01 FF 50 34 50");

        //public static IntPtr dwSetClanTag = new PatternManager(Base.process, Base.Memory).FindPattern("engine.dll", "53 56 57 8B DA 8B F9 FF 15");
    }
#else
    internal static class Offsets
    {
        public static int
            OLocalPlayer = OffsetDumper.GetOffset()["dwLocalPlayer"],
            OBoneMatrix = OffsetDumper.GetOffset()["m_dwBoneMatrix"],
            OForceJump = OffsetDumper.GetOffset()["dwForceJump"],
            OForceAttack = OffsetDumper.GetOffset()["dwForceAttack"],
            OGlowObjectManager = OffsetDumper.GetOffset()["dwGlowObjectManager"],
            OEntityList = OffsetDumper.GetOffset()["dwEntityList"],
            OPlayerResource = OffsetDumper.GetOffset()["dwPlayerResource"],
            OViewMatrix = OffsetDumper.GetOffset()["dwViewMatrix"],
            OInput = OffsetDumper.GetOffset()["dwInput"],
            OGlowUpdate = OffsetDumper.GetOffset()["force_update_spectator_glow"],
            OCurrentSequenceNumber = OffsetDumper.GetOffset()["clientstate_last_outgoing_command"];

        public static int
            OVecViewOffset = OffsetDumper.GetOffset()["m_vecViewOffset"],
            OVecVelocity = OffsetDumper.GetOffset()["m_vecVelocity"],
            OVecOrigin = OffsetDumper.GetOffset()["m_vecOrigin"],
            OShootFired = OffsetDumper.GetOffset()["m_iShotsFired"],
            OFlags = OffsetDumper.GetOffset()["m_fFlags"],
            OFlash = OffsetDumper.GetOffset()["m_flFlashMaxAlpha"],
            OCrosshair = OffsetDumper.GetOffset()["m_iCrosshairId"],
            OActiveWeapon = OffsetDumper.GetOffset()["m_hActiveWeapon"],
            OMyWeapon = OffsetDumper.GetOffset()["m_hMyWeapons"],
            OVecPunch = OffsetDumper.GetOffset()["m_aimPunchAngle"],
            OTeamNum = OffsetDumper.GetOffset()["m_iTeamNum"],
            OHealth = OffsetDumper.GetOffset()["m_iHealth"],
            OSpotted = OffsetDumper.GetOffset()["m_bSpotted"],
            OSpottedByMask = OffsetDumper.GetOffset()["m_bSpottedByMask"],
            ODormant = OffsetDumper.GetOffset()["m_bDormant"],
            OObserverMode = OffsetDumper.GetOffset()["m_iObserverMode"],
            ORank = OffsetDumper.GetOffset()["m_iCompetitiveRanking"],
            OWins = OffsetDumper.GetOffset()["m_iCompetitiveWins"],
            OLifeState = OffsetDumper.GetOffset()["m_lifeState"],
            OChamsRender = OffsetDumper.GetOffset()["m_clrRender"],
            OGlowIndex = OffsetDumper.GetOffset()["m_iGlowIndex"];

        public static int
            OOriginalOwnerXuidHigh = OffsetDumper.GetOffset()["m_OriginalOwnerXuidHigh"],
            OOriginalOwnerXuidLow = OffsetDumper.GetOffset()["m_OriginalOwnerXuidLow"],
            OAccountID = OffsetDumper.GetOffset()["m_iAccountID"],
            OEntityQuality = OffsetDumper.GetOffset()["m_iEntityQuality"],
            OItemDefinitionIndex = OffsetDumper.GetOffset()["m_iItemDefinitionIndex"],
            OItemIdHigh = OffsetDumper.GetOffset()["m_iItemIDHigh"],
            OPaintWear = OffsetDumper.GetOffset()["m_flFallbackWear"],
            OPaintKit = OffsetDumper.GetOffset()["m_nFallbackPaintKit"],
            OPaintSeed = OffsetDumper.GetOffset()["m_nFallbackSeed"],
            OCustomName = OffsetDumper.GetOffset()["m_szCustomName"],
            OStatTrack = OffsetDumper.GetOffset()["m_nFallbackStatTrak"],
            OClip = OffsetDumper.GetOffset()["m_iClip1"];

        public static int
            OClientState = OffsetDumper.GetOffset()["dwClientState"],
            OGetLocalPlayer = OffsetDumper.GetOffset()["dwClientState_GetLocalPlayer"],
            OVecViewAngles = OffsetDumper.GetOffset()["dwClientState_ViewAngles"],
            ODeltaTick = OffsetDumper.GetOffset()["clientstate_delta_ticks"],
            OPlayerInfo = OffsetDumper.GetOffset()["dwClientState_PlayerInfo"],
            OMaxPlayer = OffsetDumper.GetOffset()["dwClientState_MaxPlayer"],
            OSendPacket = OffsetDumper.GetOffset()["dwbSendPackets"];

        //public static IntPtr dwClientCmd = Base.Memory.GetPatternManager().FindPattern("engine.dll", "55 8B EC 8B 0D ? ? ? ? 81 F9 ? ? ? ? 75 0C A1 ? ? ? ? 35 ? ? ? ? EB 05 8B 01 FF 50 34 50");

        //public static IntPtr dwSetClanTag = new PatternManager(Base.process, Base.Memory).FindPattern("engine.dll", "53 56 57 8B DA 8B F9 FF 15");
    }
#endif
}