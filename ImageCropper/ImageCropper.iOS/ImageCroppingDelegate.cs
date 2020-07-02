using CoreGraphics;
using System;
using TimOliver.TOCropViewController.Xamarin;
using UIKit;

namespace Xamarin.ImageCropper.iOS
{
    public class ImageCroppingDelegate : TOCropViewControllerDelegate
    {
        public EventHandler<UIImage> OnFinishCropping { get; set; }

        public override void DidCropToImage(TOCropViewController cropViewController, UIImage image, CGRect cropRect, nint angle)
        {
            cropViewController.DismissViewController(true, () =>
            {
                OnFinishCropping?.Invoke(this, image);
            });
        }
    }
}