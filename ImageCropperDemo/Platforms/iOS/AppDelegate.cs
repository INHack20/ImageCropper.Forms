using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
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
