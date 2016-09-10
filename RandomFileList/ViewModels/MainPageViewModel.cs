using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using RandomFileList.Extensions;

namespace RandomFileList.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private IEnumerable<StorageFile> _fileList;
        public IEnumerable<StorageFile> FileList
        {
            get { return _fileList; }
            set { SetProperty(ref _fileList, value); }
        }

        private int _maxFiles = 100;
        public int MaxFiles
        {
            get { return _maxFiles; }
            set { SetProperty(ref _maxFiles, value); }
        }

        private string _fileNameRegex;
        public string FileNameRegex
        {
            get { return _fileNameRegex; }
            set { SetProperty(ref _fileNameRegex, value); }
        }

        public string FolderPath => _folder?.Path;

        public DelegateCommand SearchFilesCommand { get; private set; }

        private StorageFolder _folder;

        private IReadOnlyList<StorageFile> _files;

        public MainPageViewModel()
        {
            SearchFilesCommand = new DelegateCommand(async () => await SearchFiles(), () => _folder != null);
        }

        public async Task LoadFolder(StorageFolder folder)
        {
            _folder = folder;
            OnPropertyChanged("FolderPath");
            _files = await _folder.GetFilesAsync();
            SearchFilesCommand.RaiseCanExecuteChanged();

        }

        private async Task SearchFiles()
        {
            if (_files == null) return;

            IEnumerable<StorageFile> files = _files;
            if (!string.IsNullOrEmpty(FileNameRegex))
            {
                try
                {
                    var regex = new Regex(FileNameRegex, RegexOptions.IgnoreCase);
                    files = files.Where((file) => regex.Match(file.Name).Success);
                }
                catch (ArgumentException) { }
            }
            var shuffled = files.Shuffle();
            FileList = shuffled.Take(MaxFiles).ToArray();
        }
    }
}
