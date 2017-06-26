using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InstallerApp
{
    [Activity(Label = "",ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]
    public class IndividualRoom : Activity
    {
        List<individualRoomList> lstIndividualRoomInfoClass = new List<individualRoomList>();
        List<installerInfoList> lstInstallerInfoClass = new List<installerInfoList>();
        ListView listViewRoomInfo, listViewInstallerInfo;
        string[] getstringRooms, getSelectedInstaller;
        TextView textViewRoomInfo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.IndividualRoom);
            getstringRooms = Intent.GetStringArrayExtra("keyRoomInfo");
            getSelectedInstaller = Intent.GetStringArrayExtra("keyselectedInstaller");

            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            //Display Header Information
            displayHeaderInfo();
            textViewRoomInfo = FindViewById<TextView>(Resource.Id.textViewRoomInfo);
            textViewRoomInfo.Text = getstringRooms[0];
            displayFetchedRoomInfo();
        }

        public void displayHeaderInfo()
        {
            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            headerGeneralInfo.textViewGeneral.Text = "Job Number: " + getSelectedInstaller[3];
            headerGeneralInfo.imgViewIcon.Visibility = Android.Views.ViewStates.Gone;
        }

        public void displayFetchedRoomInfo()
        {
            //Installer Info List
            var fillInstallerProperties = new installerInfoList
            {
                Company = getSelectedInstaller[0],
                Project = getSelectedInstaller[1],
                Lot = getSelectedInstaller[2],
                JobNum = getSelectedInstaller[3],
                InstallerJobStatus = int.Parse(getSelectedInstaller[7])
            };
            lstInstallerInfoClass.Add(fillInstallerProperties);
            listViewInstallerInfo = FindViewById<ListView>(Resource.Id.lstInstallerInfo);
            //Populate Installer ListView with Data
            listViewInstallerInfo.Adapter = new JobScreenAdapter(this, lstInstallerInfoClass);

            //Room Info List
            int roomListcount=0;
            while (roomListcount < 2)
            {
                if (roomListcount == 0)
                {
                    var fillIndividualRoomProperties = new individualRoomList
                    {
                        Rooms = getstringRooms[0],
                        Style = getstringRooms[1],
                        Colour = getstringRooms[2],
                        Hardware = getstringRooms[3],
                        CounterTop = getstringRooms[4]
                    };
                    lstIndividualRoomInfoClass.Add(fillIndividualRoomProperties);
                }
                else if (roomListcount == 1)
                {
                    var fillIndividualRoomProperties = new individualRoomList
                    {
                        Rooms = "",
                        Style = "Delivery Photos: 1",
                        Colour = "",
                        Hardware = "",
                        CounterTop = "Installation Photos : 1"
                    };
                    lstIndividualRoomInfoClass.Add(fillIndividualRoomProperties);
                }
                roomListcount++;
            }
            listViewRoomInfo = FindViewById<ListView>(Resource.Id.lstIndividualRoom);
            //Populate IndividualRoom Listview with data
            listViewRoomInfo.Adapter = new IndividualRoomAdapter(this, lstIndividualRoomInfoClass);
        }
    }
}