using Android.App;
using Android.Content;
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

            // Retrieve Setting For Authenticated Users
            //retreive 
            var prefs = Application.Context.GetSharedPreferences("BetaChi", FileCreationMode.Private);
            var userAuth = prefs.GetBoolean("UserAuth", false);

            // Lauch Correct Activity
            if (userAuth)
                StartActivity(typeof (MainActivity));
            else
                StartActivity(typeof (AuthenticationActivity));
        }
    }
}