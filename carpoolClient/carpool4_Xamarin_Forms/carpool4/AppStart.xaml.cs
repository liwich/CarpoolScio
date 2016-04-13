using System.Collections.Generic;
using System.Diagnostics;
using carpool4.Models;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace Carpool
{
    public partial class AppStart : Application
    {
        public static User UserMain; 
        private List<Reservation> lr; 
        public AppStart()
        {
            UserMain = new User();
            UserMain.Id = "778245330a7a46178cdfffe22ec48164";


            InitializeComponent();
            var loginPage = new NavigationPage(new Login());
            MainPage = loginPage;
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
