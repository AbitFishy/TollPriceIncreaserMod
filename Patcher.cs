using ColossalFramework.Globalization;
using ColossalFramework.UI;
using HarmonyLib;
using System.Linq;
using CitiesHarmony.API;
//using UnityEngine;

namespace TollPriceIncreaserMod
{
    public static class Patcher
    {
        private const string HarmonyId = "RCK.TollPriceIncreaserMod";
        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;
            if (!HarmonyHelper.IsHarmonyInstalled) return;

                
            UnityEngine.Debug.Log("TollPriceIncreaser Patching");
            var harmony = new Harmony(HarmonyId);
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.PatchAll(typeof(Patcher).GetType().Assembly);
            //harmony.Patch(MethodBase(TollBoothAI.GetTollPrice), TollBoothAIGetTollPricePatch.Prefix);
            
            var patchedMeths = harmony.GetPatchedMethods();
            foreach (var method in patchedMeths)
            {
                UnityEngine.Debug.Log("TollPriceIncreaser has patched: " + method.ToString());
                UnityEngine.Debug.Log("TollPriceIncreaser has patched: " + method.Name);
            }
            UnityEngine.Debug.Log("TollPriceIncreaser has patched + " + patchedMeths.Count() + " methods");
            patched = true;
        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            patched = false;
        }
    }

    [HarmonyPatch(typeof(TollBoothAI), "GetTollPrice")]
    public static class TollBoothAIGetTollPricePatch
    {
        public static bool Prefix(ref int __result, ushort buildingID, ref Building data)
        {
            __result = data.m_education1;
            return false;
        }
    }

    [HarmonyPatch(typeof(TollBoothAI), "SetTollPrice")]
    public static class TollBoothAIGSetTollPricePatch
    {
        public static bool Prefix(ushort buildingID, ref Building data, int price)
        {
            data.m_education1 = (byte)UnityEngine.Mathf.Clamp(price, 0, 255);
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
