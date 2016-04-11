using System;

using Xamarin.Forms;
using carpool4.Models;

namespace carpool4
{
	public class App : Application
	{
		public App ()
		{
            new CarManager();
            new ReservationManager();
            //new UserManager();
            //new RouteManager();
			// The root page of your application
			MainPage = new TodoList();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

