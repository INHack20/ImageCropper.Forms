using Xamarin.Forms;

namespace Xamarin.ImageCropper.iOS
{
    public class Platform
    {
        public static void Init()
        {
            DependencyService.Register<IImageCropperWrapper, ImageCropperImplementation>();
        }
    }
}