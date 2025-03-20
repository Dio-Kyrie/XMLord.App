using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace XMLord.App.ViewModels
{
    public partial class ModulesPageViewModel : ViewModelBase
    {
        // Observable collection to store opened modules (tabs)
        [ObservableProperty]
        private ObservableCollection<Module> _openedModules = new ObservableCollection<Module>();

        // Command to create a new module tab
        [RelayCommand]
        public void CreateModule()
        {
            // Add a new module tab to the collection with a default name
            OpenedModules.Add(new Module { Name = "New Module" });
        }

        // Command to open an existing module folder (without folder selection for now)
        [RelayCommand]
        public void OpenModule()
        {
            // Add a new module tab with a default name when opening a module
            OpenedModules.Add(new Module { Name = "Opened Module" });
        }
    }

    // Simple class to represent a module
    public class Module
    {
        public string Name { get; set; }
    }
}