using Stormlion.ImageCropper;
using System;
using Xamarin.Forms;
using static Stormlion.ImageCropper.ImageCropper;

namespace Test
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

        }

        protected async void OnClickedRectangle(object sender, EventArgs e)
        {
            imageView.Source = null;
            new ImageCropper()
            {
//                PageTitle = "Test Title",
//                AspectRatioX = 1,
//                AspectRatioY = 1,
                CompressImageMaxHeigth = 4000,
                CompressImageMaxWidth = 4000,
                Success = (imageFile) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        imageView.Source = ImageSource.FromFile(imageFile);
                    });
                },
                Faiure = (ResultErrorType resultErrorType) => {
                    Console.WriteLine("Error capturando la imagen o haciendo crop.");
                }
            }.Show(this);
        }

        private void OnClickedCircle(object sender, EventArgs e)
        {
            imageView.Source = null;
            new ImageCropper()
            {
                CropShape = ImageCropper.CropShapeType.Oval,
                Success = (imageFile) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        imageView.Source = ImageSource.FromFile(imageFile);
                    });
                },
                Faiure = (ResultErrorType resultErrorType) => {
                    Console.WriteLine("Error capturando la imagen o haciendo crop.");
                }
            }.Show(this);
        }
    }
}
