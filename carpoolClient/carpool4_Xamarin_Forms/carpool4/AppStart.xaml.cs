using System.Diagnostics;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace Carpool
{
    public partial class AppStart : Application
    {
        public AppStart()
        {
            InitializeComponent();
            //MainPage = new NavigationPage(new Login());
            var loginPage = new NavigationPage(new Login());


            MainPage = loginPage;
           // MainPage = new Dashboard();
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("sleep");
        }

        protected override void OnStart()
        {
            Debug.WriteLine("start");
        }

        protected override void OnResume()
        {
            Debug.WriteLine("resume");
        }

    }
}
