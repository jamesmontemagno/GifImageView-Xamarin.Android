using Android.App;
using Android.Widget;
using Android.OS;
using Felipecsl.GifImageViewLibrary;
using System.Net.Http;
using System;
using Android.Graphics;

namespace GifImageViewSample
{
    [Activity(Label = "GifImageView Sample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, GifImageView.IOnFrameAvailableListener
    {

        GifImageView gifImageView;
        Button btnToggle;
        Button btnBlur;

        private bool shouldBlur = false;
        Blur blur;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            gifImageView = FindViewById<GifImageView>(Resource.Id.gifImageView);
            btnToggle = FindViewById<Button>(Resource.Id.btnToggle);
            btnBlur = FindViewById<Button>(Resource.Id.btnBlur);
            var btnClear = FindViewById<Button>(Resource.Id.btnClear);
            gifImageView.OnFrameAvailableListener = this;

            blur = Blur.NewInstance(this);
            btnBlur.Click += (sender, e) => 
                {
                    shouldBlur = !shouldBlur;
                };

            btnClear.Click += (sender, e) => gifImageView.Clear();

            btnToggle.Click += (sender, e) => 
                {
                    try
                    {
                        if(gifImageView.IsAnimating)
                            gifImageView.StopAnimation();
                        else
                            gifImageView.StartAnimation();
                    }
                    catch(Exception ex)
                    {
                    }
                };

            btnBlur.Enabled = false;
            btnClear.Enabled = false;
            btnToggle.Enabled = false;
           
            try
            {
                ActionBar.Title = "Loading...";
                var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync("http://dogoverflow.com/dRX5G8qK");
                gifImageView.SetBytes(bytes);
                gifImageView.StartAnimation();
                ActionBar.Title = "Gif!!!";
                btnBlur.Enabled = true;
                btnClear.Enabled = true;
                btnToggle.Enabled = true;
            }
            catch(Exception ex)
            {
                ActionBar.Title = "error downloading";
            }
        }

        public Bitmap OnFrameAvailable(Bitmap bitmap)
        {
            if (shouldBlur)
                return blur.BlurImage(bitmap);

            return bitmap;
        }

        protected override void OnStop()
        {
            base.OnStop();
            gifImageView.StopAnimation();
        }

        protected override void OnStart()
        {
            base.OnStart();
            gifImageView.StartAnimation();
        }
    }
}


