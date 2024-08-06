
namespace Stormlion.ImageCropper
{
    public interface IImageCropperWrapper
    {
        void ShowFromFile(ImageCropper imageCropper, string imageFile);

        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
