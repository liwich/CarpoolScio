using System;
using System.Globalization;
using carpool4.Models;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace carpool4
{
    public class StringToUriConverter : IValueConverter
    {
        private UserManager userManager;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (AzureStorage.ExistPhoto(value.ToString()))
                {
                    userManager = new UserManager();
                    return new Uri(parameter + value.ToString());
                }
                else
                {
                    return "profile.jpg";
                }
            }
            else
            {
                return "profile.jpg";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
