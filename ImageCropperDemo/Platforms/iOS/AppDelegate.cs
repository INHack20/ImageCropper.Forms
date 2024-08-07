using Foundation;
using UIKit;

namespace ImageCropperDemo
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Stormlion.ImageCropper.iOS.Platform.Init();


            return base.FinishedLaunching(application, launchOptions);
        }
    }
}
