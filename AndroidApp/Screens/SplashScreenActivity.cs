using Android.App;
using Android.OS;
using Java.Lang;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartActivity(typeof (MainActivity));
        }
    }
}