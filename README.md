GifImageView for Xamarin.Android
============

Xamarin.Android ImageView that handles Animated GIF images!

This is a derivative of Felipe Lima's GifImageView: https://github.com/felipecsl/GifImageView/ under MIT

### Usage

Install NuGet Package into Android project: https://www.nuget.org/packages/Refractored.GifImageView

**In your Android XML:**

```xml
<com.felipecsl.gifimageview.library.GifImageView
    android:id="@+id/gifImageView"
    android:layout_gravity="center"
    android:scaleType="fitCenter"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

**In your Activity class:**

Add using statement:
```csharp
using Felipecsl.GifImageViewLibrary;
```
Find views and load animation:

```csharp
GifImageView gifImageView;     

protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    // Set our view from the "main" layout resource
    SetContentView(Resource.Layout.Main);

    gifImageView = FindViewById<GifImageView>(Resource.Id.gifImageView);
    var buttonLoad = FindViewById<Button>(Resource.Id.loadImage);

    buttonLoad.Click += async (sender, e) => 
        {
            try
            {
                ActionBar.Title = "Error downloading";
                using(var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync("http://dogoverflow.com/dRX5G8qK");
                    gifImageView.SetBytes(bytes);
                    gifImageView.StartAnimation();
                }
            }
            catch(Exception ex)
            {
                ActionBar.Title = "Error downloading";
            }
        };
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
```

If you need to post-process the GIF frames, you can do that via ``ISetOnFrameAvailableListener``.


```csharp
public class MainActivity : Activity, GifImageView.IOnFrameAvailableListener
{
    protected override async void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.Main);

        gifImageView = FindViewById<GifImageView>(Resource.Id.gifImageView);
        gifImageView.OnFrameAvailableListener = this;
    }   

    public Bitmap OnFrameAvailable(Bitmap bitmap)
    {
        if (shouldBlur)
            return blur.BlurImage(bitmap);

        return bitmap;
    }
}   
```

You can see an example of that in the sample application.

### Outstanding issues

* Transparency on GIFs is currently not supported and will show up black.


### Copyright and license
The MIT License (MIT)

Copyright (c) 2015 James Montemagno / Refractored LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


This is a derivative of Felipe Lima's GifImageView: https://github.com/felipecsl/GifImageView/ under MIT
