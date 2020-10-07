using ApiHelper;
using ApiHelper.Models;
using DogFetchApp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DogFetchApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {

        private string selectedBreed;
        private int selectedNumber;

        private ObservableCollection<int> numbers = new ObservableCollection<int>();
        private ObservableCollection<string> breeds = new ObservableCollection<string>();
        private ObservableCollection<string> images = new ObservableCollection<string>();

        public DelegateCommand<string> FetchCommand { get; private set; }
        public DelegateCommand<string> LanguageCommand { get; private set; }

        public ObservableCollection<string> Breeds
        {
            get => breeds;
            set
            {
                breeds = value;
                OnPropertyChanged();
            }
        }

        public string SelectedBreed
        {
            get => selectedBreed;
            set
            {
                selectedBreed = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> Numbers
        {
            get => numbers;
            set
            {
                numbers = value;
                OnPropertyChanged();
            }
        }

        public int SelectedNumber
        {
            get => selectedNumber;
            set
            {
                selectedNumber = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Images
        {
            get => images;
            set
            {
                images = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            FetchCommand = new DelegateCommand<string>(loadImages);
            LanguageCommand = new DelegateCommand<string>(changeLanguage);

            initNumbers();
            loadBreeds();
        }

        private void initNumbers()
        {
            Numbers = new ObservableCollection<int> {1, 3, 5, 10};
            selectedNumber = 0;
        }

        private async void loadBreeds()
        {
            List<string> b = await DogApiProcessor.LoadBreedList();
            foreach(string i in b)
            {
                breeds.Add(i);
            }
        }

        private async void loadImages(object sender)
        {
            if(SelectedBreed == null || SelectedNumber == 0)
            {
                MessageBox.Show(Properties.Resources.Error_Message);
                return;
            }

            images.Clear();
            string img;

            for (int i = 0; i < selectedNumber; i++){

                Images.Add(await DogApiProcessor.GetImageUrl(SelectedBreed));

            }

        }

        private void changeLanguage(string param)
        {

            MessageBoxResult result = MessageBox.Show(Properties.Resources.Reboot_Message, "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Properties.Settings.Default.Language = param;
                    Properties.Settings.Default.Save();
                    restart();
                    break;
                case MessageBoxResult.No:
                    Properties.Settings.Default.Language = param;
                    Properties.Settings.Default.Save();
                    break;

            }
        }

        private void restart()
        {
            var filename = Application.ResourceAssembly.Location;
            var newFile = Path.ChangeExtension(filename, ".exe");
            Process.Start(newFile);
            Application.Current.Shutdown();
        }

    }
}
