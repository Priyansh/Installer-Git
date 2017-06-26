using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

namespace InstallerApp
{
    [Activity(Label = "InstallerApp", Icon = "@drawable/Frendel_Logo", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)] // MainLauncher = true
    public class MainActivity : Activity
    {
        ImageButton imgbtnJobs;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";
            ActionBar.SetLogo(Resource.Drawable.fk48);
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.textViewGeneral.Text = "Frendel Kitchens-Installer App";
            headerGeneralInfo.imgViewIcon.Visibility = Android.Views.ViewStates.Gone;
            headerGeneralInfo.imgbtnBack.Visibility = Android.Views.ViewStates.Gone;
            
            imgbtnJobs = FindViewById<ImageButton>(Resource.Id.imgbtnJobs);
            imgbtnJobs.Click += delegate {
                var intent = new Android.Content.Intent(this, typeof(JobScreen));
                StartActivity(intent);
                Finish();
            };

        }
        
        private void ParseAndDisplay(object fetchInfo)
        {
           
        }

    } //End MainActivity
}


