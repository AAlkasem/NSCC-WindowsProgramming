using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment1_Take2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    
    {
        TagLib.File? currentFile;

        public MainWindow()
        {
            InitializeComponent();
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            EditButton.IsEnabled = false;
        }

        public void OpenFile_Click(object sender, RoutedEventArgs e)
        {


            //ShowDialog() shows onsceen for the user
            //By default it returns true if the user selects a file and hits "Open"
            OpenFileDialog fileDlg = new OpenFileDialog();

            //Set the filter to only show MP3 files
            fileDlg.Filter = "MP3 files (*.mp3)| *.mp3| All files (*.*)|*.*";

            if (fileDlg.ShowDialog() == true)
            {
                // Example of creating a TagLib file object, for accessing MP3 metadata
                currentFile = TagLib.File.Create(fileDlg.FileName);

                myMediaPlayer.Source = new Uri(fileDlg.FileName);
                myMediaPlayer.Play();

                //Examples of reading tag data from the currently selected file
                if (currentFile != null)
                {
                    PlayButton.IsEnabled = false;
                    PauseButton.IsEnabled = true;
                    StopButton.IsEnabled = true;
                    var year = currentFile.Tag.Year;
                    var title = currentFile.Tag.Title;
                    var artist = currentFile.Tag.FirstPerformer;
                    var album = currentFile.Tag.Album;
                    EditButton.IsEnabled = true;
                    if (title == null)
                    {
                        title = "Unknown";
                    }
                    if (artist == null)
                    {
                        artist = "Unknown";
                    }
                    if (album == null)
                    {
                        album = "Unknown";
                    }
                    if (year == 0)
                    {
                        year = 0;
                    }

                    tagArtistBox.Text = "Artist: " + artist + " | Album: " + album;
                    tagTitleBox.Text = "Title: " + title + " | Year: " + year;


                    // This snippet of code was taken from StackOverflow and is used to display the album art in the program window
                    // link to the code: https://stackoverflow.com/questions/10520048/how-to-display-embedded-cover-art-in-wpf
                    // Load you image data in MemoryStream
                    
                    try 
                    {
                        TagLib.IPicture pic = currentFile.Tag.Pictures[0]; 
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);

                        // ImageSource for System.Windows.Controls.Image
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();

                        ImgArt.Source = bitmap;


                    } 
                    catch (Exception ex)
                    {
                        MessageBox.Show("No album art found"); 
                    }

                    

                    
                } 
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Play();
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            StopButton.IsEnabled = true;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Pause();
            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Stop();
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Media.Visibility= Visibility.Collapsed;
            EditTag.Visibility = Visibility.Visible;
            TagEditor.Text = "Edit Tag";
            var year = currentFile.Tag.Year;
            var title = currentFile.Tag.Title;
            var artist = currentFile.Tag.FirstPerformer;
            var album = currentFile.Tag.Album;
            if (title == null)
            {
                title = "Unknown";
            }
            if (artist == null)
            {
                artist = "Unknown";
            }
            if (album == null)
            {
                album = "Unknown";
            }
            if (year == 0)
            {
                year = 0;
            }

            Artist.Text = artist;
            Title.Text = title;
            Album.Text = album;
            Year.Text = year.ToString();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Media.Visibility = Visibility.Visible;
            EditTag.Visibility = Visibility.Collapsed;
        }

    }
}

