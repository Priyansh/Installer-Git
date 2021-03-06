using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InstallerApp
{
    [Activity(Label="")]
    public class JobScreen : Activity //ListActivity
    {        
        List<installerInfoList> lstInstallerInfoClass = new List<installerInfoList>();
        ListView listView;
        ProgressDialog dialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.JobScreen);
            if(lstInstallerInfoClass.Count <= 0){
                //Adding Loading bar 
                dialog = new ProgressDialog(this);
                dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                dialog.SetMessage("Loading...");
                dialog.SetCancelable(true);
                dialog.Show();
                //ActionBar.SetHomeButtonEnabled(true);
                //ActionBar.SetDisplayHomeAsUpEnabled(true);
                var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
                SetActionBar(toolbar);
                //Display Header Information
                displayHeaderInfo();
                //Call WebService
                ThreadPool.QueueUserWorkItem(o => longRunningMethod());
            }

        }

        public void longRunningMethod()
        {
            Thread.Sleep(3000);
            RunOnUiThread(() =>
            {
                PhonegapWebService.phonegap serviceInstaller = new PhonegapWebService.phonegap();
                //serviceInstaller.Url = "http://192.168.3.76:53435/phonegap.asmx";
                serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
                //Display WebService Information
                displayWebServiceInfo(serviceInstaller);
                dialog.Dismiss();
            });
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
            headerGeneralInfo.textViewGeneral.Text = "Jobs";
            headerGeneralInfo.imgViewIcon.SetImageResource(Resource.Drawable.ToolBox48);
        }

        public void displayWebServiceInfo(PhonegapWebService.phonegap serviceInstaller)
        {
            try
            {                
                var serviceListInstallerInfo = serviceInstaller.InsKP_GetInstaller();
                for (int i = 0; i < serviceListInstallerInfo.Length; i++)
                {
                    var fillInstallerProperties = new installerInfoList
                    {
                        Company = serviceListInstallerInfo[i].Company,
                        Project = serviceListInstallerInfo[i].Project,
                        CSID = serviceListInstallerInfo[i].CSID,
                        Lot = serviceListInstallerInfo[i].Lot,
                        JobNum = serviceListInstallerInfo[i].MasterNum.ToString().Substring(6),
                        MasterNum = serviceListInstallerInfo[i].MasterNum,
                        ShippedDone = serviceListInstallerInfo[i].ShippedDone,
                        InstallerJobStatus = serviceListInstallerInfo[i].InstallerJobStatus,
                        InstallerJobStart = serviceListInstallerInfo[i].InstallerJobStart,
                        InstallerJobComplete = serviceListInstallerInfo[i].InstallerJobComplete
                    };
                    lstInstallerInfoClass.Add(fillInstallerProperties);
                }
                listView = FindViewById<ListView>(Resource.Id.lstInstallerInfo);
                // populate the listview with data
                listView.Adapter = new JobScreenAdapter(this, lstInstallerInfoClass);
                listView.ItemClick += OnListItemClick;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Found :" + ex.ToString());
            }

        }

        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var listView = sender as ListView;
            var t = lstInstallerInfoClass[e.Position];
            string[] str = new string[] { t.Company, t.Project, t.Lot, t.JobNum, t.MasterNum, t.ShippedDone, t.CSID.ToString(), t.InstallerJobStatus.ToString(), t.InstallerJobStart, t.InstallerJobComplete };
            Bundle b = new Bundle();
            b.PutStringArray("keyInstallerInfo", str);
            var intent = new Android.Content.Intent(this, typeof(StartJobScheduleStatus));
            intent.PutExtras(b);
            StartActivity(intent);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.JobMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {            
            onRestart();
            return base.OnOptionsItemSelected(item);
            //switch (item.ItemId)
            //{
            //    case Android.Resource.Id.Home:
            //        Finish();
            //        return true;

            //    default:
            //        return base.OnOptionsItemSelected(item);
            //}
        }        
        protected void onRestart()
        {
            base.OnRestart();
            var intent = new Android.Content.Intent(this, typeof(JobScreen));
            StartActivity(intent);
            Finish();
        }
        //protected override void OnListItemClick(ListView lv, View v, int position, long id)
        //{
        //    lv.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
        //    var selectedItem = lstLot[position];
        //    Android.Widget.Toast.MakeText(this, selectedItem, ToastLength.Short).Show();
        //}
    }
}