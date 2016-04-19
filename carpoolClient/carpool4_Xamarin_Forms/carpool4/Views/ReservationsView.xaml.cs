using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Carpool
{
    public partial class ReservationsView : ContentPage
    {
        private User currentUser;
        private List<Route> routesList;
        private RouteManager routeManager;
        private List<Route> routesReservations;

        private List<Reservation> reservationsList;
        private ReservationManager reservationManager;

        public ReservationsView()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            routesList = new List<Route>();
            routeManager = new RouteManager();

            reservationsList = new List<Reservation>();
            reservationManager = new ReservationManager();



            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));

            routesListView.ItemTapped += RoutesListView_ItemTapped;

            routesListView.Refreshing += RoutesListView_Refreshing;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRoutes();
        }

        private async void LoadRoutes()
        {

            routesReservations = new List<Route>();

            routesList = await routeManager.ListRoutesWhere(route => route.Id_User != currentUser.Id && route.Depart_Date > DateTime.Now);
            reservationsList = await reservationManager.GetReservationsWhere(reservation => reservation.Id_User == currentUser.Id);

            foreach (var reservation in reservationsList)
            {
                var route = routesList.Find(routes => routes.Id == reservation.Id_Route);
                if (route != null)
                {
                    routesReservations.Add(route);
                }
            }

            if (routesReservations.Count > 0)
            {
                errorLayout.IsVisible = false;
            }
            else
            {
                errorLayout.IsVisible = true;
                errorLabel.Text = "No reservations available";
                routesListView.BackgroundColor = Color.FromHex("#009688");
                await Navigation.PopAsync();
            }

            routesListView.ItemsSource = routesReservations;

            routesListView.IsRefreshing = false;
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            LoadRoutes();
        }

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            await Navigation.PushAsync(new RoutesDetail(userRoute.IdRoute));
        }

        public void OnSearch(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                routesListView.ItemsSource = routesReservations;
            }
            else
            {
                routesListView.ItemsSource =
                    routesReservations.Where(
                        route => route.From.Contains(e.NewTextValue) || route.To.Contains(e.NewTextValue));
            }
        }
    }
}
