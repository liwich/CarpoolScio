using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Carpool;

namespace carpool4.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            SetStyles();
            global::Xamarin.Forms.Forms.Init ();
            Xamarin.FormsMaps.Init();
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            // IMPORTANT: uncomment this code to enable sync on Xamarin.iOS
            // For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
            //SQLitePCL.CurrentPlatform.Init();

			LoadApplication (new AppStart ());

			return base.FinishedLaunching (app, options);
		}

        static void SetStyles()
        {
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0, 77, 64);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = UIColor.White
            });

            UIToolbar.Appearance.BarTintColor = UIColor.FromRGB(38, 166, 154);

        }
    }
}

