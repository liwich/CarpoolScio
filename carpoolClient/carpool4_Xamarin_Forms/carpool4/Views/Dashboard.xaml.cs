using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Carpool
{
    public partial class Dashboard : ContentPage
    {
        private User currentUser;
        private List<Route> routesList;
        private RouteManager routeManager;

        private List<Reservation> reservationsList;
        private ReservationManager reservationManager;
        private ToolbarItem reservationsButton;
        private List<User> userList;
        private UserManager usersManager;
        private IEnumerable<UserRoute> usersRoutes;

        public Dashboard()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];
            usersManager = new UserManager();
            userList = new List<User>();

            routesList = new List<Route>();
            routeManager = new RouteManager();

            reservationsList = new List<Reservation>();
            reservationManager = new ReservationManager();


            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));

            routesListView.ItemTapped += RoutesListView_ItemTapped;

            routesListView.Refreshing += RoutesListView_Refreshing;

            reservationsButton = new ToolbarItem
            {
                Name = "Reservations",
                Command = new Command(this.Reservations),
                Order = ToolbarItemOrder.Primary,
                Priority = 3
            };

        }

        private async void LoadRoutesList()
        {
            userList = await usersManager.GetUsersWhere(user => user.Id != currentUser.Id);

            reservationsList = new List<Reservation>();
            routesListView.IsRefreshing = true;
            routesList = await routeManager.ListRoutesWhere(route => route.Id_User != currentUser.Id && route.Depart_Date > DateTime.Now);

            reservationsList =
                await reservationManager.GetReservationsWhere(reservation => reservation.Id_User == currentUser.Id);

            foreach (var reservation in reservationsList)
            {
                routesList.Remove(routesList.Find(route => route.Id == reservation.Id_Route));
            }

            for (int i = 0; i < routesList.Count; i++)
            {
                List<Reservation> rl = await reservationManager.GetReservationsWhere(reservation => reservation.Id_Route == routesList.ElementAt(i).Id);
                if (rl.Count == routesList.ElementAt(i).Capacity)
                {
                    routesList.RemoveAt(i);
                }
            }

            if (routesList.Count != 0)
            {
                routesListView.BackgroundColor = Color.White;
                errorLayout.IsVisible = false;
            }
            else
            {
                errorLabel.Text = "No routes available";
                errorLayout.IsVisible = true;
                routesListView.BackgroundColor = Color.FromHex("#009688");
            }

            usersRoutes = from r in routesList
                          join u in userList on r.Id_User equals u.Id
                          select new UserRoute { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };

            routesListView.ItemsSource = usersRoutes;

            if (reservationsList.Count > 0)
            {
                ToolbarItems.Add(reservationsButton);
            }

            routesListView.IsRefreshing = false;
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            searchBar.Text = "";
            if (ToolbarItems.Contains(reservationsButton))
            {
                ToolbarItems.Remove(reservationsButton);
            }
            LoadRoutesList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRoutesList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (reservationsList.Count > 0)
            {
                ToolbarItems.Remove(reservationsButton);
            }
        }

        /*Events*/

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            await Navigation.PushAsync(new RoutesDetail(userRoute.IdRoute));
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {

                IEnumerable<UserRoute> usersRoutes = from r in routesList
                                                     join u in userList on r.Id_User equals u.Id
                                                     where (r.From.ToLower().Contains(e.NewTextValue.ToLower()) || r.To.ToLower().Contains(e.NewTextValue.ToLower()))
                                                     select new UserRoute() { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;
            }
            else
            {
                routesListView.ItemsSource = usersRoutes;
            }
        }

        /*Menu Options*/

        private async void MyRoutes(object sende, EventArgs e)
        {
            await Navigation.PushAsync(new MyRoutes());
        }

        private async void ProfileDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        private void LogOut(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private async void Reservations()
        {
            await Navigation.PushAsync(new ReservationsView());
        }
    }
}