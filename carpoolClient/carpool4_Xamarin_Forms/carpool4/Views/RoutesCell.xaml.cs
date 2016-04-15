using Xamarin.Forms;

namespace Carpool
{
    using System;
    using System.Globalization;

    using carpool4;
    using carpool4.Models;
    

    public partial class RoutesCell : ViewCell
    {
        private Route route;

        public RoutesCell()
        {
            InitializeComponent();

            imageRoute.SetBinding(Image.SourceProperty, new Binding {Path = "Id_User",StringFormat = "https://carpoolimages.blob.core.windows.net/images/{0}", Converter = new BitmapImageConverter()});
            fromLabel.SetBinding(Label.TextProperty, new Binding { Path = "Id_User", StringFormat = "https://carpoolimages.blob.core.windows.net/images/{0}" });
            //fromLabel.SetBinding(Label.TextProperty, new Binding("From"));
            toLabel.SetBinding(Label.TextProperty, new Binding("To"));

        }
    }

    public class BitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));

            if (value is Uri)
                return new BitmapImage((Uri)value);

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}