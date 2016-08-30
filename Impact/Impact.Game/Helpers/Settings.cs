// Helpers/Settings.cs

using System;
using Impact.Game.Config;
using Impact.Game.Managers;
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

        private const string HighScoresSettingKey = "HighScores";
        private const string HighestCompletedLevelSettingKey = "HighestCompletedLevel";
        private const string MusicVolumeSettingKey = "MusicVolume";
        private const string SfxVolumeSettingKey = "SfxVolume";

        private static ISettings AppSettings => CrossSettings.Current;

        public static string HighScores
        {
            get { return AppSettings.GetValueOrDefault(HighScoresSettingKey, string.Empty); }
            set { AppSettings.AddOrUpdateValue(HighScoresSettingKey, value); }
        }

        public static int HighestCompletedLevel
        {
            get { return AppSettings.GetValueOrDefault(HighestCompletedLevelSettingKey, 0); }
            set { AppSettings.AddOrUpdateValue(HighestCompletedLevelSettingKey, value); }
        }

        public static int MusicVolume
        {
            get { return AppSettings.GetValueOrDefault(MusicVolumeSettingKey, GameConstants.MusicVolumeDefault); }
            set
            {
                AppSettings.AddOrUpdateValue(MusicVolumeSettingKey, value);
                GameStateManager.Instance.SetMusicVolume(value);
            }
        }

        public static int SfxVolume
        {
            get { return AppSettings.GetValueOrDefault(SfxVolumeSettingKey, GameConstants.SfxVolumeDefault); }
            set
            {
                AppSettings.AddOrUpdateValue(SfxVolumeSettingKey, value);
                GameStateManager.Instance.SetSfxVolume(value);
            }
        }

    }
}