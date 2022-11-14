using System.Diagnostics;
using System;
using Android.Graphics;
using Com.Canhub.Cropper;

namespace Stormlion.ImageCropper.Droid
{
    public class ImageCropperImplementation : IImageCropperWrapper
    {
        public void ShowFromFile(ImageCropper imageCropper, string imageFile)
        {
            try
            {
                ActivityResultLauncher<CropImageContractOptions> cropImage; 
                CropImage.ActivityBuilder activityBuilder = CropImage.Activity(Android.Net.Uri.FromFile(new Java.IO.File(imageFile)));
                activityBuilder.SetCropMenuCropButtonTitle(imageCropper.CropButtonTitle);
                activityBuilder.SetOutputCompressFormat(Bitmap.CompressFormat.Png);

                if (imageCropper.CropShape == ImageCropper.CropShapeType.Oval)
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Oval);
                }
                else
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Rectangle);
                }

                if(imageCropper.AspectRatioX > 0 && imageCropper.AspectRatioY > 0)
                {
                    activityBuilder.SetFixAspectRatio(true);
                    activityBuilder.SetAspectRatio(imageCropper.AspectRatioX, imageCropper.AspectRatioY);
                }
                else
                {
                    activityBuilder.SetFixAspectRatio(false);
                }

                if(!string.IsNullOrWhiteSpace(imageCropper.PageTitle))
                {
                    activityBuilder.SetActivityTitle(imageCropper.PageTitle);
                }

                //Console.WriteLine("CurrentActivity. " + Xamarin.Essentials.Platform.CurrentActivity.PackageName);
                activityBuilder.Start(Xamarin.Essentials.Platform.CurrentActivity);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            
        }

    }
}