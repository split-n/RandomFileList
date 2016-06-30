using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RandomFileList.ViewModels;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace RandomFileList.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = (MainPageViewModel)DataContext;
        }

        private async void OpenDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var fp = new FolderPicker();
            fp.FileTypeFilter.Add("*");
            var folder = await fp.PickSingleFolderAsync();
            ViewModel.LoadFolder(folder);
        }

        private void FileItem_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            var file = (sender as FrameworkElement)?.DataContext as StorageFile;

            if (file != null)
            {
                args.DragUI.SetContentFromDataPackage();
                args.Data.SetStorageItems(new[] { file });
            }
        }
    }
}
