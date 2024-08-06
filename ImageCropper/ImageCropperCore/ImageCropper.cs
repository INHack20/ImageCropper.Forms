using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;

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
        /// Comprimir imagen antes de hacer el crop (ancho)
        /// </summary>
        public int CompressImageMaxWidth { get; set; } = 0;

        /// <summary>
        /// Comprimir imagen antes de hacer el crop (alto)
        /// </summary>
        public int CompressImageMaxHeigth { get; set; } = 0;

        /// <summary>
        /// Boton para realizar el recorte
        /// </summary>
        public string CropButtonTitle { get; set; } = "Crop";

        public Action<string> Success { get; set; }

        public Action<ResultErrorType> Faiure { get; set; }

        /// <summary>
        /// ¿Deshabilitar captura desde la camara?
        /// </summary>
        public bool DisableCaptureFromCamera { get; set; }

        /// <summary>
        /// ¿Deshabilitar captura desde la galeria de fotos?
        /// </summary>
        public bool DisableCaptureFromLibrary { get; set; }

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
                if (!DisableCaptureFromCamera)
                {
                    enabledButtons.Add(TakePhotoTitle);
                }
                if (!DisableCaptureFromLibrary)
                {
                    enabledButtons.Add(PhotoLibraryTitle);
                }
                if (enabledButtons.Count == 0)
                {
                    throw new Exception("You must enable at least one image source (DisableCaptureFromCamera,DisableCaptureFromLibrary).");
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
                        {
                            //Comprimir aqui
                            if (CompressImageMaxWidth > 0 || CompressImageMaxHeigth > 0)
                            {
                                CompressImage(newFile, stream, CompressImageMaxWidth, CompressImageMaxHeigth);
                            }
                            else
                            {
                                using (var newStream = File.OpenWrite(newFile))
                                {
                                    await stream.CopyToAsync(newStream);
                                    stream.Close();
                                    stream.Dispose();

                                    newStream.Close();
                                    newStream.Dispose();
                                }
                            }
                            
                        }


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
                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
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

        /// <summary>
        /// Comprime una imagen
        /// </summary>
        /// <param name="imageFilePath"></param>
        private void CompressImage(string imageFilePath, Stream stream, float width = 1100, float heigth = 1100)
        {
            byte[] imageData;
            //FileStream stream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }
            stream.Close();
            stream.Dispose();
            byte[] resizedImage = DependencyService.Get<IImageCropperWrapper>()
            .ResizeImage(imageData, width, heigth);

            MemoryStream imageReady = new MemoryStream(resizedImage);
            if (System.IO.File.Exists(imageFilePath))
            {
                System.IO.File.Delete(imageFilePath);
            }
            using (FileStream file = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write))
            {
                imageReady.WriteTo(file);
                imageReady.Close();
                imageReady.Dispose();
                file.Close();
            }
        }

    }
}
