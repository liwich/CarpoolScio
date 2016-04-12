using carpool4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Carpool
{
    public partial class SignUp : ContentPage
    {
        UserManager manager;

        public SignUp()
        {
            InitializeComponent();

            manager = new UserManager();
        }

        async Task AddUser(User user)
        {
            //await manager.SaveUserAsync(user);

            User userResponse = await manager.SaveGetUserAsync(user);

            Application.Current.Properties["user"] = userResponse;

        }

        async void OnSignUp(object sender, EventArgs e)
        {

            string email = this.emailEntry.Text;
            string password = this.passwordEntry.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                this.activityIndicator.IsRunning = true;
                this.signUpButton.IsEnabled = false;

                var user = new User
                {
                    Email = email,
                    Password = password,
                    Name = string.Empty,
                    Age = 0,
                    Gender = string.Empty,
                    Phone = string.Empty
                };

                await AddUser(user);

                //await Navigation.PushModalAsync(new Profile());
                //await Navigation.PopAsync();
            }
            else
            {
                this.emailEntry.PlaceholderColor = this.emailEntry.TextColor = Color.FromHex("#f44336");
                this.passwordEntry.PlaceholderColor = this.passwordEntry.TextColor = Color.FromHex("#f44336");
                await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                this.emailEntry.Text = "";
                this.passwordEntry.Text = "";
            }

        }

        async void EmailTextChanged(object sender, EventArgs e)
        {
            Entry emailEntry = (Entry)sender;

            string email = this.emailEntry.Text;

            if (!string.IsNullOrEmpty(email))
            {
                this.emailEntry.PlaceholderColor = this.emailEntry.TextColor = Color.FromHex("#00695C");
                this.activityIndicator.IsRunning = true;
                validationLabel.IsVisible = true;
                signUpButton.IsEnabled = false;
                User usersSelect = await manager.GetUserWhere(userSelect => userSelect.Email == email);
                this.emailEntryError.IsVisible = usersSelect != null ? true : false;
                signUpButton.IsEnabled = usersSelect != null ? true : false;
                this.signUpButton.IsEnabled = !emailEntryError.IsVisible;

                this.activityIndicator.IsRunning = false;

                validationLabel.IsVisible = false;
            }

        }

        void PasswordTextChanged(object sender, EventArgs e)
        {
            string password = this.passwordEntry.Text;
            if (!string.IsNullOrEmpty(password))
            {
                this.passwordEntry.PlaceholderColor = this.passwordEntry.TextColor = Color.FromHex("#00695C");
            }
            //signUpButton.IsEnabled = password.Length > 0 ? true : false;
        }
    }
}
