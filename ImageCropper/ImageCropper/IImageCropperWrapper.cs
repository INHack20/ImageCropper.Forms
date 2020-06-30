using Plugin.Media.Abstractions;

namespace Xamarin.ImageCropper
{
    public interface IImageCropperWrapper
    {
        void ShowFromFile(ImageCropper imageCropper, MediaFile imageFile);
    }
}