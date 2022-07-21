using ICities;
using CitiesHarmony.API;

namespace TollPriceIncreaserMod
{
    public class TollPriceIncreaserMod : IUserMod
    {
        public string Name => "Toll Price Increaser";
        public string Description => "Increases Tollbooth prices so that they can actually be useful!";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }
}

