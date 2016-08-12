// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Impact.Game.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {

        private const string HighScoresSetting = "HighScores";
        private const string HighestCompletedLevelSetting = "HighestCompletedLevel";

        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }


        public static string HighScores
        {
            get { return AppSettings.GetValueOrDefault(HighScoresSetting, string.Empty); }
            set { AppSettings.AddOrUpdateValue(HighScoresSetting, value); }
        }

        public static int HighestCompletedLevel
        {
            get { return AppSettings.GetValueOrDefault(HighestCompletedLevelSetting, 0); }
            set { AppSettings.AddOrUpdateValue(HighestCompletedLevelSetting, value); }
        }

    }
}