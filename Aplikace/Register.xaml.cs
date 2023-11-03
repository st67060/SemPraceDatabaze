using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using SixLabors.ImageSharp.PixelFormats;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp.Formats.Jpeg;
using Size = SixLabors.ImageSharp.Size;
using Image = SixLabors.ImageSharp.Image;

namespace Aplikace
{
    /// <summary>
    /// Interakční logika pro Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            MouseLeftButtonDown += (s, e) => DragMove();
            InitializeComponent();
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close(); 
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files |*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;

                using (Stream stream = File.OpenRead(selectedImagePath))
                {
                    using (Image<Rgba32> rgba32Image = Image.Load<Rgba32>(stream))
                    {
                        rgba32Image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Size = new Size(90, 90),
                                Mode = SixLabors.ImageSharp.Processing.ResizeMode.Max
                            }));

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            rgba32Image.SaveAsPng(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();

                            ProfileImage.Source = bitmapImage;
                        }
                    }
                }
            }
        }


        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
