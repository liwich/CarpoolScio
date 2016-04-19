using carpool4;
using Xamarin.Forms;

namespace Carpool
{
    public partial class RoutesCell : ViewCell
    {
        public RoutesCell()
        {
            InitializeComponent();
            imageRoute.SetBinding(Image.SourceProperty,new Binding(path: "ResourceName", converter:new StringToUriConverter(),converterParameter: Constants.ContainerURL));
            fromLabel.SetBinding(Label.TextProperty, new Binding("From"));
            toLabel.SetBinding(Label.TextProperty, new Binding("To"));
        }
    }
}