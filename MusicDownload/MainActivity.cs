using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using MusicDownload.Classes;

namespace MusicDownload
{
    [Activity(Label = "MusicDownload", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : Activity
    {
        string mainUrl;
        ProgressBar pb;

        protected override void OnCreate(Bundle bundle)
        {
            mainUrl = @"http://vk.com/audio";

            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            pb = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            Button y = FindViewById<Button>(Resource.Id.button1);


            WebView x = FindViewById<WebView>(Resource.Id.webView1);
            x.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            x.Settings.JavaScriptEnabled = true;
            MyWebViewClient a = new MyWebViewClient(ShowAlertHandler, pb);
            
            x.SetWebViewClient(a);
            x.LoadUrl(mainUrl);            
            
            try
            {
                y.Click += (os, ea) =>
                {
                    if (a.IsUrlCreated)
                    {                        
                        StartDownloadAsync(a.UrlCreated, ShowAlertHandler, pb);
                    }
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }            
        }

        async void StartDownloadAsync(string url, Action showAlert, ProgressBar pb)
        {
            pb.Progress = 0;
            Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
            progressReporter.ProgressChanged += (s, args) => pb.Progress = (int)(100 * args.PercentComplete);

            Task downloadTask = CreateDownloadTask(url, showAlert, pb, progressReporter);
            await downloadTask;
        }
        
        public static async Task CreateDownloadTask(string url, Action showAlert, ProgressBar pb, IProgress<DownloadBytesProgress> progessReporter)
        {
            string pattern = @"(?<=\/)\w*(?=.mp3)";
            Regex r = new Regex(pattern);
            var x = r.Match(url).Value;

            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += (s, ea) =>
            {
                if (ea.BytesReceived != 0 && progessReporter != null)
                {
                    DownloadBytesProgress args = new DownloadBytesProgress(url, ea.BytesReceived, ea.TotalBytesToReceive);
                    progessReporter.Report(args);
                }
            };
            wc.DownloadFileCompleted += (os, ea) => { showAlert.Invoke(); };
            await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).Path, $"{x}.mp3"));
        }

        public void ShowAlertHandler()
        {            
            if (Looper.MyLooper() == null)
                Looper.Prepare();
            RunOnUiThread(ShowAlert);
        }

        public void ShowAlert()
        {
            Toast.MakeText(this, "File downloaded", ToastLength.Long).Show();
        }

    }
}


