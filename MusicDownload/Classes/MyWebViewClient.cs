using System;
using Android.App;
using Android.Widget;
using Android.Webkit;
using System.Net;
using System.IO;

namespace MusicDownload.Classes
{

    public class MyWebViewClient : WebViewClient
    {
        Action showAlert;
        ProgressBar pb;
        private bool isUrlCreated;
        private string urlCreated;


        public MyWebViewClient(Action showAlert, ProgressBar pb)
        {
            isUrlCreated = false;
            this.showAlert = showAlert;
            this.pb = pb;
        }

        public string UrlCreated { get => urlCreated; private set => urlCreated = value; }

        public bool IsUrlCreated { get => isUrlCreated; }

        public override WebResourceResponse ShouldInterceptRequest(WebView view, IWebResourceRequest request)
        {
            var url = request.Url.ToString();

            if (url.Contains(".mp3?extra"))
            {
                int indexMP3 = url.IndexOf(".mp3");
                url = url.Substring(0, indexMP3 + 4);

                if (url != null)
                {
                    urlCreated = url;
                    isUrlCreated = true;
                }
            }
            return null;
        }       
    }

}



