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

        private UserManager usersManager;
        private IEnumerable<UserRoute> usersRoutes;
        private List<User> userList;

        public ReservationsView()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            routesList = new List<Route>();
            routeManager = new RouteManager();

            reservationsList = new List<Reservation>();
            reservationManager = new ReservationManager();

            usersManager = new UserManager();
            userList = new List<User>();

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

            userList = await usersManager.GetUsersWhere(user => user.Id != currentUser.Id);

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

            usersRoutes = from r in routesReservations
                          join u in userList on r.Id_User equals u.Id
                          select new UserRoute { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };

            routesListView.ItemsSource = usersRoutes;

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
                routesListView.ItemsSource = usersRoutes;
            }
            else
            {
                IEnumerable<UserRoute> usersRoutes = from r in routesList
                                                     join u in userList on r.Id_User equals u.Id
                                                     where (r.From.ToLower().Contains(e.NewTextValue.ToLower()) || r.To.ToLower().Contains(e.NewTextValue.ToLower()))
                                                     select new UserRoute() { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;
            }
        }
    }
}
