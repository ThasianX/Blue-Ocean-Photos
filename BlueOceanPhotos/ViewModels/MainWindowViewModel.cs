﻿namespace BlueOceanPhotos
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Windows;
    using Microsoft.Win32;
    using System.IO;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Private Members
        private BitmapImage _chosenImage;
        private Uri _chosenImageUri;
        private int rotations = 0;
        #endregion
        #region Constructors
        public MainWindowViewModel()
        {
            //sleep for 3 seconds so we can show splash screen
            Thread.Sleep(3000);
        }
        #endregion
        #region Public Properties
        public BitmapImage ChosenImage
        {
            get { return this._chosenImage; }
            set
            {
                if(this._chosenImage != value)
                {
                    this._chosenImage = value;
                }
            }
        }

        public ICommand NewImageCommand
        {
            get { return new DelegateCommand(NewImage); }
        }

        public ICommand OpenImageCommand
        {
            get { return new DelegateCommand(OpenImage); }
        }

        public ICommand RotateImageCommand
        {
            get { return new DelegateCommand(RotateImage); }
        }

        public ICommand CloseImageCommand
        {
            get { return new DelegateCommand(CloseImage); }
        }

        public ICommand SaveImageCommand
        {
            get { return new DelegateCommand(SaveImage); }
        }

        #endregion
        #region Private Methods
        //creates blank bitmap so user can draw on it
        private void NewImage()
        {
            _chosenImage = new BitmapImage();
            _chosenImageUri = new Uri("Images/EmptyImage.png", UriKind.Relative);
            _chosenImage.BeginInit();
            _chosenImage.UriSource = _chosenImageUri;
            _chosenImage.EndInit();
            OnPropertyChanged(nameof(ChosenImage));
        }

        //opens file explorer so user can choose photo they want to open
        private void OpenImage()
        {
            try
            {
                //open the file explorer so user can choose the photo they want to open
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select an image";
                openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

                //if we can open the image, open it in our application
                if (openFileDialog.ShowDialog() == true)
                {
                    ChosenImage = new BitmapImage();
                    _chosenImageUri = new Uri(openFileDialog.FileName);
                    ChosenImage.BeginInit();
                    ChosenImage.UriSource = _chosenImageUri;
                    ChosenImage.EndInit();
                    OnPropertyChanged(nameof(ChosenImage));
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error importing image!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RotateImage()
        {
            //add one so we know we're on the next rotation
            rotations++;

            //keep track of what rotation we're on
            if (rotations == 5)
            {
                rotations = 1;
            }

            //switch through rotations
            switch (rotations)
            {
                //rotate 90 degrees
                case 1:
                    _chosenImage = new BitmapImage();
                    _chosenImage.BeginInit();
                    _chosenImage.Rotation = Rotation.Rotate90;
                    _chosenImage.UriSource = _chosenImageUri;
                    _chosenImage.EndInit();
                    break;
                //rotate 180 degrees
                case 2:
                    _chosenImage = new BitmapImage();
                    _chosenImage.BeginInit();
                    _chosenImage.Rotation = Rotation.Rotate180;
                    _chosenImage.UriSource = _chosenImageUri;
                    _chosenImage.EndInit();
                    break;
                //rotate 270 degrees
                case 3:
                    _chosenImage = new BitmapImage();
                    _chosenImage.BeginInit();
                    _chosenImage.Rotation = Rotation.Rotate270;
                    _chosenImage.UriSource = _chosenImageUri;
                    _chosenImage.EndInit();
                    break;
                //rotate back to 0 degrees
                case 4:
                    _chosenImage = new BitmapImage();
                    _chosenImage.BeginInit();
                    _chosenImage.Rotation = Rotation.Rotate0;
                    _chosenImage.UriSource = _chosenImageUri;
                    _chosenImage.EndInit();
                    break;
            }

            //update UI
            OnPropertyChanged(nameof(ChosenImage));
        }

        //close whatever image is currently open, basically just set it to null
        private void CloseImage()
        {
            ChosenImage = null;
            OnPropertyChanged(nameof(ChosenImage));
        }

        //save current image
        private void SaveImage()
        {
            //create filestream object to hold the bitmap
            using(FileStream stream = new FileStream("Untitled.png", FileMode.Create))
            {
                //create a PngBitmapEncoder to write the object
                PngBitmapEncoder encoder = new PngBitmapEncoder();

                //add bitmapframe to the encoder's frames collection
                encoder.Frames.Add(BitmapFrame.Create(ChosenImage));

                //save encoder's data into file stream
                encoder.Save(stream);
            }
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}