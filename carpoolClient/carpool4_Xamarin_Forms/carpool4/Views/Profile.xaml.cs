using carpool4;
using carpool4.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Carpool
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public partial class Profile : ContentPage
    {

        private User currentUser;
        UserManager manager;

        public static Profile Instance;

        public Profile()
        {
            Instance = this;

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

        async void OnCamera(object sender, EventArgs e)
        {
            IPictureTaker pictureTake =
            DependencyService.Get<IPictureTaker>();

            pictureTake.SnapPic();
        }

        public async void ShowImage(byte[] resizedImage, Stream stream)
        {
            profileImage.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
            profileImage.Rotation = 90;

            Profile.UploadPhoto(resizedImage, currentUser.Id);
        }

        public static string UploadPhoto(byte[] photobytes, string photoName)
        {
            // define your azure cloud account
            CloudStorageAccount account = CloudStorageAccount.Parse(Constants.connectionString);
            // intilaize a client for the created account
            CloudBlobClient client = account.CreateCloudBlobClient();
            // intialize the container
            CloudBlobContainer container = client.GetContainerReference(Constants.containerName);
            // create the container if doesn't exist
            container.CreateIfNotExists();
            // set public permission for your blob to be able to be used by everyone 
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            // set the photo path ... note the photo name can be either "name.jpg" or it can has a virtual path like "pics/folder/name.jpg"
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            // start uploading
            photo.UploadFromByteArray(photobytes, 0, photobytes.Length);
            // return the url of the photo after uploading
            return photo.Uri.ToString();
        }
    }
}
