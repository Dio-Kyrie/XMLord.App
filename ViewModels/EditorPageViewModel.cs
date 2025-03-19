using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Threading.Tasks;

namespace XMLord.App.ViewModels
{
    public partial class EditorPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _xText = "Select an XML file...";

        public EditorPageViewModel() { }

        [RelayCommand]
        public async Task PickFileAsync(Window window)
        {
            if (window == null) return;

            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select XML File",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("XML Files") { Patterns = new[] { "*.xml" } } }
            });

            if (files.Count > 0)
            {
                var file = files[0];
                var filePath = file.Path.LocalPath; // Get actual file path

                LoadXmlAsync(filePath);
            }
        }

        private async void LoadXmlAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string xmlContent = await Task.Run(() => File.ReadAllText(filePath));
                    XText = xmlContent;
                }
                else
                {
                    XText = "File not found.";
                }
            }
            catch (System.Exception ex)
            {
                XText = $"Failed to load XML: {ex.Message}";
            }
        }
    }
}