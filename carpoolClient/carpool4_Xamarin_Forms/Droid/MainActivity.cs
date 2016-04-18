using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Carpool;
using Gcm.Client;
using Xamarin.Forms;
using Xamarin.Media;
using Android.Content;
using System.IO;
using Android.Graphics;

[assembly: Dependency(typeof(carpool4.Droid.MainActivity))]
namespace carpool4.Droid
{
    [Activity(Label = "carpool4.Droid",
        Icon = "@drawable/ic_launcher",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/Theme.Example")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IPictureTaker
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            LoadApplication(new AppStart());

            try
            {
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);
                instance = this;
                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);

            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the Mobile Service. Verify the URL", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }
        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Canceled)
            {
                return;
            }
            else
            {
                MediaFile mediaFile = await data.GetMediaFileExtraAsync(Forms.Context);

                byte[] imageData;

                Stream stream = mediaFile.GetStream();

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    imageData = ms.ToArray();
                }

                byte[] resizedImage = MainActivity.ResizeImageAndroid(imageData, 400, 400);

                Profile.Instance.ShowImage(resizedImage, stream);
            }
        }

        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, originalImage.Width / 3, originalImage.Height / 3, false);
            Matrix matrix = new Matrix();

            matrix.PostRotate(90);

            resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, matrix,
                true);
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }

        }

        public void SelectPic()
        {
            var activity = Forms.Context as Activity;

            var picker = new MediaPicker(activity);

            var intent = picker.GetPickPhotoUI();

            activity.StartActivityForResult(intent, 1);
        }


        public void SnapPic()
        {
            var activity = Forms.Context as Activity;

            var picker = new MediaPicker(activity);

            var intent = picker.GetTakePhotoUI(new StoreCameraMediaOptions
            {
                Directory = "Carpool",
                Name = "user.jpg"
            });

            activity.StartActivityForResult(intent, 1);
        }

        static MainActivity instance = null;

        // Return the current activity instance.
        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }
    }
}

