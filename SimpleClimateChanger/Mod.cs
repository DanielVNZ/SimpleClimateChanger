using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.Settings;
using Game.Simulation;
using Game.UI.InGame;
using System.Configuration;
using Unity.Entities;




namespace SimpleClimateChanger
{


    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(SimpleClimateChanger)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        public Setting m_Setting;


        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            var weatherSystem = new DanielsWeatherSystem();

            m_Setting = new Setting(this, weatherSystem);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            AssetDatabase.global.LoadSettings(nameof(SimpleClimateChanger), m_Setting, new Setting(this, weatherSystem));

            updateSystem.UpdateAfter<ClimateSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAfter<ClimateSystem>(SystemUpdatePhase.Rendering);
            updateSystem.UpdateAfter<ClimateSystem>(SystemUpdatePhase.MainLoop);
            log.Info("Update System Ran Successfully");
            updateSystem.UpdateAfter<ClimateUISystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAfter<ClimateUISystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAfter<ClimateUISystem>(SystemUpdatePhase.Rendering);
            log.Info("Update System Ran Successfully 2");
        }


        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }

    public partial class DanielsWeatherSystem : GameSystemBase
    {
        private Mod _mod;

        //private bool _initialized = false;
        public ClimateSystem _climateSystem;
        public bool isInitialized = false;


        protected override void OnCreate()
        {
            base.OnCreate();
            
            Mod.log.Info($"Climate System found {_climateSystem.temperature}");

        }



        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            Mod.log.Info("OnGameLoadingComplete Ran Successfully");

            if (!mode.IsGameOrEditor())
                return;

            if (_climateSystem == null)
            {
                _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                Mod.log.Info("Climate System found");
                Mod.log.Info($"Temperature: {_climateSystem.temperature}");

                UpdateWeather(_mod);
                
            }
            else
            {
                Mod.log.Info("Mode is not game or editor");

                Mod.log.Warn($"Setting is {(_climateSystem == null ? "null" : "not null")}");
            }

            isInitialized = true;
            Mod.log.Info("Weather System Initialized");



        }

        public void UpdateWeather(Mod mod)
        {
            _mod = mod;
            if (mod == null)
            {
                Mod.log.Warn("Mod is null, unable to update weather.");
                return;
            }

            if (mod.m_Setting == null)
            {
                Mod.log.Warn("Setting is null, unable to update weather.");
                return;
            }

            Mod.log.Info("Max Temperature from settings: " + mod.m_Setting.MaxTemperature);

            if (_climateSystem != null)
            {
                _climateSystem.temperature.overrideValue = mod.m_Setting.MaxTemperature;
                Mod.log.Info("Weather updated successfully.");
            }
            else
            {
                Mod.log.Warn("Climate system is null, unable to update weather.");
            }
        }


        protected override void OnUpdate()
        {
            //UpdateWeather(_mod);
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }



    }










}
