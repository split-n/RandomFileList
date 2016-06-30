using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using RandomFileList.Extensions;

namespace RandomFileList.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private IEnumerable<StorageFile> _randomFiles;
        public IEnumerable<StorageFile> RandomFiles
        {
            get { return _randomFiles; }
            set { SetProperty(ref _randomFiles, value); }
        }

        private int _maxFiles = 100;
        public int MaxFiles
        {
            get { return _maxFiles; }
            set { SetProperty(ref _maxFiles, value); }
        }

        public DelegateCommand LoadFilesCommand { get; private set; }

        private StorageFolder _folder;

        public MainPageViewModel()
        {
            LoadFilesCommand = new DelegateCommand(async () => await LoadFiles());
        }

        public void LoadFolder(StorageFolder folder)
        {
            _folder = folder;
        }

        private async Task LoadFiles()
        {
            var files = await _folder.GetFilesAsync();
            var shuffled = files.Shuffle();
            RandomFiles = shuffled.Take(MaxFiles);
        }
    }
}
