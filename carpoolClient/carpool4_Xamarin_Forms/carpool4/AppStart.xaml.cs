using System.Collections.Generic;
using System.Diagnostics;
using carpool4.Models;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace Carpool
{
    public partial class AppStart : Application
    {
        private List<Reservation> lr; 
        public AppStart()
        {

            
            InitializeComponent();
            //MainPage = new NavigationPage(new Login());
            var loginPage = new NavigationPage(new Login());

            init();
            MainPage = loginPage;
           // MainPage = new Dashboard();
        }

        async void init()
        {
           var l = await new ReservationManager().GetTodoItemsAsync();

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
