using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using XMLord.App.ViewModels;

namespace XMLord.App.Views
{
    public partial class EditorPageView : UserControl
    {
        public EditorPageView()
        {
            InitializeComponent();
        }

        private async void OnSelectFileClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is EditorPageViewModel vm)
            {
                var window = this.GetVisualRoot() as Window; // Get the parent window
                if (window != null)
                {
                    await vm.PickFileAsync(window);
                }
            }
        }
    }
}