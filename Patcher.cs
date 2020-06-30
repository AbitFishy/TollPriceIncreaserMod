using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TollPriceIncreaserMod
{
    public static class Patcher
    {
        private const string HarmonyId = "RCK.TollPriceIncreaserMod";
        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;

            patched = true;
            Debug.Log("TollPriceIncreaser Patching");
            var harmony = new Harmony(HarmonyId);
            // harmony.PatchAll(typeof(Patcher).GetType().Assembly); // you can also do manual patching here!
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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
            //UnityEngine.Debug.Log("CreateRelay Prefix");
            __result = data.m_education1;
            return false;
        }
    }

    [HarmonyPatch(typeof(TollBoothAI), "SetTollPrice")]
    public static class TollBoothAIGSetTollPricePatch
    {
        public static bool Prefix(ushort buildingID, ref Building data, int price)
        {
            data.m_education1 = (byte)Mathf.Clamp(price, 0, 255);
            //Debug.Log("Set Toll Price: "+ data.m_education1);
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
