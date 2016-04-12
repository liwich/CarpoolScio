using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Carpool
{
    public partial class AddCar : ContentPage
    {
        private User currentUser;
        private CarManager manager;

        public AddCar()
        {

            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];
            manager = new CarManager();
        }


        async Task AddNewCar(Car car)
        {
            await manager.SaveCarAsync(car);
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            string model = modelEntry.Text;
            string color = colorEntry.Text;

            activityIndicator.IsRunning = true;

            var car = new Car { Color = color, Model = model, Id_User = currentUser.Id };

            if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(color))
            {
                await AddNewCar(car);
                activityIndicator.IsRunning = false;

                await DisplayAlert("Success", "Car added successfully", "Accept");
                await Navigation.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Incorrect", "Model or Color fields cannot be empty, please insert a value.", "Close");
                activityIndicator.IsRunning = false;
            }
        }
    }
}
