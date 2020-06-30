using Plugin.Media;
using System;
using Xamarin.Forms;
using Xamarin.ImageCropper;

namespace Test
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            CrossMedia.Current.Initialize();
        }

        private void OnClickedRectangle(object sender, EventArgs e)
        {
            new ImageCropper()
            {
                //PageTitle = "Test Title",
                //AspectRatioX = 1,
                //AspectRatioY = 1,
                SelectedAction = ImageCropper.Current.TakePhotoTitle,
                Success = (imageFile) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        imageView.Source = ImageSource.FromFile(imageFile);
                    });
                }
            }.Show(this);
        }

        private void OnClickedCircle(object sender, EventArgs e)
        {
            new ImageCropper()
            {
                CropShape = ImageCropper.CropShapeType.Oval,
                Success = (imageFile) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        imageView.Source = ImageSource.FromFile(imageFile);
                    });
                }
            }.Show(this);
        }
    }
}
