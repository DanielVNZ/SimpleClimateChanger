using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Entities;


namespace SimpleClimateChanger
{
    [FileLocation(nameof(SimpleClimateChanger))]
    [SettingsUIGroupOrder(kButtonGroup, kDropdownGroup)]
    [SettingsUIShowGroupName(kButtonGroup, kDropdownGroup)]
    public class Setting : ModSetting
    {

        private readonly Mod _mod;
        private readonly DanielsWeatherSystem _weatherSystem;

        public const string kSection = "Main";
        public const string kButtonGroup = "Button";
        public const string kDropdownGroup = "Dropdown";
        public const string kSliderGroup = "Slider";


        public Setting(IMod mod, DanielsWeatherSystem weatherSystem) : base(mod)
        {

            _mod = (Mod)mod;
            _weatherSystem = weatherSystem;
            Mod.log.Info("Setting initialized");


            // Subscribe to the ValueChanged event of MaxTemperature slider


            //_weatherSystem.UpdateWeather(_mod);

            /*if (GameManager.instance.gameMode == GameMode.GameOrEditor)
            {
                _weatherSystem.UpdateWeather(_mod);
                log.Info("Weather updated - MANUAL SETTINGS CHANGE");
            }*/

            Mod.log.Info($"Temp (Local): {MaxTemperature}");



        }

   

        [SettingsUISlider(min = 0, max = 10000, step = 1, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kSliderGroup)]
        public int SeedOffset { get; set; }

        [SettingsUISlider(min = -20, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float MinTemperature { get; set; }


        [SettingsUISlider(min = -50, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kTemperature)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float MaxTemperature { get; set; }
        


        /* get
         {
            return _weatherSystem._climateSystem.temperature.overrideValue; // or any default value you want to return if _climateSystem is null
         }
         //get { return MaxTemperature; }
         set
         {
             if (_weatherSystem != null && _weatherSystem._climateSystem != null)
             {
                 _weatherSystem._climateSystem.temperature.overrideValue = value;
                 _weatherSystem.UpdateWeather(_mod);
             }
             else
             {
                 // Handle the case where _weatherSystem or _climateSystem is null
                 log.Warn("Weather system or climate system is not properly initialized.");

             }
         }*/





        public override void SetDefaults()
        {
            // Set default values if needed
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting _setting;
        public LocaleEN(Setting setting)
        {
            _setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { _setting.GetSettingsLocaleID(), "Mod settings sample" },
                { _setting.GetOptionTabLocaleID(Setting.kSection), "Main" },
                { _setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Buttons" },
                { _setting.GetOptionGroupLocaleID(Setting.kSliderGroup), "Sliders" },
                { _setting.GetOptionGroupLocaleID(Setting.kDropdownGroup), "Dropdowns" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.MinTemperature)), "Minimum Temperature" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.MinTemperature)), $"Use this slider to set the minimum temperature" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.MaxTemperature)), "Maximum Temperature" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.MaxTemperature)), $"Use this slider to set the maximum temperature" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.SeedOffset)), "Seed Offset" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.SeedOffset)), $"Use this slider to set the seed offset" }
            };
        }

        public void Unload()
        {
            // Cleanup resources if needed
        }
    }
}
