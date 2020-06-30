using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using CitiesHarmony.API;

namespace TollPriceIncreaserMod
{
    public class TollPriceIncreaserMod : IUserMod
    {
        public string Name => "Toll Price Increaser";
        public string Description => "Raises the Toll Limit";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }

    //public class panel : ExceptionPanel
    //{
    //    protected override void Initialize()
    //    {
    //        base.Initialize();
    //        var slider = Find<UISlider>("SliderTicketPrice");
    //        slider.maxValue = 255;
    //        slider.minValue = 0;
    //    }
    //}

    //public class cpanel : UICheckBox
    //{
    //    public void ChangeTollLimit()
    //    {
    //        var slider = Find<UISlider>("SliderTicketPrice");
    //        slider.maxValue = 250;
    //        slider.minValue = 0;
    //    }
    //}

    //class TollPriceIncreaser : LoadingExtensionBase
    //{
    //    //public override void OnLevelLoaded(LoadMode mode)
    //    //{
    //    //    var panel = UIView.library.Get<CityServiceWorldInfoPanel>(typeof(CityServiceWorldInfoPanel).Name);
    //    //    cpanel checkBox = panel.component.AddUIComponent<cpanel>();
    //    //    checkBox.ChangeTollLimit();
    //    //    panel.component.RemoveUIComponent(checkBox);

    //    //}
    //}
}

