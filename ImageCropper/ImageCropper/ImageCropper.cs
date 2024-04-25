using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Stormlion.ImageCropper
{
    public class ImageCropper
    {
        private const string TAG = "ImageCropper";

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

        public enum ResultErrorType
        {
            None,
            CaptureNotSupported,
            CroppingError,
            CroppingCancelled,
            CroppingSaving,
        };

        public CropShapeType CropShape { get; set; } = CropShapeType.Rectangle;

        public int AspectRatioX { get; set; } = 0;

        public int AspectRatioY { get; set; } = 0;

        public string PageTitle { get; set; } = null;

        public string SelectSourceTitle { get; set; } = "Select source";

        public string TakePhotoTitle { get; set; } = "Take Photo";

        public string PhotoLibraryTitle { get; set; } = "Photo Library";

        public string CancelButtonTitle { get; set; } = "Cancel";

        /// <summary>
        /// Boton para realizar el recorte
        /// </summary>
        public string CropButtonTitle { get; set; } = "Crop";

        public Action<string> Success { get; set; }

        public Action<ResultErrorType> Faiure { get; set; }

        /// <summary>
        /// ¿Habilitar captura desde la camara?
        /// </summary>
        public bool CaptureFromCameraEnabled { get; set; } = true;

        /// <summary>
        /// ¿Habilitar captura desde la galeria de fotos?
        /// </summary>
        public bool CaptureFromLibraryEnabled { get; set; } = true;

        /*
        public PickMediaOptions PickMediaOptions { get; set; } = new PickMediaOptions
        {
            PhotoSize = PhotoSize.Large,
        };

        public StoreCameraMediaOptions StoreCameraMediaOptions { get; set; } = new StoreCameraMediaOptions();
        */

        public MediaPickerOptions MediaPickerOptions { get; set; } = new MediaPickerOptions();


        public async void Show(Page page, string imageFile = null)
        {
            var ResultError = ResultErrorType.None;
            if (imageFile == null)
            {
                FileResult file = null;
                string newFile = null;

                List<string> enabledButtons = new List<string>();
                if (CaptureFromCameraEnabled)
                {
                    enabledButtons.Add(TakePhotoTitle);
                }
                if (CaptureFromLibraryEnabled)
                {
                    enabledButtons.Add(PhotoLibraryTitle);
                }
                if (enabledButtons.Count == 0)
                {
                    throw new Exception("You must enable at least one image source (CaptureFromCameraEnabled,CaptureFromLibraryEnabled).");
                }
                string action = enabledButtons[0];
                if (enabledButtons.Count >= 2)
                {
                    action = await page.DisplayActionSheet(SelectSourceTitle, CancelButtonTitle, null, enabledButtons.ToArray());
                }

                try
                {
                    if (action == TakePhotoTitle)
                    {
                        if (MediaPicker.IsCaptureSupported)
                        {
                            file = await MediaPicker.CapturePhotoAsync(MediaPickerOptions);
                        }
                        else
                        {
                            //No soporta la captura desde la camara
                            ResultError = ResultErrorType.CaptureNotSupported;
                        }
                    }
                    else if (action == PhotoLibraryTitle)
                    {
                        file = await MediaPicker.PickPhotoAsync(MediaPickerOptions);
                    }
                    else
                    {
                        Faiure?.Invoke(ResultError);
                        return;
                    }

                    if (file != null)
                    {
                        //Hay que copiarlo porque en iOS file.FullPath no da el path absoluto de la imagen
                        // save the file into local storage
                        newFile = Path.Combine(FileSystem.CacheDirectory, file.FileName);
                        using (var stream = await file.OpenReadAsync())
                        using (var newStream = File.OpenWrite(newFile))
                            await stream.CopyToAsync(newStream);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                }

                if (file == null || newFile == null)
                {
                    Faiure?.Invoke(ResultError);
                    return;
                }
                if (Device.RuntimePlatform == Device.Android)
                {
                    //Delay for fix Xamarin.Essentials.Platform.CurrentActivity no MediaPicker
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                }
                imageFile = newFile;
            }

            // small delay
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            DependencyService.Get<IImageCropperWrapper>().ShowFromFile(this, imageFile);
        }

    }
}
