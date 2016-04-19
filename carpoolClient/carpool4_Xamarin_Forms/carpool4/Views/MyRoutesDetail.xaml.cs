using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Carpool
{
    public partial class MyRoutesDetail : ContentPage
    {
        private Route route;
        private User userRoute;
        private UserManager usersManager;
        private RouteManager routeManager;
        private User currentUser;
        private ReservationManager reservationsManager;
        private List<Reservation> reservationResult;
        private List<User> usersList;

        public MyRoutesDetail(string routeId)
        {
            reservationsManager = new ReservationManager();
            routeManager = new RouteManager();
            usersManager = new UserManager();
            reservationResult = new List<Reservation>();
            usersList = new List<User>();
            currentUser = (User)Application.Current.Properties["user"];

            
            
            
            InitializeComponent();

            this.IsBusy = true;

            LoadRouteData(routeId);


            
        }


        private async void LoadRouteData(string routeId)
        {
            route = await routeManager.GetRouteWhere(route => route.Id == routeId);

            userRoute = new User
            {
                Id = this.route.Id_User
            };

            this.LoadData();
        }

        private async Task LoadReservation()
        {
            string id_user = currentUser.Id;
            string id_route = route.Id;

            Reservation reservation = new Reservation
            {
                Id_User = id_user,
                Id_Route = id_route
            };

            reservationResult = await reservationsManager.GetReservationsWhere(res => res.Id_Route == reservation.Id_Route);

            if (reservationResult.Count != 0)
            {
                foreach (var res in reservationResult)
                {
                    usersList.Add(await usersManager.GetUserWhere(user => user.Id == res.Id_User));
                }
                foreach (var user in usersList)
                {
                    usersLayout.Children.Add(new Label() { Text = user.Name });
                }
            }
            else
            {
                usersLayout.Children.Add(new Label() { Text = "Any user reserved" });
            }
        }

        private async void LoadData()
        {


            if (route.Depart_Date.CompareTo(DateTime.Today.Add(DateTime.Now.TimeOfDay)) >= 0)
            {
                await this.LoadReservation();
            }
            else
            {
                activateButton.IsVisible = true;
                usersLabel.Text = "The time for this route passed";
            }
            userRoute = await usersManager.GetUserWhere(userSelect => userSelect.Id == userRoute.Id);
            int seats = route.Capacity;
            nameLabel.Text = userRoute.Name;
            ageLabel.Text = "Age: " + userRoute.Age;
            phoneLabel.Text = "Phone: " + userRoute.Phone;
            descriptionLabel.Text = route.Comments;
            departureLabel.Text = "Departure: \n" + route.Depart_Date.ToString("dd/MMMM H:mm ") + "h";
            if (reservationResult.Count != 0)
            {
                seats = route.Capacity - reservationResult.Count;
            }
            seatsLabel.Text = "Seats Available: " + seats + "/" + route.Capacity;
            this.IsBusy = false;

        }

        private async void OnStartingPoint(object sender, EventArgs e)
        {
            var latitude = Double.Parse(route.From_Latitude);
            var longitude = Double.Parse(route.From_Longitude);

            var position = new Position(latitude, longitude);

            await Navigation.PushAsync(new MapStartingPoint(position));

        }

        private async void OnEndingPoint(object sender, EventArgs e)
        {
            var latitude = Double.Parse(route.To_Latitude);
            var longitude = Double.Parse(route.To_Longitude);

            var position = new Position(latitude, longitude);

            await Navigation.PushAsync(new MapEndingPoint(position));
        }

        private async void OnActivate(object sender, EventArgs e)
        {
            var reservations = new Reservation
            {
                Id_Route = route.Id
            };
            IsBusy = true;
            await reservationsManager.DeleteReservationsAsync(reservations);
            usersLabel.Text = "";
            activateButton.IsVisible = false;
            IsBusy = false;


            if (DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time)).CompareTo(DateTime.Now) <= 0)
            {
                route.Depart_Date = DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time)).AddDays(1);
            }
            else
            {
                route.Depart_Date = DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time));
            }

            await routeManager.SaveRouteAsync(route);
            await DisplayAlert("Success", "Route activated", "Accept");

            LoadData();
        }


    }
}
