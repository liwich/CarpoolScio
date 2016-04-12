using carpool4.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Carpool
{
    public partial class Profile : ContentPage
    {

        private User currentUser;
        UserManager manager;

        public Profile()
        {

            InitializeComponent();


            manager = new UserManager();
            currentUser = (User)Application.Current.Properties["user"];

            loadData();

            genderPicker.SelectedIndexChanged += (sender, args) =>
            {
                genderPicker.BackgroundColor = Color.FromHex("#009688");
            };

        }


        void loadData()
        {
            string[] genders = { "Male", "Female" };

            if (currentUser != null)
            {

                if (!string.IsNullOrEmpty(currentUser.Name))
                    nameEntry.Text = currentUser.Name;

                if (currentUser.Age != 0)
                    ageEntry.Text = currentUser.Age + "";

                if (!string.IsNullOrEmpty(currentUser.Phone))
                    phoneEntry.Text = currentUser.Phone;

                Debug.WriteLine(currentUser.Gender);

                if (!string.IsNullOrEmpty(currentUser.Gender))
                {
                    genderPicker.SelectedIndex = Array.IndexOf(genders, currentUser.Gender);
                    genderPicker.BackgroundColor = Color.FromHex("#00897B");
                }

            }
        }

        async Task UpdateUser(User user)
        {
            User userResponse = await manager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        async void Dashboard(object sender, EventArgs e)
        {
            string name = this.nameEntry.Text;
            int age = string.IsNullOrEmpty(ageEntry.Text) ? 0 : Int32.Parse(ageEntry.Text);
            string phone = this.phoneEntry.Text;
            string genderSelected = genderPicker.SelectedIndex != -1 ? genderPicker.Items[genderPicker.SelectedIndex] : "";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(genderSelected) || age <= 0)
            {
                if (age == 0)
                {
                    await DisplayAlert("Error", "Age can't be null", "accept");
                }
                else
                {
                    await DisplayAlert("Error", "Fill blank fields", "Accept");
                }
            }
            else
            {
                var user = new User
                {
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Password = currentUser.Password,
                    Name = name + "",
                    Age = age,
                    Phone = phone + "",
                    Gender = genderSelected + ""
                };

                activityIndicator.IsRunning = true;
                await UpdateUser(user);
                activityIndicator.IsRunning = false;
                Application.Current.MainPage = new NavigationPage(new Dashboard());
            }

        }
    }
}
