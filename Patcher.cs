using ColossalFramework.Globalization;
using ColossalFramework.UI;
using HarmonyLib;
using UnityEngine;

namespace TollPriceIncreaserMod
{
    public static class Patcher
    {
        private const string HarmonyId = "RCK.TollPriceIncreaserMod";
        private static bool _patched = false;

        public static void PatchAll()
        {
            if (_patched) return;

            _patched = true;
            Debug.Log("TollPriceIncreaser Patching");
            var harmony = new Harmony(HarmonyId);
            harmony.PatchAll(typeof(Patcher).Assembly);
        }

        public static void UnpatchAll()
        {
            if (!_patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            _patched = false;
        }
    }

    [HarmonyPatch(typeof(TollBoothAI), "GetTollPrice")]
    public static class TollBoothAIGetTollPricePatch
    {
        public static bool Prefix(ref int __result, ushort buildingID, ref Building data)
        {
            __result = data.m_education1 * 10;
            return false;
        }
    }

    [HarmonyPatch(typeof(TollBoothAI), "SetTollPrice")]
    public static class TollBoothAISetTollPricePatch
    {
        public static bool Prefix(ushort buildingID, ref Building data, int price)
        {
            int stored = Mathf.Clamp(price / 10, 0, 255);
            data.m_education1 = (byte)stored;
            return false;
        }
    }

    [HarmonyPatch(typeof(CityServiceWorldInfoPanel), "OnTicketPriceChanged")]
    public static class CityServiceWorldInfoPanelOnTicketPriceChangedPatch
    {
        public static void Postfix(ref UILabel ___m_TicketPrice1, ref UILabel ___m_TicketPrice2,
            UIComponent component, float value)
        {
            float num = value / 10f;
            ___m_TicketPrice1.text = num.ToString(Settings.moneyFormat, LocaleManager.cultureInfo);
            ___m_TicketPrice2.text = (num * 2f).ToString(Settings.moneyFormat, LocaleManager.cultureInfo);
        }
    }
}
