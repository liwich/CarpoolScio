using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Carpool
{
    public partial class MyRoutes : ContentPage
    {
        private User currentUser;
        private List<Route> routesList;
        private RouteManager routeManager;
        private List<User> userList;
        private IEnumerable<UserRoute> usersRoutes;

        public MyRoutes()
        {
            routesList = new List<Route>();
            routeManager = new RouteManager();
            InitializeComponent();
            currentUser = (User)Application.Current.Properties["user"];

            userList = new List<User>();
            userList.Add(currentUser);



            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));
            routesListView.Refreshing += RoutesListView_Refreshing;
            routesListView.ItemTapped += RoutesListView_ItemTapped;
        }

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            await Navigation.PushAsync(new MyRoutesDetail(userRoute.IdRoute));
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            searchBar.Text = "";
            LoadRoutes();
        }

        private async void LoadRoutes()
        {
            routesListView.IsRefreshing = true;

            routesList = await routeManager.ListRoutesWhere(route => route.Id_User == currentUser.Id);

            errorLayout.Children.Clear();
            if (routesList.Count == 0)
            {
                errorLayout.Children.Add(new Label
                {
                    Text = "No routes available",
                    TextColor = Color.White,
                    FontSize = 25,
                    HorizontalTextAlignment = TextAlignment.Center
                });
            }
            else
            {

                usersRoutes = from r in routesList
                              join u in userList on r.Id_User equals u.Id
                              select new UserRoute() { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;
            }

            routesListView.IsRefreshing = false;
        }

        async void AddRoute(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddRoute());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.LoadRoutes();
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
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
