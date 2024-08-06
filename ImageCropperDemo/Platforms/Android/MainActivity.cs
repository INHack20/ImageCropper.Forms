using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Stormlion.ImageCropper.Droid;
using Platform = Stormlion.ImageCropper.Droid.Platform;

namespace ImageCropperDemo
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            Platform.Init();
            base.OnCreate(savedInstanceState);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Platform.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
