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

        public MyRoutes()
        {
            routesList = new List<Route>();
            routeManager = new RouteManager();
            InitializeComponent();
            currentUser = (User)Application.Current.Properties["user"];
            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));
            routesListView.Refreshing += RoutesListView_Refreshing;
            routesListView.ItemTapped += RoutesListView_ItemTapped;
        }

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var route = e.Item as Route;
            await Navigation.PushAsync(new MyRoutesDetail(route));
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
                routesListView.ItemsSource = routesList;
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
                routesListView.ItemsSource = routesList;
            }
            else
            {
                routesListView.ItemsSource = routesList.Where(route => (route.From).ToLower().Contains(e.NewTextValue.ToLower()) || (route.To).ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }
    }
}
