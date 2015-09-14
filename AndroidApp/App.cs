using System;
using Android.App;
using Android.Runtime;
using Parse;

namespace ParseAndroidStarterProject
{
    [Application]
    public class App : Application
    {
        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            // Initialize the parse client with your Application ID and .NET Key found on
            // your Parse dashboard
            ParseClient.Initialize("FLRGUaPZAKFc3aUGqa0ZhxisA1NVNCu27lDFIvyB",
                                   "1o3QqHO4Sb4tXUYDoGSj8cOCJCRk9t7BXJFnK9l0");
        }
    }
}