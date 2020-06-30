using Com.Theartofdev.Edmodo.Cropper;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;

namespace Stormlion.ImageCropper.Droid
{
    public class ImageCropperImplementation : IImageCropperWrapper
    {
        public void ShowFromFile(ImageCropper imageCropper, MediaFile imageFile)
        {
            try
            {
                CropImage.ActivityBuilder activityBuilder = CropImage.Activity(Android.Net.Uri.FromFile(new Java.IO.File(imageFile.Path)));

                if (imageCropper.CropShape == ImageCropper.CropShapeType.Oval)
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Oval);
                }
                else
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Rectangle);
                }

                if (imageCropper.AspectRatioX > 0 && imageCropper.AspectRatioY > 0)
                {
                    activityBuilder.SetFixAspectRatio(true);
                    activityBuilder.SetAspectRatio(imageCropper.AspectRatioX, imageCropper.AspectRatioY);
                }
                else
                {
                    activityBuilder.SetFixAspectRatio(false);
                }

                if (!string.IsNullOrWhiteSpace(imageCropper.PageTitle))
                {
                    activityBuilder.SetActivityTitle(imageCropper.PageTitle);
                }

                activityBuilder.Start(Xamarin.Essentials.Platform.CurrentActivity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}