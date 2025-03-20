using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace XMLord.App.ViewModels
{
    public partial class SettingsPageViewModel : ViewModelBase
    {
        private const string DefaultSettingsPath = "default_settings.json";
        private const string CurrentSettingsPath = "current_settings.json";

        // Fields for storing saved settings
        private string _gameFolderPath = "";

        // Observable properties for UI binding
        [ObservableProperty] private string _gameFolderPathTemp = string.Empty;
        [ObservableProperty] private string _themeToggleText = string.Empty;

        public SettingsPageViewModel()
        {
            LoadSettings();
            UpdateThemeToggleText();
        }

        /// <summary>
        /// Loads settings from the current settings file if available.
        /// If not available, copies default settings.
        /// </summary>
        private void LoadSettings()
        {
            if (!File.Exists(DefaultSettingsPath))
            {
                // Create default settings file
                var defaultSettings = new SettingsData
                {
                    GameFolderPath = ""
                };
                File.WriteAllText(DefaultSettingsPath, JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true }));
            }

            if (!File.Exists(CurrentSettingsPath))
            {
                File.Copy(DefaultSettingsPath, CurrentSettingsPath);
            }

            var json = File.ReadAllText(CurrentSettingsPath);
            var settings = JsonSerializer.Deserialize<SettingsData>(json);
            if (settings != null)
            {
                _gameFolderPath = settings.GameFolderPath;
            }

            ResetToCurrent();
        }

        /// <summary>
        /// Saves the modified settings to the current settings file.
        /// </summary>
        [RelayCommand]
        public void SaveSettings()
        {
            _gameFolderPath = GameFolderPathTemp;

            var settings = new SettingsData
            {
                GameFolderPath = _gameFolderPath
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CurrentSettingsPath, json);
        }

        /// <summary>
        /// Resets settings to default values.
        /// </summary>
        [RelayCommand]
        public void ResetToDefault()
        {
            var defaultJson = File.ReadAllText(DefaultSettingsPath);
            File.WriteAllText(CurrentSettingsPath, defaultJson);
            LoadSettings();
        }

        /// <summary>
        /// Restores the temporary settings to the current saved state.
        /// </summary>
        [RelayCommand]
        public void ResetToCurrent()
        {
            GameFolderPathTemp = _gameFolderPath;
        }

        /// <summary>
        /// Opens a folder picker dialog to allow the user to select a game folder.
        /// </summary>
        [RelayCommand]
        public async Task PickGameFolderAsync(object? param)
        {
            System.Diagnostics.Debug.WriteLine("PickGameFolderAsync invoked");

            if (param is not Window window)
            {
                System.Diagnostics.Debug.WriteLine("Parameter is not a Window instance");
                return;
            }

            System.Diagnostics.Debug.WriteLine("Window instance received");

            if (window.StorageProvider is { } storageProvider)
            {
                System.Diagnostics.Debug.WriteLine("StorageProvider found, opening folder picker...");

                var result = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    AllowMultiple = false
                });

                if (result.Count > 0)
                {
                    GameFolderPathTemp = result[0].Path.LocalPath;
                    System.Diagnostics.Debug.WriteLine($"Folder selected: {GameFolderPathTemp}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No folder selected");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("StorageProvider is null!");
            }
        }

        /// <summary>
        /// Updates the theme toggle button text based on the current theme.
        /// </summary>
        private void UpdateThemeToggleText()
        {
            var theme = Application.Current?.ActualThemeVariant;
            ThemeToggleText = theme == ThemeVariant.Dark ? "Light Mode" : "Dark Mode";
        }

        /// <summary>
        /// Toggles between light and dark theme.
        /// </summary>
        [RelayCommand]
        public void ToggleTheme()
        {
            var currentTheme = Application.Current?.ActualThemeVariant;

            Application.Current!.RequestedThemeVariant = currentTheme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;

            UpdateThemeToggleText();
        }

        /// <summary>
        /// Data structure for storing settings.
        /// </summary>
        private class SettingsData
        {
            public string GameFolderPath { get; set; } = "";
            public string AuthorName { get; set; } = "";
        }
    }
}
