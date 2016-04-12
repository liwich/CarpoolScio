using carpool4.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Carpool
{
    public partial class RoutesDetail : ContentPage
    {
        private UserManager usersManager;
        private User userRoute;
        private Route route;
        private ReservationManager reservationsManager;
        private User currentUser;

        public RoutesDetail(Route route)
        {
            InitializeComponent();
            this.route = route;

            userRoute = new User
            {
                Id = route.Id_User
            };

            usersManager = new UserManager();
            reservationsManager = new ReservationManager();
            currentUser = (User)Application.Current.Properties["user"];

            this.LoadReservation();
            this.LoadData();

        }

        private async void LoadData()
        {
            this.IsBusy = true;

            userRoute = await usersManager.GetUserWhere(userSelect => userSelect.Id == userRoute.Id);

            nameLabel.Text = userRoute.Name;
            ageLabel.Text = "Age: " + userRoute.Age;
            phoneLabel.Text = "Phone: " + userRoute.Phone;
            descriptionLabel.Text = route.Comments;
            departureLabel.Text = "Departure: \n" + route.Depart_Date.ToString("dd/MMMM H:mm ") + "h";

            Reservation reservation = new Reservation
            {
                Id_Route = route.Id
            };

            List<Reservation> reservations = await reservationsManager.GetReservationsWhere(reserv => reserv.Id_Route == reservation.Id_Route);

            seatsLabel.Text = "Seats Available: " + (route.Capacity - reservations.Count);

            this.IsBusy = false;
        }

        private async void LoadReservation()
        {
            string id_user = currentUser.Id;
            string id_route = route.Id;

            Reservation reservation = new Reservation
            {
                Id_User = id_user,
                Id_Route = id_route
            };

            List<Reservation> reservationResult = await reservationsManager.GetReservationsWhere(reserv => reserv.Id_Route == reservation.Id_Route && reserv.Id_User == reservation.Id_User);

            if (reservationResult.Count != 0)
            {
                cancelButton.IsVisible = true;
                reserveButton.IsVisible = false;
            }
            else
            {
                reserveButton.IsVisible = true;
            }
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

        private async void OnReserved(object sender, EventArgs e)
        {
            bool r = await DisplayAlert("Reservation", "Add reservation?", "Accept", "Cancel");
            if (r == true)
            {
                activityIndicator.IsRunning = true;

                var newReservation = new Reservation
                {
                    Id_Route = route.Id,
                    Id_User = currentUser.Id
                };

                await reservationsManager.SaveReservationAsync(newReservation);

                activityIndicator.IsRunning = false;
                cancelButton.IsVisible = true;
                reserveButton.IsVisible = false;
                LoadData();
            }

        }

        private async void OnCancelReservation(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Cancel Reservation", "Are you sure?", "Accept", "Cancel");
            if (answer)
            {
                var reservation = new Reservation { Id_Route = route.Id, Id_User = currentUser.Id };
                IsBusy = true;
                await reservationsManager.DeleteReservationAsync(reservation);
                IsBusy = false;
                await DisplayAlert("Success", "Reservation Canceled", "Accept");
                await Navigation.PopAsync(true);
            }

        }
    }
}
