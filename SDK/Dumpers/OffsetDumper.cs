using System;
using System.Net;
using System.Collections.Generic;

using Wolf_Hack.SDK.Interfaces;

namespace Wolf_Hack.SDK.Dumpers
{
    public unsafe class OffsetDumper
    {
        #region Params
        private static Dictionary<string, int> GetOffsetDictionary = null;
        private static string UrlOffset = "https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml";
        #endregion

        #region Methods
        /// <summary>
        /// Чтение смещений
        /// </summary>
        /// <param name="Url">Адрес скачивания смещений</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetOffset()
        {
            if (GetOffsetDictionary == null)
            {
                GetOffsetDictionary = new Dictionary<string, int>();

                string GetValueUrl = new WebClient().DownloadString(UrlOffset);

                foreach (var Value in GetValueUrl.Split('\n'))
                {
                    try
                    {
                        GetOffsetDictionary.Add(Value.Split('=')[0].TrimEnd(' '), int.Parse(Value.Split('=')[1].TrimStart(' ')));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return GetOffsetDictionary;
            }
            else
            {
                return GetOffsetDictionary;
            }
        }
        #endregion

        #region Client
        public static int
            OLocalPlayer = GetOffset()["dwLocalPlayer"],
            OBoneMatrix = GetOffset()["m_dwBoneMatrix"],
            OForceJump = GetOffset()["dwForceJump"],
            OForceAttack = GetOffset()["dwForceAttack"],
            OGlowObjectManager = GetOffset()["dwGlowObjectManager"],
            OEntityList = GetOffset()["dwEntityList"],
            OPlayerResource = GetOffset()["dwPlayerResource"],
            OViewMatrix = GetOffset()["dwViewMatrix"],
            OInput = GetOffset()["dwInput"],
            OGlowUpdate = GetOffset()["force_update_spectator_glow"],
            OCurrentSequenceNumber = GetOffset()["clientstate_last_outgoing_command"];
        #endregion Client

        #region LocalPlayer
        public static int
            OVecViewOffset = GetOffset()["m_vecViewOffset"],
            OVecVelocity = GetOffset()["m_vecVelocity"],
            OVecOrigin = GetOffset()["m_vecOrigin"],
            OShootFired = GetOffset()["m_iShotsFired"],
            OFlags = GetOffset()["m_fFlags"],
            OFlash = GetOffset()["m_flFlashMaxAlpha"],
            OCrosshair = GetOffset()["m_iCrosshairId"],
            OActiveWeapon = GetOffset()["m_hActiveWeapon"],
            OMyWeapon = GetOffset()["m_hMyWeapons"],
            OVecPunch = GetOffset()["m_aimPunchAngle"],
            OTeamNum = GetOffset()["m_iTeamNum"],
            OHealth = GetOffset()["m_iHealth"],
            OSpotted = GetOffset()["m_bSpotted"],
            OSpottedByMask = GetOffset()["m_bSpottedByMask"],
            ODormant = GetOffset()["m_bDormant"],
            OObserverMode = GetOffset()["m_iObserverMode"],
            ORank = GetOffset()["m_iCompetitiveRanking"],
            OWins = GetOffset()["m_iCompetitiveWins"],
            OLifeState = GetOffset()["m_lifeState"],
            OFov = GetOffset()["m_iFOV"],
            OTotalHitsOnServer = 0xA398,
            OViewModel = 0x32F8,
            OViewModelIndex = 0x3220,
            OChamsRender = GetOffset()["m_clrRender"],
            OGlowIndex = GetOffset()["m_iGlowIndex"];
        #endregion LocalPlayer

        #region Weapon
        public static int
            OOriginalOwnerXuidHigh = GetOffset()["m_OriginalOwnerXuidHigh"],
            OOriginalOwnerXuidLow = GetOffset()["m_OriginalOwnerXuidLow"],
            OAccountID = GetOffset()["m_iAccountID"],
            OEntityQuality = GetOffset()["m_iEntityQuality"],
            OItemDefinitionIndex = GetOffset()["m_iItemDefinitionIndex"],
            OItemIdHigh = GetOffset()["m_iItemIDHigh"],
            OPaintWear = GetOffset()["m_flFallbackWear"],
            OPaintKit = GetOffset()["m_nFallbackPaintKit"],
            OPaintSeed = GetOffset()["m_nFallbackSeed"],
            OCustomName = GetOffset()["m_szCustomName"],
            OStatTrack = GetOffset()["m_nFallbackStatTrak"],
            OClip = GetOffset()["m_iClip1"],
            OModelIndex = 0x258,//GetOffset()["m_nModelIndex"],
            OSequence = 0x28BC1;//GetOffset()["m_nSequence"];
        #endregion

        #region Engine
        public static int
            OClientState = GetOffset()["dwClientState"],
            OGetLocalPlayer = GetOffset()["dwClientState_GetLocalPlayer"],
            OVecViewAngles = GetOffset()["dwClientState_ViewAngles"],
            ODeltaTick = GetOffset()["clientstate_delta_ticks"],
            OPlayerInfo = GetOffset()["dwClientState_PlayerInfo"],
            OMaxPlayer = GetOffset()["dwClientState_MaxPlayer"],
            OSendPacket = GetOffset()["dwbSendPackets"];
        #endregion Engine

        public static IntPtr dwDispatchUserMessage = Base.Memory.GetPatternManager().FindPattern(Base.ClientAddress, "55 8B EC A1 ? ? ? ? A8 01 75 1A 83 C8 01 A3 ? ? ? ? E8 ? ? ? ? 68 ? ? ? ? E8 ? ? ? ? 83 C4 04 B9 ? ? ? ?");

        public static IntPtr dwClientCmd = Base.Memory.GetPatternManager().FindPattern(Base.EngineAddress, "55 8B EC 8B 0D ? ? ? ? 81 F9 ? ? ? ? 75 0C A1 ? ? ? ? 35 ? ? ? ? EB 05 8B 01 FF 50 34 50");

        public static IntPtr dwSetClanTag = Base.Memory.GetPatternManager().FindPattern(Base.EngineAddress, "53 56 57 8B DA 8B F9 FF 15");
    }
}