using System;
using Xamarin.Forms;
using System.Diagnostics;
using carpool4.Models;

namespace Carpool
{
    public partial class Login : ContentPage
    {
        private UserManager usersManager;

        public Login()
        {
            InitializeComponent();

            usersManager = new UserManager();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void SignIn(object sender, EventArgs e)
        {
            string email = this.emailEntry.Text;
            string password = this.passwordEntry.Text;
            var user = new User { Email = email, Password = password };

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                activityIndicator.IsRunning = true;

                User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.Email == user.Email && userSelect.Password == user.Password);

                activityIndicator.IsRunning = false;

                if (userResponse != null && userResponse.Email.Equals(email, StringComparison.Ordinal) && userResponse.Password.Equals(password, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;
                    Application.Current.MainPage = new NavigationPage(new Dashboard());
                }
                else
                {
                    await DisplayAlert("Incorrect", "Your email or password is incorrect, please try again.", "Close");
                    this.emailEntry.Text = "";
                    this.passwordEntry.Text = "";
                }
            }
            else
            {
                await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                this.emailEntry.Text = "";
                this.passwordEntry.Text = "";
            }
        }

        async void SignUp(object sender, EventArgs e)
        {
            var signUpPage = new SignUp();
            await Navigation.PushAsync(signUpPage);
        }

    }
}
