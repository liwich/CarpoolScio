using carpool4;
using carpool4.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Carpool
{
    public partial class Profile : ContentPage
    {

        private User currentUser;
        UserManager manager;
        private byte[] profileImageBytes;
        private string oldResourceName;
        private string newResourceName;

        public static Profile Instance;

        public Profile()
        {
            Instance = this;

            profileImageBytes = new byte[0];

            InitializeComponent();

            manager = new UserManager();
            currentUser = (User)Application.Current.Properties["user"];


            if (!string.IsNullOrEmpty(currentUser.ResourceName))
            {
                oldResourceName = currentUser.ResourceName;
            }

            LoadData();

            genderPicker.SelectedIndexChanged += (sender, args) =>
            {
                genderPicker.BackgroundColor = Color.FromHex("#009688");
            };

        }


        void LoadData()
        {
            IsBusy = true;
            string[] genders = { "Male", "Female" };

            Uri photoUri = null;

            if (currentUser != null)
            {

                if (!string.IsNullOrEmpty(currentUser.Name))
                    nameEntry.Text = currentUser.Name;

                if (currentUser.Age != 0)
                    ageEntry.Text = currentUser.Age + "";

                if (!string.IsNullOrEmpty(currentUser.Phone))
                    phoneEntry.Text = currentUser.Phone;

                if (!string.IsNullOrEmpty(currentUser.Gender))
                {
                    genderPicker.SelectedIndex = Array.IndexOf(genders, currentUser.Gender);
                    genderPicker.BackgroundColor = Color.FromHex("#00897B");
                }

                if (!string.IsNullOrEmpty(currentUser.ResourceName))
                {
                    photoUri = AzureStorage.DownloadPhoto(currentUser.ResourceName);
                    if (photoUri != null)
                    {
                        profileImage.Source = ImageSource.FromUri(photoUri);
                    }
                    else
                    {
                        profileImage.Source = ImageSource.FromFile("profile.jpg");
                    }
                }
            }

            IsBusy = false;
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
                if (string.IsNullOrEmpty(newResourceName))
                {
                    newResourceName = oldResourceName;
                }

                var user = new User
                {
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Password = currentUser.Password,
                    Name = name + "",
                    Age = age,
                    Phone = phone + "",
                    Gender = genderSelected + "",
                    ResourceName = newResourceName
                };

                activityIndicator.IsRunning = true;

                if (profileImageBytes.Length > 0)
                {
                    AzureStorage.UploadPhoto(profileImageBytes, newResourceName);
                    if (!string.IsNullOrEmpty(oldResourceName))
                    {
                        AzureStorage.DeletePhoto(oldResourceName);
                    }
                }

                await UpdateUser(user);
                activityIndicator.IsRunning = false;
                Application.Current.MainPage = new NavigationPage(new Dashboard());
            }

        }

        async void OnCamera(object sender, EventArgs e)
        {
            IPictureTaker pictureTake =
            DependencyService.Get<IPictureTaker>();
            pictureTake.SnapPic();
        }

        public async void ShowImage(byte[] resizedImage)
        {
            ChangeImage(resizedImage);
        }

        async void OnFile(object sender, EventArgs e)
        {
            IPictureTaker pictureTake =
            DependencyService.Get<IPictureTaker>();
            pictureTake.SelectPic();
        }

        async void OnRotateLeft(object sender, EventArgs e)
        {
            IPictureTaker pictureTaker = DependencyService.Get<IPictureTaker>();

            ChangeImage(pictureTaker.Rotate(profileImageBytes, -90));
        }

        async void OnRotateRight(object sender, EventArgs e)
        {
            IPictureTaker pictureTaker = DependencyService.Get<IPictureTaker>();
            ChangeImage(pictureTaker.Rotate(profileImageBytes, 90));
        }

        private void ChangeImage(byte[] image)
        {
            newResourceName = Guid.NewGuid().ToString();
            profileImageBytes = image;
            profileImage.Source = ImageSource.FromStream(() => new MemoryStream(image));
            leftButton.IsVisible = true;
            rightButton.IsVisible = true;
        }
    }
}
