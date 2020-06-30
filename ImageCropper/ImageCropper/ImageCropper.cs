using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.ImageCropper
{
    public class ImageCropper
    {
        #region Properties
        private bool IsEnableOptions { get; set; }
        public static ImageCropper Current { get; set; }

        public ImageCropper()
        {
            Current = this;
        }

        public enum CropShapeType
        {
            Rectangle,
            Oval
        };

        public CropShapeType CropShape { get; set; } = CropShapeType.Rectangle;

        public int AspectRatioX { get; set; } = 0;

        public int AspectRatioY { get; set; } = 0;

        public string PageTitle { get; set; } = null;

        public string SelectSourceTitle { get; set; } = "Select source";

        public string TakePhotoTitle { get; set; } = "Take Photo";

        public string PhotoLibraryTitle { get; set; } = "Photo Library";

        public string CancelButtonTitle { get; set; } = "Cancel";

        public string SelectedAction { get; set; }

        public Action<string> Success { get; set; }

        public Action Failure { get; set; }

        public PickMediaOptions PickMediaOptions { get; set; } = new PickMediaOptions
        {
            PhotoSize = PhotoSize.Large,
        };

        public StoreCameraMediaOptions StoreCameraMediaOptions { get; set; } = new StoreCameraMediaOptions();

        #endregion

        #region Methods
        public void Show(Page page)
        {
            MediaFile file = null;
            _ = ShowAsync(page, file);
        }

        public void Show(Page page, MediaFile file)
        {
            _ = ShowAsync(page, file);
        }

        private async Task ShowAsync(Page page, MediaFile file)
        {
            if (file == null)
            {
                await CrossMedia.Current.Initialize();
                IsEnableOptions = string.IsNullOrEmpty(SelectedAction);
                string action = IsEnableOptions
                    ? await page.DisplayActionSheet(SelectSourceTitle, CancelButtonTitle, null, TakePhotoTitle, PhotoLibraryTitle)
                    : SelectedAction;

                if (string.IsNullOrEmpty(action))
                {
                    Failure?.Invoke();
                    return;
                }

                if (action == TakePhotoTitle)
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        Failure?.Invoke();
                        return;
                    }

                    file = await CrossMedia.Current.TakePhotoAsync(StoreCameraMediaOptions);
                }
                else if (action == PhotoLibraryTitle)
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await page.DisplayAlert("Error", "This device is not supported to pick photo.", "OK");
                        Failure?.Invoke();
                        return;
                    }
                    file = await CrossMedia.Current.PickPhotoAsync(PickMediaOptions);
                }
                else
                {
                    Failure?.Invoke();
                    return;
                }

                if (file == null)
                {
                    Failure?.Invoke();
                    return;
                }
            }

            await Task.Delay(TimeSpan.FromMilliseconds(100));
            DependencyService.Get<IImageCropperWrapper>().ShowFromFile(this, file);
        }

        #endregion
    }
}