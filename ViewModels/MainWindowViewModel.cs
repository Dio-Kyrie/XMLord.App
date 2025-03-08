using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace XMLord.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Default state of the pane.
    [ObservableProperty] private bool _isPaneOpen = true;
    // Default page of the pane.
    [ObservableProperty] private ViewModelBase _currentPage = new EditorPageViewModel();
    // Tells which page of the pane is selected at the moment.
    [ObservableProperty] private ListBoxItemTemplate? _selectedListBoxItem;

    // Changes current page depending on the selected item of the pane.
    partial void OnSelectedListBoxItemChanged(ListBoxItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }

    /*
     * For each item in ListBoxItems it takes label from ListBoxItemTemplate and makes it appear as an item in the pane.
     * It also places specified icon along text.
     */
    public ObservableCollection<ListBoxItemTemplate> ListBoxItems { get; } =
    [
        new ListBoxItemTemplate(typeof(EditorPageViewModel), "EditSettingsRegular"),
        new ListBoxItemTemplate(typeof(SettingsPageViewModel), "LauncherSettingsRegular")
    ];
    
    // TogglePaneOpen changes state of the pane open/close.
    [RelayCommand]
    private void TogglePaneOpen()
    {
        IsPaneOpen = !IsPaneOpen;
    }

}
/*
 * ListBoxItemTemplate will get all *PageViewModel files,
 * and delete this part in label to left only * as a name of BoxItem.
 * E.G. Label "EditorPageViewModel" will be turned into "Editor".
 * Also places icon along text.
 */
public class ListBoxItemTemplate
{
    public ListBoxItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "");
        
        Application.Current!.TryFindResource(iconKey, out var res);
        ListBoxItemIcon = (StreamGeometry)res!;
    }
    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListBoxItemIcon { get; }
}