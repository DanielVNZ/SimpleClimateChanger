using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using System.Collections.Generic;



namespace SimpleClimateChanger
{
    [FileLocation(nameof(SimpleClimateChanger))]
    [SettingsUIGroupOrder(kSliderGroup, kDropdownGroup)]
    [SettingsUIShowGroupName(kSliderGroup, kDropdownGroup)]
    public class Setting : ModSetting
    {

        private readonly Mod _mod;
        private DanielsWeatherSystem _weatherSystem;



        public const string kSection = "Main";
        public const string kSection2 = "Time";
        public const string kButtonGroup = "Button";
        public const string kDropdownGroup = "Dropdown";
        public const string kSliderGroup = "Slider";
        public float currentTemp;
        public float currentPrecipitation;
        public float cloudiness;
        public bool  enableTemperature;
        public bool enablePrecipitation;
        public bool enableCloudiness;

        public bool night;
        public bool default1;
        public bool day;
        public bool goldenHour;


        public Setting(IMod mod, DanielsWeatherSystem weatherSystem) : base(mod)
        {
            _mod = (Mod)mod;
            _weatherSystem = weatherSystem;
            Mod.log.Info("Setting initialized");


            currentTemp = Temperature;
            currentPrecipitation = Precipitation;
            cloudiness = Cloudiness;
            enableTemperature = EnabableTemperature;
            enablePrecipitation = EnablePrecipitation;
            enableCloudiness = EnableCloudiness;
            night = Night;
            default1 = Default;
            day = Day;

            Default = true;

        }

        //Page1 - Weather Information

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnabableTemperature { get; set; }

        [SettingsUISlider(min = -50, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kTemperature)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Temperature { get; set; }

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnablePrecipitation { get; set; }

        [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1, unit = Unit.kFloatThreeFractions)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Precipitation { get; set; }

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnableCloudiness { get; set; }

        [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1, unit = Unit.kFloatThreeFractions)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Cloudiness { get; set; }


        //Page2 - Time Information

        [SettingsUISection(kSection2, kDropdownGroup)]
        public bool Default { get; set; }


        [SettingsUISection(kSection2, kDropdownGroup)]
        public bool Night { get; set; }

        [SettingsUISection(kSection2, kDropdownGroup)]
        public bool Day { get; set; }





        public override void Apply()
        {
            Mod.log.Info("Running Apply method..........");
            currentTemp = Temperature;
            currentPrecipitation = Precipitation;
            cloudiness = Cloudiness;
            enableTemperature = EnabableTemperature;
            enablePrecipitation = EnablePrecipitation;
            enableCloudiness = EnableCloudiness;
            night = Night;
            default1 = Default;
            day = Day;
            _weatherSystem.UpdateTimeOfDay(night, default1, day);
            _weatherSystem.UpdateWeather(currentTemp, currentPrecipitation, cloudiness); 
           
            Mod.log.Info("Weather updated successfully from Apply method.");





            if (Default == true)
            {
                Night = false;
                Day = false;
            }
            else if (Night == true)
            {
                Mod.log.Info("Night is true");
                Day = false;
                Default = false;
            }
            else if (Day == true)
            {
                Mod.log.Info("Day is true");
                Night = false;
                Default = false;
            }
            else
            {
                Mod.log.Info("No time of day selected");
            }
        }


        public override void SetDefaults()
        {
            Mod.log.Info("SetDefaults Ran Successfully");

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
                { _setting.GetSettingsLocaleID(), "Weather+" },
                { _setting.GetOptionTabLocaleID(Setting.kSection), "Weather Settings" },
                { _setting.GetOptionTabLocaleID(Setting.kSection2), "Time Settings" },
                { _setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Buttons" },
                { _setting.GetOptionGroupLocaleID(Setting.kSliderGroup), "Change Current Weather" },
                { _setting.GetOptionDescLocaleID(Setting.kSection), "Change the current weather settings." },
                { _setting.GetOptionGroupLocaleID(Setting.kDropdownGroup), "Change Visual Time Settings. De-select an option prior to changing" },


                { _setting.GetOptionLabelLocaleID(nameof(Setting.Cloudiness)), "Current Cloudiness" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Cloudiness)), $"Use this slider to set the current Cloudiness." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Precipitation)), "Current Precipitation (Rain)" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Precipitation)), $"Use this slider to set the current Precipitation (Rain) volume." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Temperature)), "Current Temperature" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Temperature)), $"Use this slider to change the current temperature. (-50 to +50 degrees)" },

                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnabableTemperature)), "Enable Custom Temperature?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnabableTemperature)), $"Tick to enable a custom temperature value" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnablePrecipitation)), "Enable Custom Precipitation?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnablePrecipitation)), $"Tick to enable a custom precipitation value" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnableCloudiness)), "Enable Custom Cloudiness?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnableCloudiness)), $"Tick to enable a custom cloudiness value" },


                { _setting.GetOptionLabelLocaleID(nameof(Setting.Default)), "Default" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Default)), $"Stops overriding the time and goes back to vanilla behaivour" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Night)), "Night" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Night)), $"Sets the time to night" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Day)), "Day" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Day)), $"Sets the time to day" },

            };
        }

        public void Unload()
        {
            
        }
    }
}
