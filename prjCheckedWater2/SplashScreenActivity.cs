using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace prjCheckedWater2
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //SetContentView(Resource.Layout.splash);

            Task.Delay(2500).Wait();

            // Create your application here
            //FindViewById<ImageView>(Resource.Id.imageView1).Click += delegate {

            //};
            //Start Activity1 Activity  
            StartActivity(typeof(LoginActivity));

        }
    }
}